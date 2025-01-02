import { Button, Checkbox, Group, PasswordInput, TextInput } from "@mantine/core";
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
        },
        validate: {
            email: isEmail('Ongeldig emailadres'),
            naam: isNotEmpty('Vul dit verplichte veld in'),
            adres: (value) =>
                value && /^[a-zA-Z\s]+[0-9]+$/.test(value)
                    ? null
                    : 'Adres moet beginnen met tekst en eindigen op een nummer',
            wachtwoord: (value) =>
                value && value.length >= 8 && /[A-Z]/.test(value) && /\d/.test(value)
                    ? null
                    : 'Wachtwoord moet minimaal 8 tekens lang zijn, een hoofdletter en een cijfer bevatten',
            wachtwoordBevestiging: (value, values) =>
                value === values.wachtwoord
                    ? null
                    : 'Wachtwoord bevestiging moet overeenkomen',
        }
    });

    const navigate = useNavigate();

    const [error, setError] = useState("");

    const naarLogin = () => {
        navigate("/login");
    };

    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        fetch("/register", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                zakelijk: form.values.zakelijk,
                kvk: form.values.kvk,
                email: form.values.email,
                naam: form.values.naam,
                adres: form.values.adres,
                wachtwoord: form.values.wachtwoord,
            }),
        })
        .then((response) => {
            if (response.ok) {
                return response.json();
            }
            throw new Error("Error registering.");
        })
        .then((data) => {
            console.log("Registration successful:", data);
            setError("Registration successful!");
        })
        .catch((error) => {
            console.error(error);
            setError("Error registering.");
        });
    };

    return (
        <div className="containerbox">
            <h3>Register</h3>

            <form onSubmit={handleSubmit}>
                <Group>
                    <Checkbox
                        mt="md"
                        label="Zakelijke huurder"
                        key={form.key('zakelijk')}
                        {...form.getInputProps('zakelijk', { type: 'checkbox' })}
                    />

                    {form.values.zakelijk && (
                        <TextInput
                            withAsterisk
                            label="KVK-nummer"
                            placeholder="123456789"
                            key={form.key('kvk')}
                            {...form.getInputProps('kvk')}
                        />
                    )}
                </Group>

                <TextInput
                    withAsterisk
                    label="Emailadres"
                    placeholder="naam@adres.nl"
                    key={form.key('email')}
                    {...form.getInputProps('email')}
                />

                <TextInput
                    withAsterisk
                    label="Naam"
                    placeholder="Naam"
                    key={form.key('naam')}
                    {...form.getInputProps('naam')}
                />

                <TextInput
                    withAsterisk
                    label="Adres"
                    placeholder="Straat 123"
                    key={form.key('adres')}
                    {...form.getInputProps('adres')}
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
                    fullWidth mt="xl"
                    size="md"
                    disabled={!form.isValid()}
                >
                    Registreren
                </Button>
            </form>

            <div>
                <p>Al een account?</p>
                <Button onClick={naarLogin}>Inloggen</Button>
            </div>

            {error && <p className="error">{error}</p>}
        </div>
    );
}
