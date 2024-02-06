namespace API.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string RowGuid { get; set; }  //this is a typo in the database as well
        public string TeamName { get; set; }
        public ICollection<TeamMember> TeamMembers { get; set; }
    }

    public class TeamMember
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int UserId { get; set; }
    }
}