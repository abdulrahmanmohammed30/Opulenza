using Opulenza.Application.Features.Authentication.Commands.RefreshToken;
using Opulenza.Application.Features.Authentication.Commands.RequestResetPassword;
using Opulenza.Application.Features.Authentication.Commands.ResetPassword;
using Opulenza.Application.Features.Authentication.LoginWithGitHubCallback;
using Opulenza.Application.Features.Authentication.Queries.Login;
using Opulenza.Application.Features.Products.Commands.AddProduct;
using Opulenza.Application.Features.Products.Queries.Common;
using Opulenza.Application.Features.Users.Commands.ChangeUserPassword;
using Opulenza.Application.Features.Users.Commands.CreateUser;
using Opulenza.Application.Features.Users.Commands.UpdateUserAddress;
using Opulenza.Application.Features.Users.Commands.UploadImage;
using Opulenza.Application.Features.Users.Queries.GetUser;
using Opulenza.Contracts.Auth;
using Opulenza.Contracts.Products;
using Opulenza.Contracts.Users;
using LoginRequest = Opulenza.Contracts.Auth.LoginRequest;
using RegisterRequest = Opulenza.Contracts.Auth.RegisterRequest;

namespace Opulenza.Api.Mapping;

public static class ContractMapping
{
    public static CreateUserCommand MapToRegisterUserCommand
        (this RegisterRequest registerRequest)
    {
        return new CreateUserCommand()
        {
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            Username = registerRequest.Username,
            Email = registerRequest.Email,
            Password = registerRequest.Password
        };
    }

    public static LoginQuery MapToLoginQuery
        (this LoginRequest loginRequest)
    {
        return new LoginQuery()
        {
            Username = loginRequest.Username,
            Password = loginRequest.Password
        };
    }

    public static LoginResponse MapToLoginResponse(this LoginResult loginResult)
    {
        return new LoginResponse()
        {
            Token = loginResult.Token,
            Expiration = loginResult.Expiration,
            RefreshToken = loginResult.RefreshToken
        };
    }

    public static RefreshTokenCommand MapToRefreshTokenCommand(this RefreshTokenRequest refreshTokenRequest)
    {
        return new RefreshTokenCommand()
        {
            Token = refreshTokenRequest.Token,
            RefreshToken = refreshTokenRequest.RefreshToken
        };
    }

    public static RefreshTokenResponse MapToRefreshTokenResponse(this TokenResult tokenResult)
    {
        return new RefreshTokenResponse()
        {
            Token = tokenResult.Token,
            Expiration = tokenResult.Expiration,
            RefreshToken = tokenResult.RefreshToken
        };
    }

    public static UploadImageResponse MapToImageResponse(this UploadUserImageResult uploadUserImageResult)
    {
        return new UploadImageResponse()
        {
            ImageUrl = uploadUserImageResult.ImageUrl,
        };
    }

    public static ChangeUserPasswordCommand MapToChangePasswordCommand(this ChangePasswordRequest changePasswordRequest)
    {
        return new ChangeUserPasswordCommand()
        {
            NewPassword = changePasswordRequest.NewPassword,
            OldPassword = changePasswordRequest.OldPassword,
        };
    }

    public static UpdateUserAddressCommand MapToUserAddressCommand(
        this UpdateUserAddressRequest updateUserAddressRequest)
    {
        return new UpdateUserAddressCommand()
        {
            StreetAddress = updateUserAddressRequest.StreetAddress,
            City = updateUserAddressRequest.City,
            Country = updateUserAddressRequest.Country,
            ZipCode = updateUserAddressRequest.ZipCode,
        };
    }

    public static UserResponse MapToGetUserQuery(this GetUserQueryResult getUserRequest)
    {
        return new UserResponse()
        {
            Username = getUserRequest.Username,
            FirstName = getUserRequest.FirstName,
            ImageUrl = getUserRequest.ImageUrl,
            LastName = getUserRequest.LastName,
            Address = getUserRequest.Address?.MapToUserAddressResponse(),
        };
    }

    public static UserAddressResponse MapToUserAddressResponse(this GetUserAddressQueryResult userAddress)
    {
        return new UserAddressResponse()
        {
            StreetAddress = userAddress.StreetAddress,
            City = userAddress.City,
            Country = userAddress.Country,
            ZipCode = userAddress.ZipCode,
        };
    }

    public static RequestResetPasswordCommand MapToRequestResetPasswordCommand(
        this SendResetPasswordRequest resetPasswordCommand)
    {
        return new RequestResetPasswordCommand()
        {
            Email = resetPasswordCommand.Email
        };
    }

    public static ResetPasswordCommand MapToResetPasswordCommand(
        this Opulenza.Contracts.Auth.ResetPasswordRequest resetPasswordRequest)
    {
        return new ResetPasswordCommand()
        {
            Token = resetPasswordRequest.Token,
            Password = resetPasswordRequest.Password,
            Email = resetPasswordRequest.Email
        };
    }

    public static ExternalLoginResponse MapToExternalLoginResponse(this ExternalLoginResult externalLoginResult)
    {
        return new ExternalLoginResponse()
        {
            Token = externalLoginResult.Token,
            Expiration = externalLoginResult.Expiration,
            RefreshToken = externalLoginResult.RefreshToken,
            ReturnUrl = externalLoginResult.ReturnUrl
        };
    }

    public static AddProductCommand MapToAddProductCommand(this AddProductRequest addProductRequest)
    {
        return new AddProductCommand()
        {
            Name = addProductRequest.Name,
            Description = addProductRequest.Description,
            Price = addProductRequest.Price,
            DiscountPrice = addProductRequest.DiscountPrice,
            Tax = addProductRequest.Tax,
            TaxIncluded = addProductRequest.TaxIncluded,
            Brand = addProductRequest.Brand,
            StockQuantity = addProductRequest.StockQuantity,
            IsAvailable = addProductRequest.IsAvailable,
            Categories = addProductRequest.Categories,
        };
    }

    public static ProductResponse MapToProductResponse(this ProductResult p)
    {
        return new ProductResponse()
        {
            Id = p.Id,
            Slug = p.Slug,
            Name = p.Name,
            Description = p.Description,
            Brand = p.Brand,
            Tax = p.Tax,
            TaxIncluded = p.TaxIncluded,
            Price = p.Price,
            DiscountPrice = p.DiscountPrice,
            IsAvailable = p.IsAvailable,
            StockQuantity = p.StockQuantity,
            Images = p.Images?.Select(i => new ImageResponse()
            {
                Id = i.Id,
                FilePath = i.FilePath,
                IsFeaturedImage = i.IsFeaturedImage,
            }).ToList(),
            Categories = p.Categories?.Select(c => new CategoryResponse()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Slug = c.Slug,
                ParentId = c.ParentId
            }).ToList(),
            Ratings = p.Ratings?.Select(r => new RatingResponse()
            {
                Value = r.Value,
                UserId = r.UserId,
                ReviewText = r.ReviewText,
            }).ToList()
        };
    }
}