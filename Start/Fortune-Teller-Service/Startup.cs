using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Pivotal.Discovery.Client;
using Steeltoe.CloudFoundry.Connector.MySql.EFCore;

using Fortune_Teller_Service.Models;

namespace Fortune_Teller_Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDiscoveryClient(Configuration);

            if (Environment.IsDevelopment())
            {
                services.AddDbContext<FortuneContext>(x => x.UseInMemoryDatabase("Fortune_teller"));
            }
            else
            {
                services.AddDbContext<FortuneContext>(x => x.UseMySql(Configuration));
            }
            services.AddTransient<IFortuneRepository, FortuneRepository>();
            services.AddOptions();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            SampleData.InitializeFortunesAsync(serviceProvider).Wait();

            app.UseMvc();
            app.UseDiscoveryClient();

        }
    }
}
