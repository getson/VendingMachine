using System;

namespace VendingMachine.Core
{
    public interface ITerminal : IDisposable
    {
        event EventHandler CancelKeyPress;

        bool Silent { get; set; }

        string ReadLine();

        void Write(string message);

        void WriteLine();

        void WriteLine(string line);

        void WriteError(Exception ex);

        void WriteError(string line);
    }
}