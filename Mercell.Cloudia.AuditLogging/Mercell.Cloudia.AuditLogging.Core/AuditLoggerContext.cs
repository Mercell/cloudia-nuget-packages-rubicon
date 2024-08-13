using System;
using System.Threading;

namespace Mercell.Cloudia.AuditLogging
{
    public class AuditLoggerContextProperties
    {
        public Guid CorrelationId { get; set; } = Guid.NewGuid();
        public Guid Ssid { get; set; } = Guid.Empty;
    }

    public class AuditLoggerContext
    {
        private static readonly ThreadLocal<AuditLoggerContextProperties> _properties = new ThreadLocal<AuditLoggerContextProperties>(() => new AuditLoggerContextProperties());
        public static AuditLoggerContextProperties Properties { get { return _properties.Value; } }

        public static void Reset()
        {
            _properties.Value = new AuditLoggerContextProperties();
        }

        public static void Clear()
        {
            // Just call Reset() for now.
            // The idea being that there's 'distinct' methods to start and end the context.
            Reset();
        }
    }
}
