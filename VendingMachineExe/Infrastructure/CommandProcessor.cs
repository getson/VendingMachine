using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Exe.Infrastructure
{
    public class CommandProcessor
    {
        private readonly IMediator _mediator;

        public CommandProcessor(IMediator mediator)
        {
            _mediator = mediator;
        }

        public string Execute<T>(T command)
        {
            try
            {
                var result = _mediator.Send(command)
                    .GetAwaiter()
                    .GetResult();
                
                return result?.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
