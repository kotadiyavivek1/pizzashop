using System;
using System.Collections.Generic;

namespace pizzashop_Repository.Models;

public partial class Permission
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime Createdat { get; set; }

    public int? Modifiedby { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int? Createdby { get; set; }

    public virtual User? CreatedbyNavigation { get; set; }

    public virtual User? ModifiedbyNavigation { get; set; }

    public virtual ICollection<Rolepermission> Rolepermissions { get; set; } = new List<Rolepermission>();
}
