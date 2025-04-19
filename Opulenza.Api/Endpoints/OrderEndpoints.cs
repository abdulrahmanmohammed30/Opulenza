using MediatR;
using Microsoft.AspNetCore.Mvc;
using Opulenza.Api.Controllers;
using Opulenza.Api.Endpoints.Internal;
using Opulenza.Api.Helpers;
using Opulenza.Api.Mapping;
using Opulenza.Application.Authentication;
using Opulenza.Application.Features.Orders.Commands._FulfillCheckout;
using Opulenza.Application.Features.Orders.Queries.GetOrder;
using Opulenza.Application.ServiceContracts;
using Opulenza.Contracts.Orders;
using Stripe;
using Stripe.BillingPortal;

namespace Opulenza.Api.Endpoints;

public class OrderEndpoints : CustomEndpoint, IEndpoints
{
    public static void DefineEndpoints(IEndpointRouteBuilder app)
    {
        // Create a Stripe checkout session
        app.MapPost(ApiEndpoints.Orders.Checkout,
                async ([FromServices] IPaymentService paymentService) =>
                {
                    var sessionUrl = await paymentService.CreateSession();
                    return sessionUrl.Match<IResult>(
                        value => Results.Ok(new { sessionUrl = value }),
                        errors => Problem(errors)
                    );
                })
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2)
            .HasApiVersion(2.0).WithTags("Order");

        // Stripe webhook endpoint (hidden from API docs)
        app.MapPost(ApiEndpoints.Orders.Webhook,
                async (HttpContext httpContext, [FromServices]ISender mediator, [FromServices]IConfiguration configuration,
                    ILogger<OrdersController> logger) =>
                {
                    var json = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
                    try
                    {
                        var stripeEvent = EventUtility.ConstructEvent(
                            json,
                            httpContext.Request.Headers["Stripe-Signature"],
                            configuration["Stripe:SigningSecret"]
                        );

                        switch (stripeEvent.Type)
                        {
                            case EventTypes.CheckoutSessionCompleted:
                            case EventTypes.CheckoutSessionAsyncPaymentSucceeded:
                            {
                                var session = stripeEvent.Data.Object as Session;
                                if (session is null)
                                    return Results.Problem(statusCode: 400, detail: "Invalid session");

                                var command = new FulfillCheckoutCommand { SessionId = session.Id };
                                var result = await mediator.Send(command);

                                return result.Match<IResult>(
                                    ok => Results.Ok(ok),
                                    error => Problem(error)
                                );
                            }
                            case EventTypes.CheckoutSessionAsyncPaymentFailed:
                                break;
                        }
                    }
                    catch (StripeException ex)
                    {
                        logger.LogError(ex, "Invalid checkout session");
                        return Results.BadRequest("Invalid checkout session");
                    }

                    return Results.NoContent();
                })
            .ExcludeFromDescription()
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Order")
;

        // Get all orders (admin only)
        app.MapGet(ApiEndpoints.Orders.GetOrders,
                async ([AsParameters] GetOrdersRequest request, [FromServices]ISender mediator, CancellationToken cancellationToken) =>
                {
                    var query = request.MapToGetOrdersQuery();
                    var result = await mediator.Send(query, cancellationToken);
                    return result.Match<IResult>(
                        value => Results.Ok(value.MapToGetOrdersResponse()),
                        Problem
                    );
                })
            .RequireAuthorization(AuthConstants.AdminUserPolicyName)
            .Produces<GetOrdersResponse>()
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Order")
;

        // Get a single order by ID (admin only)
        app.MapGet(ApiEndpoints.Orders.GetOrder,
                async (int id, [FromServices]ISender mediator, CancellationToken cancellationToken) =>
                {
                    var query = new GetOrderQuery { Id = id };
                    var result = await mediator.Send(query, cancellationToken);
                    return result.Match<IResult>(
                        value => Results.Ok(value.MapToGetOrderResponse()),
                        Problem
                    );
                })
            .RequireAuthorization(AuthConstants.AdminUserPolicyName)
            .Produces<GetOrderResponse>()
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Order")
;
    }

    public static void AddServices(IServiceCollection services, [FromServices]IConfiguration configuration)
    {
    }
}