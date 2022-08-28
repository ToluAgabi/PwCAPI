using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PwCAssessment.Data;
using PwCAssessment.Extensions;
using PwCAssessment.Interfaces;
using PwCAssessment.Models;
using PwCAssessment.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddDbContext<ApplicationDbContext>(b =>
{
    b.UseSqlServer(builder.Configuration["DefaultConnection"]);
});

builder.Services.AddIdentityCore<ApplicationUser>(options => { });
new IdentityBuilder(typeof(ApplicationUser), typeof(ApplicationRole), builder.Services)
    .AddRoleManager<RoleManager<ApplicationRole>>().AddUserManager<UserManager<ApplicationUser>>()
    .AddSignInManager<SignInManager<ApplicationUser>>()
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddSwaggerApiDoc();
builder.Services.AddHttpClient();
builder.Services.AddJwtAuth(builder.Configuration);
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<ITravelService, TravelService>();

builder.Services.AddControllers(options =>
    {
        options.Filters.Add(typeof(ValidatorActionFilter));
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

    });   

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

}

app.UseStaticFiles();
app.UseApiDoc();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();

var provider = scope.ServiceProvider;

await CreateRoles(provider);


app.Run();


async Task CreateRoles(IServiceProvider serviceProvider)
{
    try
    {


        //initializing custom roles   
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        string[] roleNames = { "Admin" };
        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                //create the roles and seed them to the database 
                roleResult = await roleManager.CreateAsync(new ApplicationRole(roleName));
            }
        }

        var adminExists = await userManager.FindByEmailAsync("admin@test.com");
        if (adminExists is null)
        {
            var user = new ApplicationUser
            {
                Email = "admin@test.com",
                UserName = "admin@test.com",
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "User",
                PhoneNumber = "+23490343456"

            };
            var res = await userManager.CreateAsync(user, "Admin2022!");

            await userManager.AddToRoleAsync(user, "admin");
        }
    }
    catch
    {
        // ignored
    }
}