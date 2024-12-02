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
import { Users } from './pages/Dashboard/Users.tsx';
import { Aanvragen } from './pages/Abonnement/Aanvragen.tsx';

const router = createBrowserRouter([
    {
        path: '/',
        element: <Landing/>,
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
