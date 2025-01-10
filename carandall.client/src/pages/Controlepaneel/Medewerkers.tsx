import { Button, Group, Modal, PasswordInput, Table, TextInput } from "@mantine/core";
import { useAuthorisatie } from "../../utilities/useAuthorisatie";
import { useEffect, useState } from "react";
import { isEmail, isNotEmpty, useForm } from "@mantine/form";

type Medewerker = {
  medewerkerID: string;
  naam: string;
  personeelsnummer: number;
  email: string;
  wachtwoord: string;
  permissies: string[];
}

export function Medewerkers() {
  useAuthorisatie(["BackofficeMedewerker"]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [data, setData] = useState<Medewerker[] | null>(null);
  
  const form = useForm({
    mode: 'controlled',
    initialValues: {
      naam: "",
      personeelsnummer: null,
      email: "",
      wachtwoord: "",
      wachtwoordBevestiging: ""
    },
    validate: {
      naam: isNotEmpty("Vul dit verplichte veld in"),
      personeelsnummer: isNotEmpty("Vul dit verplichte veld in"),
      email: isEmail("Ongeldig emailadres"),
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
          : "Wachtwoord bevestiging moet overeenkomen"
    },
  });

  useEffect(() => {
    const getMedewerkers = async () => {
      try {
        const resultaat = await fetch('http://localhost:5202/api/Medewerker/GetMedewerkers', { credentials: 'include' });

        if (!resultaat.ok) {
          throw new Error(`Foutmelding: ${resultaat.status}`);
        }
  
        const data = await resultaat.json();
        console.log(data);
        setData(data);
      } catch (error) {
        console.error(error);
      }
    };  
      getMedewerkers();
    }, []);

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    
    const isValid = await form.validate();
    
    if (isValid.hasErrors) {
      console.log("Form has errors");
    } else {
      console.log("Form submitted successfully", form.values);
    }
  };

  const renderRows = (data: Medewerker[]) => {
    return data.map((data) => (
      <tr key={data.medewerkerID}>
        <td>{data.naam}</td>
        <td>{data.personeelsnummer}</td>
        <td>{data.email}</td>
        <td>{data.permissies}</td>
        <td>
          <Button onClick={() => setIsModalOpen(true)}>Bewerkern</Button>
        </td>
      </tr>
    ));
  };

  return (
    <div>
      <h2>Medewerkers beheren</h2>

      <form onSubmit={handleSubmit}>
        <h4>Medewerker toevoegen</h4>
        <Group grow>
          <TextInput
            withAsterisk
            label="Naam"
            placeholder="Naam"
            {...form.getInputProps("naam")}
          />
            
          <TextInput
            withAsterisk
            label="Personeelsnummer"
            placeholder="Personeelsnummer"
            {...form.getInputProps("personeelsnummer")}
          />

          <TextInput
            withAsterisk
            label="Emailadres"
            placeholder="medewerker@caa.nl"
            {...form.getInputProps("email")}
          />
        </Group>
        
        <Group grow>
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
            <th>Permissies</th>
          </tr>
        </thead>
        
        <tbody>
          
        </tbody>
      </Table>
      
      <Modal
        opened={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title="Medewerker toevoegen"
      >
        <p></p>
      </Modal>
    </div>
  );
}
