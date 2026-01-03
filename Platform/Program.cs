using Core.Filters.Swagger;
using Core.Helpers;
using Core.Logger;
using Core.Services;
using Hangfire;
//appendUsingSeeder
using Hangfire.SqlServer;
using Idata.Data;
using Idata.Helpers;
using Ihelpers.Middleware.TokenManager.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

var runSeeders = false;

// Add services to the container.
#region global parameters setting
builder = Idata.IdataServiceProvider.Boot(builder);
builder = Core.CoreServiceProvider.Boot(builder);
//appendBuilder
builder = Icomments.IcommentsServiceProvider.Boot(builder);
#endregion


#region Security
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        IssuerSigningKeyResolver = (tokenString, securityToken, identifier, parameters) =>
        {
            string alg = null;
            if (securityToken is JwtSecurityToken jwtSecurityToken)
                alg = jwtSecurityToken.SignatureAlgorithm;
            if (securityToken is Microsoft.IdentityModel.JsonWebTokens.JsonWebToken jsonWebToken)
                alg = jsonWebToken.Alg;
            if (parameters.IssuerSigningKey is SymmetricSecurityKey symIssKey
                && alg != null)
            {
                // workaround for breaking change in "System.IdentityModel.Tokens.Jwt 6.30.1+ 
                switch (alg?.ToLowerInvariant())
                {
                    case "hs256":
                        return new[] { AuthHelper.ExtendKeyLengthIfNeeded(symIssKey, 32) };
                    case "hs512":
                        return new[] { AuthHelper.ExtendKeyLengthIfNeeded(symIssKey, 64) };
                }
            }
            return new[] { parameters.IssuerSigningKey };
        }
    };
});
#endregion

#region GlobalConfiguration SqlServerStorageOptions
var options = new SqlServerStorageOptions
{
    CommandBatchMaxTimeout = TimeSpan.FromMinutes(7),
    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(7),
    QueuePollInterval = TimeSpan.Zero
};

GlobalConfiguration.Configuration.UseSqlServerStorage(Ihelpers.Helpers.ConfigurationHelper.GetConfig("ConnectionStrings:DefaultConnection"), options);
#endregion

//This permissions doesn't follow controller endpoints standard
#region ImaginaColombiaExtensions
builder.Services.CheckPermissionAccessInsideControllerBase(
    new PermissionBaseOption { controller = "Users", permissionBase = "profile.user" },
    new PermissionBaseOption { controller = "Roles", permissionBase = "profile.role" },
    new PermissionBaseOption { controller = "Export", ignore = true },
    new PermissionBaseOption { controller = "Config", ignore = true },
    new PermissionBaseOption { controller = "Setting", ignore = true },
    new PermissionBaseOption { controller = "Page", ignore = true },
    new PermissionBaseOption { controller = "Auth", endpoint = "impersonate", ignore = true }
    );
#endregion

//builder.Services.AddHangfireServer(); // uncoment to test Hangfire local!
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "platformAPI", Version = "v1" });
    c.SchemaFilter<SwaggerSkipPropertyFilter>();
    c.TagActionsBy(api =>
    {
        if (api.GroupName != null)
        {
            return new[] { api.GroupName };
        }

        var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
        if (controllerActionDescriptor != null)
        {
            return new[] { controllerActionDescriptor.ControllerName };
        }

        throw new InvalidOperationException("Unable to determine tag for endpoint.");
    });
    c.DocInclusionPredicate((name, api) => true);
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement{{new OpenApiSecurityScheme{
Reference = new OpenApiReference{
        Type = ReferenceType.SecurityScheme,Id = "Bearer"}},
        new string[] {}}});
});


builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en" };

    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
    options.SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
});


builder.Services.AddHttpClient<ReportingService>();

var app = builder.Build();

// Aquí es donde debes poner UseRequestLocalization
var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);

#region seeders
if (runSeeders)
{
    //Run modules seeder if needed.
    //appendSeeder
    //SeederExecutor.ExecuteSeeder(IprofileSeeder.Seed, "ProfileSeeder", app);
}
#endregion

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseMiddleware<TokenManagerMiddleware>();

app.UseCors(x => x
               .AllowAnyMethod()
               .AllowAnyHeader()
               .SetIsOriginAllowed(origin => true) // allow any origin
               .AllowCredentials());

app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard();
app.MapControllers();

//Store class types with paths inside cache
EntitiesHelper.StoreClassesPath();

//Store static vital services
Ihelpers.Extensions.ConfigContainer.Configure(app.Services);

app.Run();



