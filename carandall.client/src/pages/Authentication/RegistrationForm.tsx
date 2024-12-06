import {
  Anchor,
  Button,
  Checkbox,
  Container,
  Paper,
  PasswordInput,
  Text,
  TextInput,
  Title,
} from '@mantine/core';
import classes from './AuthenticationForm.module.css';
import { useForm } from '@mantine/form';
import { upperFirst, useToggle } from '@mantine/hooks';



export function RegistrationForm() {

  const [type, toggle] = useToggle(['login', 'register']);

  const form = useForm({
    initialValues: {
      email: '',
      name: '',
      password: '',
      accountType: ''
    }
  })

  return (
    <div>
      <Container className={classes.form} p={30}>
        <Title order={2} className={classes.title} ta="center" mt="md" mb={50}>
          Registreren bij CarAndAll!
        </Title>

        <TextInput label="Email"
          placeholder="hello@mantine.dev"
          value={form.values.email}
          onChange={(event) => form.setFieldValue('email', event.currentTarget.value)}
          error={form.errors.email && 'Invalid email'}
          size="md" />
        <PasswordInput
          required
          label="Password"
          placeholder="Uw wachtwoord"
          mt="md"
          size="md"
          onChange={(event) => form.setFieldValue('password', event.currentTarget.value)}
        // error={form.errors.password && }


        />
        <Checkbox label="Keep me logged in" mt="xl" size="md" />


        {type === 'register'}
        <Button fullWidth mt="xl" size="md">
          Registreren
        </Button>

        <Text ta="center" mt="md">
          Heeft u nog geen account?{' '}
          <Anchor<'a'> href="#" fw={700} onClick={(event) => {
            console.log(type);
            toggle();
            event.preventDefault();
          }}>
            Registreren
          </Anchor>
        </Text>
      </Container>
    </div>
  );
}