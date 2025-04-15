using System;
using System.Collections.Generic;

namespace pizzashop_Repository.Models;

public partial class Feedback
{
    public int Id { get; set; }

    public string? Comments { get; set; }

    public int Orderid { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int? Modifiedby { get; set; }

    public int? Food { get; set; }

    public int? Service { get; set; }

    public int? Ambience { get; set; }

    public int? Avgrating { get; set; }

    public virtual User CreatedbyNavigation { get; set; } = null!;

    public virtual User? ModifiedbyNavigation { get; set; }

    public virtual Order Order { get; set; } = null!;
}
