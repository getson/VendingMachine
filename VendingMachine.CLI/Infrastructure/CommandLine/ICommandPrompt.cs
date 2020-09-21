using System;

namespace VendingMachine.CLI.Infrastructure
{
    public interface ICommandPrompt
    {
        bool ReadBool(string description, bool defaultValue);

        string ReadValue(string description, string defaultValue, Func<string, bool> validator);
    }
}