import { Paper, Container } from "@mantine/core";
import { useEffect, useState } from "react";
import { useNotificaties } from "../../utilities/NotificatieContext";
import { useAuthorisatie } from "../../utilities/useAuthorisatie";
import { logUit } from "../../utilities/logUit";

export function Loguit() {
  useAuthorisatie(null);
  const { addNotificatie } = useNotificaties();
  const [isUitgelogd, setIsUitgelogd] = useState(false);

  useEffect(() => {
    if (!isUitgelogd) {
      logUit(addNotificatie);
      setIsUitgelogd(true);
    }
  }, [addNotificatie, isUitgelogd]);

  return (
    <main>
      <Container size="xs" style={{ marginTop: "10vh" }}>
        <Paper withBorder>
          <div style={{ padding: "1em" }}>
            <h1>Uitloggen</h1>
            <p>U wordt uitgelogd...</p>
          </div>
        </Paper>
      </Container>
    </main>
  );
}
