import { Button, Container, Text, Title } from '@mantine/core';
//import { Dots } from './Dots';
import classes from './HeroHeader.module.css';
import { useNavigate } from 'react-router-dom';

export function HeroHeader() {
    const navigate = useNavigate();

    return (
        <Container className={classes.wrapper} size={1400}>

            <div className={classes.inner}>
                <Title className={classes.title}>
                    Ontdek de vrijheid met {' '}
                    <Text component="span" className={classes.highlight} inherit>
                        CarAndAll
                    </Text>
                </Title>

                <Container p={0} size={600}>
                    <Text size="lg" c="dimmed" className={classes.description}>
                        Huur eenvoudig en snel een auto online bij CarAndAll. Kies uit een breed scala aan voertuigen
                        voor elke gelegenheid, en geniet van de flexibiliteit en gemak die wij bieden.
                    </Text>
                </Container>

                <div className={classes.controls}>
                    <Button className={classes.control} size="lg" variant="default" color="gray" onClick={(e) => {
                        navigate("/register");
                        e.preventDefault();
                    }}>
                        Registreren
                    </Button>
                    <Button 
                    onClick={(e) => {
                        navigate("/login");
                        e.preventDefault();
                    }}
                    className={classes.control} size="lg">
                        Inloggen
                    </Button>
                    
                </div>
            </div>
        </Container>
    );
}