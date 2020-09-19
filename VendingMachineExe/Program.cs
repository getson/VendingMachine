using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using VendingMachine.Core;
using VendingMachine.Exe.Infrastructure;
using VendingMachine.Exe.Providers;
using VendingMachine.Commands;
using VendingMachine.Commands.Handlers;

namespace VendingMachine
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Vending Machine Starting...");

            //DI setup
            var serviceProvider = new ServiceCollection()
                .AddSingleton<CommandProcessor>()
                .AddMediatR(typeof(SelectProductHandler).GetTypeInfo().Assembly)
                .AddValidators()
                .AddTransient(typeof(IRequestExceptionHandler<,>),typeof(ExceptionBehaviour<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>))
                .AddSingleton<IVendingMachineProvider, VendingMachineProvider>()
                .BuildServiceProvider();

            var commandProcessor = serviceProvider.GetService<CommandProcessor>();

            var result = commandProcessor.Execute(new SelectProduct("Espreso"));

            Console.WriteLine(result);
            Console.Read();
        }
    }
}