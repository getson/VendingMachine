using FluentValidation;
using System.Linq;
using System.Reflection;
using VendingMachine.Commands;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            var validators = from type in typeof(SelectProduct).GetTypeInfo()
                .Assembly.GetTypes()
                             where !type.IsAbstract && !type.IsGenericTypeDefinition
                             let validatorInterfaces =
                                 from iface in type.GetInterfaces()
                                 where iface.IsGenericType
                                 where iface.GetGenericTypeDefinition() == typeof(IValidator<>)
                                 select iface
                             where validatorInterfaces.Any()
                             select type;

            foreach (var validatorType in validators)
            {
                foreach (var interfaceType in validatorType.GetInterfaces().Where(x => x.IsGenericType))
                {
                    services.AddTransient(interfaceType, validatorType);
                }
            }
            return services;
        }
    }
}