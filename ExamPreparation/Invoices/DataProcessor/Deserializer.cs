namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.DataProcessor.ImportDto;
    using Invoices.Extensions;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            StringBuilder sb = new();
            List<ImportClientDto> importClientDtos = xmlString.DeserializeFromXml<List<ImportClientDto>>("Clients");
            List<Client> clients = new();
            foreach (var importClientDto in importClientDtos)
            {
                if (!IsValid(importClientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var clientToAdd = new Client
                {
                    Name = importClientDto.Name,
                    NumberVat = importClientDto.NumberVat
                };

                foreach (var address in importClientDto.Addresses)
                {
                    if (IsValid(address))
                    {
                        clientToAdd.Addresses.Add(new Address
                        {
                            City = address.City,
                            Country = address.Country,
                            PostCode = address.PostCode,
                            StreetName = address.StreetName,
                            StreetNumber = address.StreetNumber,                            
                        });
                    }
                    else
                    {
                        sb.AppendLine(ErrorMessage);
                    }
                }
                clients.Add(clientToAdd);
                sb.AppendLine(string.Format(SuccessfullyImportedClients, importClientDto.Name));
            }
            context.Clients.AddRange(clients);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            List<ImportInvoiceDto> invoiceDtos = jsonString.DeserializeFromJson<List<ImportInvoiceDto>>();
            List<Invoice> invoices = new();
            StringBuilder sb = new();
            foreach (var invoiceDto in invoiceDtos)
            {
                if (!IsValid(invoiceDto) || invoiceDto.DueDate < invoiceDto.IssueDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (invoiceDto.DueDate == DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture) ||
                   invoiceDto.IssueDate == DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                {
                    sb.AppendLine("Invalid data!");
                    continue;
                }
                var invoice = new Invoice
                {
                    Number = invoiceDto.Number,
                    IssueDate = invoiceDto.IssueDate,
                    DueDate = invoiceDto.DueDate,
                    Amount = invoiceDto.Amount,
                    CurrencyType = invoiceDto.CurrencyType,
                    ClientId = invoiceDto.ClientId
                };
                invoices.Add(invoice);
                sb.AppendLine(string.Format(SuccessfullyImportedInvoices, invoice.Number));

            }
            context.Invoices.AddRange(invoices);
            context.SaveChanges();
            return sb.ToString().TrimEnd(); 

        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            List<ImportProductDto> productDtos = jsonString.DeserializeFromJson<List<ImportProductDto>>();
           
            List<Product> products = new();
            StringBuilder sb = new();
            int[] clientIds = context.Clients.Select(x => x.Id).ToArray();

            foreach (var productDto in productDtos)
            {
                if (!IsValid(productDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var product = new Product
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    CategoryType = productDto.CategoryType,
                };

                foreach (var clientId in productDto.Clients.Distinct())
                {
                    if (clientIds.Contains(clientId))
                    {
                        product.ProductsClients.Add(new ProductClient
                        {
                            ClientId = clientId                            
                        });
                    }
                    else
                    {
                        sb.AppendLine(ErrorMessage);
                    }
                }
                products.Add(product);
                sb.AppendLine(string.Format(SuccessfullyImportedProducts, product.Name, product.ProductsClients.Count()));
            }

            context.Products.AddRange(products);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
            
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    } 
}
