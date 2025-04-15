using System;
using System.Collections.Generic;

namespace pizzashop_Repository.Models;

public partial class Mappingmenuitemwithmodifier
{
    public int Id { get; set; }

    public int Menuitemid { get; set; }

    public int Modifiergroupid { get; set; }

    public int? Minselectionrequired { get; set; }

    public int? Maxselectionallowed { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int? Modifiedby { get; set; }

    public virtual User CreatedbyNavigation { get; set; } = null!;

    public virtual Menuitem Menuitem { get; set; } = null!;

    public virtual User? ModifiedbyNavigation { get; set; }

    public virtual Modifiergroup Modifiergroup { get; set; } = null!;
}
