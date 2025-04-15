using System;
using System.Collections.Generic;

namespace pizzashop_Repository.Models;

public partial class Ordereditemmodifiermapping
{
    public int Id { get; set; }

    public int Orderitemid { get; set; }

    public int Modifierid { get; set; }

    public int? Quantityofmodifier { get; set; }

    public decimal Totalamount { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int? Modifiedby { get; set; }

    public virtual User CreatedbyNavigation { get; set; } = null!;

    public virtual User? ModifiedbyNavigation { get; set; }

    public virtual Modifier Modifier { get; set; } = null!;

    public virtual Ordereditem Orderitem { get; set; } = null!;
}
