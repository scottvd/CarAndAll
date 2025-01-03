import { Button, Checkbox, Group, PasswordInput, TextInput, Paper, Container } from "@mantine/core";
import { isEmail, isNotEmpty, useForm } from "@mantine/form";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

export function Register() {
    const form = useForm({
        initialValues: {
            zakelijk: false,
            kvk: "",
            email: "",
            naam: "",
            adres: "",
            wachtwoord: "",
            wachtwoordBevestiging: "",
            bedrijfNaam: "",
            bedrijfAdres: ""
        },
        validate: {
            email: isEmail("Ongeldig emailadres"),
            naam: isNotEmpty("Vul dit verplichte veld in"),
            adres: (value) =>
                value && /^[a-zA-Z\s]+[0-9]+$/.test(value)
                    ? null
                    : "Adres moet beginnen met tekst en eindigen op een nummer",
            wachtwoord: (value) =>
                value &&
                value.length >= 8 &&
                /[A-Z]/.test(value) &&
                /\d/.test(value)
                    ? null
                    : "Wachtwoord moet minimaal 8 tekens lang zijn, een hoofdletter en een cijfer bevatten",
            wachtwoordBevestiging: (value, values) =>
                value === values.wachtwoord
                    ? null
                    : "Wachtwoord bevestiging moet overeenkomen",
            kvk: (value, values) =>
                values.zakelijk && !value
                    ? "KVK-nummer is verplicht voor zakelijke huurders"
                    : null,
            bedrijfNaam: isNotEmpty("Vul dit verplichte veld in"),
            bedrijfAdres: (value) =>
                value && /^[a-zA-Z\s]+[0-9]+$/.test(value)
                    ? null
                    : "Adres moet beginnen met tekst en eindigen op een nummer"
        },
    });

    const navigate = useNavigate();
    const [error, setError] = useState("");

    const naarLogin = () => {
        navigate("/login");
    };

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        if (!form.isValid()) {
            setError("Please fix the errors in the form.");
            return;
        }

        try {
            const requestBody: any = {
                Zakelijk: form.values.zakelijk,
                Email: form.values.email,
                Naam: form.values.naam,
                Adres: form.values.adres,
                Wachtwoord: form.values.wachtwoord,
            };

            if (form.values.zakelijk) {
                requestBody.Kvk = Number(form.values.kvk);
                requestBody.BedrijfNaam = form.values.bedrijfNaam;
                requestBody.BedrijfAdres = form.values.bedrijfAdres;
            }

            const resultaat = await fetch("http://localhost:5202/api/Registreer/RegistreerGebruiker", {
                method: "POST",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(requestBody)
            });

            if (resultaat.ok) {
                alert("U bent geregistreerd!");
                form.reset();
            } else {
                console.log(await resultaat.json());
                alert("Er is iets fout gegaan tijdens het aanmaken van uw account. Probeer het opnieuw!");
            }
        } catch (error) {
            console.error("Fout: ", error);
        }
    };

    return (
        <Container size="xs" style={{ marginTop: "10vh" }}>
            <Paper withBorder>
                <div style={{ padding: "1em" }}>
                    <h3>Registreren</h3>

                    <form onSubmit={handleSubmit}>
                        <Group>
                            <Checkbox
                                mt="md"
                                label="Zakelijke huurder"
                                key={form.key("zakelijk")}
                                {...form.getInputProps("zakelijk", { type: "checkbox" })}
                            />

                            {form.values.zakelijk && (
                                <TextInput
                                    withAsterisk
                                    label="KVK-nummer"
                                    placeholder="123456789"
                                    key={form.key("kvk")}
                                    {...form.getInputProps("kvk")}
                                />
                            )}
                        </Group>

                        {form.values.zakelijk && (
                            <Group>
                                <TextInput
                                    withAsterisk
                                    label="Bedrijfsnaam"
                                    placeholder="Bedrijfsnaam"
                                    key={form.key("bedrijfNaam")}
                                    {...form.getInputProps("bedrijfNaam")}
                                />

                                <TextInput
                                    withAsterisk
                                    label="Bedrijfsadres"
                                    placeholder="Bedrijfsadres"
                                    key={form.key("bedrijfAdres")}
                                    {...form.getInputProps("bedrijfAdres")}
                                />
                            </Group>
                        )}

                        <TextInput
                            withAsterisk
                            label="Emailadres"
                            placeholder="naam@adres.nl"
                            key={form.key("email")}
                            {...form.getInputProps("email")}
                        />

                        <TextInput
                            withAsterisk
                            label="Naam"
                            placeholder="Naam"
                            key={form.key("naam")}
                            {...form.getInputProps("naam")}
                        />

                        <TextInput
                            withAsterisk
                            label="Adres"
                            placeholder="Straat 123"
                            key={form.key("adres")}
                            {...form.getInputProps("adres")}
                        />

                        <PasswordInput
                            required
                            label="Wachtwoord"
                            placeholder="Wachtwoord"
                            mt="md"
                            size="md"
                            {...form.getInputProps("wachtwoord")}
                        />

                        <PasswordInput
                            required
                            label="Wachtwoord bevestigen"
                            placeholder="Wachtwoord"
                            mt="md"
                            size="md"
                            {...form.getInputProps("wachtwoordBevestiging")}
                        />

                        <Button
                            type="submit"
                            fullWidth
                            mt="xl"
                            size="md"
                            disabled={!form.isValid()}
                        >
                            Registreren
                        </Button>
                    </form>
                </div>

                <Group style={{ padding: "1em" }}>
                    <p>Al een account?</p>
                    <Button variant="outline" onClick={naarLogin}>
                        Inloggen
                    </Button>
                </Group>

                {error && <p style={{ color: "red", textAlign: "center" }}>{error}</p>}
            </Paper>
        </Container>
    );
}
