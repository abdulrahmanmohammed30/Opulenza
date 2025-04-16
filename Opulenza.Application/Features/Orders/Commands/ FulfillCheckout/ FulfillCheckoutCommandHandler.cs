using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.ServiceContracts;
using Opulenza.Domain.Entities.Enums;
using Opulenza.Domain.Entities.Orders;
using Opulenza.Domain.Entities.Payments;
using Serilog.Context;
using Invoice = Opulenza.Domain.Entities.Invoices.Invoice;
using Order = Opulenza.Domain.Entities.Orders.Order;
using PaymentMethod = Opulenza.Domain.Entities.Enums.PaymentMethod;

namespace Opulenza.Application.Features.Orders.Commands._FulfillCheckout;

public class CheckoutCommandHandler(
    IPaymentService paymentService,
    ICartRepository cartRepository,
    ILogger<CheckoutCommandHandler> logger,
    IRepository<Order> orderRepository,
    IRepository<Payment> paymentRepository,
    ISessionRepository sessionRepository,
    IRepository<Invoice> invoiceRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<FulfillCheckoutCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(FulfillCheckoutCommand request, CancellationToken cancellationToken)
    {
        if (await sessionRepository.ExistsAsync(request.SessionId))
        {
            return "Session has already processed.";
        }
        
        var session = (await paymentService.GetSessionAsync((request.SessionId))).Value;
        if (session == null)
        {
            return Error.Unexpected("FailedToLoadSession", "failed to load session");
        }
        
        if (session.PaymentStatus != "unpaid")
        {
            logger.LogInformation("Started Fulfilling Checkout Session {SessionId}", session.Id);

            // create the order 
            // Product is available & Stock Quantity is not null, otherwise throw an exception 
            // like how the payment was conducted and we don't have any items in the stock? 
            // Say a user bought n items 
            // you need to reflect these bought items in the database 
            // what you need to do is, fetch all the bought items corresponding products from the database, fetch 
            // these items with tracking, then for each item, decrease the amount bought 
            // if it reached 0, set isAvailable to false and stok quantity to zero 
            // create a payment 
            // create an order 
            // simulate a shipment 

            // Todo: refactor to use transaction 

            // Store the session id in the database after successfully creating the order 


            // var productSlugs = new List<string>();
            // foreach (var product in session.LineItems.Data)
            // {
            //     var slug = product.Metadata.FirstOrDefault(d => d.Key == "slug").Value;
            //     if (string.IsNullOrWhiteSpace(slug))
            //     {
            //         using (LogContext.PushProperty("StripeLineItemId", product.Id))
            //         using (LogContext.PushProperty("StripeProductId", product.Price?.Product?.Id ?? "Unknown"))
            //         {
            //             logger.LogError(
            //                 "Critical: Missing 'slug' in Stripe product metadata. Unable to locate corresponding product in the database. This prevents stock adjustment and order creation.");
            //         }
            //
            //         return Error.Failure("Product metadata is incomplete. Please contact support.");
            //     }
            //
            //     productSlugs.Add(slug);
            // }


            // var userId = session.Metadata.FirstOrDefault(x => x.Key == "userId").Value;
            var userId = "5";
            if (string.IsNullOrWhiteSpace(userId))
            {
                using (LogContext.PushProperty("StripeSessionId", session.Id))
                using (LogContext.PushProperty("CustomerId", session.CustomerId ?? "Unknown"))
                {
                    logger.LogCritical(
                        "Critical Error: 'userId' is missing from Stripe session metadata. Unable to associate session with a user. Halting further processing.");
                }

                return Error.Failure("Unable to process payment: missing user information. Please contact support.");
            }

            var cart = await cartRepository.GetTrackedCartAsync(int.Parse(userId), cancellationToken);
            if (cart == null || !cart.Items.Any())
            {
                using (LogContext.PushProperty("StripeSessionId", session.Id))
                using (LogContext.PushProperty("CustomerId", session.CustomerId ?? "Unknown"))
                {
                    logger.LogCritical(
                        "Critical Error: Cart is null or contains no items after payment. This indicates a serious inconsistency in the order processing workflow. Halting further processing.");
                }

                return Error.Failure(
                    "Unable to process order: cart information is missing or invalid. Please contact support.");
            }

            // update database products 
            foreach (var item in cart.Items)
            {
                if (item.Product == null)
                {
                    logger.LogCritical(
                        "Critical Error: Cart item with ID {CartItemId} has no associated product. This indicates a serious data inconsistency. Halting further processing.",
                        cart.Id);

                    return Error.Failure(
                        "Unable to process order: product information is missing. Please contact support.");
                }

                if (item.Quantity > item.Product.StockQuantity)
                {
                    using (LogContext.PushProperty("CartItemId", item.Id))
                    using (LogContext.PushProperty("UserId", userId))
                    using (LogContext.PushProperty("RequestedQuantity", item.Quantity))
                    using (LogContext.PushProperty("AvailableStock", item.Product.StockQuantity))
                    {
                        logger.LogCritical(
                            "Critical Error: Requested quantity ({RequestedQuantity}) for product ID {CartItemId} exceeds available stock ({AvailableStock}). Halting further processing.",
                            item.Quantity, item.Product.StockQuantity, cart.Id);
                    }

                    return Error.Failure(
                        "Unable to process order: requested quantity exceeds available stock. Please adjust your cart and try again.");
                }

                item.Product.StockQuantity -= item.Quantity;
                if (item.Product.StockQuantity == 0)
                {
                    // Todo: update  stripe as well 
                    item.Product.StockQuantity = 0;
                }
            }


            // create the order 
            var order = new Order()
            {
                Items = cart.Items.Select(x => new OrderItem()
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    TotalPrice = x.Product!.Price * x.Quantity +
                                 (x.Product!.TaxIncluded == false ? x.Product.Tax * x.Product.Price : 0),
                    TaxIncluded = x.Product!.TaxIncluded,
                    UnitPrice = x.Product!.Price,
                    Tax = x.Product!.Tax
                }).ToList(),
                UserId = int.Parse(userId),
                OrderStatus = OrderStatus.Pending,
                TotalAmount = cart.TotalPriceAfterDiscount ?? cart.TotalPrice,
            };

            orderRepository.Add(order);
            await unitOfWork.CommitChangesAsync(cancellationToken);

            // create payment
            var payment = new Payment()
            {
                PaymentStatus = PaymentStatus.Completed,
                PaymentMethod = PaymentMethod.CreditCard,
                OrderId = order.Id
            };
            paymentRepository.Add(payment);
            await unitOfWork.CommitChangesAsync(cancellationToken);

            // update the order paymentId 
            order.PaymentId = payment.Id;

            // store the session id 
            var databaseSession = new Opulenza.Domain.Entities.Sessions.Session()
            {
                SessionId = request.SessionId
            };
            sessionRepository.Add(databaseSession);

            var invoice = new Invoice()
            {
                OrderId = order.Id,
                InvoiceUrl = session.InvoiceId
            };
            invoiceRepository.Add(invoice);
            
            // clear the cart 
            cart.Items.Clear();
            cart.TotalPrice = 0;
            cart.TotalPriceAfterDiscount = 0;


            await unitOfWork.CommitChangesAsync(cancellationToken);
            logger.LogInformation("Ended Fulfilling Checkout Session {SessionId}", session.Id);
        }


        return "";
    }
}