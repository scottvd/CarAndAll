import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import '@mantine/core/styles.css';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { MantineProvider } from '@mantine/core';
import Landing from './pages/landing.tsx';

const router = createBrowserRouter([
    {
        path: '/',
        element: <Landing/>,
    }
]);
createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <MantineProvider>
            <RouterProvider router={router}/>
        </MantineProvider>
    </StrictMode>,
)
