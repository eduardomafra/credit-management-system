using CreditProposalService.Application.Settings;
using CreditProposalService.Infrastructure.Data;
using CreditProposalService.Infrastructure.Messaging;
using CreditProposalService.IoC;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.RegisterServices(builder.Configuration);

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.Configure<CreditProposalSettings>(builder.Configuration.GetSection("CreditProposalSettings"));

builder.Services.AddDbContext<CreditProposalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHostedService<MessageConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.Run();