using Confluent.Kafka;
using MediatR;
using Microsoft.EntityFrameworkCore;
using web_api_lib_application.Infraestructure.UnitOfWork;
using web_api_lib_data.Context;
using web_api_permision.ElkConfiguration;

var builder = WebApplication.CreateBuilder(args);

/// ConnectionString
var connectionString = builder.Configuration.GetConnectionString("webApiConnectionString") ?? throw new InvalidOperationException("Connection string 'webApiConnectionString' not found.");

//MediatR
builder.Services.AddMediatR(typeof(web_api_lib_application.Logic.Handlers.CreateTaskHandler).Assembly);

builder.Services.AddDbContext<WebApiContext>(options =>
    options
    .UseLazyLoadingProxies()
    .UseSqlServer(connectionString));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddElasticsearch(builder.Configuration);

var producerConfiguration = new ProducerConfig();
builder.Configuration.Bind("producerconfiguration", producerConfiguration);

builder.Services.AddSingleton<ProducerConfig>(producerConfiguration);


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors( x => 
    x.AllowAnyMethod()
    .AllowAnyOrigin()
    .AllowAnyHeader()
 );


//Create Db if not exist
using var scope = app.Services.CreateScope();
await using var dbContext = scope.ServiceProvider.GetRequiredService<WebApiContext>();
await dbContext.Database.MigrateAsync();


app.Run();
