using Opulenza.Application.ServiceContracts;
using Opulenza.Domain.Entities.Users;
using Stripe;
using ErrorOr;
using Microsoft.Extensions.Logging;
using Opulenza.Application.Common.interfaces;
using Stripe.Checkout;

namespace Opulenza.Infrastructure.Services;

public class PaymentService(
    ICartRepository cartRepository,
    ICurrentUserProvider currentUserProvider,
    ILogger<PaymentService> logger) : IPaymentService
{
    public async Task<ErrorOr<Session?>> GetSessionAsync(string sessionId)
    {
        var options = new SessionGetOptions
        {
            Expand = ["line_items"],
        };

        var sessionService = new SessionService();
        var session = await sessionService.GetAsync(sessionId, options);
        return session;
    }

    public async Task CreateCustomer(ApplicationUser user)
    {
        var customerOptions = new CustomerCreateOptions()
        {
            Name = $"{user.FirstName} {user.LastName}",
            Email = user.Email,
            Phone = user.PhoneNumber,
            Metadata = new Dictionary<string, string>()
            {
                { "userId", user.Id.ToString() },
                { "username", user.UserName ?? "" }
            }
        };

        await new CustomerService().CreateAsync(customerOptions);
    }

    public async Task<string> CreateProduct(Opulenza.Domain.Entities.Products.Product product)
    {
        var productOptions = new ProductCreateOptions()
        {
            Name = product.Name,
            Description = product.Description,
            Metadata = new Dictionary<string, string>()
            {
                { "slug", product.Slug }
            },
            Active = product.IsAvailable,
            DefaultPriceData = new ProductDefaultPriceDataOptions()
            {
                Currency = "USD",
                UnitAmount = (long)(product.Price * 100.00m)
            }
        };

        var createdProduct = await new ProductService().CreateAsync(productOptions);
        return createdProduct.Id;
    }

    public async Task<ErrorOr<string>> CreateSession()
    {
        var email = currentUserProvider.GetCurrentUser().Email;
        var customerService = new CustomerService();
        var customerListOptions = new CustomerListOptions { Limit = 1, Email = email };
        var customer = (await customerService.ListAsync(customerListOptions))?.FirstOrDefault();
        if (customer == null)
        {
            logger.LogError("Customer with email {Email} was not found when retrieving customer data for checkout.",
                email);

            throw new InvalidOperationException(
                $"Customer with email {email} was not found when retrieving customer data for checkout.");
        }

        var userId = currentUserProvider.GetCurrentUser().Id;
        var cart = await cartRepository.GetCartItemsByUserIdAsync(userId);
        if (cart?.Items == null || cart.Items.Count == 0)
        {
            return Error.Conflict("CartEmptyWhenCheckout", "Cart must have at least one item to proceed to checkout.");
        }

        var ids = cart.Items.Select(x => x.PaymentServiceId).ToList();
        var productListOptions = new ProductListOptions()
        {
            Ids = ids
        };
        var productService = new ProductService();
        var productStripeList = await productService.ListAsync(productListOptions);

        var products = cart.Items.Select(x =>
        {
            var product = productStripeList.Data.FirstOrDefault(p => p.Id == x.PaymentServiceId);
            return new
            {
                Id = product.Id,
                DefaultPriceId = product.DefaultPriceId,
                Quantity = x.Quantity,
            };
        }).ToList();

        var domain = "http://localhost:5279";
        var options = new SessionCreateOptions
        {
            ClientReferenceId = customer.Id,
            Currency = "USD",
            Mode = "payment",
            SuccessUrl = domain + "/success.html",
            CancelUrl = domain + "/cancel.html",
            CustomerEmail = customer.Email,
            LineItems = products.Select(p => new SessionLineItemOptions()
            {
                Price = p.DefaultPriceId,
                Quantity = p.Quantity
            }).ToList()
        };
        var service = new SessionService();
        var session = await service.CreateAsync(options);

        return session.Url;
    }
}