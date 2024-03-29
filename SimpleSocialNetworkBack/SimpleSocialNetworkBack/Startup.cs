using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoMapper;
using Business;
using Business.Common;
using Business.Services;
using Business.Services.Implementations;
using DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SimpleSocialNetworkBack.Misc;

namespace SimpleSocialNetworkBack
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opt => { opt.Filters.Add<CustomExceptionFilter>(); });
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<AddRequiredHeaderParameter>();

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddDbContext<SocialDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("Default"))
                   .UseLazyLoadingProxies();
            });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IOpMessageService, OpMessageService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();

            services.AddHttpContextAccessor();
            services.AddTransient(opt => new TypedClaimsPrincipal(opt.GetService<IHttpContextAccessor>()
                                                                     ?.HttpContext.User
                                                                  ?? throw new NotImplementedException(
                                                                      "Isn't reachable, i hope")));

            services.AddScoped<IMapper>(x =>
            {
                var myProfile = new MapperProfile();
                var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

                return new Mapper(configuration);
            });

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(opt =>
                    {
                        opt.RequireHttpsMetadata = false;
                        opt.SaveToken = true;
                        opt.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                                        .RequireAuthenticatedUser()
                                        .Build();
            });

            services.AddCors(o => o.AddPolicy("allow_all", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

            app.UseCors("allow_all");

            // accept X-Authorization and rename to Authorization so swagger ui works
            // https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1295#issuecomment-588297906
            app.Use((httpContext, next) =>
            {
                if (httpContext.Request.Headers["X-Authorization"]
                               .Any())
                    httpContext.Request.Headers.Add("Authorization", httpContext.Request.Headers["X-Authorization"]);

                return next();
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
