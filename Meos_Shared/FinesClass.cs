using System;
using System.Text.Json.Serialization;
namespace Meos_Shared;

public class FinesClass
{
    public string Identifier { get; set; }
    public int Amount { get; set; }
    public PersonClass? Person { get; set; }
}
