using File = Opulenza.Domain.Common.File;

namespace Opulenza.Domain.Entities.Users;

public class UserImage:File
{
    public int UserId { get; set; }
}