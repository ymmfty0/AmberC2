using Microsoft.EntityFrameworkCore;
using TeamServer.Data;

namespace TeamServer.Models.Listeners.Types
{
    public class HttpHandler : IHandler
    {

        public string Id { get; set; }

        private CancellationTokenSource _tokenSource;

        public HttpHandler(string id)
        {
            Id = id;
        }

        public void Stop()
        {
            _tokenSource.Cancel();
        }

        public async Task Start(string BindHost, int BindPort)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHostDefaults(host =>
                {
                    host.UseUrls($"http://0.0.0.0:{BindPort}");
                    host.Configure(ConfigureApp);
                    host.ConfigureServices(ConfigureServices);
                });

            var host = hostBuilder.Build();

            _tokenSource = new CancellationTokenSource();
            var task = host.RunAsync(_tokenSource.Token);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite("DataSource=amberc2.db");
            });
            services.AddSignalR();
        }

        private void ConfigureApp(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(e =>
            {
                e.MapControllerRoute("/", "/", new { controller = "HttpListener", action = "HandleImplant" });
            });
        }

    }
}
