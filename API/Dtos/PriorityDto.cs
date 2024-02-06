namespace API.DTOs
{
    public class PriorityDto
    {
        public int Id { get; set;}
        public string RowGuid { get; set;}
        public string PriorityName { get; set;}
        public int TimeType { get; set;} //the enum
        public int Time { get; set;} //the amount 
        public int ListIndex { get; set;}
    }
}

    // public class Priority
    // {
    //     public int Id { get; set; }
    //     public string RowGuid { get; set; }
    //     public string PriorityName { get; set; }

    //     public TimeType TimeType { get; set; }
    //     public int Time { get; set; }
    //     public int ListIndex { get; set; }

    // }