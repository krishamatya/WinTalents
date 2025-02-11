using AuthenticationService.Model;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IQRCodeService _qrCodeService;
        private readonly IOTPService _otpService;
        private readonly ISmsService _smsService;
        private readonly IMemoryCache _cache;
        
        public AuthenticationController(IQRCodeService qrCodeService, IOTPService otpService, ISmsService smsService, IMemoryCache cache)
        {
            _qrCodeService = qrCodeService;
            _otpService = otpService;
            _smsService = smsService;
            _cache = cache;
        }
        [HttpPost("register")]
        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        public IActionResult Register([FromBody] UserRegistrationModel model)
        {
            
            model.UniqueKey  = QRCodeService.GenerateUserSecret(model.UserName, model.PhoneNumber);
            int userId = _qrCodeService.RegisterUser(model);
            //after success generate BarCode 
            string qrData = $"UniqueKey: {model.UniqueKey}, UserId: {userId}";
            string qrCodeImage = _qrCodeService.GenerateQRCode(qrData);
            // Let the user save the QR.

            return Ok(new { qrCode = qrCodeImage, userId });
        }
        [HttpPost("login/qr")]
       
        public IActionResult LoginWithQRCode([FromBody] QRLoginModel model)
        {
           QRLoginModel  data = _qrCodeService.VerifyQRCode(model.ScannedData);

            if (data !=null)
            {//token addition and get user details.
                var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, model.UserName)
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "your-issuer",
                    audience: "your-audience",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });


                #region Commented For Future Use 
                /*
                        string? key = QRCodeService.GenerateUserSecret(model.UserName, model.UserPhoneNumber);
                        // Generate OTP and send to user's phone number
                        string otp = _otpService.GenerateOtp(key);
                        SaveKey(model.UserPhoneNumber, key);
                        // store this key somewhere
                        _smsService.SendSms(model.UserPhoneNumber, $"Your OTP code is: {otp}");

                        return Ok("OTP sent.");
                */
                #endregion

            }

            return Unauthorized("Invalid QR Code.");
        }
        
        #region Commented For Future Use 
        /*
        public void SaveKey(string phonenumber, string key)
        {
            var options = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5)); // OTP expires in 5 minutes

            _cache.Set(phonenumber, key, options);
        }
        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] VerifyOtpModel model)
        {
            if (_cache.TryGetValue(model.UserPhoneNumber, out string secretKey))
            {

                //pass the stored key
                bool isVerified = _otpService.VerifyOtp(secretKey, model.Otp);

                if (isVerified)
                {

                    return Ok("Login successful.");
                }
            }

            return Unauthorized("Invalid OTP.");
        }*/
        #endregion
    }
}
