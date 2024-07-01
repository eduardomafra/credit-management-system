using CreditCardService.Application.Settings;
using CreditCardService.Infrastructure.Data;
using CreditCardService.Infrastructure.Messaging;
using CreditCardService.IoC;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.RegisterServices(builder.Configuration);

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMQ"));
//builder.Services.Configure<CreditProposalSettings>(builder.Configuration.GetSection("CreditProposalSettings"));

builder.Services.AddDbContext<CreditCardDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddHostedService<MessageConsumer>();

var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.Run();