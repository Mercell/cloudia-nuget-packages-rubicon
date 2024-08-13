using System.Threading.Tasks;

namespace Mercell.Cloudia.AuditLogging
{
    public interface IAuditLogger
    {
#if NET461_OR_GREATER
        void Send(AuditLog log);
        
        Task SendAsync(AuditLog log);
#elif NETSTANDARD2_0_OR_GREATER
        Task SendAsync(AuditLog log);
#endif
    }
}
