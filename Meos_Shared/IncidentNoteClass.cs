using System;
using System.Text.Json.Serialization;

namespace Meos_Shared;

public class IncidentNoteClass
{
    public int Id { get; set; }
    public int IncidentId { get; set; }
    public string Note { get; set; }

    public IncidentClass? incident { get; set; }
}
