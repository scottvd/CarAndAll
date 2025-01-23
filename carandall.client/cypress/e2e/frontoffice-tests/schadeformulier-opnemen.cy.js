/// <reference types="cypress" />

// Om deze test te runnen moet er een gebruiker in de database zijn met de creds ja@rule.com en Test123! de datum voor date picker moet evt ook aangepast worden, de datepicker is lastig om te testen

describe('Schadeformulier maken en voertuig zoeken', () => {
    it('logt in, gaat naar controlepaneel, en voegt een schadeformulier toe met 14 dagen herstelperiode en lege omschrijving. daarna correct ingevuld en controleer of het voertuig nog te vinden is.', () => {
        cy.visit('https://localhost:60281/');
        cy.contains('Inloggen').click();

        cy.get('input[placeholder="naam@adres.nl"]').type('scott@caa.nl');
        cy.get('input[placeholder="Wachtwoord"]').type('Test123!');
        cy.get('button[type="submit"]').click();

        cy.contains('Controlepaneel').click();
        cy.contains('Verhuuraanvragen afhandelen').click();

        cy.get('div[data-with-border="true"]')
            .first()
            .within(() => {
                cy.get('p').first().invoke('text').then((voertuig) => {
                    const voertuigType = voertuig.split(' ').pop();
                    cy.wrap(voertuigType).as('voertuigType');
                });
                cy.contains('Schadeformulier bijvoegen').click();
            });

        cy.get('input[placeholder="Aantal dagen"]').clear().type('14');

        cy.contains('Voertuig innemen').click();

        cy.contains('Voertuig kon niet ingenomen worden. Probeer het opnieuw!').should('be.visible');

        cy.wait(1000);

        cy.get('input[placeholder="Beschrijf de schade aan het voertuig"]').type("Motorblok ontbreekt");
        cy.contains('Voertuig innemen').click();
        
        cy.contains('Voertuig succesvol ingenomen!').should('be.visible');

        cy.wait(1000);

        cy.contains('Uitloggen').click();
        cy.contains('Log uit').click();

        cy.wait(5000);

        cy.contains('Inloggen').click();
        cy.get('input[placeholder="naam@adres.nl"]').type('ja@rule.nl');
        cy.get('input[placeholder="Wachtwoord"]').type('Test123!');
        cy.get('button[type="submit"]').click();
        
        cy.contains('Huren').click();
        cy.contains('Voertuigen bekijken').click();
    
        cy.contains('Klik hier om een ophaaldatum te kiezen').click();
        cy.get(
            'button.mantine-DatePickerInput-day[aria-label="26 January 2025"]'
        ).click();
    
        cy.contains('Klik hier om een inleverdatum te kiezen').click();
        cy.get(
            'button.mantine-DatePickerInput-day[aria-label="26 January 2025"]'
        ).click();

        cy.get('button').contains('Voertuigen weergeven').click();

        cy.get('@voertuigType').then((voertuigType) => {
            cy.get('input[placeholder="Voer een merk in"]').type(voertuigType);

            cy.contains(voertuigType).should('not.exist');
        });
    });
});
