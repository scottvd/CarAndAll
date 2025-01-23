import { Button, Checkbox, Group, Modal, PasswordInput, Table, TextInput } from "@mantine/core";
import { useAuthorisatie } from "../../utilities/useAuthorisatie";
import { useEffect, useState } from "react";
import { isEmail, isNotEmpty, useForm } from "@mantine/form";
import { getCsrfToken } from "../../utilities/getCsrfToken";
import { Medewerker } from "../../types/Types";
import { useNotificaties } from "../../utilities/NotificatieContext";

export function Medewerkers() {
  useAuthorisatie(["BackofficeMedewerker"]);
  const { addNotificatie } = useNotificaties();   
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
    const csrfToken = getCsrfToken();

    if (!isValid.hasErrors) {
      if(csrfToken) {
        try {
          const { wachtwoordBevestiging, ...newData } = createFormulier.values;

          const resultaat = await fetch("http://localhost:5202/api/Medewerker/AddMedewerker", {
            method: "POST",
            headers: {
              "Content-Type": "application/json",
              "X-CSRF-Token": csrfToken
            },
            credentials: "include",
            body: JSON.stringify(newData)
          });

          if (resultaat.ok) {
            addNotificatie("Medewerker toegevoegd!", "success", false);
            await getMedewerkers();
            createFormulier.reset();
          } else {
            addNotificatie("Er is iets fout gegaan tijdens het toevoegen van de medewerker. Probeer het opnieuw!", "error", true);
          }
        } catch (error) {
          console.error("Fout: ", error);
        }
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
    const csrfToken = getCsrfToken();

    if (!isValid.hasErrors) {
      if(csrfToken) {
        try {
          const { oudWachtwoord: wachtwoordBevestiging, ...updatedData } = editFormulier.values;
          const bewerkteMedewerker = {...updatedData, medewerkerID: selectedMedewerker?.id}

          const resultaat = await fetch("http://localhost:5202/api/Medewerker/EditMedewerker",
            {
              method: "PUT",
              headers: {
                "Content-Type": "application/json",
                "X-CSRF-Token": csrfToken
              },
              credentials: "include",
              body: JSON.stringify(bewerkteMedewerker)
            }
          );

          if (resultaat.ok) {
            addNotificatie("Medewerker bijgewerkt!", "success", false);
            setIsModalOpen(false);
            await getMedewerkers();
          } else {
            addNotificatie("Er is iets fout gegaan tijdens het bijwerken van de medewerker. Probeer het opnieuw!", "error", true);
          }
        } catch (error) {
          console.error("Fout: ", error);
        }
      }
    }
  };

  const handleVerwijderen = async (id: string) => {
    const csrfToken = getCsrfToken();

    if(csrfToken) {
      try {
        const resultaat = await fetch("http://localhost:5202/api/Medewerker/DeleteMedewerker", {
          method: "DELETE",
          headers: {
            "Content-Type": "application/json",
            "X-CSRF-Token": csrfToken
          },
          credentials: "include",
          body: JSON.stringify(id)
        });

        if (resultaat.ok) {
          addNotificatie("Medewerker succesvol verwijderd!", "success", false);
          setIsModalOpen(false);
          await getMedewerkers();
        } else {
          const errorMessage = await resultaat.text();
          addNotificatie(`Er is iets fout gegaan: ${errorMessage}`, "error", true);
        }
      } catch (error) {
        console.error("Fout: ", error);
      }
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
          <Button color="#28282B" onClick={() => openModal(medewerker)}>Bewerken</Button>
        </td>
      </tr>
    ));
  };

  return (
    <div>
      <h1>Medewerkers beheren</h1>

      <form onSubmit={handleToevoegen}>
        <h2>Medewerker toevoegen</h2>
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
            placeholder="Wachtwoord bevestigen"
            mt="md"
            size="md"
            {...createFormulier.getInputProps("wachtwoordBevestiging")}
          />
        </Group>

        <Button color="#2E8540" type="submit" fullWidth mt="xl" size="md">
          Toevoegen
        </Button>
      </form>

      <h2>Medewerkers beheren</h2>
      <Table border={1} className="basistabel">
        <thead>
          <tr>
            <th scope="col">Naam</th>
            <th scope="col">Personeelsnummer</th>
            <th scope="col">Emailadres</th>
            <th scope="col">Rol</th>
            <th scope="col">Acties</th>
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
            <Button color="#2E8540" onClick={handleBewerken} fullWidth mt="xl">
              Opslaan
            </Button>

            <Button color="#E31C3D" onClick={() => selectedMedewerker && handleVerwijderen(selectedMedewerker.id)} fullWidth mt="xl">
              Verwijder medewerker
            </Button>
          </Group>
        </form>
      </Modal>
    </div>
  );
}
