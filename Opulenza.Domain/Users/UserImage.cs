using Opulenza.Domain.Identity;

namespace Opulenza.Domain.Users;

public class UserImage:File
{
    public int UserId { get; set; }
}