using ErrorOr;
using Opulenza.Application.Models;

namespace Opulenza.Application.ServiceContracts;

public interface IEmailService
{
    Task<ErrorOr<string>> SendEmailAsync(Email email);
}