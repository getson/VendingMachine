using Autofac;
using FluentValidation;
using MediatR;
using System.Reflection;
using VendingMachine.CLI.Providers;
using VendingMachine.Commands;
using VendingMachine.Core;
using VendingMachine.Core.Domain.Services;
using VendingMachine.EventHandlers;
using VendingMachine.Queries;

namespace VendingMachine.CLI.Infrastructure
{
    public class ApplicationModule : Autofac.Module
    {
        private readonly string[] _args;

        public ApplicationModule(string[] args)
        {
            _args = args;
        }

        protected override void Load(ContainerBuilder builder)
        {
            RegisterMediatRServices(builder);

            RegisterCommandLineServices(builder);

            builder.RegisterType<VendingMachineProvider>()
                .As<IVendingMachineProvider>()
                .SingleInstance();
            
            builder.RegisterType<ChangeCalculator>()
                .As<IChangeCalculator>()
                .SingleInstance();

        }

        private void RegisterCommandLineServices(ContainerBuilder builder)
        {
            builder.RegisterType<Terminal>()
                .As<ITerminal>()
                .SingleInstance();

            builder.RegisterType<CommandPrompt>()
                .As<ICommandPrompt>()
                .SingleInstance();

            builder.RegisterType<CommandParser>()
                .WithParameter(new TypedParameter(typeof(string[]), _args))
                .As<ICommandParser>()
                .SingleInstance();

            builder.RegisterType<CommandProcessor>()
               .AsSelf()
               .SingleInstance();
        }

        private static void RegisterMediatRServices(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(SelectProduct).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(GetSelectedProductPrice).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(ShowProductPriceWhenProductIsSelectedEventHandler).GetTypeInfo().Assembly)
             .AsClosedTypesOf(typeof(INotificationHandler<>));

            // Register the Command's Validators (Validators based on FluentValidation library)
            builder
                .RegisterAssemblyTypes(typeof(SelectProduct).GetTypeInfo().Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();

            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => componentContext.TryResolve(t, out var o) ? o : null;
            });

            builder.RegisterGeneric(typeof(ValidatorBehavior<,>))
                .As(typeof(IPipelineBehavior<,>));
        }
    }
}