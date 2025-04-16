using Opulenza.Domain.Entities.Users;
using ErrorOr;
using Stripe.Checkout;

namespace Opulenza.Application.ServiceContracts;

public interface IPaymentService
{
     Task CreateCustomer(ApplicationUser user);
     Task<string> CreateProduct(Opulenza.Domain.Entities.Products.Product product);
     Task<ErrorOr<string>> CreateSession();

     Task<ErrorOr<Session?>> GetSessionAsync(string sessionId);
}