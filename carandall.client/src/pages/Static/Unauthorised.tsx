import { Button, Container, Group, Text, Title } from '@mantine/core';
import classes from './NotFound.module.css';
import { useNavigate } from 'react-router-dom';

export function Unauthorised() {

    const navigate = useNavigate();

    return (
        <Container className={classes.root}>
            <div className={classes.label}>401</div>
            <Title className={classes.title}>Log in om toegang te verkrijgen tot deze pagina</Title>
            <Text size="lg" ta="center" className={classes.description}>
                Om toegang te verkrijgen tot de pagina die u zoekt zal u in moeten loggen. Heeft u nog geen account? Klik dan op de knop om te registreren.
            </Text>
            <Group justify="center">
                <Button onClick={() => {
                    navigate("/login");
                }} size="md">
                    Inloggen
                </Button>

                <Button onClick={() => {
                    navigate("/register");
                }} size="md">
                    Registreren
                </Button>
            </Group>
        </Container>
    );
}