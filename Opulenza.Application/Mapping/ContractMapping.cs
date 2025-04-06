using Opulenza.Application.Features.Products.Commands.AddProduct;
using Opulenza.Application.Features.Users.Commands.Create;
using Opulenza.Application.Features.Users.Commands.CreateUser;
using Opulenza.Application.Features.Users.Queries.GetUser;
using Opulenza.Domain.Entities.Products;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Mapping;

public static class ContractMapping
{
    public static ApplicationUser MapToApplicationUser(this CreateUserCommand createUserCommand)
    {
        return new ApplicationUser()
        {
            FirstName = createUserCommand.FirstName!,
            LastName = createUserCommand.LastName!,
            UserName = createUserCommand.Username,
            Email = createUserCommand.Email
        };
    }

    public static GetUserQueryResult MapToGetUserResult(this ApplicationUser applicationUser)
    {
        return new GetUserQueryResult()
        {
            Username = applicationUser.UserName,
            FirstName = applicationUser.FirstName,
            LastName = applicationUser.LastName,
            ImageUrl = applicationUser.Image?.FilePath,
            Address = applicationUser.Address?.MapToGetAddressUserResult()
        };
    }

    public static GetUserAddressQueryResult MapToGetAddressUserResult(this UserAddress userAddress)
    {
        return new GetUserAddressQueryResult()
        {
            City = userAddress.City,
            Country = userAddress.Country,
            StreetAddress = userAddress.StreetAddress,
            ZipCode = userAddress.ZipCode,
        };
    }
}
