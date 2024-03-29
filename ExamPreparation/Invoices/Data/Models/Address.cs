﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Data.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        [Required]       
        public string StreetName { get; set; }
        [Required]
        public int StreetNumber { get; set; }
        [Required]
        public string PostCode { get; set; }
        [Required]        
        public string City { get; set; }
        [Required]        
        public string Country { get; set; }
        [Required]
        public int ClientId { get; set; }

        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; }

    }
}

//•	Id – integer, Primary Key
//•	StreetName – text with length [10…20] (required)
//•	StreetNumber – integer (required)
//•	PostCode – text (required)
//•	City – text with length [5…15] (required)
//•	Country – text with length [5…15] (required)
//•	ClientId – integer, foreign key (required)
//•	Client – Client
