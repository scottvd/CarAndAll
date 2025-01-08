export async function heeftToestemming(rollen: string[] | null) {
    try {
        const body = rollen !== null ? JSON.stringify(rollen) : null;

        const response = await fetch("http://localhost:5202/api/Authenticatie/HeeftToestemming", {
            method: "POST",
            credentials: "include",
            headers: {
                "Content-Type": "application/json",
            },
            body: body,
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();
        return data;
    } catch (error) {
        console.error("Error checking permissions:", error);
        throw error;
    }
}
