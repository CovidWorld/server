using System.Threading;
using System.Threading.Tasks;
using SmsGate.Opis.Minv;
using Sygic.Corona.Domain.Common;

namespace Sygic.Corona.Infrastructure.Services.SmsMessaging
{
    public class MinvSmsMessagingService : ISmsMessagingService
    {
        private readonly SmsExtClient smsService;

        public MinvSmsMessagingService(SmsExtClient smsService)
        {
            this.smsService = smsService;
        }
        public async Task SendMessageAsync(string messageText, string phoneNumber, CancellationToken cancellationToken)
        {
            await smsService.OpenAsync();
            var response = await smsService.SendMessageAsync(phoneNumber, messageText);
            await smsService.CloseAsync();

            if (response.ResultStatus != SmsExtRet.ResultTypeEnum.OK)
            {
                throw new DomainException("Error during sending sms.");
            }
        }
    }
}
