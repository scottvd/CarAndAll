/// <reference types="cypress" />

// Voor deze test moet er een openstaande verhuuraanvraag zijn met een datum van vandaag of in het verleden


describe('Verhuuraanvraag goedkeuren', () => {
    it('logt in, gaat naar controlepaneel, en accepteert een verhuuraanvraag', () => {
        cy.visit('https://localhost:60281/');
        cy.contains('Inloggen').click();

        cy.get('input[placeholder="naam@adres.nl"]').type('scott@caa.nl');
        cy.get('input[placeholder="Wachtwoord"]').type('Test123!');
        cy.get('button[type="submit"]').click();

        cy.contains('Controlepaneel').click();
        cy.contains('Verhuuraanvragen beheren').click();

        cy.get('tbody tr')
            .should('exist')
            .then(($rows) => {
                const randomRow = Cypress._.sample($rows);

                cy.wrap(randomRow)
                    .find('[aria-label*="Accepteer verhuuraanvraag"]')
                    .click();
            });

        cy.contains('Verhuuraanvraag succesvol behandeld!').should('be.visible');
    });
});
