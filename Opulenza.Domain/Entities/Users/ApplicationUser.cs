using Microsoft.AspNetCore.Identity;
using Opulenza.Domain.Entities.Carts;
using Opulenza.Domain.Entities.Wishlists;

namespace Opulenza.Domain.Entities.Users;

public class ApplicationUser: IdentityUser<int>
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
}