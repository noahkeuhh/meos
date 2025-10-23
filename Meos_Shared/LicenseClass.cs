using System;
using System.Text.Json.Serialization;

namespace Meos_Shared;

public class LicenseClass
{
    public string? Type { get; set; }
    public string? Owner { get; set; }

    public PersonClass? Person { get; set; }
}
