using System;
using System.Collections.Generic;

namespace pizzashop_Repository.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int Invoiceid { get; set; }

    public decimal Amount { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int? Modifiedby { get; set; }

    public string Paymentmethod { get; set; } = null!;

    public bool Status { get; set; }

    public virtual User CreatedbyNavigation { get; set; } = null!;

    public virtual User? ModifiedbyNavigation { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
