using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealer.DTOs
{
    public class CarPartsDto
    {
        public string Make {  get; set; }   
        public string Model { get; set; }
        public long TraveledDistance { get; set; }
        public ICollection<Part> Parts { get;set;}
    }
}
