using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using VendingMachine.Exe.Infrastructure;
using VendinMachine.Commands;
using VendinMachine.Commands.Handlers;
using VendinMachine.Commands.Validators;
using VendinMachine.Core;

namespace VendingMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Vending Machine Starting...");

            //DI setup
            var serviceProvider = new ServiceCollection()
                .AddSingleton<CommandProcessor>()
                .AddMediatR(typeof(SelectProductHandler).GetTypeInfo().Assembly)
                .AddValidators()
                .AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidatorBehavior<,>))
                .AddSingleton<IVendingMachineProvider,VendingMachineProvider>()
                .BuildServiceProvider();

            var commandProcessor = serviceProvider.GetService<CommandProcessor>();

            var result = commandProcessor.Execute(new SelectProduct(""));

            Console.WriteLine(result);
            Console.Read();
        }
    }
}
