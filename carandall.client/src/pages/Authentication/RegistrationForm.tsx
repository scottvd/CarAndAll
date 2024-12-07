import {
  Anchor,
  Button,
  Checkbox,
  Container,
  Input,
  NativeSelect,
  Paper,
  PasswordInput,
  rem,
  Select,
  Text,
  TextInput,
  Title,
} from '@mantine/core';
import classes from './AuthenticationForm.module.css';
import { hasLength, isEmail, isInRange, isNotEmpty, matchesField, useForm } from '@mantine/form';
import { upperFirst, useToggle } from '@mantine/hooks';
import { IconChevronDown } from '@tabler/icons-react';



export function RegistrationForm() {
  const form = useForm({
    validateInputOnBlur: true,
    initialValues: {
      email: '',
      naam: '',
      password: '',
      confirmPassword: '',
      accountType: '',
      adres: '',
      kvkNummer: ''
    },
    validate: {
      email: isEmail("Ongeldige e-mail."),
      naam: isNotEmpty("Naam is verplicht."),
      password: hasLength({ min: 8 }, 'Password must be at least 8 characters long'),
      confirmPassword: matchesField('password', 'Passwords do not match'),
    }
  })


  return (
    <div>
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

          onChange={(e) => {
            form.setFieldValue('accountType', e.currentTarget.value);
            console.log(e.currentTarget.value);
          }}
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
          onClick={(e) => {
            console.log(form);
          }}
          fullWidth mt="xl" size="md">
          Registreren
        </Button>

        {/* <Text ta="center" mt="md">
          Heeft u nog geen account?{' '}
          <Anchor<'a'> href="#" fw={700} onClick={(event) => {
            console.log(type);
            toggle();
            event.preventDefault();
          }}>
            Registreren
          </Anchor>
        </Text> */}
      </Container>
    </div>
  );
}