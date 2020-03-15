using System;

namespace Sygic.Corona.Infrastructure.Services.DateTimeConverting
{
    public class DateTimeConvertService : IDateTimeConvertService
    {
        public DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            //dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }

        public long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            var dateTimeOffset = new DateTimeOffset(dateTime);
            long unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
            return unixDateTime;
        }
    }
}
