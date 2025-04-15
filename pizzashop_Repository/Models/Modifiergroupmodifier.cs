using System;
using System.Collections.Generic;

namespace pizzashop_Repository.Models;

public partial class Modifiergroupmodifier
{
    public int Id { get; set; }

    public int Modifiergroupid { get; set; }

    public int Modifierid { get; set; }

    public int Createdby { get; set; }

    public DateTime? Createdat { get; set; }

    public int? Modifiedby { get; set; }

    public DateTime? Modifiedat { get; set; }

    public virtual User CreatedbyNavigation { get; set; } = null!;

    public virtual User? ModifiedbyNavigation { get; set; }

    public virtual Modifier Modifier { get; set; } = null!;

    public virtual Modifiergroup Modifiergroup { get; set; } = null!;
}
