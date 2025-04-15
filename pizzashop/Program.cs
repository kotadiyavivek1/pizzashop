using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using pizzashop_Repository.Implementation;
using pizzashop_Repository.ImplementationService;
using pizzashop_Repository.Interface;
using pizzashop_Repository.interfaceService;
using pizzashop_Services.ImplementationService;
using pizzashop_Services.interfaceService;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<pizzashop_Repository.Models.PizzashopContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();


builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
var utf8Key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "");
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
});



var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("JWT Key is not configured.");
}
var asciiKey = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(30); // Optional: session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddScoped<IAuth_Service, Auth_Sevice>();
builder.Services.AddScoped<Auth_middleware>();
builder.Services.AddScoped<IAuth_Repository, Auth_Repository>();
builder.Services.AddScoped<IJwt_Service, Jwt_Service>();
builder.Services.AddScoped<IEmail_Service, Email_Sevice>();
builder.Services.AddScoped<IUser_Service, User_Service>();
builder.Services.AddScoped<IUser_Repository, User_Repository>();
builder.Services.AddScoped<IRole_Services, Role_Services>();
builder.Services.AddScoped<IRolePermission_Repository, RolePermission_Repository>();
builder.Services.AddScoped<IMenuCategory_Service, MenuCategory_Service>();
builder.Services.AddScoped<IMenuCategory_Repository, MenuCategory_Repository>();
builder.Services.AddScoped<ITableSection_Repository, TableSection_Repository>();
builder.Services.AddScoped<ITableSection_Service, TableSection_Service>();
builder.Services.AddScoped<ITaxFees_Service, TaxFeesService>();
builder.Services.AddScoped<ITaxFees_Repository, TaxFees_Repository>();
builder.Services.AddScoped<IOrderExport_Repository, OrderExport_Repository>();
builder.Services.AddScoped<IOrderExport_Service, OrderExport_Service>();
builder.Services.AddScoped<IViewRender_Service, ViewRender_Service>();
builder.Services.AddScoped<ICustomer_Repository, Customer_Repository>();
builder.Services.AddScoped<ICustomer_Service, Customer_Service>();
builder.Services.AddScoped<IKotRespository,KotRepository>();
builder.Services.AddScoped<IKotService,KotService>();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization();

var app = builder.Build();



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Auth/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
builder.Services.AddSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");
app.UseSession();
app.Run();