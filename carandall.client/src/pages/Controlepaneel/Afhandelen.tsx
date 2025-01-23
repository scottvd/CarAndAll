import "@mantine/dates/styles.css";
import { useAuthorisatie } from "../../utilities/useAuthorisatie";
import { getCsrfToken } from "../../utilities/getCsrfToken";
import { VerhuuraanvraagAfhandelen } from "../../types/Types";
import { useEffect, useState } from "react";
import { Card, Group, Text, TextInput, Button, Collapse, NumberInput } from "@mantine/core";
import { IconChevronDown } from "@tabler/icons-react";
import { useNotificaties } from "../../utilities/NotificatieContext";

export function Afhandelen() {
    useAuthorisatie(["FrontofficeMedewerker", "BackofficeMedewerker"]);
    const { addNotificatie } = useNotificaties();   
    const [data, setData] = useState<VerhuuraanvraagAfhandelen[] | null>(null);
    const [formulier, setFormulier] = useState<{ [key: string]: boolean }>({});
    const [formulierData, setFormulierData] = useState<{ [key: string]: { omschrijving: string, herstelperiode: number } }>({});

    const getVerhuuraanvragen = async () => {
        try {
            const resultaat = await fetch("http://localhost:5202/api/Huur/GetGeaccepteerdeVerhuuraanvragen", {
                method: "GET",
                credentials: "include",
            });

            if (!resultaat.ok) {
                throw new Error(`Foutmelding: ${resultaat.status}`);
            }

            const data = await resultaat.json();
            setData(data);
        } catch (error) {
            console.log(error);
        }
    };

    const toggleFormulier = (id: string) => {
        setFormulier((prevState) => ({
            ...prevState,
            [id]: !prevState[id],
        }));
    };

    const handleFormulier = (id: string, field: string, value: string | number) => {
        setFormulierData((prevState) => ({
            ...prevState,
            [id]: {
                ...prevState[id],
                [field]: value,
            },
        }));
    };

    const handleVerhuuraanvraagAfhandelen = async (id: string) => {
        const csrfToken = await getCsrfToken();

        const verhuuraanvraagId = Number(id);

        const requestBody = {
            verhuuraanvraagId: verhuuraanvraagId, 
            herstelPeriode: formulierData[id].herstelperiode,
            beschrijving: formulierData[id].omschrijving
        };

        console.log(requestBody);
        
        if(csrfToken) {
            try {
                const response = await fetch("http://localhost:5202/api/Huur/VerhuuraanvraagAfhandelen", {
                    method: "PUT",
                    credentials: "include",
                    headers: {
                        "Content-Type": "application/json",
                        "X-CSRF-Token": csrfToken
                    },
                    body: JSON.stringify(requestBody)
                });

                if (!response.ok) {
                    throw new Error(`Foutmelding: ${response.status}`);
                }

                addNotificatie("Voertuig succesvol ingenomen!", "success", false)
            } catch (error) {
                addNotificatie("Voertuig kon niet ingenomen worden. Probeer het opnieuw!", "error", true)
            }
        }
    };

    const verhuuraanvraagCards = () => {
        if (!data) {
            return <p>Op dit moment zijn er geen innames gepland</p>;
        }

        return data.map((verhuuraanvraag) => (
            <Card key={verhuuraanvraag.verhuuraanvraagId} padding="md" shadow="sm" radius="sm" withBorder>
                <Card.Section inheritPadding py="md" withBorder>
                    <Group justify="space-between">
                        <Group>
                            <Text fw={500}>{verhuuraanvraag.voertuig}</Text>
                            <Text>gehuurd door: {verhuuraanvraag.huurder}</Text>
                        </Group>

                        <Text>
                            {new Date(verhuuraanvraag.inleverDatum).toLocaleDateString("nl-NL", {
                                year: "numeric",
                                month: "long",
                                day: "numeric",
                            })}
                        </Text>
                    </Group>
                </Card.Section>

                <Card.Section inheritPadding mt="sm">
                    <Group justify="space-between">
                        <Button 
                            color="#2E8540" 
                            onClick={() => handleVerhuuraanvraagAfhandelen(verhuuraanvraag.verhuuraanvraagId.toString())}
                            aria-label={`Neem ${verhuuraanvraag.voertuig}, gehuurd door ${verhuuraanvraag.huurder} tot ${verhuuraanvraag.inleverDatum} in`}
                        >
                            Voertuig innemen
                        </Button>

                        <Group
                            style={{ cursor: "pointer", color: "#E31C3D" }}
                            onClick={() => toggleFormulier(verhuuraanvraag.verhuuraanvraagId.toString())}
                        >
                            <Text style={{ display: "inline-flex", alignItems: "center" }} tabIndex={0} aria-label={`Voeg een schadeformulier toe voor ${verhuuraanvraag.voertuig}, gehuurd door ${verhuuraanvraag.huurder} tot ${verhuuraanvraag.inleverDatum} in`}>
                                Schadeformulier bijvoegen{" "}
                                <IconChevronDown
                                    size={16}
                                    style={{
                                        transition: "transform 0.2s ease",
                                        transform: formulier[verhuuraanvraag.verhuuraanvraagId]
                                            ? "rotate(180deg)"
                                            : "rotate(0deg)",
                                    }}
                                />
                            </Text>
                        </Group>
                    </Group>
                </Card.Section>

                <Collapse in={formulier[verhuuraanvraag.verhuuraanvraagId]} mt="sm">
                    <form onSubmit={(e) => e.preventDefault()}>
                        <TextInput
                            label="Omschrijving"
                            placeholder="Beschrijf de schade aan het voertuig"
                            value={formulierData[verhuuraanvraag.verhuuraanvraagId]?.omschrijving || ""}
                            onChange={(e) =>
                                handleFormulier(verhuuraanvraag.verhuuraanvraagId.toString(), "omschrijving", e.target.value)
                            }
                            style={{ marginBottom: "0.5rem" }}
                        />
                        <NumberInput
                            label="Herstelperiode (in dagen)"
                            placeholder="Aantal dagen"
                            min={1}
                            value={formulierData[verhuuraanvraag.verhuuraanvraagId]?.herstelperiode || 1}
                            onChange={(value) =>
                                handleFormulier(verhuuraanvraag.verhuuraanvraagId.toString(), "herstelperiode", value || 1)
                            }
                            style={{ marginBottom: "0.5rem" }}
                        />
                    </form>
                </Collapse>
            </Card>
        ));
    };

    useEffect(() => {
        getVerhuuraanvragen();
    }, []);

    return (
        <div>
            <h1>Verhuuraanvragen afhandelen</h1>

            <div>{verhuuraanvraagCards()}</div>
        </div>
    );
}
