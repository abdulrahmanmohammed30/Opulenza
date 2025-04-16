using Opulenza.Domain.Entities.Sessions;

namespace Opulenza.Application.Common.interfaces;

public interface ISessionRepository: IRepository<Session>
{
    Task<bool> ExistsAsync(string sessionId);
}