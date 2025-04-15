using System;
using System.Collections.Generic;

namespace pizzashop_Repository.Models;

public partial class Country
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int? Modifiedby { get; set; }

    public virtual User CreatedbyNavigation { get; set; } = null!;

    public virtual User? ModifiedbyNavigation { get; set; }

    public virtual ICollection<State> States { get; set; } = new List<State>();
}
