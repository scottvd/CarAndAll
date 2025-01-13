import { Button, Checkbox, Group, Modal, PasswordInput, Table, TextInput } from "@mantine/core";
import { useAuthorisatie } from "../../utilities/useAuthorisatie";
import { useEffect, useState } from "react";
import { isEmail, isNotEmpty, useForm } from "@mantine/form";
import { fetchCsrf } from "../../utilities/fetchCsrf";

type Medewerker = {
  id: string;
  naam: string;
  personeelsNummer: number;
  email: string;
  wachtwoord?: string;
  rol: string;
};

export function Medewerkers() {
  useAuthorisatie(["BackofficeMedewerker"]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [data, setData] = useState<Medewerker[]>([]);
  const [selectedMedewerker, setSelectedMedewerker] = useState<Medewerker | null>(null);

  const createFormulier = useForm({
    mode: "controlled",
    initialValues: {
      naam: "",
      personeelsNummer: 0,
      email: "",
      wachtwoord: "",
      wachtwoordBevestiging: "",
    },
    validate: {
      naam: isNotEmpty("Vul dit verplichte veld in"),
      personeelsNummer: isNotEmpty("Vul dit verplichte veld in"),
      email: isEmail("Ongeldig emailadres"),
      wachtwoord: (value) =>
        value &&
        value.length >= 8 &&
        /[A-Z]/.test(value) &&
        /\d/.test(value)
          ? null
          : "Wachtwoord moet minimaal 8 tekens lang zijn, een hoofdletter en een cijfer bevatten",
      wachtwoordBevestiging: (value, values) =>
        value === values.wachtwoord ? null : "Wachtwoord bevestiging moet overeenkomen",
    },
  });

  const editFormulier = useForm({
    mode: "controlled",
    initialValues: {
      naam: "",
      personeelsNummer: 0,
      email: "",
      nieuwWachtwoord: "",
      oudWachtwoord: "",
      backoffice: false,
    },
    validate: {
      naam: isNotEmpty("Vul dit verplichte veld in"),
      personeelsNummer: isNotEmpty("Vul dit verplichte veld in"),
      email: isEmail("Ongeldig emailadres"),
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
    }
  });
  

  const getMedewerkers = async () => {
    try {
      const resultaat = await fetch(
        "http://localhost:5202/api/Medewerker/GetMedewerkers",
        { credentials: "include" }
      );

      if (!resultaat.ok) {
        throw new Error(`Foutmelding: ${resultaat.status}`);
      }

      const fetchedData = await resultaat.json();
      setData(fetchedData);
    } catch (error) {
      console.error(error);
    }
  };

  useEffect(() => {
    getMedewerkers();
  }, []);

  const handleToevoegen = async (event: React.FormEvent) => {
    event.preventDefault();
    const isValid = await createFormulier.validate();

    if (!isValid.hasErrors) {
      try {
        const { wachtwoordBevestiging, ...newData } = createFormulier.values;

        const resultaat = await fetchCsrf("http://localhost:5202/api/Medewerker/AddMedewerker", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          credentials: "include",
          body: JSON.stringify(newData),
        });

        if (resultaat.ok) {
          alert("Medewerker toegevoegd!");
          await getMedewerkers();
          createFormulier.reset();
        } else {
          alert("Er is iets fout gegaan tijdens het toevoegen van de medewerker. Probeer het opnieuw!");
        }
      } catch (error) {
        console.error("Fout: ", error);
      }
    }
  };

  const openModal = (medewerker: Medewerker) => {
    console.log(medewerker);

    setSelectedMedewerker(medewerker);
    editFormulier.setValues({
      naam: medewerker.naam,
      personeelsNummer: medewerker.personeelsNummer,
      email: medewerker.email,
    });
    setIsModalOpen(true);
  };

  const handleBewerken = async () => {
    const isValid = await editFormulier.validate();

    if (!isValid.hasErrors) {
      try {
        const { oudWachtwoord: wachtwoordBevestiging, ...updatedData } = editFormulier.values;
        const bewerkteMedewerker = {...updatedData, medewerkerID: selectedMedewerker?.id}

        console.log(bewerkteMedewerker);

        const resultaat = await fetchCsrf("http://localhost:5202/api/Medewerker/EditMedewerker",
          {
            method: "PUT",
            headers: {
              "Content-Type": "application/json",
            },
            credentials: "include",
            body: JSON.stringify(bewerkteMedewerker),
          }
        );

        if (resultaat.ok) {
          alert("Medewerker bijgewerkt!");
          setIsModalOpen(false);
          await getMedewerkers();
        } else {
          alert("Er is iets fout gegaan tijdens het bijwerken van de medewerker. Probeer het opnieuw!");
        }
      } catch (error) {
        console.error("Fout: ", error);
      }
    }
  };

  const handleVerwijderen = async (id: string) => {
    try {
      const resultaat = await fetchCsrf("http://localhost:5202/api/Medewerker/DeleteMedewerker", {
        method: "DELETE",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
        body: JSON.stringify(id),
      });

      if (resultaat.ok) {
        alert("Medewerker succesvol verwijderd!");
        setIsModalOpen(false);
        await getMedewerkers();
      } else {
        const errorMessage = await resultaat.text();
        alert(`Er is iets fout gegaan: ${errorMessage}`);
      }
    } catch (error) {
      console.error("Fout: ", error);
      alert("Er is een fout opgetreden bij het verwijderen van de medewerker.");
    }
  };
  

  const renderRows = () => {
    return data.map((medewerker) => (
      <tr key={medewerker.id}>
        <td>{medewerker.naam}</td>
        <td>{medewerker.personeelsNummer}</td>
        <td>{medewerker.email}</td>
        <td>{medewerker.rol}</td>
        <td>
          <Button onClick={() => openModal(medewerker)}>Bewerken</Button>
        </td>
      </tr>
    ));
  };

  return (
    <div>
      <h2>Medewerkers beheren</h2>

      <form onSubmit={handleToevoegen}>
        <h4>Medewerker toevoegen</h4>
        <Group grow>
          <TextInput
            withAsterisk
            label="Naam"
            placeholder="Naam"
            {...createFormulier.getInputProps("naam")}
          />

          <TextInput
            withAsterisk
            label="Personeelsnummer"
            placeholder="Personeelsnummer"
            {...createFormulier.getInputProps("personeelsNummer")}
          />

          <TextInput
            withAsterisk
            label="Emailadres"
            placeholder="medewerker@caa.nl"
            {...createFormulier.getInputProps("email")}
          />
        </Group>

        <Group grow>
          <PasswordInput
            label="Wachtwoord"
            placeholder="Wachtwoord"
            mt="md"
            size="md"
            {...createFormulier.getInputProps("wachtwoord")}
          />

          <PasswordInput
            label="Wachtwoord bevestigen"
            placeholder="Wachtwoord"
            mt="md"
            size="md"
            {...createFormulier.getInputProps("wachtwoordBevestiging")}
          />
        </Group>

        <Button color="green" type="submit" fullWidth mt="xl" size="md">
          Toevoegen
        </Button>
      </form>

      <h4>Medewerkers beheren</h4>
      <Table border={1} className="basistabel">
        <thead>
          <tr>
            <th>Naam</th>
            <th>Personeelsnummer</th>
            <th>Emailadres</th>
            <th>Rol</th>
            <th>Acties</th>
          </tr>
        </thead>
        <tbody>{renderRows()}</tbody>
      </Table>

      <Modal
        opened={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title="Medewerker bewerken"
      >
        <form onSubmit={(e) => e.preventDefault()}>
          <Group grow>
            <TextInput
              withAsterisk
              label="Naam"
              placeholder="Naam"
              {...editFormulier.getInputProps("naam")}
            />
            <TextInput
              withAsterisk
              label="Personeelsnummer"
              placeholder="Personeelsnummer"
              {...editFormulier.getInputProps("personeelsNummer")}
            />
          </Group>

          <TextInput
            withAsterisk
            label="Emailadres"
            placeholder="Emailadres"
            {...editFormulier.getInputProps("email")}
          />

          <PasswordInput
            label="Nieuw wachtwoord (optioneel)"
            placeholder="Wachtwoord"
            {...editFormulier.getInputProps("nieuwWachtwoord")}
          />

          <PasswordInput
            label="Oud wachtwoord"
            placeholder="Oud wachtwoord"
            {...editFormulier.getInputProps("oudWachtwoord")}
          />

          <Checkbox label="Geef medewerker backoffice permissies" {...editFormulier.getInputProps("backoffice")} />

          <Group justify="space-between" grow>
            <Button color="green" onClick={handleBewerken} fullWidth mt="xl">
              Opslaan
            </Button>

            <Button color="red" onClick={() => selectedMedewerker && handleVerwijderen(selectedMedewerker.id)} fullWidth mt="xl">
              Verwijder medewerker
            </Button>
          </Group>
        </form>
      </Modal>
    </div>
  );
}
