using MediatR.Pipeline;
using System;

namespace VendingMachine.Exe.Infrastructure
{
    public class ExceptionBehaviour<TRequest, TResponse> 
        : RequestExceptionHandler<TRequest, TResponse, Exception>
    {
        //TODO fix me
        protected override void Handle(TRequest request, Exception exception, RequestExceptionHandlerState<TResponse> state)
        {
            Console.WriteLine(exception.Message);
            state.SetHandled(state.Response);
        }
    }
}