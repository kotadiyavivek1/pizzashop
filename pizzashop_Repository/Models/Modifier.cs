using System;
using System.Collections.Generic;

namespace pizzashop_Repository.Models;

public partial class Modifier
{
    public int Id { get; set; }

    public int Modifiergroupid { get; set; }

    public string Name { get; set; } = null!;

    public int Unitid { get; set; }

    public decimal Rate { get; set; }

    public int? Quantity { get; set; }

    public string? Description { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int? Modifiedby { get; set; }

    public virtual User CreatedbyNavigation { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual User? ModifiedbyNavigation { get; set; }

    public virtual Modifiergroup Modifiergroup { get; set; } = null!;

    public virtual ICollection<Modifiergroupmodifier> Modifiergroupmodifiers { get; set; } = new List<Modifiergroupmodifier>();

    public virtual ICollection<Ordereditemmodifiermapping> Ordereditemmodifiermappings { get; set; } = new List<Ordereditemmodifiermapping>();

    public virtual Unit Unit { get; set; } = null!;
}
