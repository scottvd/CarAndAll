import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import '@mantine/core/styles.css';
import '@mantine/notifications/styles.css';
import { createBrowserRouter, Outlet, RouterProvider } from 'react-router-dom';
import { MantineProvider } from '@mantine/core';
import Landing from './pages/Landing.tsx';
import { Footer } from './components/Footer/Footer.tsx';
import { NotFound } from './pages/Error.tsx';
import { Dashboard } from './pages/Dashboard/Dashboard.tsx';
import { Aanvragen } from './pages/Abonnement/Aanvragen.tsx';
import { ModalsProvider } from '@mantine/modals';
import { Notifications } from '@mantine/notifications';
import { Voertuigen } from './pages/Voertuigen/Voertuigen.tsx';
import { Voertuig } from './pages/Voertuigen/Voertuig.tsx';
import { Toevoegen } from './pages/Voertuigen/Toevoegen.tsx';

/*
Router creeert de manier om met de SPA door de verschillende paginas te gaan.
*/
const router = createBrowserRouter([
    {
        path: '/', // Het pad (dus de link)
        element: <Landing />, // De pagina/component die wordt weergeven door het RouterProvider component.
    },
    {
        path: '/dashboard',
        element: <Dashboard />,
        children: [
            { path: 'abonnement', element: <Aanvragen /> },
            { 
                path: 'voertuigen', 
                element: <Outlet />,
                children: [
                    { index: true, element: <Voertuigen />},
                    { path: 'voertuig', element: <Voertuig />},
                    { path: 'toevoegen', element: <Toevoegen />}
                ]
            },
        ]
    },
    {
        path: '*',
        element: <NotFound />,
    }
]);

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <MantineProvider>
            <ModalsProvider>
                <Notifications/>
                <RouterProvider router={router} />
                <Footer />
            </ModalsProvider>
        </MantineProvider>
    </StrictMode>
)
