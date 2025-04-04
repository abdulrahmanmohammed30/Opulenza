using Opulenza.Application.Features.Authentication.Queries.Login;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.ServiceContracts;

public interface IAuthenticationService
{
    string GetRefreshToken();
    Task<TokenResult> GenerateJwt(ApplicationUser? user);
}