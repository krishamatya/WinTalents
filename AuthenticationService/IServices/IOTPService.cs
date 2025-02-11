
namespace AuthenticationService.Services
{

    public interface IOTPService
    {
        string GenerateOtp(string secret);


        bool VerifyOtp(string secret, string otp);
        
    }
}
