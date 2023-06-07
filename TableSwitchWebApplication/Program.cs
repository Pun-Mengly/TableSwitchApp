using Blazored.LocalStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Serilog.Events;
using Serilog;
using System.Security.Principal;
using TableSwitchWebApplication.BusineseLogics;
using TableSwitchWebApplication.Data;
using TableSwitchWebApplication.Helper;
using Serilog.Extensions.Hosting;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using TableSwitchWebApplication.HangFireTask;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

#region Database Connection
var cs = builder.Configuration.GetConnectionString("AmkDatabase");//Not Use
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(AppCon.ConStr));
#endregion

#region User Management
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 5;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedEmail = false;
})  
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<DataContext>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>()!.HttpContext!.User);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Authenticated", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});
#endregion

#region Radzen Configuration
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
#endregion

#region Depandancy Injection 
builder.Services.AddScoped<IBusineseLogic, BusineseLogic>();
#endregion

#region Local Storage
builder.Services.AddBlazoredLocalStorage();
#endregion

#region Serailog
builder.Services.AddSingleton(Log.Logger);
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddSerilog(dispose: true);
});
// Register the DiagnosticContext service
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<DiagnosticContext>();
Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .WriteTo.File(AppCon.LogFilePath+ "log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
#endregion

#region HangeFire
// Add Hangfire services.
builder.Services.AddHangfire(configuration =>
    configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(AppCon.ConStr,
        new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        }));

// Add the processing server as IHostedService
builder.Services.AddHangfireServer();
#endregion

#region IIS Config In Production
builder.Services.Configure<IISOptions>(options =>
{
    options.ForwardClientCertificate = false;
});
builder.Services.AddServerSideBlazor().AddCircuitOptions(options =>
{
    options.DetailedErrors = true;
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Add Serilog request logging
app.UseSerilogRequestLogging();

//app.UseHangfireDashboard();
app.UseHangfireServer();
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "AMK Background Process",
    Authorization = new[]
    {
        new  HangfireAuthorizationFilter("Admin")
    }
});


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

#region Delete Log File 
RecurringJob.AddOrUpdate(
    "recurring-job-delete-log-file",
    () => DeleteLogFile.DeleteFile(),
    Cron.Monthly);
#endregion


app.Run();
