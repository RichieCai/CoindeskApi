using CoindeskApi.Interface.Repository;
using CoindeskApi.Interface.Service;
using CoindeskApi.Middleware;
using CoindeskApi.Repository;
using CoindeskApi.Resources;
using CoindeskApi.Service;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MyCommon;
using MyCommon.Interface;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddTransient<ICoindeskService, CoindeskService>();
builder.Services.AddTransient<ICoindeskRepository, CoindeskRepository>();
builder.Services.AddTransient<ICoindeskTWRepositroy, CoindeskTWRepositroy>();

builder.Services.AddScoped<IMsDBConn>(provider => new MsDBConn(builder.Configuration.GetConnectionString("dbConnection")));


// 加載 log4net 配置
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


// 添加多國語系服務
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// 配置支援的語言和默認語言
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
 {
        new CultureInfo("en-US"),
        new CultureInfo("zh-TW")
    };

    options.DefaultRequestCulture = new RequestCulture("zh-TW");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

var app = builder.Build();

//app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseMiddleware<RequestResponseLoggingMiddleware>(); // 註冊中介層


// 啟用多國語系中介層
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);

var localizer = app.Services.GetRequiredService<IStringLocalizer<SharedResource>>();
var requiredFieldMessage = localizer["RequiredField"];
Console.WriteLine($"RequiredField Message: {requiredFieldMessage}");




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
