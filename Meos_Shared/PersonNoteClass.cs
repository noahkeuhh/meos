using System;
using System.Text.Json.Serialization;

namespace Meos_Shared;

public class PersonNoteClass
{
    public int Id { get; set; }
    public string Identifier { get; set; }
    public string Note { get; set; }

    public PersonClass? Person { get; set; }
}
