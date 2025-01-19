import React, { useState } from "react";
import { useLocation } from "react-router-dom";
import { getCsrfToken } from "../../utilities/getCsrfToken";
import { useAuthorisatie } from "../../utilities/useAuthorisatie";

export function Voertuig() {
    useAuthorisatie(["BackofficeMedewerker", "FrontofficeMedewerker"]);
       
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

                alert("Voertuig succesvol verwijderd!");
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

                alert("Voertuig succesvol bijgewerkt!");
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
                <h2>{editedVoertuig.merk} {editedVoertuig.type}, {editedVoertuig.aanschafjaar}</h2>
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
                        <button className="save-button" onClick={updateVoertuig}>Opslaan</button>
                        <button className="cancel-button" onClick={() => setEditModus(false)}>Annuleren</button>
                    </>
                ) : (
                    <>
                        <button className="edit-button" onClick={() => setEditModus(true)}>Bewerken</button>
                        <button className="delete-button" onClick={verwijderVoertuig}>Verwijder</button>
                    </>
                )}
            </div>
        </div>
    );
}
