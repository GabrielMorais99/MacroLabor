using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.Swagger;

namespace API_FOTO_vs
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors();
            services.AddMvc();//.AddApplicationPart(Assembly.Load(new AssemblyName("GerenciadorRepositorio2")));
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("https://apigeg.azurewebsites.net", "http://localhost:8100", "http://localhost:8101", "http://localhost:8102", "http://localhost:8103", "http://app.genteegestao.com.br", "http://localhost:5000", "http://localhost:8000")
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     .AllowCredentials());
            });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseCors(builder =>
              builder.WithOrigins("https://apigeg.azurewebsites.net", "http://localhost:8100", "http://localhost:8101", "http://localhost:8102", "http://localhost:8103", "http://app.genteegestao.com.br", "http://localhost:5000", "http://localhost:8000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());


            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
