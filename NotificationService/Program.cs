
using NotificationSystem.Clients;
using NotificationSystem.Business.Business;
using NotificationSystem.Infrastructure.Caching;
using NotificationSystem.Contracts.Clients;
using NotificationSystem.Contracts.Business;
using NotificationSystem.Contracts.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

#region Clients
builder.Services.AddSingleton<IGateway, Gateway>();
#endregion

#region Business
builder.Services.AddSingleton<INotificationBO, NotificationBO>(); 
#endregion

#region Caching
builder.Services.AddSingleton<ICacheProvider, MemoryCacheProvider>();
#endregion

#region [Web API Setup]
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddMemoryCache();

var app = builder.Build();

//Configure Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); 
#endregion

app.Run();
