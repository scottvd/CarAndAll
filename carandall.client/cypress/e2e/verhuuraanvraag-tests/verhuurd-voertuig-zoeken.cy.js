/// <reference types="cypress" />

describe('Verhuurd voertuig zoeken', () => {
    Cypress.on('uncaught:exception', (err) => {
      if (err.message.includes('ResizeObserver loop completed with undelivered notifications')) {
        return false;
      }
      return true;
    });
    
      it('logt in, kiest vaste datums, zoekt voertuig wat a verhuurd is', () => {
        cy.visit('https://localhost:60281/');
        cy.contains('Inloggen').click();
        cy.get('input[placeholder="naam@adres.nl"]').type('testin@test.nl');
        cy.get('input[placeholder="Wachtwoord"]').type('Test123!');
        cy.get('button[type="submit"]').click();
        
        cy.contains('Huren').click();
        cy.contains('Voertuigen bekijken').click();
        
        cy.contains('Klik hier om een ophaaldatum te kiezen').click();
        cy.get(
          'button.mantine-DatePickerInput-day[aria-label="30 January 2025"]'
        ).click();
    
        cy.contains('Klik hier om een inleverdatum te kiezen').click();
        cy.get(
          'button.mantine-DatePickerInput-day[aria-label="31 January 2025"]'
        ).click();
    
        cy.get('button').contains('Voertuigen weergeven').click();
    
        cy.get('input[placeholder="Voer een merk in"]').type('Mercedes');
        cy.get('input[placeholder="Selecteer een soort"]').click();
        cy.get('span').contains('Camper').click();
        cy.get('input[placeholder="Selecteer een prijsklasse"]').click();
        cy.get('div').contains('€50 - €75').click();
    
        cy.contains('Mercedes Sprinter').should('not.exist');
      });
    });
    