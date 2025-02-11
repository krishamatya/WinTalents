using System.ComponentModel.DataAnnotations;
using Twilio.Rest.Video.V1.Room.Participant;

namespace AuthenticationService.Model
{
    public class UserRegistrationModel
    {
        [Key]
        public int Id { get; set; }
        public string UniqueKey { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
