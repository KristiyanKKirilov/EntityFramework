using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlDocumentCreation
{
    public class Book
    {
        public string Title { get; set; } = null!;
        public Author Author { get; set; } = null!;
        public int Year { get; set; }
        public decimal Price { get; set; }


    }
}
