using System;

namespace Sygic.Corona.Infrastructure.Services.DateTimeConverting
{
    public interface IDateTimeConvertService
    {
        DateTime UnixTimeStampToDateTime(int unixTimeStamp);
        long DateTimeToUnixTimestamp(DateTime dateTime);
    }
}
