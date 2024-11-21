import { AppShell, Burger } from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
//import './App.css';


function App() {
    const [opened, { toggle }] = useDisclosure();

    return (
        <AppShell
            header={{ height: 60 }}
            navbar={{
                width: 300,
                breakpoint: 'sm',
                collapsed: { mobile: !opened },
            }}
            padding="md"
        >
            <AppShell.Header>
                <Burger
                    opened={opened}
                    onClick={toggle}
                    hiddenFrom="sm"
                    size="sm"
                />
                <div>Logo</div>
            </AppShell.Header>

            <AppShell.Navbar p="md">Navbar</AppShell.Navbar>

            <AppShell.Main>Main</AppShell.Main>
        </AppShell>
    );
    //return (
    //    <div>
    //        <h1 id="tableLabel">Home</h1>
    //        <p>Car And All Home</p>
    //    </div>
    //);
}

export default App;