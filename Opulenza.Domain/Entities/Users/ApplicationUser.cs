using Microsoft.AspNetCore.Identity;
using Opulenza.Domain.Entities.Carts;
using Opulenza.Domain.Entities.Ratings;
using Opulenza.Domain.Entities.Wishlists;

namespace Opulenza.Domain.Entities.Users;

public class ApplicationUser: IdentityUser<int>, IEntity
{
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        
        public UserAddress? Address { get; set; }
        
        public UserImage? Image { get; set; }
        
        public Cart? Cart { get; set; }
        
        public List<WishListItem>? WishListItems{ get; set; }
        
        public string? RefreshToken { get; set; }
        
        public DateTime RefreshTokenExpiry { get; set; }
        
        public bool IsDeleted { get; set; } 
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
        public List<Rating>? Ratings { get; set; }
        
        
        // BlockedAt BlockedUntil 
        // BlockedAt is null  -> user is not blocked 
        // BlockedAt is not null but BlockedUntil is null -> is is blocked permanently
        // BlockedAt is not null & BlockedUntil is not null -> blocked for a particular period of time 
        // Consequences? User won't be able to use the app if the user was blocked 
        // When log in, return a message informing the frontend that the user is blocked 
        public DateTime? BlockedAt { get; set; }
        
        public DateTime? BlockedUntil { get; set; }
        
        public string? BlockedReason { get; set; }
}
