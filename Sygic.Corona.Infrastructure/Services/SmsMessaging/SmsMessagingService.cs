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
        private readonly string senderPhoneNumber;

        public SmsMessagingService(string accountSid, string authToken, string senderPhoneNumber)
        {
            this.accountSid = accountSid;
            this.authToken = authToken;
            this.senderPhoneNumber = senderPhoneNumber;
        }
        public async Task SendMessageAsync(string messageText, string phoneNumber, CancellationToken cancellationToken)
        {
            TwilioClient.Init(accountSid, authToken);

            var message = await MessageResource.CreateAsync(
                body: messageText,
                from: new Twilio.Types.PhoneNumber(senderPhoneNumber),
                to: new Twilio.Types.PhoneNumber(phoneNumber)
            );
        }
    }
}
