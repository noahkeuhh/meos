using System;
using System.Text.Json.Serialization;

namespace Meos_Shared;

public class VehicleNoteClass
{
    public int Id { get; set; }
    public string Plate { get; set; }
    public string Note { get; set; }

    public VehicleClass? Vehicle { get; set; }
}
