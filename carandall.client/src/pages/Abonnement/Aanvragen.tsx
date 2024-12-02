import { Container, Grid, Title } from "@mantine/core";
import { FeaturesCard } from "../../components/FeaturesCard/FeaturesCard";
import { IconCurrencyEuro } from "@tabler/icons-react";

export function Aanvragen() {

  return (
    <>
      <Title p={36} ta={'center'} order={2}>Aanvragen bedrijfsabonnement</Title>
      <Container>
        <Grid>
          <Grid.Col span={6}>
            <FeaturesCard
              naam={"Prepaid"}
              price={2}
              description="Bij het prepaid abonnement betaalt u voor het huren de prijs."
              features={
                [
                  {
                    label: "Vooraf betalen",
                    icon: IconCurrencyEuro
                  }
                ]
              }
              /></Grid.Col>
          <Grid.Col span={6}>
            <FeaturesCard 
            naam={"Pay as you go"} 
            price={2}
            description="U hoeft niet vooraf te betalen, betaal wanner u wilt."
            features={
              [
                {
                  label: "Achteraf betalen",
                  icon: IconCurrencyEuro
                }
              ]
            }
             /></Grid.Col>
        </Grid>
      </Container>


    </>
  );
}