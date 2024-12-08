import {
  Anchor,
  Button,
  Container,
  Paper,
  PasswordInput,
  Text,
  TextInput,
  Title,
} from '@mantine/core';
import classes from './LoginForm.module.css';
import { useNavigate } from 'react-router-dom';
import { notifications } from '@mantine/notifications';
import { AxiosError } from 'axios';
import { api } from '../../main';
import { useForm, isEmail } from '@mantine/form';

export function LoginForm() {
  const navigate = useNavigate();


  const form = useForm({
    validateInputOnBlur: true,
    initialValues: {
      email: '',
      password: '',
    },
    validate: {
      email: isEmail("Ongeldige e-mail."),
      password: (value, values) => {
        if (!value) return 'Wachtwoord veld mag niet leeg zijn.';
        return null;
      },
    }
  })

  const handleSubmit = (values: any) => {
    console.log(values);

    api.post("/login?useCookies=true", {
      ...values
    }).
      then((v) => {
        console.log(v);
        notifications.show({
          title: "Ingelogd",
          message: " U wordt nu doorgeleid naar het dashboard."
        });

        setTimeout(() => {
          navigate('/dashboard');
        }, 1500)
      })
      .catch((err: AxiosError) => {
        console.log(err.response);
        
        if (err.response?.status == 401) {
          notifications.show({
            color: 'red',
            title: 'Ongeldige inloggegevens.',
            message: "Check uw gegevens en probeer het nogmaals."
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
    <Container>
      <div className={classes.wrapper}>
        <Paper className={classes.form} radius={0} p={30}>
          <Title order={2} className={classes.title} ta="center" mt="md" mb={50}>
            Welkom terug bij CarAndAll!
          </Title>

          <form onSubmit={form.onSubmit((values) => handleSubmit(values))}>
            <TextInput label="Email address" placeholder="admin@carandall.nl" size="md"  {...form.getInputProps("email")} />
            <PasswordInput label="Wachtwoord" placeholder="Uw wachtwoord" mt="md" size="md"  {...form.getInputProps("password")} />
            {/* <Checkbox label="Keep me logged in" mt="xl" size="md" /> */}
            <Button type="submit" fullWidth mt="xl" size="md">
              Login
            </Button>

            <Text ta="center" mt="md">
              Heeft u nog geen account?{' '}
              <Anchor<'a'> href="#" fw={700} onClick={(event) => {
                navigate("/register");
                event.preventDefault()
              }}>
                Registreren
              </Anchor>
            </Text>
          </form>
        </Paper>
      </div >
    </Container>
  );
}