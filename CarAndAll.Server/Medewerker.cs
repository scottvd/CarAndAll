Using System;

namespace CarAndAll.Server
{
    public interface IMedewerker{

        int Personeelsnummer{get;set;}
        Account Account {get; set;}
    }
    

    public class Medewerker : IMedewerker {
        private int Personeelsnummer{get;set;}
        public Account Account {get; set;}
    }
}    
