﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class User
    {
        public int UserId {get; set; }
        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public ICollection<Bet> Bets { get; set; }  

    }
}
