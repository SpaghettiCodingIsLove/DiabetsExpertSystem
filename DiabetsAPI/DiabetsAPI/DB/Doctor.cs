using System;
using System.Collections.Generic;

namespace DiabetsAPI.DB;

public partial class Doctor
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string LastName { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }

    public bool IsAdmin { get; set; }

    public virtual ICollection<Examination> Examinations { get; } = new List<Examination>();
}
