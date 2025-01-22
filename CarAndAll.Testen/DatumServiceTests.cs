using CarAndAll.Services;

public class DatumServiceTests
{
    [Fact]
    public void IsVerlopenMetDatumOverDeDrempel()
    {
        var datum = DateTime.UtcNow.AddDays(-91).Date;
        var drempelWaarde = 90;

        var result = DatumService.IsVerlopen(datum, drempelWaarde);

        Assert.True(result);
    }

    [Fact]
    public void IsVerlopenMetDatumOnderDeDrempel()
    {
        var datum = DateTime.UtcNow.AddDays(-30).Date;
        var drempelWaarde = 90;

        var result = DatumService.IsVerlopen(datum, drempelWaarde);

        Assert.False(result);
    }

    [Fact]
    public void IsVerlopenMetDatumOpDeDrempel()
    {
        var datum = DateTime.UtcNow.AddDays(-90).Date;
        var drempelWaarde = 90;

        var result = DatumService.IsVerlopen(datum, drempelWaarde);

        Assert.False(result);
    }
}
