using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Opulenza.Api.Mapping;
using Opulenza.Application.Features.Orders.Commands._FulfillCheckout;
using Opulenza.Application.Features.Orders.Queries.GetOrder;
using Opulenza.Application.ServiceContracts;
using Opulenza.Contracts.Orders;
using Stripe;
using Stripe.BillingPortal;

namespace Opulenza.Api.Controllers;

[ApiVersion("1.0")]
public class OrdersController(
    ISender mediator,
    IPaymentService paymentService,
    IConfiguration configuration,
    ILogger<OrdersController> logger) : CustomController
{
    [Authorize]
    [HttpPost]
    [Route(ApiEndpoints.Orders.Checkout)]
    public async Task<IActionResult> Checkout()
    {
        var sessionUrl = await paymentService.CreateSession();

        if (sessionUrl.IsError)
        {
            return Problem(sessionUrl.Errors);
        }

        // Response.Headers.Append("Location", sessionUrl.Value);
        // return new StatusCodeResult(303);
        return Ok(new { sessionUrl.Value });
    }

    [HttpPost]
    [Route(ApiEndpoints.Orders.Webhook)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Webhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        try
        {
            var m = configuration["Stripe:SecretKey"];
            var d = HttpContext.Request.Headers["Stripe-Signature"];
            var g = configuration["Stripe:SigningSecret"];
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                HttpContext.Request.Headers["Stripe-Signature"],
                configuration["Stripe:SigningSecret"]
            );
            switch (stripeEvent.Type)
            {
                case EventTypes.CheckoutSessionCompleted:
                case EventTypes.CheckoutSessionAsyncPaymentSucceeded:
                {
                    var session = stripeEvent.Data.Object as Session;
                    if (session == null)
                    {
                        return Problem(statusCode: 400, detail: "Invalid session");
                    }

                    var command = new FulfillCheckoutCommand()
                    {
                        SessionId = session.Id
                    };

                    var result = await mediator.Send(command);
                    return result.Match(Ok, Problem);
                }
                case EventTypes.CheckoutSessionAsyncPaymentFailed:
                    break;
            }
        }
        catch (StripeException ex)
        {
            logger.LogError("Invalid checkout session");
            return BadRequest("Invalid Checkout session");
        }

        return NoContent();
    }

    //[Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpGet]
    [ProducesResponseType(typeof(GetOrdersResponse), StatusCodes.Status200OK)]
    [Route(ApiEndpoints.Orders.GetOrders)]
    public async Task<IActionResult> GetOrders([FromQuery] GetOrdersRequest request)
    {
        var command = request.MapToGetOrdersQuery();
        var result = await mediator.Send(command);
        return result.Match(value => Ok(value.MapToGetOrdersResponse()), Problem);
    }

    //[Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpGet]
    [ProducesResponseType(typeof(GetOrderResponse), StatusCodes.Status200OK)]
    [Route(ApiEndpoints.Orders.GetOrder)]
    public async Task<IActionResult> GetOrder([FromRoute] int id)
    {
        var command = new GetOrderQuery()
        {
            Id = id
        };
        var result = await mediator.Send(command);
        return result.Match(value => Ok(value.MapToGetOrderResponse()), Problem);
    }
}