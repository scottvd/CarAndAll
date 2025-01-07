import { Button, Container, Group, Text, Title } from '@mantine/core';
import classes from './NotFound.module.css';
import { useNavigate } from 'react-router-dom';

export function IncorrectRole() {

    const navigate = useNavigate();

    return (
        <Container className={classes.root}>
            <div className={classes.label}>401</div>
            <Title className={classes.title}>U heeft geen toestemming om deze pagina te bekijken</Title>
            <Text size="lg" ta="center" className={classes.description}>
                Het is helaas niet mogelijk om met uw accounttype om deze pagina te bezoeken.
            </Text>
            <Group justify="center">
                <Button onClick={() => {
                    navigate("/dashboard");
                }} variant="subtle" size="md">
                    Terug naar het dashboard
                </Button>
            </Group>
        </Container>
    );
}