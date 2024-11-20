using System;

namespace CarAndAll.Server{

public interface IKlant
{   string Naam{get; set;}
{   string Adres{get; set;}
{   string Email{get; set;}
    
}
 public class Klant : IKlant
{
    private string naam {get; set;}
    private string adres {get; set;}
    private string Email {get; set;}
    // Met Get&Set neergezet, zodat het makkelijker is bij de UI gebruik.
}
}