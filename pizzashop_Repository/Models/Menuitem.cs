using System;
using System.Collections.Generic;

namespace pizzashop_Repository.Models;

public partial class Menuitem
{
    public int Id { get; set; }

    public int Categoryid { get; set; }

    public string Name { get; set; } = null!;

    public bool? Type { get; set; }

    public decimal Rate { get; set; }

    public int? Quantity { get; set; }

    public int? Unitid { get; set; }

    public bool? Isavailable { get; set; }

    public string? Image { get; set; }

    public string? Description { get; set; }

    public decimal? Taxpercentage { get; set; }

    public bool? Isfavourite { get; set; }

    public string? Shortcode { get; set; }

    public bool? Isdefaulttax { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int? Modifiedby { get; set; }

    public int? Totalamountwithtax { get; set; }

    public virtual Menucategory Category { get; set; } = null!;

    public virtual User CreatedbyNavigation { get; set; } = null!;

    public virtual ICollection<Mappingmenuitemwithmodifier> Mappingmenuitemwithmodifiers { get; set; } = new List<Mappingmenuitemwithmodifier>();

    public virtual User? ModifiedbyNavigation { get; set; }

    public virtual ICollection<Ordereditem> Ordereditems { get; set; } = new List<Ordereditem>();

    public virtual Unit? Unit { get; set; }
}
