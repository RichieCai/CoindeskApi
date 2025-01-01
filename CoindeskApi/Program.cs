using CoindeskApi.Interface.Repository;
using CoindeskApi.Interface.Service;
using CoindeskApi.Middleware;
using CoindeskApi.Models.Data;
using CoindeskApi.Repository;
using CoindeskApi.Service;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyCommon;
using MyCommon.Encryption;
using MyCommon.Interface;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// 添加多國語系服務
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// 添加控制器支持並啟用 DataAnnotations 的多國語系驗證
builder.Services.AddControllers()
    .AddDataAnnotationsLocalization();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddTransient<ICoindeskService, CoindeskService>();
builder.Services.AddTransient<ICoindeskRepository, CoindeskRepository>();
builder.Services.AddTransient<ICoindeskTWRepositroy, CoindeskTWRepositroy>();

builder.Services.Configure<EncryptionSettings>(builder.Configuration.GetSection("EncryptionSettings"));//因範例 暫時寫在setting
builder.Services.AddScoped<IMsDBConn>(provider => new MsDBConn(builder.Configuration.GetConnectionString("dbConnection")));

builder.Services.AddSingleton<AesEncryptionService>();

// 加載 log4net 配置
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.SuppressModelStateInvalidFilter = false; // 確保模型沒有被禁用
//});

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

//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.InvalidModelStateResponseFactory = context =>
//    {
//        var errors = context.ModelState.Values
//            .SelectMany(v => v.Errors)
//            .Select(e => e.ErrorMessage)
//            .ToList();

//        var response = new ApiResponse<object>
//        {
//            Success = false,
//            Message = "Validation Failed",
//            Errors = errors
//        };

//        return new BadRequestObjectResult(response);
//    };
//});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestResponseLoggingMiddleware>(); // 註冊中介層


// 啟用多國語系中介層
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;

localizationOptions.ApplyCurrentCultureToResponseHeaders = true;
app.UseRequestLocalization(localizationOptions);


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
