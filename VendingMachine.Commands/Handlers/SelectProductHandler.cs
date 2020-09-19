﻿using MediatR;
using System;
using VendingMachine.Core;

namespace VendingMachine.Commands.Handlers
{
    public class SelectProductHandler : RequestHandler<SelectProduct>
    {
        private readonly IVendingMachineProvider _vendingMachineProvider;

        public SelectProductHandler(IVendingMachineProvider vendingMachineProvider)
        {
            _vendingMachineProvider = vendingMachineProvider;
        }

        protected override void Handle(SelectProduct request)
        {
            var machine = _vendingMachineProvider.GetVendingMachine();

            machine.SelectProduct(Enum.Parse<Product>(request.ProductName));
        }
    }
}