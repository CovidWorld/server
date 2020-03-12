using System.Threading;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Sygic.Corona.Infrastructure.Services.SmsMessaging
{
    public class SmsMessagingService : ISmsMessagingService
    {
        private readonly string accountSid;
        private readonly string authToken;

        public SmsMessagingService(string accountSid, string authToken)
        {
            this.accountSid = accountSid;
            this.authToken = authToken;
        }
        public async Task SendMessageAsync(string messageText, string phoneNumber, CancellationToken cancellationToken)
        {
            TwilioClient.Init(accountSid, authToken);

            var message = await MessageResource.CreateAsync(
                body: messageText,
                from: new Twilio.Types.PhoneNumber("TODO"),
                to: new Twilio.Types.PhoneNumber(phoneNumber)
            );
        }
    }
}
