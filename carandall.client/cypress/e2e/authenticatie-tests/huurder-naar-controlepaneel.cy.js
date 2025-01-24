//Voor deze test moet er een gebruiker ja@rule.com met het wachtwoord Test123! in de database bestaan

describe('Huurder probeert gebruik te maken van het controlepaneel', () => {
    it('logt in, past de url om naar het controlepaneel te gaan, en wordt verwezen naar unauthorized.', () => {
        cy.visit('https://localhost:60281/');
        cy.contains('Inloggen').click();

        cy.get('input[placeholder="naam@adres.nl"]').type('ja@rule.nl');
        cy.get('input[placeholder="Wachtwoord"]').type('Test123!');
        cy.get('button[type="submit"]').click();

        cy.visit('https://localhost:60281/dashboard/controlepaneel');
        cy.contains('401').should('exist');
    });
});
