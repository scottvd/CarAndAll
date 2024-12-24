import React, { useState } from "react";
import { DateInput } from "@mantine/dates";
import { Button, Group, Table } from "@mantine/core";
import '@mantine/dates/styles.css';

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
 
export function Overzicht() {
    const[ophaalDatum, setOphaalDatum] = useState<Date | null>(null);
    const[inleverDatum, setInleverDatum] = useState<Date | null>(null);
    const[data, setData] = useState<Voertuig[] | null>(null);

    const getVoertuigen = async () => {
      try {
        if(ophaalDatum && inleverDatum) {
          const parameters = new URLSearchParams({
            ophaalDatum: ophaalDatum.toISOString(),
            inleverDatum: inleverDatum.toISOString(),
          });

          const resultaat = await fetch(`http://localhost:5202/api/Huur/GetVoertuigen?${parameters}`, {
            method: "GET",
            headers: {
              "Content-Type": "application/json",
            },
            credentials: "include",
          });
      
          if (!resultaat.ok) {
            throw new Error(`Foutmelding: ${resultaat.status}`);
          }
      
          const data = await resultaat.json();
          setData(data);
        }
      } catch (error) {
        console.error(error);
      }
    };

    const rows = data ? data.map((voertuig) => (
      <tr key={voertuig.voertuigID}>
        <td>{voertuig.kenteken}</td>
        <td>{voertuig.soort}</td>
        <td>{voertuig.merk}</td>
        <td>{voertuig.type}</td>
        <td>{voertuig.aanschafjaar}</td>
        <td className="button">
          <Button>Meer info</Button>
        </td>
      </tr>
    )) : null;

    return(
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
                    placeholder="Klik hier om een ophaaldatum te kiezen"
                />
                <Button onClick={getVoertuigen}>Voertuigen weergeven</Button>
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
    );
}
