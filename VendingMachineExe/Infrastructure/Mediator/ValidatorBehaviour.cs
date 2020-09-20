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
        private readonly IValidator<TRequest> _validator;

        public ValidatorBehavior(IValidator<TRequest> validator)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next
        )
        {
            var failures = _validator.Validate(request)
                .Errors
                .Where(error => error != null)
                .Select(error => error.ErrorMessage)
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