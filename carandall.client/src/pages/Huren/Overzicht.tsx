import { useState } from "react";
import { DateInput } from "@mantine/dates";
import { Button, Group, Grid, Card, Text } from "@mantine/core";
import "@mantine/dates/styles.css";
import { useAuthorisatie } from "../../utilities/useAuthorisatie";

type Voertuig = {
  voertuigID: number;
  kenteken: string;
  soort: string;
  merk: string;
  type: string;
  aanschafjaar: number;
  prijs: number;
  verhuuraanvragen: Record<string, any> | null;
  schademeldingen: Record<string, any> | null;
};

export function Overzicht() {  
  useAuthorisatie(["Particulier", "Zakelijk", "Wagenparkbeheerder"]);
  
  const [ophaalDatum, setOphaalDatum] = useState<Date | null>(null);
  const [inleverDatum, setInleverDatum] = useState<Date | null>(null);
  const [data, setData] = useState<Voertuig[] | null>(null);

  const getVoertuigen = async () => {
    try {
      if (ophaalDatum && inleverDatum) {
        const parameters = new URLSearchParams({
          ophaalDatum: ophaalDatum.toISOString(),
          inleverDatum: inleverDatum.toISOString(),
        });

        const resultaat = await fetch(`http://localhost:5202/api/Huur/GetVoertuigen?${parameters}`, {
          method: "GET",
          credentials: "include",
          headers: {
            "Content-Type": "application/json",
          }
        });

        if (!resultaat.ok) {
          console.log(new Error(`Foutmelding: ${resultaat.status}`));
          throw new Error(`Foutmelding: ${resultaat.status}`);
        }

        const data = await resultaat.json();
        console.log(data)
        
        setData(data);
      }
    } catch (error) {
      console.error(error);
    }
  };

  const voertuigCards = data
    ? data.map((voertuig) => (
        <Grid.Col span={6} key={voertuig.voertuigID}>
          <Card shadow="sm" padding="lg" radius="sm" withBorder>
            <Card.Section withBorder inheritPadding py="xs">
              <Group justify="space-between">
                <Text fw={500}>{voertuig.merk} {voertuig.type}</Text>
                <Text>€{voertuig.prijs} /dag</Text>
              </Group>
            </Card.Section>

            <Card.Section inheritPadding py="xs">
              <Text><Text span>Soort:</Text> {voertuig.soort}</Text>
              <Text><Text span>Aanschafjaar:</Text> {voertuig.aanschafjaar}</Text>
              <Text><Text span>Kenteken:</Text> {voertuig.kenteken}</Text>
            </Card.Section>

            <Button>Verhuuraanvraag indienen</Button>
          </Card>
        </Grid.Col>
      ))
    : null;

  return (
    <div>
      <h2>Huren</h2>
      <p>Wanneer wilt u een voertuig huren?</p>
      <Group>
        <DateInput
          value={ophaalDatum}
          onChange={setOphaalDatum}
          label="Ophaaldatum"
          placeholder="Klik hier om een ophaaldatum te kiezen"
        />
        <DateInput
          value={inleverDatum}
          onChange={setInleverDatum}
          label="Inleverdatum"
          placeholder="Klik hier om een inleverdatum te kiezen"
        />
        <Button onClick={getVoertuigen}>Voertuigen weergeven</Button>
      </Group>

      <Grid gutter="md">
        {voertuigCards}
      </Grid>
    </div>
  );
}
