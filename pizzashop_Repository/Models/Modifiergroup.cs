using System;
using System.Collections.Generic;

namespace pizzashop_Repository.Models;

public partial class Modifiergroup
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int? Modifiedby { get; set; }

    public virtual User CreatedbyNavigation { get; set; } = null!;

    public virtual ICollection<Mappingmenuitemwithmodifier> Mappingmenuitemwithmodifiers { get; set; } = new List<Mappingmenuitemwithmodifier>();

    public virtual User? ModifiedbyNavigation { get; set; }

    public virtual ICollection<Modifiergroupmodifier> Modifiergroupmodifiers { get; set; } = new List<Modifiergroupmodifier>();

    public virtual ICollection<Modifier> Modifiers { get; set; } = new List<Modifier>();
}
