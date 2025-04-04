using Opulenza.Application.Models;

namespace Opulenza.Application.Common.interfaces;

public interface ICurrentUserProvider
{
     CurrentUser GetCurrentUser();
}