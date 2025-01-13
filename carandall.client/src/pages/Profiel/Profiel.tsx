import { useEffect, useState } from "react";
import "@mantine/dates/styles.css";
import { useAuthorisatie } from "../../utilities/useAuthorisatie";
import { fetchCsrf } from "../../utilities/fetchCsrf";
import { Button, Group, PasswordInput, TextInput } from "@mantine/core";
import { isEmail, isNotEmpty, useForm } from "@mantine/form";

type Gebruiker = {
    gebruikerID: string;
    naam: string;
    email: string;
    adres: string;
    wachtwoord: string;
    rol: string;
};

export function Profiel() {
    useAuthorisatie(["Particulier", "Zakelijk", "Wagenparkbeheerder"]);

    const [data, setData] = useState<Gebruiker | null>(null);

    useEffect(() => {
      getGebruiker();
    }, []);

    const formulier = useForm({
      mode: 'controlled',
      initialValues: {
        email: "",
        naam: "",
        adres: "",
        nieuwWachtwoord: "",
        oudWachtwoord: "",
      },
      validate: {
        naam: isNotEmpty("Vul dit verplichte veld in"),
        email: isEmail("Ongeldig emailadres"),
        adres: (value) =>
          !value || value.trim() === ""
            ? "Adres is verplicht"
            : !/^[a-zA-Z\s]+[0-9]+$/.test(value)
            ? "Adres moet beginnen met tekst en eindigen op een nummer"
            : null,
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
        }
      },
    });

    const getGebruiker = async () => {
        try {
            const resultaat = await fetch( "http://localhost:5202/api/Profiel/GetGebruiker",
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

                <TextInput
                    label="Adres"
                    placeholder={data?.adres}
                    mb="sm"
                    {...formulier.getInputProps("adres")}
                />

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
                    <Button color="green" onClick={() => console.log()}>
                        Opslaan
                    </Button>
                    
                    <Button color="red" onClick={() => console.log()}>
                        Gegevens verwijderen
                    </Button>
                </Group>
            </form>
        </div>
    );
}
