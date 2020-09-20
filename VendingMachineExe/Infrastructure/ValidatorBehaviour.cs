using FluentValidation;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VendingMachine.CLI.Infrastructure
{
    public class ValidatorBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IValidator<TRequest>[] _validators;

        public ValidatorBehavior(IValidator<TRequest>[] validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next
        )
        {
            var failures = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .Select(x => x.ErrorMessage)
                .ToList();

            if (failures.Any())
            {
                throw new VendingMachineValiationException(string.Join(" | ", failures));
            }

            return await next();
        }
    }

    public class VendingMachineValiationException : Exception
    {
        public VendingMachineValiationException(string message)
            : base(message)
        {
        }
    }
}