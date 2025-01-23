/// <reference types="cypress" />

describe('Registreren met een ongeldig emailadres', () => {
    it('Geeft feedback bij ongeldig emailadres', () => {
        cy.visit('https://localhost:60281/');

        cy.contains('Registreren').click();

        cy.get('input[placeholder="Naam"]').type('Test Gebruiker');
        cy.get('input[placeholder="naam@adres.nl"]').type('testabc@test.nl');
        cy.get('input[placeholder="Straat 123"]').type('Hoofdstraat 12');
        cy.get('input[placeholder="Wachtwoord"]').type('SterkW@chtwoord1');
        cy.get('input[placeholder="Wachtwoord bevestigen"]').type('SterkW@chtwoord1');

        cy.contains('button', 'Registreren').click();

        cy.url().should('include', '/login');

        cy.get('input[placeholder="naam@adres.nl"]').type('testabc@test.nl');
        cy.get('input[placeholder="Wachtwoord"]').type('SterkW@chtwoord1');
        cy.get('button[type="submit"]').click();
        
        cy.contains('Profiel').click();
        cy.contains('Gegevens bewerken').click();

        cy.wait(3000);

        cy.contains('Gegevens verwijderen').click();

        cy.contains('Verwijderingsverzoek succesvol ingediend! Over 6 maanden worden uw gegevens definitief verwijderd.')
    });
});
