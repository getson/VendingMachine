using VendingMachine.Core;

namespace VendingMachine.Core
{
    public interface IVendingMachineProvider
    {
        VMachine GetVendingMachine();
    }
}