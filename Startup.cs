using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;

namespace Gateways.NET
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
            #region [ DataBase Configuration ]

            // Registering UnitOfWork, DbContext and Repositories
            services.RegisterUnitOfWork(this.Configuration.GetConnectionString("DefaultConnection"));
            services.RegisterRepositories();

            #endregion

            #region [ Commands Handlers & Validators ]

            services.AddCommands();

            #endregion

            #region [ Controllers ]

            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    })
                    .AddJsonOptions(options => options.JsonSerializerOptions.IgnoreNullValues = true)
                    .ConfigureApiBehaviorOptions(options =>
                    {
                        options.SuppressConsumesConstraintForFormFileParameters = true;
                        options.SuppressInferBindingSourcesForParameters = true;
                        options.SuppressModelStateInvalidFilter = true;
                        options.SuppressMapClientErrors = true;
                    })
                    .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<Startup>());

            #endregion

            #region [ AutoMapper ]

            services.AddAutoMapper(cfg =>
                {
                    cfg.AllowNullCollections = true;
                    cfg.AllowNullDestinationValues = true;
                }, AppDomain.CurrentDomain.GetAssemblies());

            #endregion

            #region [ CORS ]

            services.AddCors(); 

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DocExpansion(DocExpansion.None);
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "MusalaSoft Gateways.NET Example");
                options.RoutePrefix = string.Empty;
                options.DefaultModelRendering(ModelRendering.Example);
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }        
    }
}
