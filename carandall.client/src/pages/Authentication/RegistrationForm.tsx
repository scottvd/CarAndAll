import {
  Button,
  Container,
  NativeSelect,
  PasswordInput,
  rem,
  TextInput,
  Title,
} from '@mantine/core';
import classes from './AuthenticationForm.module.css';
import { hasLength, isEmail, isNotEmpty, matchesField, useForm } from '@mantine/form';
import { IconChevronDown } from '@tabler/icons-react';
// import axios from 'axios';
// import { AxiosError } from 'axios';
import { api } from '../../main';
import { notifications } from '@mantine/notifications';
import { useNavigate } from 'react-router-dom';
import { AxiosError } from 'axios';


export function RegistrationForm() {
  const navigate = useNavigate();
  const form = useForm({
    validateInputOnBlur: true,
    initialValues: {
      email: '',
      naam: '',
      password: '',
      confirmPassword: '',
      accountType: "Particulier",
      adres: '',
      kvkNummer: ''
    },
    validate: {
      email: isEmail("Ongeldige e-mail."),
      naam: isNotEmpty("Naam is verplicht."),
      password: (value, values) => {
        if (!value) return 'Password is required';
        if (!/[^a-zA-Z0-9]/.test(value)) return 'Password moet minimaal 1 niet-alfanumerisch karakter bevatten.';
        if (!/\d/.test(value)) return 'Wachtwoord moet minimaal 1 cijfer bevatten.';
        if (!/[A-Z]/.test(value)) return 'Wachtwoord moet minimaal 1 hoofdletter bevatten.';
        return null;
      },
      confirmPassword: matchesField('password', 'Wachtwoord is niet hetzelfde.'),
      accountType: isNotEmpty("Account type is verplicht."),
      adres: isNotEmpty("Adres is verplicht."),
      kvkNummer: (value, values) => {
        if (values.accountType === 'Zakelijk') {
          if (!value) {
            return 'KvK-nummer is verplicht voor zakelijke accounts';
          }
          if (!/^\d{8}$/.test(value)) {
            return 'KvK-nummer moet 8 cijfers bevatten';
          }
          // Add more conditions here if needed
        }
        return null;
      }
    }
  })

  const handleSubmit = (values: any) => {
    console.log(values);

    api.post("/register_dto", {
      ...values
    }).
      then((v) => {
        console.log(v);
        notifications.show({
          title: "Account aangemaakt.",
          message: "U kunt nu inloggen. U wordt nu doorgeleid..."
        });

        setTimeout(() => {
          navigate('/login');
        }, 1500)
      })
      .catch((err: any) => {
        console.log(err.response);

        if (err.response.data.code == "DuplicateUserName") { // @t
          notifications.show({
            color: 'red',
            title: 'E-mail al in gebruik.',
            message: "Probeer een ander email-adres."
          })
        } else {

          notifications.show({
            color: 'red',
            title: 'Er ging iets mis.',
            message: "Probeer het zometeen nog eens."
          })
        }


      });
  };



  return (
    <form onSubmit={form.onSubmit((values) => handleSubmit(values))}>
      <Container className={classes.form} p={30}>
        <Title order={2} className={classes.title} ta="center" mt="md" mb={50}>
          Registreren bij CarAndAll!
        </Title>


        <NativeSelect
          required
          label="Account Type"
          rightSection={<IconChevronDown style={{ width: rem(16), height: rem(16) }} />}
          data={['Zakelijk', 'Particulier']}
          mt="md"
          {...form.getInputProps("accountType")}
          size="md"
        />


        <TextInput label="Naam"
          required
          placeholder="Piet Jan"
          size="md"
          {...form.getInputProps("naam")}
        />

        <TextInput label="Email"
          required
          placeholder="admin@carandall.nl"
          mt="md"
          size="md"
          {...form.getInputProps("email")}
        />
        <PasswordInput
          required
          label="Password"
          placeholder="Uw wachtwoord"
          mt="md"
          size="md"
          {...form.getInputProps("password")}
        />
        <PasswordInput
          required
          label="Wachtwoord-bevestiging"
          placeholder="Uw wachtwoord"
          mt="md"
          size="md"
          {...form.getInputProps("confirmPassword")}
        />


        <TextInput label="Adres"
          required
          placeholder="Hilversumstraat 20"
          mt="md"
          size="md"
          {...form.getInputProps("adres")}
        />

        {form.values.accountType == "Zakelijk" ?
          (
            <TextInput label="KvK-nummer"
              required
              placeholder="12345678"
              size="md"
              {...form.getInputProps("kvkNummer")}
            />
          )

          : <></>

        }

        <Button
          type="submit"
          fullWidth mt="xl"
          size="md"
          disabled={!form.isValid()}
        >
          Registreren
        </Button>
      </Container>
    </form>
  );
}