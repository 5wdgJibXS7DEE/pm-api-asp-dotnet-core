using JsonFlatFileDataStore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.Definitions;
using ProjectManagement.Logic;

namespace ProjectManagement.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            ConfigureServicesForStorage(services);
            ConfigureServicesForLogic(services);
        }

        protected virtual void ConfigureServicesForStorage(IServiceCollection services)
        {
            string filename = Configuration.GetValue<string>("datastoreFilename");

            services.AddSingleton<IDataStore>(new DataStore(filename));
        }

        protected void ConfigureServicesForLogic(IServiceCollection services)
        {
            services.AddScoped<ITasksLogic, TasksLogic>();
            services.AddScoped<ITaskOverlapsLogic, TaskOverlapsLogic>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}