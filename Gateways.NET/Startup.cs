using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.IO;
using System.Reflection;

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

            #region [ Commands Handlers, Validators & Query Services]

            services.AddCommands();
            services.AddQueryServices();

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

            #region [ Swagger ] 

            services.AddSwaggerGen(options => {
                options.IncludeXmlComments(XmlCommentsFilePath);
            });

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateways.NET v1.0");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        protected static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }
    }
}
