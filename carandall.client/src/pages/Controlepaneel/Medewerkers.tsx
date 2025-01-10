import { useAuthorisatie } from "../../utilities/useAuthorisatie";

export function Medewerkers() {
  useAuthorisatie(["BackofficeMedewerker"]);

  return (
    <div>
      
    </div>
  );
}