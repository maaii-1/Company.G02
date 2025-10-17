using Twilio.Rest.Api.V2010.Account;

namespace Company.G02.PL.Helpers.Interface
{
    public interface ITwilioService
    {
        public MessageResource SendSms(Sms sms);
    }
}
