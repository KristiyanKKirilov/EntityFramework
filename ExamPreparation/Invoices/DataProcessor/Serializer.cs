namespace Invoices.DataProcessor
{
    using Invoices.Data;
    using Invoices.DataProcessor.ExportDto;
    using Invoices.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            var clients = context.Clients
                 .Where(c => c.Invoices.Any(i => i.IssueDate > date))
                 .Include(c => c.Invoices)
                 .Select(c => new ExportClientWithInvoiceDto()
                 {
                     ClientName = c.Name,
                     VatNumber = c.NumberVat,
                     InvoicesCount = c.Invoices.Count,
                     Invoices = c.Invoices
                     .OrderBy(i => i.IssueDate)
                     .ThenByDescending(i => i.DueDate)
                     .Select(i => new ExportInvoiceDto()
                     {
                         InvoiceNumber = i.Number,
                         InvoiceAmount = decimal.Parse(i.Amount.ToString().TrimEnd('0')),
                         DueDate = i.DueDate.ToString("MM/dd/yyyy"),
                         Currency = i.CurrencyType.ToString() 

                     }).ToArray()
                 }).OrderByDescending(c => c.InvoicesCount)
                        .ThenBy(c => c.ClientName)
                            .ToArray();

            return clients.SerializeToXml<ExportClientWithInvoiceDto[]>("Clients");
        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {

            var products = context.Products
                .Where(p => p.ProductsClients.Any(pc => pc.Client.Name.Length >= nameLength))
                .Include(p => p.ProductsClients)
                    .ThenInclude(p => p.Client)
                 .Select(p => new ExportProductDto
                 {
                     Name = p.Name,
                     Price = decimal.Parse(p.Price.ToString().TrimEnd('0')),
                     Category = p.CategoryType,
                     Clients = p.ProductsClients
                     .Where(pc => pc.Client.Name.Length >= nameLength)
                     .Select(pc => new ExportClientDto()
                     {
                         Name = pc.Client.Name,
                         NumberVat = pc.Client.NumberVat

                     }).OrderBy(c=> c.Name).ToArray()
                 }).OrderByDescending(ep => ep.Clients.Count())
                    .ThenBy(ep => ep.Name)
                    .Take(5)
                    .ToArray();
            return products.SerializeToJson<ExportProductDto[]>();
        }
    }
}