﻿using Invoices.Data.Models;
using Invoices.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.DataProcessor.ImportDto
{
    public class ImportInvoiceDto
    {
        [Required]
        [Range(1_000_000_000, 1_500_000_000)]
        public int Number { get; set; }
        [Required]
        public DateTime IssueDate { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        [EnumDataType(typeof(CurrencyType))]
        public CurrencyType CurrencyType { get; set; }
        [Required]
        public int ClientId { get; set; }
        
    }
}
