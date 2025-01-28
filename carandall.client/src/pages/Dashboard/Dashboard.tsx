import { AppShell, Burger, Group } from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import { Outlet } from 'react-router-dom';
import { useAuthorisatie } from '../../utilities/useAuthorisatie';
import { NavbarHuurders } from './Navbar/NavbarHuurders';
import { NavbarMedewerkers } from './Navbar/NavbarMedewerkers';
import { useEffect, useState } from 'react';

export function Dashboard() {
  useAuthorisatie(null);

  const [opened, { toggle }] = useDisclosure();
  const [rollen, setRoles] = useState<string[]>([]);
  const [laden, setLoading] = useState(true);

  useEffect(() => {
    const fetchRoles = async () => {
      try {
        const response = await fetch('http://localhost:5202/api/Authenticatie/GebruikerRollen', {
          credentials: 'include',
        });

        if (response.ok) {
          const data = await response.json();
          setRoles(data || []);
        } 
      } catch (error) {
        console.error('Error fetching roles:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchRoles();
  }, []);

  const renderNavbar = () => {
    if (rollen.includes('FrontofficeMedewerker') || rollen.includes('BackofficeMedewerker')) {
      return <NavbarMedewerkers />;
    }

    if (
      rollen.includes('Particulier') ||
      rollen.includes('Zakelijk') ||
      rollen.includes('Wagenparkbeheerder')
    ) {
      return <NavbarHuurders />;
    }

    return null;
  };

  return (
    <AppShell
      header={{ height: 60 }}
      navbar={{ width: 300, breakpoint: 'sm', collapsed: { mobile: !opened } }}
      padding="md"
    >
      <AppShell.Header>
        <Group h="100%" px="md">
          <Burger opened={opened} onClick={toggle} hiddenFrom="sm" size="sm" />
        </Group>
      </AppShell.Header>
      <AppShell.Navbar>
        {laden ? <div>Menu is aan het laden...</div> : renderNavbar()}
      </AppShell.Navbar>
      <AppShell.Main>
        <Outlet />
      </AppShell.Main>
    </AppShell>
  );
}
