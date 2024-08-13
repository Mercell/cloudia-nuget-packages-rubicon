using System;
using System.Threading;

namespace Mercell.Cloudia.AuditLogging
{
    public class AuditLoggerContextProperties
    {
        public Guid CorrelationId { get; set; } = Guid.NewGuid();
        public Guid Ssid { get; set; } = Guid.Empty;
    }

    /// <summary>
    /// Context class for the audit logging.
    /// A place to store thread specific properties for audit logging.
    /// * Correlation id
    /// * Ssid
    /// </summary>
    public class AuditLoggerContext
    {
        private static readonly ThreadLocal<AuditLoggerContextProperties> _properties = new ThreadLocal<AuditLoggerContextProperties>(() => new AuditLoggerContextProperties());
        public static AuditLoggerContextProperties Properties { get { return _properties.Value; } }

        /// <summary>
        /// Initialize the context for the audit logging.
        /// Context stores properties such as correlation id and ssid.
        /// 
        /// Initialization sets the default values
        /// * Correlation id to new Guid value.
        /// * Ssid to empty Guid value (all zeroes).
        /// </summary>
        public static void Initialize()
        {
            _properties.Value = new AuditLoggerContextProperties();
        }

        /// <summary>
        /// Clear the current audit logging context.
        /// Context stores properties such as correlation id and ssid.
        /// 
        /// Initialization sets the default values
        /// * Correlation id to new Guid value.
        /// * Ssid to empty Guid value (all zeroes).
        /// </summary>
        public static void Clear()
        {
            // Just call Initialize() for now.
            // The idea being that there's 'distinct' methods to start and end the context.
            Initialize();
        }
    }
}
