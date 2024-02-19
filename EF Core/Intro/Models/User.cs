using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Intro.Models;

public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(20)]
    public string Username { get; set; } = null!;

    [StringLength(20)]
    public string Password { get; set; } = null!;
}
