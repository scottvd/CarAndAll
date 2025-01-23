/// <reference types="cypress" />

describe('Registreren met een bestaand KVK-nummer', () => {
    it('ontvangt een foutmelding van de API voor een bestaand KVK-nummer', () => {
        cy.visit('https://localhost:60281/');

        cy.contains('Registreren').click();

        cy.get('input[placeholder="Naam"]').type('Zakelijk Huurder');
        cy.get('input[placeholder="naam@adres.nl"]').type('zakelijk@huurder.nl');
        cy.get('input[placeholder="Straat 123"]').type('Bedrijfsstraat 45');

        cy.get('label:contains("Zakelijke huurder")').click();

        cy.get('input[placeholder="123456789"]').type('123456789');

        cy.get('input[placeholder="Bedrijfsnaam"]').type('Bestaand Bedrijf');
        cy.get('input[placeholder="Bedrijfsadres"]').type('Zakelijkeweg 10');

        cy.get('input[placeholder="Wachtwoord"]').type('SterkW@chtwoord1');
        cy.get('input[placeholder="Wachtwoord bevestigen"]').type('SterkW@chtwoord1');

        cy.get('button[type="submit"]').click();

        cy.contains("Er is iets fout gegaan tijdens het aanmaken van uw account. Probeer het opnieuw!").should('be.visible');
    });
});
