using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using VendingMachine.CLI.Infrastructure;

namespace VendingMachine
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Vending Machine Starting...");

            var container = new ContainerBuilder();

            container.RegisterModule(new ApplicationModule(args));

            var serviceProvider = new AutofacServiceProvider(
                container.Build()
                );

            serviceProvider
                .GetService<CommandProcessor>()
                .Execute();

            serviceProvider
                .GetService<ITerminal>()
                .ReadLine();
        }
    }
}