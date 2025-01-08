import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { heeftToestemming } from "./heeftToestemming";

export function useAuthorisatie(rollen: string[] | null) {
  const navigate = useNavigate();

  useEffect(() => {
    async function checkRollen() {
      try {
        const response = await heeftToestemming(rollen);

        if (!response) {
          navigate("/unauthorised");
        }
      } catch (error) {
        console.error("Failed to verify permissions:", error);
        navigate("/unauthorised");
      }
    }

    checkRollen();
  }, [rollen, navigate]);
}
