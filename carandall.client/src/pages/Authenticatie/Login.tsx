import { Button, Checkbox, Group, PasswordInput, TextInput, Paper, Container } from "@mantine/core";
import { isNotEmpty, useForm } from "@mantine/form";
import { useState } from "react";

export function Login() {
    const form = useForm({
        initialValues: {
            email: "",
            wachtwoord: "",
        },
        validate: {
            email: isNotEmpty("Vul een emailadres in."),
            wachtwoord: isNotEmpty("Vul een wachtwoord in.")
        },
    });

    const [error, setError] = useState("");

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        if (!form.isValid()) {
            setError("Please fix the errors in the form.");
            return;
        }

        try {
            const requestBody: any = {
                Email: form.values.email,
                Wachtwoord: form.values.wachtwoord
            };

            const resultaat = await fetch("http://localhost:5202/api/Authenticatie/LogIn", {
                method: "POST",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(requestBody)
            });

            if (resultaat.ok) {
                alert("U bent ingelogd!");
                form.reset();
            } else {
                console.log(await resultaat.json());
                alert("Er is iets fout gegaan tijdens het inloggen. Probeer het opnieuw.");
            }
        } catch (error) {
            console.error("Fout: ", error);
        }
    };

    return (
        <Container size="xs" style={{ marginTop: "10vh" }}>
            <Paper withBorder>
                <div style={{ padding: "1em" }}>
                    <h3>Inloggen</h3>

                    <form onSubmit={handleSubmit}>
                        <TextInput
                            required
                            label="Emailadres"
                            placeholder="naam@adres.nl"
                            key={form.key("email")}
                            {...form.getInputProps("email")}
                        />

                        <PasswordInput
                            required
                            label="Wachtwoord"
                            placeholder="Wachtwoord"
                            mt="md"
                            size="md"
                            {...form.getInputProps("wachtwoord")}
                        />

                        <Button
                            type="submit"
                            fullWidth
                            mt="xl"
                            size="md"
                            disabled={!form.isValid()}
                        >
                            Inloggen
                        </Button>
                    </form>
                </div>

                {error && <p style={{ color: "red", textAlign: "center" }}>{error}</p>}
            </Paper>
        </Container>
    );
}
