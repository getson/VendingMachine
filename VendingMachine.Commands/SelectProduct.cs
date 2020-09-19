using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendinMachine.Commands
{
    public class SelectProduct: IRequest
    {
        public SelectProduct(string productName)
        {
            ProductName = productName;
        }
        public string ProductName { get; }
    }

}
