import { useEffect, useState } from "react";
import { Table } from "@mantine/core";
import '../../styles/table.css';
import { IconCheck, IconX } from "@tabler/icons-react";
import { useAuthorisatie } from "../../utilities/useAuthorisatie";
import { getCsrfToken } from "../../utilities/getCsrfToken";
import { Verhuuraanvraag } from "../../types/Types";

export function Verhuuraanvragen() {
  useAuthorisatie(["BackofficeMedewerker"]);

  const [data, setData] = useState<Verhuuraanvraag[] | null>(null);
  const [openstaandeData, setOpenstaandeData] = useState<Verhuuraanvraag[]>([]);
  const [behandeldeData, setBehandeldeData] = useState<Verhuuraanvraag[]>([]);

  useEffect(() => {
    const getVerhuuraanvragen = async () => {
      try {
        const resultaat = await fetch('http://localhost:5202/api/Verhuuraanvraag/GetVerhuuraanvragen', { credentials: 'include' });

        if (!resultaat.ok) {
          throw new Error(`Foutmelding: ${resultaat.status}`);
        }

        const data = await resultaat.json();
        setData(data);

        setOpenstaandeData(data.filter((item: Verhuuraanvraag) => item.status === "InBehandeling"));
        setBehandeldeData(data.filter((item: Verhuuraanvraag) => item.status !== "InBehandeling"));
      } catch (error) {
        console.error(error);
      }
    };

    getVerhuuraanvragen();
  }, []);

  const handleStatusChange = async (aanvraagID: number, status: string) => {
    const dto = { aanvraagID: aanvraagID, status };
    const csrfToken = getCsrfToken();

    if(csrfToken) {
      try {
        const response = await fetch('http://localhost:5202/api/Verhuuraanvraag/BehandelVerhuuraanvraag', {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            "X-CSRF-Token": csrfToken
          },
          credentials: 'include',
          body: JSON.stringify(dto)
        });

        if (!response.ok) {
          throw new Error(`Error: ${response.status}`);
        }

        setOpenstaandeData((prev) => prev.filter((item) => item.verhuuraanvraagID !== aanvraagID));
        setBehandeldeData((prev) => [
          ...prev,
          { ...openstaandeData.find((item) => item.verhuuraanvraagID === aanvraagID)!, status },
        ]);

      } catch (error) {
        console.error(error);
      }
    }
  };

  const renderRows = (data: Verhuuraanvraag[], isInBehandeling: boolean) => {
    return data.map((data) => (
      <tr key={data.verhuuraanvraagID}>
        <td>{data.voertuig}</td>
        <td>{data.kenteken}</td>
        <td>{data.huurder}</td>
        <td>{data.ophaaldatum}</td>
        <td>{data.inleverdatum}</td>
        <td>{data.status}</td>
        <td>
          {isInBehandeling ? (
            <>
              <IconCheck
                color="#2E8540"
                style={{ cursor: 'pointer', marginRight: '10px' }}
                onClick={() => handleStatusChange(data.verhuuraanvraagID, 'Geaccepteerd')}
                tabIndex={0}
                aria-label={`Accepteer verhuuraanvraag voor ${data.voertuig} van ${data.ophaaldatum} tot ${data.inleverdatum}`}
              />
              <IconX
                color="#E31C3D"
                style={{ cursor: 'pointer' }}
                onClick={() => handleStatusChange(data.verhuuraanvraagID, 'Afgewezen')}
                tabIndex={0}
                aria-label={`Weiger verhuuraanvraag voor ${data.voertuig} van ${data.ophaaldatum} tot ${data.inleverdatum}`}
              />
            </>
          ) : null}
        </td>
      </tr>
    ));
  };

  if (!data) return <div><p>De pagina is aan het laden. Een ogenblik geduld, alstublieft...</p></div>;

  return (
    <div>
      <h1>Controlepaneel CarAndAll vloot</h1>

      <div>
        <h2>Openstaande verhuuraanvragen</h2>
        <Table border={1} className="basistabel">
          <thead>
            <tr>
              <th scope="col">Voertuig</th>
              <th scope="col">Kenteken</th>
              <th scope="col">Huurder</th>
              <th scope="col">Ophaaldatum</th>
              <th scope="col">Inleverdatum</th>
              <th scope="col">Status</th>
              <th scope="col">Handelingen</th>
            </tr>
          </thead>
          
          <tbody>
            {renderRows(openstaandeData, true)}
          </tbody>
        </Table>
      </div>

      <div>
        <h2>Behandelde verhuuraanvragen</h2>
        <Table border={1} className="basistabel">
          <thead>
            <tr>
              <th scope="col">Voertuig</th>
              <th scope="col">Kenteken</th>
              <th scope="col">Huurder</th>
              <th scope="col">Ophaaldatum</th>
              <th scope="col">Inleverdatum</th>
              <th scope="col">Status</th>
            </tr>
          </thead>
          <tbody>{renderRows(behandeldeData, false)}</tbody>
        </Table>
      </div>
    </div>
  );
}
