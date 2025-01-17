export type Verhuuraanvraag = {
  verhuuraanvraagID: number;
  voertuig: string;
  kenteken: string;
  huurder: string;
  ophaaldatum: string;
  inleverdatum: string;
  status: string;
};

export type Medewerker = {
  id: string;
  naam: string;
  personeelsNummer: number;
  email: string;
  wachtwoord?: string;
  rol: string;
};

export type Voertuig = {
  voertuigId: number;
  kenteken: string;
  soort: string;
  merk: string;
  type: string;
  aanschafjaar: number;
  verhuuraanvragen: Record<string, any> | null;
  schademeldingen: Record<string, any> | null;
};

export type VoertuigMetPrijs = Voertuig & {
  prijs: number;
};


export type Huurder = {
    id: string;
    naam: string;
    email: string;
    adres: string;
    rol: string;
    bedrijfsAdres?: string;
    bedrijfsNaam?: string;
    kvkNummer?: number;
};

export type Soort = "Auto" | "Camper" | "Caravan";

export type Prijs = "< €50" | "€50 - €75" | "> €75";