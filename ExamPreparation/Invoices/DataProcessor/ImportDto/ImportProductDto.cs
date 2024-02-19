using Invoices.Data.Models.Enums;
using Invoices.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.DataProcessor.ImportDto
{
    public class ImportProductDto
    {
        [MinLength(9)]
        [MaxLength(30)]
        [Required]
        public string Name { get; set; }
        [Range(5.0, 1000.0)]
        [Required]
        public decimal Price { get; set; }
        [Required]
        [EnumDataType(typeof(CategoryType))]
        public CategoryType CategoryType { get; set; }
        public int[] Clients { get; set; }
    }
}
