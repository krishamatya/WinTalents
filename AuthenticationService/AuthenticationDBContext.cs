using AuthenticationService.Model;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AuthenticationService
{
    public class AuthenticationDBContext : DbContext{
        public AuthenticationDBContext(DbContextOptions<AuthenticationDBContext> options) { }
        public DbSet<QRLoginModel> QRLogin { get; set; }
        public DbSet<UserRegistrationModel> UserRegistration { get; set; }

        public DbSet<VerifyOtpModel> VerifyOtp { get; set; }
    }
}
