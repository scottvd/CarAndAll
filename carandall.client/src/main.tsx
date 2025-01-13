import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import "@mantine/core/styles.css";
import "@mantine/notifications/styles.css";
import { createBrowserRouter, Outlet, RouterProvider } from "react-router-dom";
import { MantineProvider } from "@mantine/core";
import { ModalsProvider } from "@mantine/modals";
import { Notifications } from "@mantine/notifications";
import axios from "axios";

import Landing from "./pages/Static/Landing.tsx";
import { NotFound } from "./pages/Static/NotFound.tsx";
import { Dashboard } from "./pages/Dashboard/Dashboard.tsx";
import { Aanvragen } from "./pages/Abonnement/Aanvragen.tsx";
import { Voertuigen } from "./pages/Voertuigen/Voertuigen.tsx";
import { Voertuig } from "./pages/Voertuigen/Voertuig.tsx";
import { Toevoegen } from "./pages/Voertuigen/Toevoegen.tsx";
import { Overzicht } from "./pages/Huren/Overzicht.tsx";
import { Login } from "./pages/Authenticatie/Login.tsx";
import { Register } from "./pages/Authenticatie/Registreer.tsx";
import { Unauthorised } from "./pages/Static/Unauthorised.tsx";
import { IncorrectRole } from "./pages/Static/IncorrectRole.tsx";
import { Verhuuraanvragen } from "./pages/Controlepaneel/Verhuuraanvragen.tsx";
import { Medewerkers } from "./pages/Controlepaneel/Medewerkers.tsx";
import { Profiel } from "./pages/Profiel/Profiel.tsx";

export const api = axios.create({
  baseURL: "https://localhost:7140",
  withCredentials: true,
});

const router = createBrowserRouter([
  {
    path: "/",
    element: <Landing />,
  },
  {
    path: "/dashboard",
    element: <Dashboard />,
    children: [
      { path: "abonnement", element: <Aanvragen /> },
      {
        path: "profiel",
        element: <Outlet />,
        children: [
          { index: true, element: <Profiel /> },
        ]
      },
      {
        path: "voertuigen",
        element: <Outlet />,
        children: [
          { index: true, element: <Voertuigen /> },
          { path: "voertuig", element: <Voertuig /> },
          { path: "toevoegen", element: <Toevoegen /> }
        ]
      },
      { path: "huren", element: <Overzicht /> },
      {
        path: "controlepaneel",
        element: <Outlet />,
        children: [
          { index: true, element: <Medewerkers />},
          { path: "verhuuraanvragen", element: <Verhuuraanvragen /> },
        ]
      }
    ]
  },
  {
    path: "/register",
    element: <Register />,
  },
  {
    path: "/login",
    element: <Login />,
  },
  {
    path: "/unauthorised",
    element: <Unauthorised />,
  },
  {
    path: "/incorrectrole",
    element: <IncorrectRole />,
  },
  {
    path: "*",
    element: <NotFound />,
  }
]);

createRoot(document.getElementById("root")!).render(
    <StrictMode>
        <MantineProvider>
            <ModalsProvider>
                <Notifications />
                <RouterProvider router={router} />
            </ModalsProvider>
        </MantineProvider>
    </StrictMode>
);
