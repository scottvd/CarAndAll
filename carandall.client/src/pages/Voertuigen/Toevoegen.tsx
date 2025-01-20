import React from "react";
import { TextInput, NumberInput, Button, Group } from "@mantine/core";
import { useForm } from "@mantine/form";
import { getCsrfToken } from "../../utilities/getCsrfToken";
import { useAuthorisatie } from "../../utilities/useAuthorisatie";
import { useNotificaties } from "../../utilities/NotificatieContext";
import { useNavigate } from "react-router-dom";

export function Toevoegen() {
  useAuthorisatie(["BackofficeMedewerker", "FrontofficeMedewerker"]);
  const { addNotificatie } = useNotificaties();
  const navigate = useNavigate();

  const form = useForm({
    initialValues: {
      kenteken: "",
      soort: "",
      merk: "",
      type: "",
      aanschafjaar: undefined,
    },
    validate: {
      kenteken: (value) => (value.trim().length === 0 ? "Kenteken is verplicht" : null),
      soort: (value) => (value.trim().length === 0 ? "Soort is verplicht" : null),
      merk: (value) => (value.trim().length === 0 ? "Merk is verplicht" : null),
      type: (value) => (value.trim().length === 0 ? "Type is verplicht" : null),
      aanschafjaar: (value) =>
        value && (value < 1900 || value > new Date().getFullYear())
          ? `Aanschafjaar moet tussen 1900 en ${new Date().getFullYear()} liggen`
          : null,
    },
  });

  const voertuigToevoegen = async (values: typeof form.values) => {
    const csrfToken = getCsrfToken();

    if (csrfToken) {
      try {
        const resultaat = await fetch("http://localhost:5202/api/VoertuigBeheer/AddVoertuig", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            "X-CSRF-Token": csrfToken,
          },
          credentials: "include",
          body: JSON.stringify(values),
        });

        if (resultaat.ok) {
          const newVoertuig = await resultaat.json();
          addNotificatie("Voertuig toegevoegd!", "success", false);
          navigate("/dashboard/voertuigen/voertuig", { state: { voertuig: newVoertuig } });
        } else {
          addNotificatie(
            "Er is iets fout gegaan tijdens het toevoegen van het voertuig. Probeer het opnieuw!",
            "error",
            true
          );
        }
      } catch (error) {
        console.error("Fout: ", error);
      }
    }
  };

  return (
    <div>
      <h1>Voertuig toevoegen</h1>

      <form onSubmit={form.onSubmit(voertuigToevoegen)}>
        <TextInput
          label="Kenteken"
          placeholder="Vul het kenteken in"
          {...form.getInputProps("kenteken")}
          required
        />

        <TextInput
          label="Soort"
          placeholder="Voer het soort voertuig in"
          {...form.getInputProps("soort")}
          required
        />

        <TextInput
          label="Merk"
          placeholder="Voer het merk in"
          {...form.getInputProps("merk")}
          required
        />

        <TextInput
          label="Type"
          placeholder="Voer het type voertuig in"
          {...form.getInputProps("type")}
          required
        />

        <NumberInput
          label="Aanschafjaar"
          placeholder="Voer het aanschafjaar in"
          min={1900}
          max={new Date().getFullYear()}
          {...form.getInputProps("aanschafjaar")}
          required
        />

        <Group mt="md">
          <Button color="#2E8540" type="submit">
            Toevoegen
          </Button>
        </Group>
      </form>
    </div>
  );
}