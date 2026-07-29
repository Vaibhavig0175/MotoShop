using MotoShop.Interfaces.Services;
using MotoShop.Models;
using QuestPDF.Infrastructure;
using QuestPDF.Fluent;

namespace MotoShop.Services
{
    public class InvoiceService: IInvoiceService
    {
        public byte[] GenerateReceipt(Payment payment)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);

                    page.Header()
                        .Text("MotoShop")
                        .FontSize(28)
                        .Bold();

                    page.Content().Column(column =>
                    {
                        column.Spacing(10);

                        column.Item().Text("PAYMENT RECEIPT")
                            .FontSize(22)
                            .Bold();

                        column.Item().Text($"Receipt No : {payment.Id}");

                        column.Item().Text($"Transaction : {payment.TransactionId}");

                        column.Item().Text($"Date : {payment.PaymentDate}");

                        column.Item().Text($"Buyer : {payment.Buyer.FullName}");

                        column.Item().Text($"Seller : {payment.Auction.Vehicle.Seller.FullName}");

                        column.Item().LineHorizontal(1);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Description").Bold();

                                header.Cell().AlignRight().Text("Amount").Bold();
                            });

                            table.Cell().Text(
                                payment.Auction.Vehicle.VehicleBrand.Name + " " +
                                payment.Auction.Vehicle.VehicleModel.Name);

                            table.Cell()
                                .AlignRight()
                                .Text($"₹ {payment.Amount:N0}");
                        });

                        column.Item().LineHorizontal(1);

                        column.Item()
                            .AlignRight()
                            .Text($"Total : ₹ {payment.Amount:N0}")
                            .Bold()
                            .FontSize(18);
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text("Thank you for using MotoShop.");
                });
            }).GeneratePdf();
        }

        public byte[] GenerateInvoice(Payment payment)
        {
            return GenerateReceipt(payment);
        }
    }
}
