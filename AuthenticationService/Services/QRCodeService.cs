using AuthenticationService.Model;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using System.Text;
using ZXing;
namespace AuthenticationService.Services
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public class QRCodeService : IQRCodeService
    {
        private readonly AuthenticationDBContext _context;
        public QRCodeService(AuthenticationDBContext context) { 
            _context = context;
        }
        public int RegisterUser(UserRegistrationModel model) {
            _context.UserRegistration.Add(model);
            _context.SaveChanges();
            return model.Id;
        }
        public string GenerateQRCode(string data)
        {
            #region QRCode
            //QRCodeGenerator qrGenerator = new QRCodeGenerator();
            //QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            //QRCode qrCode = new QRCode(qrCodeData);

            //using (Bitmap bitMap = qrCode.GetGraphic(20))
            //{
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        bitMap.Save(ms, ImageFormat.Png);
            //        byte[] byteImage = ms.ToArray();
            //        string base64Image = Convert.ToBase64String(byteImage);
            //        return base64Image;
            //    }
            //}
            #endregion

            BarcodeWriter barcodeWriter = new BarcodeWriter
            {
                // Choose the barcode format you prefer (e.g., CODE_128, EAN_13, etc.)
                Format = BarcodeFormat.CODE_128, // You can change this to other formats like QR_CODE, EAN_13, etc.
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 300, // Barcode width
                    Height = 150 // Barcode height
                }
            };

            // Generate the barcode from the combined string 
            var barcodeBitmap = barcodeWriter.Write(data);
            using (var ms = new System.IO.MemoryStream())
            {
                barcodeBitmap.Save(ms,ImageFormat.Png);
                byte[] byteImage = ms.ToArray();
                string base64Image = Convert.ToBase64String(byteImage);
                return base64Image;
               
            }
            
        }
        public QRLoginModel VerifyQRCode(string scannedData)
        {
            //// Here you would check if the scannedData matches a user session or data stored
            //return scannedData == userName; // For example, we compare scannedData with userId

            var barcodeReader = new BarcodeReader();
            barcodeReader.Options.PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE, BarcodeFormat.CODE_128 };
            using (var bitmap = new Bitmap(scannedData))
            {
                var result = barcodeReader.Decode(bitmap);

                return _context.QRLogin.FirstOrDefault(x=>x.UniqueKey == result.Text);
            }
        }
        public static string GenerateUserSecret(string username, string phoneNumber)
        {
            string key = GenerateSecretKey(32);
            // Combine username and phone number into a single string
            string inputString = username + phoneNumber;

            // Convert the input string into bytes
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputString);

            // Create a new instance of HMACSHA256 using the secret key
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                // Compute the hash of the combined input
                byte[] hashBytes = hmac.ComputeHash(inputBytes);

                // Convert the hash bytes to a hexadecimal string
                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }
       private static string GenerateSecretKey(int keyLength = 32)
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                // Create an array of bytes with the desired length
                byte[] secretKeyBytes = new byte[keyLength];
                rng.GetBytes(secretKeyBytes); // Fill the array with cryptographically secure random bytes

                // Convert the byte array to a Base64 string (or Hex string, depending on your needs)
                return Convert.ToBase64String(secretKeyBytes);
            }
        }
    }
}
        
    

    

