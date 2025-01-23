/// <reference types="cypress" />

describe('Registreren met een ongeldig emailadres', () => {
    it('Geeft feedback bij ongeldig emailadres', () => {
        cy.visit('https://localhost:60281/');

        cy.contains('Registreren').click();

        cy.get('input[placeholder="Naam"]').type('Test Gebruiker');
        cy.get('input[placeholder="naam@adres.nl"]').type('ongeldig-email');
        cy.get('input[placeholder="Straat 123"]').type('Hoofdstraat 12');
        cy.get('input[placeholder="Wachtwoord"]').type('SterkW@chtwoord1');
        cy.get('input[placeholder="Wachtwoord bevestigen"]').type('SterkW@chtwoord1');

        cy.contains('button', 'Registreren').click();

        cy.contains('Ongeldig emailadres').should('be.visible');
    });
});
