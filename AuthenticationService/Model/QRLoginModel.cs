namespace AuthenticationService.Model
{
    public class QRLoginModel
    {
        public string UniqueKey { get; set; }
        public string UserName { get; set; }
        public string ScannedData { get; set; }
        public string UserPhoneNumber { get; set; }
    }
}
