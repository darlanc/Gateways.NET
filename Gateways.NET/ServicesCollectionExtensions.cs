using FluentValidation;
using Gateways.NET.Contracts;
using Gateways.NET.ViewModels;
using Gateways.NET.Repository.Infraestructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gateways.NET
{
    public static class ServicesCollectionExtensions
    {
        /// <summary>
        /// Register UnitOfWork and DbContext to DI
        /// </summary>
        /// <param name="services"></param>
        /// <param name="ConnectionString"></param>
        /// <param name="sensitiveDataLogging"></param>
        public static void RegisterUnitOfWork(this IServiceCollection services, string ConnectionString, bool sensitiveDataLogging = true)
        {
            services.AddDbContext<GatewaysDbContext>(options => {
                options.UseSqlServer(ConnectionString);
                options.EnableSensitiveDataLogging(sensitiveDataLogging);
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>((provider) => new UnitOfWork(provider.GetService<GatewaysDbContext>()));
        }

        /// <summary>
        /// Register Repositories to DI
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterRepositories(this IServiceCollection services)
        {
            var typesToRegister =
                from type in Assembly.GetExecutingAssembly().GetTypes()
                where !string.IsNullOrEmpty(type.Namespace)
                    && type.BaseType != null
                    && type.BaseType.IsGenericType
                    && type.BaseType.GetGenericTypeDefinition() == typeof(BaseEntityTypeConfiguration<>)
                select type;

            foreach (var type in typesToRegister)
            {
                Type entity = type.BaseType.GetGenericArguments().Single();

                var serviceType = typeof(IRepository<>).MakeGenericType(entity);
                var implementationType = typeof(Repository<>).MakeGenericType(entity);

                services.AddScoped(serviceType, (provider) => Activator.CreateInstance(implementationType, provider.GetService<GatewaysDbContext>()));
            }
        }

        /// <summary>
        /// Add Command Handlers and Validators to DI
        /// </summary>
        /// <param name="services"></param>
        public static void AddCommands(this IServiceCollection services)
        {
            var cache = AssemblyScanner.FindValidatorsInAssembly(Assembly.GetExecutingAssembly())
                .ToDictionary(k => k.InterfaceType, v => v.ValidatorType);

            // Adding Command Dispatcher to DI
            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();

            var existCommandHandlers = typeof(ICommandHandler).Assembly.GetTypes()
                .Where(p => p.IsClass && !p.IsGenericType && !p.IsAbstract && p.GetInterfaces().Contains(typeof(ICommandHandler)));            

            foreach (var commandHandlerImplementation in existCommandHandlers)
            {
                var commandHandlerImplementedInterfaces = commandHandlerImplementation.GetInterfaces()
                    .Where(p => p.GetGenericArguments().Length == 1 && p.GetGenericTypeDefinition() == typeof(ICommandHandler<>));

                // associate all ICommandHandler interface implemented by this type.
                foreach (var commandHandlerInterface in commandHandlerImplementedInterfaces)
                {
                    services.AddScoped(commandHandlerInterface, commandHandlerImplementation);

                    // Get command implemented for interface and create a entry registration in DI
                    Type commandType = commandHandlerInterface.GetGenericArguments().Single();

                    // add asociated command validator 
                    Type validationInterfaceType = typeof(IValidator<>).MakeGenericType(commandType);
                    if (cache.TryGetValue(validationInterfaceType, out Type validatorType))
                        services.AddScoped(validationInterfaceType, validatorType);
                }
            }
        }

        /// <summary>
        /// Add query services to DI
        /// </summary>
        /// <param name="services"></param>
        /// <param name="others"></param>
        public static void AddQueryServices(this IServiceCollection services, params Assembly[] others)
        {
            List<Assembly> assemblies = new List<Assembly>();
            if (others != null)
                assemblies.AddRange(others);
            assemblies.Add(Assembly.GetExecutingAssembly());

            List<Type> existServices = new List<Type>();
            foreach (var assembly in assemblies)
                existServices.AddRange(assembly.GetTypes().Where(p => p.IsClass && !p.IsAbstract && p.GetInterfaces().Contains(typeof(IQueryService))));

            foreach (var service in existServices)
            {
                var referenceInterfaces = service
                    .GetInterfaces()
                    .Where(p => p.GetInterfaces().Contains(typeof(IQueryService)));
                foreach (var @interface in referenceInterfaces)
                    services.AddScoped(@interface, service);
            }
        }
    }
}
