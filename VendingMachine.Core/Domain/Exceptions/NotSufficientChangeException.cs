﻿using System;

namespace VendingMachine.Core.Domain
{
    public class NotSufficientChangeException : Exception
    {
        public NotSufficientChangeException(string message) 
            : base(message)
        {
        }
    }
}
