import { Button, PasswordInput, TextInput, Paper, Container } from "@mantine/core";
import { isNotEmpty, useForm } from "@mantine/form";
import { useNotificaties } from "../../utilities/NotificatieContext";
import { useNavigate } from "react-router-dom";

export function Login() {
  const form = useForm({
    initialValues: {
      email: "",
      wachtwoord: "",
    },
    validate: {
      email: isNotEmpty("Vul een emailadres in."),
      wachtwoord: isNotEmpty("Vul een wachtwoord in."),
    },
  });

  const { addNotificatie } = useNotificaties();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (!form.isValid()) {
      addNotificatie("Het invullen van een emailadres en wachtwoord is verplicht.", "error", true);
      return;
    }

    try {
      const requestBody: any = {
        Email: form.values.email,
        Wachtwoord: form.values.wachtwoord,
      };

      const resultaat = await fetch("http://localhost:5202/api/Authenticatie/LogIn", {
        method: "POST",
        credentials: "include",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(requestBody),
      });

      const response = await resultaat.json()

      if (resultaat.ok) {
        if(response.verlopen) {
          addNotificatie("Uw wachtwoord is verlopen. U moet uw wachtwoord resetten.", "info", true);
          navigate("/reset-wachtwoord", { state: { email: form.values.email } });
          return;
        }

        addNotificatie("U bent ingelogd! U wordt naar het dashboard gebracht.", "success", false);
        form.reset();
        navigate("/dashboard"); 
      } else {
        addNotificatie("Er is iets fout gegaan tijdens het inloggen. Probeer het opnieuw.", "error", true);
      }
    } catch (error) {
      if (error) {
        addNotificatie(`Fout: ${error}`, "error", true);
      }
    }
  };

  return (
    <main>
        <Container size="xs" style={{ marginTop: "10vh" }}>
            <Paper withBorder>
                <div style={{ padding: "1em" }}>
                    <h1>Inloggen</h1>

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
                            color="#28282B"
                            disabled={!form.isValid()}
                        >
                            Inloggen
                        </Button>
                    </form>
                </div>
            </Paper>
        </Container>
    </main>
  );
}
