import { useEffect, useState } from "react";
import { Table, Button, TextInput, Group } from "@mantine/core";
import { useNavigate } from "react-router-dom";
import '../../styles/table.css';
import { IconSearch } from "@tabler/icons-react";
import { useAuthorisatie } from "../../utilities/useAuthorisatie";
import { Voertuig } from "../../types/Types";

export function Voertuigen() {
  useAuthorisatie(["BackofficeMedewerker", "FrontofficeMedewerker"]);

  const [data, setData] = useState<Voertuig[] | null>(null);
  const navigate = useNavigate(); 
  const [zoekwaarde, setZoekwaarde] = useState('');

  useEffect(() => {
    const getVoertuigen = async () => {
      try {
        const resultaat = await fetch('http://localhost:5202/api/VoertuigBeheer/GetAllVoertuigen', { credentials: 'include' });

        if (!resultaat.ok) {
          throw new Error(`Foutmelding: ${resultaat.status}`);
        }

        const data = await resultaat.json();
        setData(data);
      } catch (error) {
        console.error(error);
      }
    };

    getVoertuigen();
  }, []);

  if (!data) return <div><p>De pagina is aan het laden. Een ogenblik geduld, alstublieft...</p></div>;

  const bewerken = (voertuig: Voertuig) => {
    navigate("/dashboard/voertuigen/voertuig", { state: { voertuig } });
  };

  const zoekVoertuig = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { value } = event.currentTarget;
    setZoekwaarde(value);
  };

  const rows = data.map((voertuig) => (
    <tr key={voertuig.voertuigId}>
      <td>{voertuig.kenteken}</td>
      <td>{voertuig.soort}</td>
      <td>{voertuig.merk}</td>
      <td>{voertuig.type}</td>
      <td>{voertuig.aanschafjaar}</td>
      <td className="button">
        <Button 
          aria-label={`Bewerk de ${voertuig.soort} ${voertuig.merk} ${voertuig.type} met kenteken ${voertuig.kenteken} en aanschafjaar ${voertuig.aanschafjaar}`} color="#28282B" onClick={() => bewerken(voertuig)}
        >
          Bewerken
        </Button>
      </td>
    </tr>
  ));

  return (
    <div>
      <h1>Controlepaneel CarAndAll vloot</h1>

      <div>
        <Group>
          <TextInput
            placeholder="Zoek een voertuig"
            mb="md"
            leftSection={<IconSearch size={16} stroke={1.5} />}
            value={zoekwaarde}
            onChange={zoekVoertuig}
          />
          <Button color="#28282B" mb="md" onClick={() => navigate('/dashboard/voertuigen/toevoegen')}>Voertuig toevoegen</Button>
        </Group>

        <div>
          <Table border={1} className="basistabel">
            <caption>Overzicht van de voertuigen</caption>
            <thead>
              <tr>
                <th scope="col">Kenteken</th>
                <th scope="col">Soort</th>
                <th scope="col">Merk</th>
                <th scope="col">Type</th>
                <th scope="col">Aanschafjaar</th>
              </tr>
            </thead>
              
            <tbody>
              {rows}
            </tbody>
          </Table>
        </div>            
      </div>
    </div>
  );
}
