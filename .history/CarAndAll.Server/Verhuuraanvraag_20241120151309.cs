using System;

namespace CarAndAll.Server{
    
    public class Verhuuraanvraag{
        private DateTime Startdatum{}
        private DateTime Einddatum{}
        public string Status{}
        public Voertuig Voertuig{get; set;}   
    
    }
}