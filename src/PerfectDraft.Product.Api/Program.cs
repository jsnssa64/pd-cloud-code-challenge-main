using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PerfectDraft.Product.Service.Configuration;
using PerfectDraft.Product.Shared.ValidatorConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

builder.Services.AddControllers();

builder.Services.RegisterDTOValidators();

builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;
