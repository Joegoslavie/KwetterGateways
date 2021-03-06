using Kwetter.Business.Manager;
using Kwetter.Business.Seed;
using Kwetter.DataAccess;
using Kwetter.DataAccess.Service;
using Kwetter.UserGateway.Attribute;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Kwetter.UserGateway
{
    public class Startup
    {
        readonly string corsPolicyName = "kwetterCorsPolicy";
        private readonly bool runSeeder = false;
        private readonly int usersToSeed = 10;

        /// <summary>
        /// Configuration instance.
        /// </summary>
        private readonly IConfiguration config;
        public Startup(IConfiguration configuration)
        {
            this.config = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            
            services.AddSingleton<AppSettings>();
            services.AddTransient<AuthenticationService>();
            services.AddTransient<DataAccess.Service.FollowingService>();
            services.AddTransient<ProfileService>();
            services.AddTransient<TweetService>();

            services.AddTransient<AccountManager>();
            services.AddTransient<AuthenticationManager>();
            services.AddTransient<TweetManager>();
            services.AddTransient<ProfileManager>();
            services.AddTransient<FollowManager>();
            services.AddTransient<KwetterSeeder>();
            services.AddAuthentication(options =>
            {
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
                    ValidAudience = this.config["JWT:ValidAudience"],
                    ValidIssuer = this.config["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config["JWT:Secret"]))
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kwetter Gateway v1"));

                try
                {
                    if (this.runSeeder)
                    {
                        using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                        {
                            var seeder = serviceScope.ServiceProvider.GetService<KwetterSeeder>();
                            Task.Run(async () =>
                            {
                                await seeder.RunAll(this.usersToSeed).ConfigureAwait(false);
                            });
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    // oops...
                }
            }

            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthorization();
            app.UseMiddleware<JwtMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
