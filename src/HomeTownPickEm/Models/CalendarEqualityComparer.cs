namespace HomeTownPickEm.Models;

public class CalendarEqualityComparer : IEqualityComparer<Models.Calendar>
{
    public bool Equals(Models.Calendar x, Models.Calendar y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (ReferenceEquals(x, null))
        {
            return false;
        }

        if (ReferenceEquals(y, null))
        {
            return false;
        }

        if (x.GetType() != y.GetType())
        {
            return false;
        }

        return x.Season == y.Season && x.Week == y.Week && x.SeasonType == y.SeasonType;
    }

    public int GetHashCode(Models.Calendar obj)
    {
        return HashCode.Combine(obj.Season, obj.Week, obj.SeasonType);
    }
}