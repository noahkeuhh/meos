using System;
using System.Diagnostics.Contracts;
using System.Text.Json.Serialization;

namespace Meos_Shared;

public class IncidentClass
{
    public int IncidentId { get; set; }
    public string Identifier { get; set; }
    public string Artikelen { get; set; }
    public string IngenomenGoederen { get; set; }
    public bool Rechten { get; set; }
    public string Agent { get; set; } = string.Empty;
    public DateTime Datum { get; set; }
    //public string Type { get; set; }
    //public int Hoeveelheid { get; set; }
    //public int Boete { get; set; }

    public virtual ICollection<IncidentNoteClass> IncidentNotes { get; set; } = new List<IncidentNoteClass>();

    public PersonClass? Person { get; set; }
    public virtual ICollection<UsersClass> Users { get; set; } = new List<UsersClass>();
}
