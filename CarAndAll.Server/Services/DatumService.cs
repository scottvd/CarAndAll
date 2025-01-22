using System;

namespace CarAndAll.Services
{
    public static class DatumService
    {
        public static bool IsVerlopen(DateTime datum, int drempelWaarde)
        {
            var huidigeDatum = DateTime.UtcNow.Date;

            var drempelDatum = huidigeDatum.AddDays(-drempelWaarde);

            return datum.Date < drempelDatum.Date;
        }
    }
}
