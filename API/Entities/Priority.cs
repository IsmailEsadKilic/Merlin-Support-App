using API.enums;

namespace API.Entities
{
    public class Priority
    {
        public int Id { get; set; }
        public string RowGuid { get; set; }
        public string PriorityName { get; set; }

        public TimeType TimeType { get; set; }
        public int Time { get; set; }
        public int ListIndex { get; set; }

    }
}