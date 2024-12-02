import { Icon } from '@tabler/icons-react';
import { Button, Card, Center, Group, Text } from '@mantine/core';
import classes from './FeaturesCard.module.css';


interface Feature {
  label: string,
  icon: Icon
}
interface FeatureCardProps {
  naam: string,
  price: number,
  description: string,
  features: Feature[]
}
export function FeaturesCard(props: FeatureCardProps) {
  const features = props.features.map((feature) => (
    <Center key={feature.label}>
      <feature.icon size={16} className={classes.icon} stroke={1.5} />
      <Text size="xs">{feature.label}</Text>
    </Center>
  ));

  return (
    <Card withBorder radius="md" className={classes.card}>

      <Group justify="center" mt="md">
        <div>
          <Text ta={'center'} fw={500}>{props.naam}</Text>
          <Text ta={'center'} fz="xs" c="dimmed">
            {props.description}
          </Text>
        </div>
      </Group>

      <Card.Section className={classes.section} mt="md">
        <Text fz="sm" c="dimmed" className={classes.label}>
          Voordelen
        </Text>

        <Group gap={8} mb={-8}>
          {features}
        </Group>
      </Card.Section>

      <Card.Section className={classes.section}>
        <Group gap={30}>
          <div>
            <Text fz="xl" fw={700} style={{ lineHeight: 1 }}>
            €{props.price.toFixed(2)}
            </Text>
            <Text fz="sm" c="dimmed" fw={500} style={{ lineHeight: 1 }} mt={3}>
              per maand
            </Text>
          </div>

          <Button radius="xl" style={{ flex: 1 }}>
            Aanvragen
          </Button>
        </Group>
      </Card.Section>
    </Card>
  );
}