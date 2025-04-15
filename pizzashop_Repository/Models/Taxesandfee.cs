using System;
using System.Collections.Generic;

namespace pizzashop_Repository.Models;

public partial class Taxesandfee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool? Type { get; set; }

    public decimal? Flatamount { get; set; }

    public decimal? Percentage { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdefault { get; set; }

    public decimal? Taxvalue { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int? Modifiedby { get; set; }

    public virtual User CreatedbyNavigation { get; set; } = null!;

    public virtual User? ModifiedbyNavigation { get; set; }

    public virtual ICollection<Ordertaxmapping> Ordertaxmappings { get; set; } = new List<Ordertaxmapping>();
}
