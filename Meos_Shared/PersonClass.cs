using System;
using System.ComponentModel.DataAnnotations;

namespace Meos_Shared;

public class PersonClass
{
    [Key]
    public string Identifier { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Bsn { get; set; }
    public string? DateOfBirth { get; set; }
    public string? Job { get; set; }

    public List<LicenseClass> Licenses { get; set; } = new List<LicenseClass>();
    public ICollection<FinesClass> Fines { get; set; } = new List<FinesClass>();
    public virtual ICollection<VehicleClass> Vehicles { get; set; } = new List<VehicleClass>();
    public virtual ICollection<PersonNoteClass> PersonNotes { get; set; } = new List<PersonNoteClass>();
    public virtual ICollection<ArrestWarrantClass> ArrestWarrant { get; set; } = new List<ArrestWarrantClass>();
    public virtual ICollection<IncidentClass> Incident { get; set; } = new List<IncidentClass>();
}
