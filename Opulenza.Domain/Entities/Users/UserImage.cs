using Opulenza.Domain.Common;
using File = Opulenza.Domain.Common.File;

namespace Opulenza.Domain.Entities.Users;

public class UserImage:File, IUserOwned
{
    public int UserId { get; set; }
}