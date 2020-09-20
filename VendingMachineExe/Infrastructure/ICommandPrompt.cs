using System;

namespace VendingMachine.CLI.Infrastructure
{
    public interface ICommandPrompt
    {
        bool ReadBool(string argName, string description, bool defaultValue);

        string ReadValue(
          string argName,
          string description,
          string defaultValue,
          Func<string, bool> validator);
    }
}
