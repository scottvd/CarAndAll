import { useEffect, useState } from "react";
import { Table, Button, TextInput, Group } from "@mantine/core";
import { useNavigate } from "react-router-dom";
import '../../styles/table.css';
import { IconSearch } from "@tabler/icons-react";

type Voertuig = {
  voertuigID: number;
  kenteken: string;
  soort: string;
  merk: string;
  type: string;
  aanschafjaar: number;
  verhuuraanvragen: Record<string, any> | null;
  schademeldingen: Record<string, any> | null;
};

export function Voertuigen() {
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

  const huren = (voertuig: Voertuig) => {
    navigate("/dashboard/voertuigen/voertuig", { state: { voertuig } });
  };

  const zoekVoertuig = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { value } = event.currentTarget;
    setZoekwaarde(value);
  };

  const rows = data.map((voertuig) => (
    <tr key={voertuig.voertuigID}>
      <td>{voertuig.kenteken}</td>
      <td>{voertuig.soort}</td>
      <td>{voertuig.merk}</td>
      <td>{voertuig.type}</td>
      <td>{voertuig.aanschafjaar}</td>
      <td className="button">
        <Button onClick={() => huren(voertuig)}>Bewerken</Button>
      </td>
    </tr>
  ));

  return (
    <div>
      <h2>Controlepaneel CarAndAll vloot</h2>

      <div>
        <Group>
          <TextInput
            placeholder="Zoek een voertuig"
            mb="md"
            leftSection={<IconSearch size={16} stroke={1.5} />}
            value={zoekwaarde}
            onChange={zoekVoertuig}
          />
          <Button mb="md" onClick={() => navigate('/dashboard/voertuigen/toevoegen')}>Voertuig toevoegen</Button>
        </Group>

        <div>
          <Table border={1} className="basistabel">
            <thead>
              <tr>
                <th>Kenteken</th>
                <th>Soort</th>
                <th>Merk</th>
                <th>Type</th>
                <th>Aanschafjaar</th>
                </tr>
              </thead>
              <tbody>{rows}</tbody>
            </Table>
          </div>            
        </div>
      </div>
  );
}
