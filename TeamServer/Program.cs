using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using TeamServer.Data;
using TeamServer.Hubs;
using TeamServer.Interfaces.AgentTasks;
using TeamServer.Services.AgentTasks;
using TeamServer.Services.Listeners;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IListenerService, ListenerService>();
builder.Services.AddSingleton<IAgentTasktQueueService, AgentTasktQueueService>();
builder.Services.AddSingleton<ListenerFormatService>();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHostedService<StartupListenersService>();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
          new[] { "application/octet-stream" });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting(); // Добавьте эту строку

app.UseResponseCompression();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<AmberHub>("/AmberHub");
});
app.UseAuthorization();

app.Run();