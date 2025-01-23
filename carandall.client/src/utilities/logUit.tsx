export const logUit = async (addNotificatie: (message: string, type: "error" | "success" | "info", persistent: boolean) => void) => {
  try {
    const resultaat = await fetch("http://localhost:5202/api/Authenticatie/Loguit", {
      method: "POST",
      credentials: "include",
    });

    if (resultaat.ok) {
      addNotificatie("U bent succesvol uitgelogd!", "success", false);
      setTimeout(() => {
        window.location.href = "https://localhost:60281/";
      }, 1000);
    } else {
      addNotificatie("Er is iets fout gegaan tijdens het uitloggen.", "error", true);
    }
  } catch (error) {
    addNotificatie(`Fout tijdens uitloggen: ${error}`, "error", true);
  }
};
