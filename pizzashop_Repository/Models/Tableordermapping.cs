using System;
using System.Collections.Generic;

namespace pizzashop_Repository.Models;

public partial class Tableordermapping
{
    public int Id { get; set; }

    public int Orderid { get; set; }

    public int Tableid { get; set; }

    public int Noofperson { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int? Modifiedby { get; set; }

    public virtual User CreatedbyNavigation { get; set; } = null!;

    public virtual User? ModifiedbyNavigation { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Table Table { get; set; } = null!;
}
