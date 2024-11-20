using System;

namespace CarAndAll.Server{

public interface IKlant
{   string Naam{get; set;}
{   string Adres{get; set;}
{   string Email{get; set;}
    
} public class Klant : IKlant
{
    public string naam {get; set;}
    public string adres {get; set;}
    public string Email {get; set;}
    // Met Get&Set neergezet, zodat het makkelijker is bij de UI gebruik.
}
}