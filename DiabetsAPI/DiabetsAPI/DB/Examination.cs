using System;

namespace DiabetsAPI.DB;

public partial class Examination
{
    public long Id { get; set; }

    public long DoctorId { get; set; }

    public long PatientId { get; set; }

    public DateTime Date { get; set; }

    public string Measures { get; set; }

    public virtual Doctor Doctor { get; set; }

    public virtual Patient Patient { get; set; }
}
