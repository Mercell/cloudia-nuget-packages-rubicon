using System;
using System.Collections.Generic;

namespace Mercell.Cloudia.AuditLogging
{
    public class AuditLog
    {
        public string AppName { get; set; }
        public string OperationName { get; private set; }
        public string OperationId { get; private set; }
        public long? ObjectId { get; private set; }
        public long? ObjectTypeId { get; private set; }
        public string Status { get; set; }
        public string Country { get; private set; }
        public DateTime Timestamp { get; private set; }
        public Guid? CorrelationId { get; set; }
        public Guid? CausationId { get; set; }
        public Guid? Ssid { get; set; }
        public string Level { get; set; }
        public string Message { get; private set; }
        public IDictionary<string, object> Properties { get; set; }

        public AuditLog(
            string message,
            string appName = null,
            string operationId = null,
            string operationName = null,
            long? objectId = null,
            long? objectTypeId = null,
            string status = null,
            string country = null,
            DateTime? timestamp = null,
            Guid? correlationId = null,
            Guid? causationId = null,
            Guid? ssid = null,
            string level = null,
            IDictionary<string, object> properties = null
        )
        {
            AppName = appName;
            OperationId = operationId;
            OperationName = operationName ?? operationId;
            ObjectId = objectId;
            ObjectTypeId = objectTypeId;
            Status = status;
            Country = country;
            Timestamp = timestamp ?? DateTime.UtcNow;
            CorrelationId = correlationId;
            CausationId = causationId;
            Ssid = ssid;
            Level = level;
            Message = message;
            Properties = properties;
        }
    }
}
