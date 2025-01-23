/// <reference types="cypress" />

describe('Wachtwoord bijwerken via Profiel', () => {
    it('logt in, gaat naar profiel, en wijzigt het wachtwoord', () => {
      cy.visit('https://localhost:60281/');
  
      cy.contains('Inloggen').click();

      cy.get('input[placeholder="naam@adres.nl"]').type('tony@baronesanitation.com');
      cy.get('input[placeholder="Wachtwoord"]').type('Test123!');
      cy.get('button[type="submit"]').click();
  
      cy.contains('Profiel').click();
      cy.contains('Gegevens bewerken').click();
  
      cy.get('input[placeholder="Oud wachtwoord"]').type('Test123!');
      cy.get('input[placeholder="Wachtwoord"]').type('Test321!');
  
      cy.contains('Opslaan').click();
  
      cy.contains('Gegevens bijgewerkt!').should('be.visible');
  
      cy.contains('Uitloggen').click();
      cy.contains('Log uit').click();

      cy.contains('U bent succesvol uitgelogd!').should('be.visible');

      cy.contains('Inloggen').click();

      cy.get('input[placeholder="naam@adres.nl"]').type('tony@baronesanitation.com');
      cy.get('input[placeholder="Wachtwoord"]').type('Test321!');
      cy.get('button[type="submit"]').click();
  
      cy.contains('U bent ingelogd!').should('be.visible');
    });
});
  