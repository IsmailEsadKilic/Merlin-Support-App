using API.DTOs;
using API.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class TeamRepository
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public TeamRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<TeamDto>> GetTeamDtosAsync()
        {
            var teams = await _context.Teams
                .Include(t => t.TeamMembers)
                .ProjectTo<TeamDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            //get user names for each team member

            foreach (var team in teams)
            {
                foreach (var teamMember in team.TeamMemberDtos)
                {
                    var user = await _context.Users.FindAsync(teamMember.UserId);
                    teamMember.Name = user.UserName;
                }
            }

            return teams;
        }

        public async Task<TeamDto> GetTeamDtoAsync(int id)
        {
            var team = await _context.Teams
                .Where(t => t.Id == id)
                .Include(t => t.TeamMembers)
                .ProjectTo<TeamDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            //get user names for each team member

            foreach (var teamMember in team.TeamMemberDtos)
            {
                var user = await _context.Users.FindAsync(teamMember.UserId);
                teamMember.Name = user.UserName;
            }

            return team;
        }

        public async Task<int> AddTeamAsync(Team team)
        {
            await _context.Teams.AddAsync(team);

            var advancedMode = false;
            if (advancedMode)
            {
                //add team members
                foreach (var teamMember in team.TeamMembers)
                {
                    TeamMember teamMemberAdd = new TeamMember
                    {
                        UserId = teamMember.UserId,
                        TeamId = team.Id
                    };

                    await _context.TeamMembers.AddAsync(teamMemberAdd);
                }
            }

            await _context.SaveChangesAsync();
            return team.Id;
        }

        public async Task<bool> UpdateTeamAsync(int id, TeamDto teamDto, List<int> teamMemberIdsToDel)
        {
            Team team = await _context.Teams.FindAsync(id);

            if (team == null)
            {
                return false;
            }

            team.TeamName = teamDto.TeamName;

            //add team members
            foreach (var teamMember in teamDto.TeamMemberDtos)
            {
                TeamMember teamMemberAdd = new TeamMember
                {
                    UserId = teamMember.UserId,
                    TeamId = team.Id
                };

                await _context.TeamMembers.AddAsync(teamMemberAdd);
            }

            //delete team members

            foreach (var teamMemberId in teamMemberIdsToDel)
            {
                var teamMember = await _context.TeamMembers.FindAsync(teamMemberId);
                _context.TeamMembers.Remove(teamMember);
            }

            _context.Entry(team).State = EntityState.Modified;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteTeamAsync(int id)
        {
            // Team team = await _context.Teams.FindAsync(id);
            // include team members

            Team team = await _context.Teams
                .Where(t => t.Id == id)
                .Include(t => t.TeamMembers)
                .SingleOrDefaultAsync();

            if (team == null)
            {
                return false;
            }

            //remove team members

            foreach (var teamMember in team.TeamMembers)
            {
                _context.TeamMembers.Remove(teamMember);
            }

            _context.Teams.Remove(team);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}