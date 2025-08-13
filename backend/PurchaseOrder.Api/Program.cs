using Microsoft.EntityFrameworkCore;
using PurchaseOrder.Application.Interfaces;
using PurchaseOrder.Application.Mapping;
using PurchaseOrder.Application.Services;
using PurchaseOrder.Infrastructure.Data;
using PurchaseOrder.Infrastructure.Repositories;
using static PurchaseOrder.Domain.Interfaces.IRepository;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// --- CORS setup ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// Database & DI
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();



builder.Services.AddAutoMapper(typeof(MappingProfile));

//


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// --- Enable CORS ---
app.UseCors("AllowAngularDev");

app.UseAuthorization();

app.MapControllers();

app.Run();
