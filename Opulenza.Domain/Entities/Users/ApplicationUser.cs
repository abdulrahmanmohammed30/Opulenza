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
        
        public Wishlist? Wishlist { get; set; }
}