using Microsoft.EntityFrameworkCore;
using PurchaseOrder.Api.Data;
using PurchaseOrder.Api.Interfaces;
using PurchaseOrder.Api.Mapping;
using PurchaseOrder.Api.Repositories;
using PurchaseOrder.Api.Services;
using static PurchaseOrder.Api.Interfaces.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//
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

app.UseAuthorization();

app.MapControllers();

app.Run();
