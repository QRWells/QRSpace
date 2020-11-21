using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using QRSpace.Server.Data;
using QRSpace.Server.Entities;
using QRSpace.Server.Hubs;
using QRSpace.Server.Services;
using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Logging;

namespace QRSpace.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(opts =>
            {
                opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddIdentity<ApplicationUser, ApplicationRole>(opts =>
                {
                    opts.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            // Redis Setup
            var section = Configuration.GetSection("Redis:Default");
            var connectionString = section.GetSection("Connection").Value;
            var instanceName = section.GetSection("InstanceName").Value;
            var defaultDb = int.Parse(section.GetSection("DefaultDb").Value ?? "0");
            services.AddSingleton(new RedisHelper(connectionString, instanceName, defaultDb));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = Configuration.GetSection("JwtConfig")["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration.GetSection("JwtConfig")["Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration.GetSection("JwtConfig")["SecurityKey"])),
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.FromMinutes(5),
                    ValidateLifetime = true,
                };
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });
            //services.AddHttpsRedirection(opt => opt.HttpsPort = 4397);
            services.AddCors(options =>
            {
                options.AddPolicy("TimeGoesBy", builder =>
                {
                    builder.WithOrigins(
                        "http://timegoesby.wang",
                        "http://www.timegoesby.wang",
                        "http://localhost:4396",
                        "http://139.196.30.69:4396",
                        "http://139.196.30.69");
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                    builder.AllowCredentials();
                });
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddScoped<IShogiRecordRepository, ShogiRecordRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<ILowMarryTimeLineService, LowMarryTimeLineService>();
            services.AddSingleton<IUserIdGenerator, UserIdGenerator>();
            services.AddSignalR(opts =>
            {
                opts.EnableDetailedErrors = true;
            });
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120)
            };
            webSocketOptions.AllowedOrigins.Add("https://139.196.30.69");
            webSocketOptions.AllowedOrigins.Add("*");

            app.UseWebSockets(webSocketOptions);

            app.UseForwardedHeaders();
            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("TimeGoesBy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<ShogiHub>("/shogihub");
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}