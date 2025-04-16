using Microsoft.EntityFrameworkCore;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Sessions;
using Opulenza.Infrastructure.Common.Persistence;

namespace Opulenza.Infrastructure.Sessions;

public class SessionRepository(AppDbContext context): Repository<Session>(context), ISessionRepository
{
    public async Task<bool> ExistsAsync(string sessionId)
    {
        return await context.Sessions.AnyAsync(s=>s.SessionId == sessionId);
    }
}