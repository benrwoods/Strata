using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Strata.API.Security;
using Strata.Data;
using Strata.Interfaces.Messaging;
using Strata.Interfaces.Security;
using Strata.Interfaces.Shopping;
using Strata.Services.Messaging;
using Strata.Services.Security;
using Strata.Services.Shopping;
using System;
using System.Security.Claims;
using System.Text;

namespace Strata.API {
    public class Startup {

        //Use X509 certificate for better security
        private const string SecurityKey = "SecurePassword1234";
        private const string Audience = "User";
        private const string Issuer = "Strata";


        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            services.AddDbContext<ShoppingCartContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ShoppingCartContext")));

            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IMessagingService, MessagingService>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddMvc();

            services.AddAuthorization(x => x.AddPolicy("LoggedIn", policy => policy.RequireAuthenticatedUser()));

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecurityKey));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options => {
                options.Issuer = Issuer;
                options.Audience = Audience;
                options.SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidIssuer = Issuer,

                        ValidateAudience = true,
                        ValidAudience = Audience,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = securityKey,

                        RequireExpirationTime = true,
                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.Zero,
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
