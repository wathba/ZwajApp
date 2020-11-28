using System.Text;
using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using ZwajApp.Api.Data;
using ZwajApp.Api.Helper;
using ZwajApp.Api.Model;
using ZwajApp.Api.Models;

namespace ZwajApp.Api
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
            services.AddDbContext<DataContext>(x=>x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
   IdentityBuilder builder = services.AddIdentityCore<User>(option => {
    option.Password.RequireDigit = false;
    option.Password.RequiredLength = 4;
    option.Password.RequireUppercase = false;
    option.Password.RequireNonAlphanumeric = false;
   });
   builder = new IdentityBuilder(builder.UserType,typeof(Role),builder.Services);
   builder.AddEntityFrameworkStores<DataContext>();
   builder.AddRoleValidator<RoleValidator<Role>>();
   builder.AddRoleManager<RoleManager<Role>>();
   builder.AddSignInManager<SignInManager<User>>();
     services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(Options =>
   {Options.TokenValidationParameters = new TokenValidationParameters
   {
       ValidateIssuerSigningKey = true,
       IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
       ValidateIssuer=false,
       ValidateAudience= false

   };

   });
   services.AddAuthorization(option=>{
    option.AddPolicy("RequireAdimRole", policy => policy.RequireRole("Admin"));
    option.AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin","Moderator"));
    option.AddPolicy("VipRole", policy => policy.RequireRole("VIP"));
   });
   services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
   services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
   services.AddMvc(option=>{
    var policy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
        .Build();
    option.Filters.Add(new AuthorizeFilter(policy));

   }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddJsonOptions(option=>{
             option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
   services.BuildServiceProvider().GetService<DataContext>().Database.Migrate();
   services.AddCors();
  
   services.AddSignalR();
     services.AddAutoMapper();
//    Mapper.Reset();

   services.AddTransient<TrialData>();
    services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddScoped<IAuthRepository, AuthRepository>();
   services.AddScoped<IZwajRepository, ZwajRepository>();
   services.AddScoped<LogUserActivity>();
 
  }
        public void ConfigureDevelopmentServices(IServiceCollection services)
        {   
            services.AddDbContext<DataContext>(x=>x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
   IdentityBuilder builder = services.AddIdentityCore<User>(option => {
    option.Password.RequireDigit = false;
    option.Password.RequiredLength = 4;
    option.Password.RequireUppercase = false;
    option.Password.RequireNonAlphanumeric = false;
   });
   builder = new IdentityBuilder(builder.UserType,typeof(Role),builder.Services);
   builder.AddEntityFrameworkStores<DataContext>();
   builder.AddRoleValidator<RoleValidator<Role>>();
   builder.AddRoleManager<RoleManager<Role>>();
   builder.AddSignInManager<SignInManager<User>>();
     services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(Options =>
   {Options.TokenValidationParameters = new TokenValidationParameters
   {
       ValidateIssuerSigningKey = true,
       IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
       ValidateIssuer=false,
       ValidateAudience= false

   };

   });
   services.AddAuthorization(option=>{
    option.AddPolicy("RequireAdimRole", policy => policy.RequireRole("Admin"));
    option.AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin","Moderator"));
    option.AddPolicy("VipRole", policy => policy.RequireRole("VIP"));
   });
   services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
   services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
   services.AddMvc(option=>{
    var policy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
        .Build();
    option.Filters.Add(new AuthorizeFilter(policy));

   }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddJsonOptions(option=>{
             option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
  services.AddCors();
  
   services.AddSignalR();
     services.AddAutoMapper();
//    Mapper.Reset();

   services.AddTransient<TrialData>();
    services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddScoped<IAuthRepository, AuthRepository>();
   services.AddScoped<IZwajRepository, ZwajRepository>();
   services.AddScoped<LogUserActivity>();
 
  }

  // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
  [System.Obsolete]
  public void Configure(IApplicationBuilder app, IHostingEnvironment env,TrialData trialData)
        {
   StripeConfiguration.SetApiKey(Configuration.GetSection("Stripe:SecretKey").Value);
   if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            { 
                app.UseExceptionHandler(BuilderExtensions=>{
                 BuilderExtensions.Run(async context=>
                 {
                  context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                  var error = context.Features.Get<IExceptionHandlerFeature>();
                  if (error != null)
                  {
                    context.Response.AddApplicationError(error.Error.Message);
                   await context.Response.WriteAsync(error.Error.Message);
                  }
                 });
                });
    //app.UseHsts();
   }

          trialData.TrialUsers();
         app.UseHttpsRedirection();

          app.UseCors(x=>x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
   app.UseSignalR(routes =>{
    routes.MapHub<ChatHub>("/chat");
   });
   app.UseAuthentication();
   app.Use(async (context, next) =>
   {
    await next();
    if(context.Response.StatusCode==404){
     context.Request.Path = "/index.html";
     await next();
    }

   });
   app.UseDefaultFiles();
   app.UseStaticFiles();
   app.UseMvc();
        }

 }
}
