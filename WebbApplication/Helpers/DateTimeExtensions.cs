namespace WebbApplication.Helpers;

/* Denna klass är genererad av Chat GPT 4.0 för att visa i projektvyn
 hur lång tid det är kvar till slutdatum av respektive projekt */

public static class DateTimeExtensions
{
    public static string ToTimeLeft(this DateTime? endDate)
    {
        if (endDate == null)
            return "No due date";

        var remaining = endDate.Value.Date - DateTime.Now.Date;

        if (remaining.TotalDays < 0)
            return "Due time passed";

        if (remaining.TotalDays < 7)
        {
            var days = remaining.Days;
            return days == 1 ? "1 day left" : $"{days} days left";
        }

        if (remaining.TotalDays < 30)
        {
            var weeks = (int)(remaining.TotalDays / 7);
            return weeks == 1 ? "1 week left" : $"{weeks} weeks left";
        }

        var months = (int)(remaining.TotalDays / 30);
        return months == 1 ? "1 month left" : $"{months} months left";
    }
}