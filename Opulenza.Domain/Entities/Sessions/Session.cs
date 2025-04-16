using Opulenza.Domain.Common;

namespace Opulenza.Domain.Entities.Sessions;

public class Session:BaseEntity
{ 
    public required string SessionId { get; set; }
}