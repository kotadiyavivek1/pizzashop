using System;
using System.Collections.Generic;

namespace pizzashop_Repository.Models;

public partial class Rolepermission
{
    public int Id { get; set; }

    public int Permissionid { get; set; }

    public int Roleid { get; set; }

    public bool? Canview { get; set; }

    public bool? Canedit { get; set; }

    public bool? Candelete { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int? Modifiedby { get; set; }

    public virtual User CreatedbyNavigation { get; set; } = null!;

    public virtual User? ModifiedbyNavigation { get; set; }

    public virtual Permission Permission { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
