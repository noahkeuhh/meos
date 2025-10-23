using System;
using System.Text.Json.Serialization;

namespace Meos_Shared;

public class ArrestWarrantClass
{
    public int Id { get; set; }
    public string Identifier { get; set; }
    public DateTime Date { get; set; }
    public string Agent { get; set; }
    public string Message { get; set; }


    public PersonClass? Person { get; set; }
}
