using System;
using System.Collections.Generic;

namespace pizzashop_Repository.Models;

public partial class Ordertaxmapping
{
    public int Id { get; set; }

    public int Orderid { get; set; }

    public int Taxid { get; set; }

    public decimal? Totalamount { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int? Modifiedby { get; set; }

    public virtual User CreatedbyNavigation { get; set; } = null!;

    public virtual User? ModifiedbyNavigation { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Taxesandfee Tax { get; set; } = null!;
}
