import { useEffect, useState } from "react";
import "@mantine/dates/styles.css";
import { useAuthorisatie } from "../../utilities/useAuthorisatie";
import { fetchCsrf } from "../../utilities/fetchCsrf";
import { Button, Group, PasswordInput, TextInput } from "@mantine/core";
import { isEmail, useForm } from "@mantine/form";

type Huurder = {
    id: string;
    naam: string;
    email: string;
    adres: string;
    rol: string;
    bedrijfsAdres?: string;
    bedrijfsNaam?: string;
    kvkNummer?: number;
};

export function Profiel() {
    useAuthorisatie(["Particulier", "Zakelijk", "Wagenparkbeheerder"]);

    const [data, setData] = useState<Huurder | null>(null);

    useEffect(() => {
      getHuurder();
    }, []);

    const formulier = useForm({
      mode: 'controlled',
      initialValues: {
        email: data?.email || "",
        naam: data?.naam || "",
        adres: data?.adres || "",
        nieuwWachtwoord: "",
        oudWachtwoord: "",
        bedrijfsAdres: data?.bedrijfsAdres || "",
        bedrijfsNaam: data?.bedrijfsNaam || "",
        kvkNummer: data?.kvkNummer || "",
      },
      validate: {
        naam: (value) => value && value.trim() === "" ? "Vul dit verplichte veld in" : null,
        email: (value) => value && !isEmail(value) ? "Ongeldig emailadres" : null,
        adres: (value) => {
          if (data?.rol !== "Wagenparkbeheerder" && value && !/^[a-zA-Z\s]+[0-9]+$/.test(value)) {
            return "Adres moet beginnen met tekst en eindigen op een nummer";
          }
          return null;
        },
        nieuwWachtwoord: (value) => {
          if (value && (value.length < 8 || !/[A-Z]/.test(value) || !/\d/.test(value))) {
            return "Wachtwoord moet minimaal 8 tekens lang zijn, een hoofdletter en een cijfer bevatten";
          }
          return null;
        },
        oudWachtwoord: (value, values) => {
          if (values.nieuwWachtwoord && !value) {
            return "Oud wachtwoord is verplicht als nieuw wachtwoord is ingevuld.";
          }
          return null;
        },
        bedrijfsAdres: (value) => {
          if (data?.rol === "Wagenparkbeheerder" && value && value.trim() === "") {
            return "Bedrijfsadres is verplicht";
          }
          return null;
        },
        bedrijfsNaam: (value) => {
          if (data?.rol === "Wagenparkbeheerder" && value && value.trim() === "") {
            return "Bedrijfsnaam is verplicht";
          }
          return null;
        },
        kvkNummer: (value) => {
            if (data?.rol === "Wagenparkbeheerder") {
                if (!value) {
                    return "KVK-nummer is verplicht";
                }
                if (!/^\d{8}$/.test(String(value))) {
                    return "KVK-nummer moet exact 8 cijfers bevatten";
                }
            }
          return null;
        },
      },
    });

    const getHuurder = async () => {
        try {
            const resultaat = await fetch( "http://localhost:5202/api/Profiel/GetHuurder",
            {
                method: "GET",
                credentials: "include",
                headers: {
                "Content-Type": "application/json",
                }
            });

            if (!resultaat.ok) {
            console.log(new Error(`Foutmelding: ${resultaat.status}`));
            throw new Error(`Foutmelding: ${resultaat.status}`);
            }

            const data = await resultaat.json();
            setData(data);
        } catch (error) {
            console.error(error);
        }
    };

    const handleBewerken = async () => {
        const isValid = await formulier.validate();
    
        if (!isValid.hasErrors) {
            if(data) {
                try {
                    const bewerkteHuurder = {
                        HuurderID: data.id,
                        Email: formulier.values.email || null,
                        Naam: formulier.values.naam || null,
                        Adres: formulier.values.adres || null,
                        NieuwWachtwoord: formulier.values.nieuwWachtwoord || null,
                        OudWachtwoord: formulier.values.oudWachtwoord || null,
                        BedrijfsAdres: formulier.values.bedrijfsAdres || null,
                        BedrijfsNaam: formulier.values.bedrijfsNaam || null,
                        KVKNummer: formulier.values.kvkNummer || null,
                        Rol: data.rol
                    };
            
                    const resultaat = await fetchCsrf("http://localhost:5202/api/Profiel/EditHuurder", {
                        method: "PUT",
                        headers: {
                            "Content-Type": "application/json",
                        },
                        credentials: "include",
                        body: JSON.stringify(bewerkteHuurder),
                    });
            
                    if (resultaat.ok) {
                        alert("Medewerker bijgewerkt!");
                        await getHuurder();
                    } 
                    else {
                        alert("Er is iets fout gegaan tijdens het bijwerken van de medewerker. Probeer het opnieuw!");
                    }
                } catch (error) {
                    console.error("Fout: ", error);
                }
            }
        }
    };

    const handleVerwijderingsverzoek = async () => {
        if(data) {
            try {        
                const resultaat = await fetchCsrf("http://localhost:5202/api/Profiel/AddVerwijderingsverzoek", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    credentials: "include",
                    body: JSON.stringify(data.id),
                });
        
                if (resultaat.ok) {
                    alert("Verwijderingsverzoek succesvol ingediend! Over 6 maanden worden uw gegevens definitied verwijderd.");
                } 
                else {
                    alert("Er is iets fout gegaan tijdens het aanmaken van het verwijderingsverzoek. Probeer het opnieuw!");
                }
            } catch (error) {
                console.error("Fout: ", error);
            }
        }
    };


    return (
        <div>
            <h2>Gegevens bewerken</h2>

            <form onSubmit={(e) => e.preventDefault()}>
                <TextInput
                    label="Emailadres"
                    placeholder={data?.email}
                    mb="sm"
                    {...formulier.getInputProps("email")}
                />

                <TextInput
                    label="Naam"
                    placeholder={data?.naam}
                    mb="sm"
                    {...formulier.getInputProps("naam")}
                />

                {data?.rol === "Particulier" && (
                    <TextInput
                        label="Adres"
                        placeholder={data?.adres}
                        mb="sm"
                        {...formulier.getInputProps("adres")}
                    />
                )}

                {data?.rol === "Wagenparkbeheerder" && (
                    <>
                        <TextInput
                            label="Bedrijfsadres"
                            placeholder={data?.bedrijfsAdres}
                            mb="sm"
                            {...formulier.getInputProps("bedrijfsAdres")}
                        />
                        <TextInput
                            label="Bedrijfsnaam"
                            placeholder={data?.bedrijfsNaam}
                            mb="sm"
                            {...formulier.getInputProps("bedrijfsNaam")}
                        />
                        <TextInput
                            label="KVK-nummer"
                            placeholder={String(data?.kvkNummer)}
                            mb="sm"
                            {...formulier.getInputProps("kvkNummer")}
                        />
                    </>
                )}

                <PasswordInput
                    label="Nieuw wachtwoord (optioneel)"
                    placeholder="Wachtwoord"
                    mb="sm"
                    {...formulier.getInputProps("nieuwWachtwoord")}
                />
                
                <PasswordInput
                    label="Oud wachtwoord"
                    placeholder="Oud wachtwoord"
                    mb="sm"
                    {...formulier.getInputProps("oudWachtwoord")}
                />

                <Group justify="space-between" grow>
                    <Button color="green" onClick={() => handleBewerken()}>
                        Opslaan
                    </Button>
                    
                    <Button color="red" onClick={() => handleVerwijderingsverzoek()}>
                        Gegevens verwijderen
                    </Button>
                </Group>
            </form>
        </div>
    );
}
