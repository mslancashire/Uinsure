using Asp.Versioning;
using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;
using Uinsure.Core.Repositories;
using Uinsure.Data;

var builder = WebApplication.CreateBuilder(args);
var commonAssembly = Assembly.GetAssembly(typeof(Uinsure.Core.AssemblyReference)) ?? throw new ApplicationException("Common Assembly not found");

builder.Services.AddControllers();

// services
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<IPolicyRepository, StaticPolicyRepository>();

// add validation
builder.Services.AddValidatorsFromAssemblyContaining<Uinsure.Core.AssemblyReference>(includeInternalTypes: true);
builder.Services.AddFluentValidationAutoValidation();

// api documentation
builder.Services.AddOpenApi();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
