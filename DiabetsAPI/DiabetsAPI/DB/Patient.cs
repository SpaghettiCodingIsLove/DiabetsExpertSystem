using System;
using System.Collections.Generic;

namespace DiabetsAPI.DB;

public partial class Patient
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string LastName { get; set; }

    public string Pesel { get; set; }

    public DateOnly BirthDate { get; set; }

    public virtual ICollection<Examination> Examinations { get; } = new List<Examination>();
}
