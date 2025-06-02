var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<BiometriaService>();
builder.Services.AddScoped<DocumentoService>();
builder.Services.AddScoped<NotificacaoService>();
builder.Services.AddValidatorsFromAssemblyContaining<BiometriaRequestValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(...);
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(...);
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Host.UseSerilog(...);

var app = builder.Build();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseIpRateLimiting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
