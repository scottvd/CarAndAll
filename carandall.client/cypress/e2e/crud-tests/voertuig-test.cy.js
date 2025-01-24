describe('Voertuig maken en aanpassen', () => {
    it('logt in, maakt een nieuw voertuig aan, en bewerkt het voertuig.', () => {
        cy.visit('https://localhost:60281/');
        cy.contains('Inloggen').click();

        cy.get('input[placeholder="naam@adres.nl"]').type('scott@caa.nl');
        cy.get('input[placeholder="Wachtwoord"]').type('Test123!');
        cy.get('button[type="submit"]').click();

        cy.contains('Vloot').click();
        cy.contains('Vloot beheren').click();

        cy.contains('Voertuig toevoegen').click();

        const kenteken = '227-GXFR';
        const soort = 'Fiets';
        const merk = 'Gazelle';
        const type = 'Uit de sloot';
        const aanschafjaar = 1973;

        cy.get('input[placeholder="Vul het kenteken in"]').type(kenteken);
        cy.get('input[placeholder="Voer het soort voertuig in"]').type(soort);
        cy.get('input[placeholder="Voer het merk in"]').type(merk);
        cy.get('input[placeholder="Voer het type voertuig in"]').type(type);
        cy.get('input[placeholder="Voer het aanschafjaar in"]').type(aanschafjaar);

        cy.contains('Toevoegen').click();

        cy.wait(2000);

        cy.contains('Deactiveren').click();
        cy.contains('Voertuig succesvol gedeactiveerd!').should('exist');
    });
});
