using OtpNet;

namespace AuthenticationService.Services
{
    public class OTPService:IOTPService
    {
        public string GenerateOtp(string secret)
        {
            var otpGenerator = new Totp(Base32Encoding.ToBytes(secret));
            return otpGenerator.ComputeTotp();
        }

        public bool VerifyOtp(string secret, string otp)
        {
            var otpGenerator = new Totp(Base32Encoding.ToBytes(secret));
            return otpGenerator.VerifyTotp(otp, out long timeStepMatched);
        }
    }
}
