import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
// import App from './App.tsx'
import '@mantine/core/styles.css';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { MantineProvider } from '@mantine/core';
import Landing from './pages/Landing.tsx';
import { Footer } from './components/Footer/Footer.tsx';
import { NotFound } from './pages/Error.tsx';
import { Dashboard } from './pages/Dashboard/Dashboard.tsx';
import { Aanvragen } from './pages/Abonnement/Aanvragen.tsx';


/*
Router creeert de manier om met de SPA door de verschillende paginas te gaan.
*/
const router = createBrowserRouter([
    {
        path: '/', // Het pad (dus de link)
        element: <Landing/>, // De pagina/component die wordt weergeven door het RouterProvider component.
    },
    {
        path: '/dashboard',
        element: <Dashboard />,
        children: [
            {path: 'abonnement', element: <Aanvragen/>},
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
            <RouterProvider router={router} />
            <Footer />
        </MantineProvider>
    </StrictMode>,
)
