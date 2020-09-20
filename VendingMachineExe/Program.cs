using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using VendingMachine.CLI.Infrastructure;
using VendingMachine.CLI.Providers;
using VendingMachine.Commands.Handlers;
using VendingMachine.Core;
using VendingMachine.Queries;

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
                .AddSingleton<ITerminal, Terminal>()
                .AddSingleton<ICommandPrompt, CommandPrompt>()
                .AddSingleton<ICommandProvider, CommandProvider>(x => new CommandProvider(x.GetRequiredService<ICommandPrompt>(), args))
                .AddMediatR(typeof(SelectProductHandler).GetTypeInfo().Assembly, typeof(GetSelectedProductPrice).GetTypeInfo().Assembly)
                .AddValidators()
                .AddTransient(typeof(IRequestExceptionHandler<,>), typeof(ExceptionBehaviour<,>))
                //.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>))
                .AddSingleton<IVendingMachineProvider, VendingMachineProvider>()
                .BuildServiceProvider();

            serviceProvider
                .GetService<CommandProcessor>()
                .Execute();

            serviceProvider
                .GetService<ITerminal>()
                .ReadLine();
        }
    }
}