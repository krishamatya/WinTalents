
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace AuthenticationService.Services
{

    public class SmsService:ISmsService
    {
        private readonly string accountSid = "your_account_sid"; // Twilio SID
        private readonly string authToken = "your_auth_token"; // Twilio Auth Token

        public void SendSms(string toPhoneNumber, string message)
        {
            TwilioClient.Init(accountSid, authToken);

            var messageResource = MessageResource.Create(
                body: message,
                from: new PhoneNumber("your_twilio_phone_number"), // Your Twilio phone number
                to: new PhoneNumber(toPhoneNumber)
            );
        }
    }


}
