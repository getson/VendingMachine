using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace VendinMachine.Commands.Handlers
{
    public class SelectProductHandler : RequestHandler<SelectProduct>
    {
        public SelectProductHandler()
        {
        }

        protected override void Handle(SelectProduct request)
        {
            
        }
    }
}