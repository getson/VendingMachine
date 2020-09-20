using System;

namespace VendingMachine.CLI.Infrastructure
{
    public static class CommandValidators
    {
        public static bool BoolValidator(string value)
        {
            if (!string.Equals(value, "true", StringComparison.OrdinalIgnoreCase) && !string.Equals(value, "false", StringComparison.OrdinalIgnoreCase) && !string.Equals(value, "Y", StringComparison.CurrentCultureIgnoreCase))
                return string.Equals(value, "N", StringComparison.CurrentCultureIgnoreCase);
            return true;
        }

        public static bool NonEmptyValidator(string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}
