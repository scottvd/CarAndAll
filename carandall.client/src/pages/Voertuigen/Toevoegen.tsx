import React from "react";

export function Toevoegen() {
    const voertuigToevoegen = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        const formulier = e.currentTarget;
        const voertuigData = {
            kenteken: (formulier.elements.namedItem("kenteken") as HTMLInputElement).value,
            soort: (formulier.elements.namedItem("soort") as HTMLInputElement).value,
            merk: (formulier.elements.namedItem("merk") as HTMLInputElement).value,
            type: (formulier.elements.namedItem("type") as HTMLInputElement).value,
            aanschafjaar: parseInt((formulier.elements.namedItem("aanschafjaar") as HTMLInputElement).value, 10),
        };

        try {
            const resultaat = await fetch("http://localhost:5202/api/VoertuigBeheer/AddVoertuig", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(voertuigData)
            });
            if (resultaat.ok) {
                alert("Voertuig toegevoegd!");
                formulier.reset();
            } else {
                alert("Er is iets fout gegaan tijdens het toevoegen van het voertuig. Probeer het opnieuw!");
            }
        } catch (error) {
            console.error("Fout: ", error);
        }
    };

    return (
        <form onSubmit={voertuigToevoegen}>
            <div>
                <label>
                    Kenteken:
                    <input type="text" name="kenteken" required />
                </label>
            </div>
            <div>
                <label>
                    Soort:
                    <input type="text" name="soort" required />
                </label>
            </div>
            <div>
                <label>
                    Merk:
                    <input type="text" name="merk" required />
                </label>
            </div>
            <div>
                <label>
                    Type:
                    <input type="text" name="type" required />
                </label>
            </div>
            <div>
                <label>
                    Aanschafjaar:
                    <input type="number" name="aanschafjaar" required />
                </label>
            </div>
            <button type="submit">Toevoegen</button>
        </form>
    );
}
