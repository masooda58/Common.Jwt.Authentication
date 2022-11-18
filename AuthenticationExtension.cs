using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Common.Jwt.Authentication
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection AddOurAuthentication(this IServiceCollection services, JwtSettingModel jwtSetting)
        {
            services.AddAuthentication(options =>
                {
                    //options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })

                // Adding Jwt Bearer
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = jwtSetting.ValidAudience,
                        ValidIssuer = jwtSetting.ValidIssuer,
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.Zero,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Secret))
                    };
                });
            services.AddSingleton(jwtSetting);

            return services;
        }
    }

}
