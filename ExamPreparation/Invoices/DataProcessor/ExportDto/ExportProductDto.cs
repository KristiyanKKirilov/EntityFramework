using Invoices.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Invoices.DataProcessor.ExportDto
{
    public class ExportProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public CategoryType Category { get; set; }
        public ExportClientDto[] Clients { get; set; }
    }
}
