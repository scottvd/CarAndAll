import { Anchor, Container, Group, Text } from '@mantine/core';
//import { MantineLogo } from '@mantinex/mantine-logo';
import classes from './Footer.module.css';

const links = [
    { link: '#', label: 'Contact' },
    { link: '#', label: 'Privacy' },
    { link: '#', label: 'Blog' },
    { link: '#', label: 'Careers' },
];

export function Footer() {
    const items = links.map((link) => (
        <Anchor<'a'>
            c="dimmed"
            key={link.label}
            href={link.link}
            onClick={(event) => event.preventDefault()}
            size="sm"
        >
            {link.label}
        </Anchor>
    ));

    return (
        <div className={classes.footer}>
            <Container className={classes.inner}>
                <Text size="md">CarAndAll</Text>
                <Group className={classes.links}>{items}</Group>
            </Container>
        </div>
    );
}