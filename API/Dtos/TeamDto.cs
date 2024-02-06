namespace API.DTOs
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string RowGuid { get; set; }  //this is a typo in the database as well
        public string TeamName { get; set; }
        public IList<TeamMemberDto> TeamMemberDtos { get; set; }
    }

    public class TeamMemberDto
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int UserId { get; set; }
    }
}