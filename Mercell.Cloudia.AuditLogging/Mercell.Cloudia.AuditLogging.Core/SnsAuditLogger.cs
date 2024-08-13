using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Mercell.Cloudia.AuditLogging
{
    public class SnsAuditLogger : IAuditLogger
    {
        private readonly string _appName;
        private readonly string _region;
        private readonly string _topicArn;
        private readonly string _messageGroupId;
        private readonly string _profile;
        private readonly IAmazonSimpleNotificationService _client;

        public SnsAuditLogger(string appName, string region, string topicArn, string messageGroupId, string profile = null)
        {
            _appName = appName;
            _region = region;
            _topicArn = topicArn;
            _messageGroupId = messageGroupId;
            _profile = profile;

            var credentials = DetermineCredentials(_profile);
            _client = new AmazonSimpleNotificationServiceClient(credentials, Amazon.RegionEndpoint.GetBySystemName(_region));
        }

        private static AWSCredentials DetermineCredentials(string profile)
        {
            if (!string.IsNullOrEmpty(profile))
            {
                var credentialProfileStore = new CredentialProfileStoreChain();
                if (credentialProfileStore.TryGetAWSCredentials(profile, out var credentials))
                {
                    return credentials;
                }
            }
            return FallbackCredentialsFactory.GetCredentials();
        }

        private string FormatMessage(AuditLog log, AuditLoggerContextProperties properties)
        {
            log.AppName = log.AppName ?? _appName;
            log.CorrelationId = properties.CorrelationId;
            log.Ssid = properties.Ssid;
            return JsonConvert.SerializeObject(log);
        }

#if NET461_OR_GREATER
        public void Send(AuditLog log)
        {
            var message = FormatMessage(log, AuditLoggerContext.Properties);
            _client.Publish(new PublishRequest()
            {
                TopicArn = _topicArn,
                Message = message,
                MessageGroupId = _messageGroupId,
            });
        }

        public async Task SendAsync(AuditLog log)
        {
            var message = FormatMessage(log, AuditLoggerContext.Properties);
            await _client.PublishAsync(new PublishRequest()
            {
                TopicArn = _topicArn,
                Message = message,
                MessageGroupId = _messageGroupId,
            });
        }
#elif NETSTANDARD2_0_OR_GREATER
        public async Task SendAsync(AuditLog log)
        {
            var message = FormatMessage(log, AuditLoggerContext.Properties);
            await _client.PublishAsync(new PublishRequest()
            {
                TopicArn = _topicArn,
                Message = message,
                MessageGroupId = _messageGroupId,
            });
        }
#endif
    }
}
