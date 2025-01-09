import { useState } from "react";
import { DateInput } from "@mantine/dates";
import { Button, Group, Grid, Card, Text, Modal } from "@mantine/core";
import "@mantine/dates/styles.css";
import { useAuthorisatie } from "../../utilities/useAuthorisatie";
import { fetchCsrf } from "../../utilities/fetchCsrf";

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
  const [selectedVoertuig, setSelectedVoertuig] = useState<Voertuig | null>(null);
  const [isModalOpen, setIsModalOpen] = useState(false);

  const getVoertuigen = async () => {
    try {
      if (ophaalDatum && inleverDatum) {
        const parameters = new URLSearchParams({
          ophaalDatum: ophaalDatum.toISOString(),
          inleverDatum: inleverDatum.toISOString(),
        });

        const resultaat = await fetch(
          `http://localhost:5202/api/Huur/GetVoertuigen?${parameters}`,
          {
            method: "GET",
            credentials: "include",
            headers: {
              "Content-Type": "application/json",
            },
          }
        );

        if (!resultaat.ok) {
          console.log(new Error(`Foutmelding: ${resultaat.status}`));
          throw new Error(`Foutmelding: ${resultaat.status}`);
        }

        const data = await resultaat.json();
        console.log(data);

        setData(data);
      }
    } catch (error) {
      console.error(error);
    }
  };

  const handleVerhuuraanvraag = (voertuig: Voertuig) => {
    setSelectedVoertuig(voertuig);
    setIsModalOpen(true);
  };

  const confirmAanvraag = async () => {
    if (!selectedVoertuig || !ophaalDatum || !inleverDatum) {
      alert("De data om uw verhuuraanvraag te doen is incompleet, probeer het opnieuw.");
      return;
    }

    const verhuuraanvraagData = {
      VoertuigId: selectedVoertuig.voertuigID,
      OphaalDatum: ophaalDatum.toISOString(),
      InleverDatum: inleverDatum.toISOString(),
    };

    try {
      const resultaat = await fetchCsrf("http://localhost:5202/api/Huur/DoeVerhuuraanvraag", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
        body: JSON.stringify(verhuuraanvraagData),
      });

      if (resultaat.ok) {
        alert("Uw verhuuraanvraag is opgenomen!");
      } else {
        alert("Er is iets fout gegaan tijdens het maken van uw aanvraag. Probeer het opnieuw!");
      }
    } catch (error) {
      console.error("Fout: ", error);
    }

    setIsModalOpen(false);
  };

  const voertuigCards = data
    ? data.map((voertuig) => (
        <Grid.Col span={6} key={voertuig.voertuigID}>
          <Card shadow="sm" padding="lg" radius="sm" withBorder>
            <Card.Section withBorder inheritPadding py="xs">
              <Group justify="space-between">
                <Text fw={500}>
                  {voertuig.merk} {voertuig.type}
                </Text>
                <Text>€{voertuig.prijs} /dag</Text>
              </Group>
            </Card.Section>

            <Card.Section inheritPadding py="xs">
              <Text>
                <Text span>Soort:</Text> {voertuig.soort}
              </Text>
              <Text>
                <Text span>Aanschafjaar:</Text> {voertuig.aanschafjaar}
              </Text>
              <Text>
                <Text span>Kenteken:</Text> {voertuig.kenteken}
              </Text>
            </Card.Section>

            <Button onClick={() => handleVerhuuraanvraag(voertuig)}>
              Verhuuraanvraag indienen
            </Button>
          </Card>
        </Grid.Col>
      ))
    : null;

  const tomorrow = new Date();
  tomorrow.setDate(tomorrow.getDate() + 1);

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
          minDate={tomorrow} // Prevent selecting dates earlier than tomorrow
          maxDate={inleverDatum || undefined} // Ensure the start date is before the end date
        />
        <DateInput
          value={inleverDatum}
          onChange={setInleverDatum}
          label="Inleverdatum"
          placeholder="Klik hier om een inleverdatum te kiezen"
          minDate={ophaalDatum || tomorrow} // Ensure the end date is after the start date and tomorrow
        />
        <Button onClick={getVoertuigen}>Voertuigen weergeven</Button>
      </Group>

      <Grid gutter="md">{voertuigCards}</Grid>

      <Modal
        opened={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title="Bevestig uw verhuuraanvraag"
      >
        {selectedVoertuig && (
          <div>
            <Text>
              <strong>
                {selectedVoertuig.merk} {selectedVoertuig.type}
              </strong>
            </Text>
            <Text>
              Huurperiode:{" "}
              <strong>{ophaalDatum?.toLocaleDateString()}</strong>
              {" "}tot{" "}
              <strong>{inleverDatum?.toLocaleDateString()}</strong>
            </Text>
          </div>
        )}
      </Modal>
    </div>
  );
}
