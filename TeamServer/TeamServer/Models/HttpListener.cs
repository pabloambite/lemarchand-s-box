using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace TeamServer.Models
{
    public class HttpListener : Listener
    {
        public override string Name { get; }
        public int BindPort { get; }

        private CancellationTokenSource _tokenSource;

        public HttpListener(string name, int port)
        {
        
            Name = name;
            BindPort = port;

        }
        public override async Task Start()
        {
            var hostBuidler = new HostBuilder().ConfigureWebHostDefaults(host =>
            {
                host.UseUrls($"http://0.0.0.0:{BindPort}");
                host.Configure(ConfigureApp);
                host.ConfigureServices(ConfigureServices);
            });

            var host = hostBuidler.Build();
            
            _tokenSource = new CancellationTokenSource();
            host.RunAsync( _tokenSource.Token );
            
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        private void ConfigureApp(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(e =>
            {
                e.MapControllerRoute("/","/", new { controller = "HttpListener", action = "HandleImplant" });
            });
        }

        public override void Stop()
        {
            _tokenSource.Cancel();
        }
    }
}
