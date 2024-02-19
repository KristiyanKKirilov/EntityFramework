using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Intro.Models;

[Keyless]
public partial class VEmployeesHiredAfter20002
{
    [StringLength(50)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [Column(TypeName = "smalldatetime")]
    public DateTime HireDate { get; set; }
}
