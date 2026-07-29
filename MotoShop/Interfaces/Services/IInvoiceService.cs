using MotoShop.Models;

namespace MotoShop.Interfaces.Services
{
    public interface IInvoiceService
    {
        byte[] GenerateReceipt(Payment payment);

        byte[] GenerateInvoice(Payment payment);
    }
}
