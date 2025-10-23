using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Meos_Shared;

public class UsersClass
{
    [Key]
    public int Id { get; set; }
    public string Lastname { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string Rang { get; set; }

    public ICollection<IncidentClass> Incidents { get; set; } = new List<IncidentClass>();
}
