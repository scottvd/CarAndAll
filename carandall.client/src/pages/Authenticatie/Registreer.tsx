import { Button, Checkbox, Group, PasswordInput, TextInput, Paper, Container } from "@mantine/core";
import { isEmail, isNotEmpty, useForm } from "@mantine/form";
import { useNavigate } from "react-router-dom";
import { useNotificaties } from "../../utilities/NotificatieContext";

export function Register() {
  const form = useForm({
    mode: 'controlled',
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
      naam: isNotEmpty("Vul dit verplichte veld in"),
      email: isEmail("Ongeldig emailadres"),
      adres: (value, values) =>
        !values.zakelijk && (!value || value.trim() === "")
          ? "Adres is verplicht"
          : !values.zakelijk && !/^[a-zA-Z\s]+[0-9]+$/.test(value)
          ? "Adres moet beginnen met tekst en eindigen op een nummer"
          : null,
      kvk: (value, values) =>
        values.zakelijk && !value
          ? "KVK-nummer is verplicht voor zakelijke huurders"
          : null,
      bedrijfNaam: (value, values) =>
        values.zakelijk && !value ? "Bedrijfsnaam is verplicht" : null,
      bedrijfAdres: (value, values) =>
        values.zakelijk && (!value || value.trim() === "")
          ? "Adres is verplicht"
          : values.zakelijk && !/^[a-zA-Z\s]+[0-9]+$/.test(value)
          ? "Adres moet beginnen met tekst en eindigen op een nummer"
          : null,
      wachtwoord: (value) =>
        value &&
        value.length >= 8 &&
        /[A-Z]/.test(value) &&
        /\d/.test(value) &&
        /[!@#$%^&*(),.?":{}|<>]/.test(value)
          ? null
          : "Wachtwoord moet minimaal 8 tekens lang zijn, een hoofdletter, een cijfer en een leesteken bevatten",
      wachtwoordBevestiging: (value, values) =>
        value === values.wachtwoord
          ? null
          : "Wachtwoord bevestiging moet overeenkomen"
    },
  });

  const { addNotificatie } = useNotificaties();
  const navigate = useNavigate();

  const naarLogin = () => {
    navigate("/login");
  };

  const handleFormSubmit = async (values: typeof form.values) => {
    try {
      const requestBody: any = {
        Zakelijk: values.zakelijk,
        Email: values.email,
        Naam: values.naam,
        Adres: values.adres,
        Wachtwoord: values.wachtwoord
      };

      if (values.zakelijk) {
        requestBody.Kvk = Number(values.kvk);
        requestBody.BedrijfNaam = values.bedrijfNaam;
        requestBody.BedrijfAdres = values.bedrijfAdres;
      }

      const resultaat = await fetch("http://localhost:5202/api/Registratie/Registreer", {
        method: "POST",
        credentials: "include",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify(requestBody)
      });

      if (resultaat.ok) {
        addNotificatie("U bent geregistreerd! U wordt naar de login pagina gebracht.");
        form.reset();
        naarLogin();
      } else {
        addNotificatie("Er is iets fout gegaan tijdens het aanmaken van uw account. Probeer het opnieuw!");
      }
    } catch (error) {
      addNotificatie("Fout: " + error);
    }
  };

  return (
    <main>
      <Container size="xs" style={{ marginTop: "8vh" }}>
        <Paper withBorder>
          <div style={{ padding: "1em" }}>
            <h1>Registreren</h1>

            <form onSubmit={form.onSubmit(handleFormSubmit)}>
              <TextInput
                withAsterisk
                label="Naam"
                placeholder="Naam"
                {...form.getInputProps("naam")}
              />

              <TextInput
                withAsterisk
                label="Emailadres"
                placeholder="naam@adres.nl"
                {...form.getInputProps("email")}
              />

              <Group>
                <Checkbox
                  mt="md"
                  label="Zakelijke huurder"
                  {...form.getInputProps("zakelijk", { type: "checkbox" })}
                />

                {form.values.zakelijk && (
                  <TextInput
                    withAsterisk
                    label="KVK-nummer"
                    placeholder="123456789"
                    {...form.getInputProps("kvk")}
                  />
                )}
              </Group>

              {form.values.zakelijk ? (
                <Group>
                  <TextInput
                    withAsterisk
                    label="Bedrijfsnaam"
                    placeholder="Bedrijfsnaam"
                    {...form.getInputProps("bedrijfNaam")}
                  />

                  <TextInput
                    withAsterisk
                    label="Bedrijfsadres"
                    placeholder="Bedrijfsadres"
                    {...form.getInputProps("bedrijfAdres")}
                  />
                </Group>
              ) : (
                <TextInput
                  withAsterisk
                  label="Adres"
                  placeholder="Straat 123"
                  {...form.getInputProps("adres")}
                />
              )}

              <PasswordInput
                label="Wachtwoord"
                placeholder="Wachtwoord"
                mt="md"
                size="md"
                {...form.getInputProps("wachtwoord")}
              />

              <PasswordInput
                label="Wachtwoord bevestigen"
                placeholder="Wachtwoord"
                mt="md"
                size="md"
                {...form.getInputProps("wachtwoordBevestiging")}
              />

              <Button type="submit" fullWidth mt="xl" size="md" color="#28282B">
                Registreren
              </Button>
            </form>
          </div>

          <Group style={{ padding: "1em" }}>
            <p>Al een account?</p>
            <Button variant="outline" color="#28282B" onClick={naarLogin}>
              Inloggen
            </Button>
          </Group>
        </Paper>
      </Container>
    </main>
  );
}
