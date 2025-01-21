import { Button, PasswordInput, Container, Paper } from "@mantine/core";
import { isNotEmpty, useForm } from "@mantine/form";
import { useNotificaties } from "../../utilities/NotificatieContext";
import { useNavigate, useLocation } from "react-router-dom";

export function ResetWachtwoord() {
  const form = useForm({
    initialValues: {
      huidigWachtwoord: "",
      nieuwWachtwoord: "",
      bevestigNieuwWachtwoord: "",
    },
    validate: {
      huidigWachtwoord: isNotEmpty("Vul je huidige wachtwoord in."),
      nieuwWachtwoord: (value) =>
        value && value.length >= 8 && /[A-Z]/.test(value) && /\d/.test(value)
          ? null
          : "Wachtwoord moet minimaal 8 tekens lang zijn, een hoofdletter en een cijfer bevatten",
      bevestigNieuwWachtwoord: (value, values) =>
        value === values.nieuwWachtwoord
          ? null
          : "Wachtwoord bevestiging moet overeenkomen",
    },
  });

  const { addNotificatie } = useNotificaties();
  const navigate = useNavigate();
  const location = useLocation();

  const email = location.state?.email;

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (!form.isValid()) {
      addNotificatie("Alle velden moeten correct ingevuld zijn.", "error", true);
      return;
    }

    try {
      const requestBody = {
        HuidigWachtwoord: form.values.huidigWachtwoord,
        NieuwWachtwoord: form.values.nieuwWachtwoord,
        Email: email
      };

      const resultaat = await fetch("http://localhost:5202/api/Authenticatie/ResetWachtwoord", {
        method: "PUT",
        credentials: "include",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(requestBody),
      });

      if (resultaat.ok) {
        addNotificatie("Uw wachtwoord is succesvol gewijzigd!", "success", false);
        form.reset();
        navigate("/login");
      } else {
        addNotificatie("Er is iets fout gegaan bij het wijzigen van je wachtwoord. Probeer het opnieuw.", "error", true);
      }
    } catch (error) {
      addNotificatie(`Fout: ${error}`, "error", true);
    }
  };

  return (
    <main>
      <Container size="xs" style={{ marginTop: "10vh" }}>
        <Paper withBorder>
          <div style={{ padding: "1em" }}>
            <h1>Wachtwoord wijzigen</h1>

            <form onSubmit={handleSubmit}>
              <PasswordInput
                required
                label="Huidig Wachtwoord"
                placeholder="Huidig Wachtwoord"
                mt="md"
                size="md"
                {...form.getInputProps("huidigWachtwoord")}
              />

              <PasswordInput
                required
                label="Nieuw Wachtwoord"
                placeholder="Nieuw Wachtwoord"
                mt="md"
                size="md"
                {...form.getInputProps("nieuwWachtwoord")}
              />

              <PasswordInput
                required
                label="Bevestig Nieuw Wachtwoord"
                placeholder="Bevestig Nieuw Wachtwoord"
                mt="md"
                size="md"
                {...form.getInputProps("bevestigNieuwWachtwoord")}
              />

              <Button
                type="submit"
                fullWidth
                mt="xl"
                size="md"
                color="#28282B"
                disabled={!form.isValid()}
              >
                Wachtwoord wijzigen
              </Button>
            </form>
          </div>
        </Paper>
      </Container>
    </main>
  );
}
