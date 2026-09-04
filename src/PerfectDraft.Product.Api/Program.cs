using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PerfectDraft.Product.Api.Configuration;
using PerfectDraft.Product.Shared.ValidatorConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.RegisterDTOValidators();

builder.Services.RegisterServices();

var app = builder.Build();


app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;
