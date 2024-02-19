using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace SoftUni.DTO
{
    public class PersonDto
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        public string LastName { get; set; }
        [JsonProperty("address")]
        public string AddressAddressText { get; set; }
        public string AddressTownName { get; set; }
        public string FullName =>  $"{FirstName} {LastName}";
          
    }
}
