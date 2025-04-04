using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Users.Commands.UpdateUserAddress;

public class UpdateUserAddressCommand: IRequest<ErrorOr<string>>
{
    public string? StreetAddress { get; init; }
    
    public string? Country { get; init; }
    
    public string? City { get; init; }
    
    public  string? ZipCode { get; init; }
}