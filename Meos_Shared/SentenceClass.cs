using System;

namespace Meos_Shared;

public class SentenceClass
{
    public int Id { get; set; }
    public int IncidentId { get; set; }
    public int Fine { get; set; }
    public string Type { get; set; }
    public int Amount { get; set; }

    public IncidentClass? incident { get; set; }
}
