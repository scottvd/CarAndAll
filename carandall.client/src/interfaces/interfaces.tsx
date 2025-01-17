import { Prijs, Soort } from "../types/Types";

export interface Filterwaarden {
    zoekTerm: string;
    soort: Soort[];
    prijs: Prijs[];
};