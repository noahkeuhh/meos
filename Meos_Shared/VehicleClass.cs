using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Meos_Shared;

public class VehicleClass
{
    [Key]
    public string Plate { get; set; }

    public string Owner { get; set; }

    public virtual ICollection<VehicleNoteClass> Notes { get; set; } = new List<VehicleNoteClass>();

    public PersonClass? Person { get; set; }
    
}
