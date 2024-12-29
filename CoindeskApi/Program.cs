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


// �[�� log4net �t�m
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


// �K�[�h��y�t�A��
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// �t�m�䴩���y���M�q�{�y��
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
app.UseMiddleware<RequestResponseLoggingMiddleware>(); // ���U�����h


// �ҥΦh��y�t�����h
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
