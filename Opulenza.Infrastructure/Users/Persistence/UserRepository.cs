using Microsoft.EntityFrameworkCore;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Features.Common;
using Opulenza.Application.Features.Users.Queries.GetUsers;
using Opulenza.Domain.Entities.Users;
using Opulenza.Infrastructure.Common.Persistence;

namespace Opulenza.Infrastructure.Users.Persistence;

public class UserRepository(AppDbContext context) : Repository<ApplicationUser>(context), IUserRepository
{
    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
    {
        return context.Users.AnyAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<GetUsersResult> GetUsersAsync(GetUsersQuery request, CancellationToken cancellationToken)
    {
        IQueryable<ApplicationUser> query = context.Users;

        if (string.IsNullOrWhiteSpace(request.FirstName) == false)
        {
            query = query.Where(x => EF.Functions.Like(x.FirstName, $"%{request.FirstName}%"));
        }

        if (string.IsNullOrWhiteSpace(request.LastName) == false)
        {
            query = query.Where(x => EF.Functions.Like(x.FirstName, $"%{request.LastName}%"));
        }

        if (string.IsNullOrWhiteSpace(request.Username) == false)
        {
            query = query.Where(x => EF.Functions.Like(x.UserName, $"%{request.Username}%"));
        }

        if (string.IsNullOrWhiteSpace(request.Email) == false)
        {
            query = query.Where(x => x.Email != null && x.Email.ToLower() == request.Email.ToLower());
        }

        if (string.IsNullOrWhiteSpace(request.Country) == false)
        {
            query = query.Where(x => x.Address != null && x.Address.Country.ToLower() == request.Country.ToLower());
        }

        if (string.IsNullOrWhiteSpace(request.City) == false)
        {
            query = query.Where(x => x.Address != null && x.Address.City.ToLower() == request.City.ToLower());
        }

        if (request.Joined != null)
        {
            query = query.Where(x => x.CreatedAt == request.Joined);
        }

        if (request.SortOptions != SortOptions.None && request.SortBy != GetUsersSortBy.None)
        {
            query = request.SortBy switch
            {
                GetUsersSortBy.Email => request.SortOptions == SortOptions.Asc
                    ? query.OrderBy(x => x.Email)
                    : query.OrderByDescending(x => x.Email),

                GetUsersSortBy.FirstName => request.SortOptions == SortOptions.Asc
                    ? query.OrderBy(x => x.FirstName)
                    : query.OrderByDescending(x => x.FirstName),

                GetUsersSortBy.LastName => request.SortOptions == SortOptions.Asc
                    ? query.OrderBy(x => x.LastName)
                    : query.OrderByDescending(x => x.LastName),

                GetUsersSortBy.Username => request.SortOptions == SortOptions.Asc
                    ? query.OrderBy(x => x.UserName)
                    : query.OrderByDescending(x => x.UserName),
                _ => query.OrderBy(x => x.CreatedAt)
            };
        }

        query = query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);


        var totalCount = await query.CountAsync(cancellationToken);

        var users = await query.Select(x => new GetUserResult()
        {
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Username = x.UserName,
            Email = x.Email,
            Joined = x.CreatedAt,
            Address = x.Address == null
                ? null
                : new GetUserAddressResult()
                {
                    City = x.Address.City,
                    Country = x.Address.Country,
                    StreetAddress = x.Address.StreetAddress,
                    ZipCode = x.Address.ZipCode
                },
            ImageUrl = x.Image == null ? null : x.Image.FilePath,
        }).ToListAsync(cancellationToken);


        return new GetUsersResult()
        {
            Users = users,
            TotalCount = totalCount
        };
    }
}