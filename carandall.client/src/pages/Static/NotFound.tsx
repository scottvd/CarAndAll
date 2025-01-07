import { Button, Container, Group, Text, Title } from '@mantine/core';
import classes from './NotFound.module.css';
import { useNavigate } from 'react-router-dom';

export function NotFound() {

    const navigate = useNavigate();

    return (
        <Container className={classes.root}>
            <div className={classes.label}>404</div>
            <Title className={classes.title}>Pagina niet gevonden</Title>
            <Text size="lg" ta="center" className={classes.description}>
                De pagina die u zoekt kon niet gevonden worden. Controleer de link en probeer het opnieuw!
            </Text>
            <Group justify="center">
                <Button onClick={() => {
                    navigate("/");
                }} variant="subtle" size="md">
                    Terug naar de startpagina
                </Button>
            </Group>
        </Container>
    );
}