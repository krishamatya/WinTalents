using AuthenticationService;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using AuthenticationService.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCors(options => {
    options.AddPolicy("CORSPolicy", builder => builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed((hosts) => true));
});
builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection("JwtSetting"));
var jwtSetting = builder.Configuration.GetSection("JwtSetting").Get<JwtSetting>();
builder.Services.AddDbContext<AuthenticationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IMSConnection")));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddControllers();
builder.Services.AddTransient<IQRCodeService, QRCodeService>();
builder.Services.AddTransient<IOTPService, OTPService>();
builder.Services.AddTransient<ISmsService, SmsService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseCors("CORSPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.Run();


