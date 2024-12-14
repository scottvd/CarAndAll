import { Container, Grid, Title, Button, Text } from "@mantine/core";
import { FeaturesCard } from "../../components/FeaturesCard/FeaturesCard";
import { IconCurrencyEuro } from "@tabler/icons-react";
import { modals } from '@mantine/modals';
import { notifications } from '@mantine/notifications';


export function Aanvragen() {

  const openModal = () => modals.openConfirmModal({
    title: 'Please confirm your action',
    children: (
      <Text size="sm">
        Weet u zeker dat u dit abonnement wilt aanvragen?
      </Text>
    ),
    labels: { confirm: 'Confirm', cancel: 'Cancel' },
    onCancel: () => console.log('Cancel'),
    onConfirm: () => {
      console.log("Confirmed");
      notifications.show({
        message: 'Aanvraag voltooid. U krijgt een notificatie/e-mail wanneer deze aanvraag is goedgekeurd.',
        autoClose: 5000,
      });

    },
  });


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
              clickFunc={openModal}
              /></Grid.Col>
          <Grid.Col span={6}>
            <FeaturesCard 
            naam={"Pay as you go"} 
            price={2}
            description="U hoeft niet vooraf te betalen, betaal wanner u wilt."
            clickFunc={ openModal}
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