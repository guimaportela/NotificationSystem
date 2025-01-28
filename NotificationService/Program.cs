
using NotificationSystem.Clients;
using NotificationSystem.Business.Services;
using NotificationSystem.Business.Business;
using NotificationSystem.Infrastructure.Caching;
using NotificationSystem.Contracts.Clients;
using NotificationSystem.Contracts.Business;
using NotificationSystem.Contracts.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//TODO: Ver o que fazer com isso aqui
/*var gateway = new Gateway("123");
var service = new NotificationService();

service.Send("news", "user", "news 1");
service.Send("news", "user", "news 2");
service.Send("news", "user", "news 3");
service.Send("news", "another user", "news 1");
service.Send("update", "user", "update 1");*/


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
