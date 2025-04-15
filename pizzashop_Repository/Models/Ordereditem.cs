using System;
using System.Collections.Generic;

namespace pizzashop_Repository.Models;

public partial class Ordereditem
{
    public int Id { get; set; }

    public int Orderid { get; set; }

    public int Menuitemid { get; set; }

    public int Quantity { get; set; }

    public decimal? Totalamount { get; set; }

    public string? Instruction { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int? Modifiedby { get; set; }

    public bool ReadyItem { get; set; }

    public int? Noofreadyitems { get; set; }

    public virtual User CreatedbyNavigation { get; set; } = null!;

    public virtual Menuitem Menuitem { get; set; } = null!;

    public virtual User? ModifiedbyNavigation { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual ICollection<Ordereditemmodifiermapping> Ordereditemmodifiermappings { get; set; } = new List<Ordereditemmodifiermapping>();
}
