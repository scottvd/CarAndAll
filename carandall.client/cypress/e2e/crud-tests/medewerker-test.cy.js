describe('Medewerker maken en verwijderen', () => {
    it('logt in, maakt een nieuwe medewerker aan, en verwijdert de medewerker.', () => {
        cy.visit('https://localhost:60281/');
        cy.contains('Inloggen').click();

        cy.get('input[placeholder="naam@adres.nl"]').type('scott@caa.nl');
        cy.get('input[placeholder="Wachtwoord"]').type('Test123!');
        cy.get('button[type="submit"]').click();

        cy.contains('Controlepaneel').click();
        cy.contains('Medewerkers beheren').click();

        const medewerkerNaam = "Feltcute Mightdeletelater";
        const personeelsNummer = 123456;
        const emailAdres = "fcmdl@caa.nl";
        const wachtwoord = "T3stW@chtwoord123";

        cy.get('input[placeholder="Naam"]').type(medewerkerNaam);
        cy.get('input[placeholder="Personeelsnummer"]').type(personeelsNummer);
        cy.get('input[placeholder="medewerker@caa.nl"]').type(emailAdres);
        cy.get('input[placeholder="Wachtwoord"]').type(wachtwoord);
        cy.get('input[placeholder="Wachtwoord bevestigen"]').type(wachtwoord);
        cy.contains('Toevoegen').click();

        cy.contains(medewerkerNaam).should('exist');

        cy.contains(medewerkerNaam).parents('tr').within(() => {
            cy.contains('Bewerken').click();
        });

        cy.get('button').contains('Verwijder medewerker').click();

        cy.on('window:confirm', (text) => {
            expect(text).to.contains('Weet u zeker dat u deze medewerker wilt verwijderen?');
            return true;
        });

        cy.contains(medewerkerNaam).should('not.exist');
    });
});
