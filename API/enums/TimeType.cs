namespace API.enums
{
    public enum TimeType
    {
        Minutes = 1,
        Hours = 2,
        Days = 3
    }

        class TimeTypeHelper
    {
        public static Dictionary<string, string> GetTimeTypesDict()
        {
            var dict = new Dictionary<string, string>
            {
                {"1", "Dakika"},
                {"2", "Saat"},
                {"3", "Gün"}
            };

            return dict;
        }
    }
}