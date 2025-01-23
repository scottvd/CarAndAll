/// <reference types="cypress" />

// Om deze test te runnen moet er een gebruiker in de database zijn met de creds testin@test.nl en Test123!
// De datum in de datepicker moet evt ook bijgesteld worden afhankelijk van wanneer je test
// Verder zijn de prijzen gerandomised dus kijk even goed welk voertuig je wilt testen met filters etc


describe('Voertuigen filteren', () => {
    it('logt in, kiest vaste datums, filtert op Mercedes, prijsklasse 50-75, en verwijdert filters na het vinden van Opel Movano', () => {
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
      
      cy.get('input[placeholder="Selecteer een prijsklasse"]').click();
      cy.get('div').contains('€50 - €75').click();
      
      cy.get('.mantine-Card-root')
        .each(($el) => {
          cy.wrap($el)
            .find('.mantine-Text-root')
            .should('include.text', 'Mercedes');
        });
  
      cy.contains('Mercedes Sprinter').should('exist');
  
      cy.get('input[placeholder="Voer een merk in"]').clear();
  
      cy.get('.mantine-Card-root')
        .first()
        .should('not.have.text', 'Mercedes');
    });
  });
  