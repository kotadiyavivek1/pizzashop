using System;
using System.Collections.Generic;

namespace pizzashop_Repository.Models;

public partial class Order
{
    public int Id { get; set; }

    public int Customerid { get; set; }

    public decimal? Totalamount { get; set; }

    public decimal? Subtotal { get; set; }

    public decimal? Discount { get; set; }

    public decimal? Paidamount { get; set; }

    public string? Notes { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int? Modifiedby { get; set; }

    public string? Orderstatus { get; set; }

    public int? Paymentid { get; set; }

    public string? Instruction { get; set; }

    public virtual User CreatedbyNavigation { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual User? ModifiedbyNavigation { get; set; }

    public virtual ICollection<Ordereditem> Ordereditems { get; set; } = new List<Ordereditem>();

    public virtual ICollection<Ordertaxmapping> Ordertaxmappings { get; set; } = new List<Ordertaxmapping>();

    public virtual Payment? Payment { get; set; }

    public virtual ICollection<Tableordermapping> Tableordermappings { get; set; } = new List<Tableordermapping>();
}
