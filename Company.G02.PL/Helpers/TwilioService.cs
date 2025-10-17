using Company.G02.PL.Helpers.Interface;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Company.G02.PL.Helpers
{
    public class TwilioService(IOptions<TwilioSettings> _options) : ITwilioService
    {
        public MessageResource SendSms(Sms sms)
        {
            // Initialize Connection
            TwilioClient.Init(_options.Value.AccountID, _options.Value.AuthToken);

            // Message
            var message = MessageResource.Create
                (
                body: sms.Body,
                to: sms.To,
                from: _options.Value.PhoneNumber                
                );

            // Return Message
            return message;

        }
    }
}
