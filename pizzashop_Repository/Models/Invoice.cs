using System;
using System.Collections.Generic;

namespace pizzashop_Repository.Models;

public partial class Invoice
{
    public int Id { get; set; }

    public int Orderid { get; set; }

    public int Modifierid { get; set; }

    public int? Quantityofmodifier { get; set; }

    public decimal? Rateofmodifier { get; set; }

    public decimal? Totalamount { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int? Modifiedby { get; set; }

    public virtual User CreatedbyNavigation { get; set; } = null!;

    public virtual User? ModifiedbyNavigation { get; set; }

    public virtual Modifier Modifier { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
