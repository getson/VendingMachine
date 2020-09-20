using System;
using VendingMachine.Core;

namespace VendingMachine.CLI.Infrastructure
{
    public sealed class Terminal : ITerminal, IDisposable
    {
        public Terminal()
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler(ConsoleCancelKeyPressHandler);
        }

        public bool Silent { get; set; }

        public event EventHandler CancelKeyPress;

        private void ConsoleCancelKeyPressHandler(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            EventHandler cancelKeyPress = CancelKeyPress;
            if (cancelKeyPress == null)
            {
                return;
            }

            cancelKeyPress(this, e);
        }

        public string ReadLine()
        {
            string str = Console.ReadLine();
            return str;
        }

        public void Write(string message)
        {
            if (Silent)
            {
                return;
            }

            Console.Write(message);
        }

        public void WriteLine()
        {
            WriteLine(string.Empty);
        }

        public void WriteLine(string line)
        {
            if (Silent)
            {
                return;
            }

            Console.WriteLine(line);
        }

        public void WriteError(Exception ex)
        {
            if (Silent)
            {
                return;
            }

            Console.Error.WriteLine(ex.Message);
        }

        public void WriteError(string line)
        {
            if (Silent)
            {
                return;
            }

            Console.Error.WriteLine(line);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            try
            {
                Console.CancelKeyPress -= new ConsoleCancelEventHandler(ConsoleCancelKeyPressHandler);
            }
            catch /*(Exception ex)*/
            {
                //
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}