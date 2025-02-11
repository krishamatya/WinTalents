
using AuthenticationService.Model;

namespace AuthenticationService.Services
{
  public interface IQRCodeService
    {
        string GenerateQRCode(string data);
        QRLoginModel VerifyQRCode(string scannedData);
        int RegisterUser(UserRegistrationModel model);


    }
}
