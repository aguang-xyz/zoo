using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zoo.Examples.Contract.Services;
using Zoo.Examples.Provider.Services;
using Zoo.Protocol.AspNetCore.Extensions;

namespace Zoo.Examples.Provider
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Inject RPC client service.
            services.AddRpcClientService();

            // Add normal service.
            services.AddScoped<IGreetingService, GreetingService>();
            
            // Add provider service.
            services.AddProvider<IGreetingService>();

            // Add controllers.
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseRpc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}