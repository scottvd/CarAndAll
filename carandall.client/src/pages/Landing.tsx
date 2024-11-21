import { Button, Container, Overlay, Text, Title } from '@mantine/core';
import classes from './landing.module.css';

function Landing() {
    return (
        <div className={classes.hero}>
            <Overlay
                gradient="linear-gradient(180deg, rgba(0, 0, 0, 0.25) 0%, rgba(0, 0, 0, .65) 40%)"
                opacity={1}
                zIndex={0}
            />
            <Container className={classes.container} size="md">
                <Title className={classes.title}> Car4All - Uw Betrouwbare Autoverhuurservice </Title>
                <Text className={classes.description} size="xl" mt="xl">
                    Bij Car4All bieden we een breed scala aan voertuigen om aan al uw reisbehoeften te voldoen. Of u nu op zoek bent naar een compacte auto voor stadsritten of een ruime SUV voor gezinsuitjes, wij hebben het perfecte voertuig voor u. Geniet van concurrerende tarieven en uitstekende klantenservice die uw huurervaring soepel en aangenaam maakt.
                </Text>
                <Button variant="gradient" size="xl" radius="xl" className={classes.control}>
                    Account aanmaken
                </Button>

            </Container>
        </div>
    );
}



export default Landing;