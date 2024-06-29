using CreditCardService.Application.Settings;
using CreditCardService.Infrastructure.Data;
using CreditCardService.Infrastructure.Messaging;
using CreditCardService.IoC;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.RegisterServices(builder.Configuration);

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMQ"));
//builder.Services.Configure<CreditProposalSettings>(builder.Configuration.GetSection("CreditProposalSettings"));

builder.Services.AddDbContext<CreditCardDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHostedService<MessageConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.Run();