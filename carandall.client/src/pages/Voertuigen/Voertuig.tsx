import React, { useState } from "react";
import { useLocation } from "react-router-dom";
import { getCsrfToken } from "../../utilities/getCsrfToken";
import { useAuthorisatie } from "../../utilities/useAuthorisatie";
import { useNotificaties } from "../../utilities/NotificatieContext";
import { Button } from "@mantine/core";

export function Voertuig() {
    useAuthorisatie(["BackofficeMedewerker", "FrontofficeMedewerker"]);
    const { addNotificatie } = useNotificaties();   
    const location = useLocation();
    const { voertuig } = location.state || {};

    const [editModus, setEditModus] = useState(false);
    const [editedVoertuig, setEditedVoertuig] = useState({
        kenteken: voertuig?.kenteken || "",
        soort: voertuig?.soort || "",
        merk: voertuig?.merk || "",
        type: voertuig?.type || "",
        aanschafjaar: voertuig?.aanschafjaar || "",
    });

    const verwijderVoertuig = async () => {
        const csrfToken = getCsrfToken();

        if(csrfToken) {
            try {
                const resultaat = await fetch(`http://localhost:5202/api/VoertuigBeheer/DeleteVoertuig/${voertuig.voertuigId}`, { 
                    method: "DELETE",
                    headers: {
                        "X-CSRF-Token": csrfToken
                    },
                    credentials: "include"
                });
        
                if (!resultaat.ok) {
                throw new Error(`Foutmelding: ${resultaat.status}`);
                }

                addNotificatie("Voertuig succesvol verwijderd!", "success", false);
            } catch (error) {
            console.error(error);
            }
        }
    };

    const updateVoertuig = async () => {
        const csrfToken = getCsrfToken();

        console.log(voertuig);
        if(csrfToken) {
            try {
                const voertuigGegevens = { ...editedVoertuig, voertuigID: voertuig.voertuigId };

                const resultaat = await fetch(`http://localhost:5202/api/VoertuigBeheer/EditVoertuig/${voertuig.voertuigId}`, {
                    method: "PUT",
                    headers: {
                        "Content-Type": "application/json",
                        "X-CSRF-Token": csrfToken
                    },
                    credentials: "include",
                    body: JSON.stringify(voertuigGegevens)
                });

                if (!resultaat.ok) {
                    throw new Error(`Foutmelding: ${resultaat.status}`);
                }

                addNotificatie("Voertuig succesvol bijgewerkt!", "success", true);
                setEditModus(false);
            } catch (error) {
                console.error(error);
            }
        }
    };

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setEditedVoertuig({ ...editedVoertuig, [name]: value });
    };

    if (!voertuig) {
        return <p>Geen voertuiggegevens beschikbaar.</p>;
    }

    return (
        <div>
            <div>
                <h1>{editedVoertuig.merk} {editedVoertuig.type}, {editedVoertuig.aanschafjaar}</h1>
                {editModus ? (
                    <div>
                        <p><strong>Kenteken:</strong> <input type="text" name="kenteken" value={editedVoertuig.kenteken} onChange={handleInputChange} /></p>
                        <p><strong>Soort:</strong> <input type="text" name="soort" value={editedVoertuig.soort} onChange={handleInputChange} /></p>
                        <p><strong>Merk:</strong> <input type="text" name="merk" value={editedVoertuig.merk} onChange={handleInputChange} /></p>
                        <p><strong>Type:</strong> <input type="text" name="type" value={editedVoertuig.type} onChange={handleInputChange} /></p>
                        <p><strong>Aanschafjaar:</strong> <input type="number" name="aanschafjaar" value={editedVoertuig.aanschafjaar} onChange={handleInputChange} /></p>
                    </div>
                ) : (
                    <div>
                        <p><strong>Kenteken:</strong> {voertuig.kenteken}</p>
                        <p><strong>Soort:</strong> {voertuig.soort}</p>
                        <p><strong>Merk:</strong> {voertuig.merk}</p>
                        <p><strong>Type:</strong> {voertuig.type}</p>
                        <p><strong>Aanschafjaar:</strong> {voertuig.aanschafjaar}</p>
                    </div>
                )}
            </div>
            <div className="button-container">
                {editModus ? (
                    <>
                        <Button color="#2E8540" mr="1rem" className="save-button" onClick={updateVoertuig}>Opslaan</Button>
                        <Button color="#E31C3D" className="cancel-button" onClick={() => setEditModus(false)}>Annuleren</Button>
                    </>
                ) : (
                    <>
                        <Button color="#2E8540" mr="1rem" className="edit-button" onClick={() => setEditModus(true)}>Bewerken</Button>
                        <Button color="#E31C3D" className="delete-button" onClick={verwijderVoertuig}>Verwijder</Button>
                    </>
                )}
            </div>
        </div>
    );
}
