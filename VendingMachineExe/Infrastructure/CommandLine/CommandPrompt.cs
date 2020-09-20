using System;
using VendingMachine.Core;

namespace VendingMachine.CLI.Infrastructure
{
    public sealed class CommandPrompt : ICommandPrompt
    {
        private readonly ITerminal _terminal;

        public CommandPrompt(ITerminal terminal)
        {
            _terminal = terminal;
        }

        public bool ReadBool(string description, bool defaultValue)
        {
            string a = ReadValue(description, defaultValue ? "Y" : "N",
                new Func<string, bool>(CommandValidators.BoolValidator)
             );

            if (!string.Equals(a, "true", StringComparison.OrdinalIgnoreCase))
            {
                return string.Equals(a, "Y", StringComparison.CurrentCultureIgnoreCase);
            }

            return true;
        }

        public string ReadValue(
          string description,
          string defaultValue,
          Func<string, bool> validator)
        {
            string input;
            while (true)
            {
                do
                {
                    string action;
                    if (!string.IsNullOrEmpty(defaultValue))
                    {
                        action = $"{description} (press enter for {defaultValue})";
                    }
                    else
                    {
                        action = $"{description}";
                    }

                    _terminal.Write(action + " > ");

                    input = _terminal.ReadLine()?.Trim() ?? string.Empty;

                    if (string.IsNullOrEmpty(input) && !string.IsNullOrEmpty(defaultValue))
                    {
                        return defaultValue;
                    }
                }
                while (string.IsNullOrEmpty(input));
                if (!validator(input))
                {
                    _terminal.WriteLine($"Enter a valid value for {description}.");
                }
                else
                {
                    break;
                }
            }
            return input;
        }
    }
}