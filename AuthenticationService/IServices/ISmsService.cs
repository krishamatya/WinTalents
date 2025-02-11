namespace AuthenticationService.Services
{

    public interface ISmsService
    {
        void SendSms(string toPhoneNumber, string message);
        
    }


}
