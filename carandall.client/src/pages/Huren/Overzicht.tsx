import { useState, useEffect } from "react";
import { DateInput, DatePicker, DatePickerInput } from "@mantine/dates";
import { Button, Group, Grid, Card, Text, Modal, TextInput, MultiSelect } from "@mantine/core";
import "@mantine/dates/styles.css";
import { useAuthorisatie } from "../../utilities/useAuthorisatie";
import { getCsrfToken } from "../../utilities/getCsrfToken";
import { Prijs, Soort, VoertuigMetPrijs } from "../../types/Types";
import { Filterwaarden } from "../../interfaces/interfaces";
import { tijdzoneConverter } from "../../utilities/tijdzoneConverter";
import { useNotificaties } from "../../utilities/NotificatieContext";

export function Overzicht() {
  useAuthorisatie(["Particulier", "Zakelijk", "Wagenparkbeheerder"]);
  const { addNotificatie } = useNotificaties();
  const [ophaalDatum, setOphaalDatum] = useState<Date | null>(null);
  const [inleverDatum, setInleverDatum] = useState<Date | null>(null);
  const [data, setData] = useState<VoertuigMetPrijs[] | null>(null);
  const [gefilterdeData, setGefilterdeData] = useState<VoertuigMetPrijs[] | null>(null);
  const [geselecteerdVoertuig, setGeselecteerdVoertuig] = useState<VoertuigMetPrijs | null>(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [filters, setFilters] = useState<Filterwaarden>({
    zoekTerm: "",
    soort: [],
    prijs: [],
  });

  const filterVoertuigen = (waarde: Filterwaarden) => {
    if (!data) return [];
    
    const prijsKlasse: Record<string, [number, number]> = {
      "< €50": [0, 49.99],
      "€50 - €75": [50, 75],
      "> €75": [75.01, Infinity],
    };
  
    return data.filter((item) => {
      const zoekTerm = waarde.zoekTerm.toUpperCase();
      const merkEnTypeFilter = (item.merk?.toUpperCase().includes(zoekTerm) || 
                           item.type?.toUpperCase().includes(zoekTerm));
      const soortFilter = waarde.soort.length > 0 
        ? waarde.soort.map((s) => s.toUpperCase()).includes(item.soort.toUpperCase() as Soort) 
        : true;
      const prijsKlasseFilter = waarde.prijs.length > 0 
        ? waarde.prijs.some((radius) => {
            const [min, max] = prijsKlasse[radius];
            return item.prijs >= min && item.prijs <= max;
          })
        : true;
  
      return merkEnTypeFilter && soortFilter && prijsKlasseFilter;
    });
  };
  

  useEffect(() => {
    setGefilterdeData(filterVoertuigen(filters));
  }, [filters, data]);

  const getVoertuigen = async () => {
    try {
      if (ophaalDatum && inleverDatum) {
        const parameters = new URLSearchParams({
          ophaalDatum: tijdzoneConverter(ophaalDatum),
          inleverDatum: tijdzoneConverter(inleverDatum),
        });

        const resultaat = await fetch(
          `http://localhost:5202/api/Verhuuraanvraag/GetVoertuigen?${parameters}`,
          {
            method: "GET",
            credentials: "include",
            headers: {
              "Content-Type": "application/json",
            },
          }
        );

        if (!resultaat.ok) {
          throw new Error(`Foutmelding: ${resultaat.status}`);
        }

        const data = await resultaat.json();
        setData(data);
        setGefilterdeData(filterVoertuigen(filters));
      }
    } catch (error) {
      console.error(error);
    }
  };

  const handleVerhuuraanvraag = (voertuig: VoertuigMetPrijs) => {
    console.log(voertuig);
    setGeselecteerdVoertuig(voertuig);
    setIsModalOpen(true);
  };

  const bevestigAanvraag = async () => {
    if (!geselecteerdVoertuig || !ophaalDatum || !inleverDatum) {
      addNotificatie("De data om uw verhuuraanvraag te doen is incompleet, probeer het opnieuw.", "error", true);
      return;
    }

    const verhuuraanvraagData = {
      VoertuigId: geselecteerdVoertuig.voertuigId,
      OphaalDatum: tijdzoneConverter(ophaalDatum),
      InleverDatum: tijdzoneConverter(inleverDatum),
    };

    const csrfToken = getCsrfToken();

    if(csrfToken) {
      try {
        const resultaat = await fetch("http://localhost:5202/api/Verhuuraanvraag/AddVerhuuraanvraag", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            "X-CSRF-Token": csrfToken
          },
          credentials: "include",
          body: JSON.stringify(verhuuraanvraagData),
        });

        if (resultaat.ok) {
          addNotificatie("Uw verhuuraanvraag is opgenomen!", "success", false);
        } else {
          addNotificatie("Er is iets fout gegaan tijdens het maken van uw aanvraag. Probeer het opnieuw!", "error", true);
        }
      } catch (error) {
        console.error("Fout: ", error);
      }
    }

    setIsModalOpen(false);
  };

  const berekenSubtotaal = (ophaalDatum: Date | null, inleverDatum: Date | null, dagPrijs: number) => {
    if (ophaalDatum && inleverDatum) {
      const days = Math.max(
        1,
        Math.ceil((inleverDatum.getTime() - ophaalDatum.getTime()) / (1000 * 60 * 60 * 24)) + 1
      );
      return (days * dagPrijs).toFixed(2);
    }
    return "0.00";
  };

  const voertuigCards = (gefilterdeData || []).map((voertuig: VoertuigMetPrijs) => (
    <Grid.Col span={6} key={voertuig.voertuigId}>
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

        <Button color="#28282B" onClick={() => handleVerhuuraanvraag(voertuig)}>
          Verhuuraanvraag indienen
        </Button>
      </Card>
    </Grid.Col>
  ));

  const datumVanMorgen = new Date();
  datumVanMorgen.setDate(datumVanMorgen.getDate() + 1);

  return (
    <div>
      <h1>Huren</h1>

      <Group>
        <DatePickerInput
          value={ophaalDatum}
          onChange={setOphaalDatum}
          label="Ophaaldatum"
          aria-label="Selecteer een ophaaldatum"
          placeholder="Klik hier om een ophaaldatum te kiezen"
          minDate={datumVanMorgen}
          maxDate={inleverDatum || undefined}
        />

        <DatePickerInput
          value={inleverDatum}
          onChange={setInleverDatum}
          label="Inleverdatum"
          placeholder="Klik hier om een inleverdatum te kiezen"
          minDate={ophaalDatum || datumVanMorgen}
        />
        
        <Button color="#28282B" onClick={getVoertuigen}>Voertuigen weergeven</Button>
      </Group>

      <Group>
        <TextInput
          value={filters.zoekTerm}
          onChange={(event) =>
            setFilters({ ...filters, zoekTerm: event.currentTarget.value })
          }
          label="Zoekterm"
          placeholder="Voer een merk in"
        />

        <MultiSelect
          label="Soort"
          placeholder="Selecteer een soort"
          data={["Auto", "Camper", "Caravan"]}
          value={filters.soort}
          onChange={(value) => setFilters({ ...filters, soort: value as Soort[] })}
        />

        <MultiSelect
          label="Prijsklasse"
          placeholder="Selecteer een prijsklasse"
          data={["< €50", "€50 - €75", "> €75"]}
          value={filters.prijs}
          onChange={(value) => setFilters({ ...filters, prijs: value as Prijs[] })}
        />
      </Group>

      <Grid mt="md">{voertuigCards}</Grid>

      <Modal opened={isModalOpen} onClose={() => setIsModalOpen(false)} title="Verhuuraanvraag">
        <p>
          {geselecteerdVoertuig?.merk} {geselecteerdVoertuig?.type} ({geselecteerdVoertuig?.soort})
        </p>
        <p>
          Subtotaal: €{" "}
          {berekenSubtotaal(ophaalDatum, inleverDatum, geselecteerdVoertuig?.prijs ?? 0)}
        </p>
        <Button color="#2E8540" onClick={bevestigAanvraag}>Bevestigen</Button>
      </Modal>
    </div>
  );
}
