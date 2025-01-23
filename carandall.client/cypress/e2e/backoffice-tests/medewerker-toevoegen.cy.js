/// <reference types="cypress" />

describe('Medewerkers toevoegen en wijzigen', () => {
    it('logt in, gaat naar controlepaneel, en voert CRUD operaties uit', () => {
      cy.visit('https://localhost:60281/');
  
      cy.contains('Inloggen').click();

      cy.get('input[placeholder="naam@adres.nl"]').type('scott@caa.nl');
      cy.get('input[placeholder="Wachtwoord"]').type('Test123!');
      cy.get('button[type="submit"]').click();

      cy.contains('Controlepaneel').click();
      cy.contains('Medewerkers beheren').click();

      cy.get('input[placeholder="Personeelsnummer"]').type(123);
      cy.get('input[placeholder="Wachtwoord"]').type("N3pW@chtwoord123");
      cy.get('input[placeholder="Wachtwoord bevestigen"]').type("N3pW@chtwoord123");
      cy.contains('Toevoegen').click();

      cy.contains('Vul dit verplichte veld in').should('be.visible');
      cy.contains('Ongeldig emailadres').should('be.visible');
    
      cy.wait(2000);

      cy.get('input[placeholder="Naam"]').type("Mikey Palmice");
      cy.get('input[placeholder="medewerker@caa.nl"]').type("mike@caa.nl");

      cy.contains('Toevoegen').click();

      cy.get('input[placeholder="Naam"]').type("Michael Scott");
      cy.get('input[placeholder="Personeelsnummer"]').type(123456);
      cy.get('input[placeholder="medewerker@caa.nl"]').type("mike@caa.nl"); 
      cy.get('input[placeholder="Wachtwoord"]').type("N3pW@chtwoord123");
      cy.get('input[placeholder="Wachtwoord bevestigen"]').type("N3pW@chtwoord123");
      cy.contains('Toevoegen').click();

      cy.contains('Er is iets fout gegaan tijdens het toevoegen van de medewerker. Probeer het opnieuw!').should('be.visible');
    });
});
  