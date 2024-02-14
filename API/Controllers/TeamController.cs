using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using API.Data;
using API.DTOs;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TeamController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _uow;

        public TeamController(IMapper mapper, UnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<TeamDto>> GetTeamsAsync()
        {
            return Ok(await _uow.TeamRepository.GetTeamDtosAsync());
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamDto>> GetTeamAsync(int id)
        {
            return Ok(await _uow.TeamRepository.GetTeamDtoAsync(id));
        }

        [Authorize(Roles = "113, 136")]
        [HttpPut("add")]
        public async Task<ActionResult<TeamDto>> AddTeamAsync(TeamDto teamDto)
        {


            Team team = _mapper.Map<Team>(teamDto);
            team.RowGuid = Guid.NewGuid().ToString();
            
            var id = await _uow.TeamRepository.AddTeamAsync(team);

            if (id > 0)
            {
                return Ok(await _uow.TeamRepository.GetTeamDtoAsync(id));
            }
            else
            {
                return BadRequest("Failed to add team");
            }
        }

        [Authorize(Roles = "113, 137")]
        [HttpPost("update/{id}")]
        public async Task<ActionResult<TeamDto>> UpdateTeamAsync(int id, TeamUpdateRequest updateRequest)
        {
            var ok = await _uow.TeamRepository.UpdateTeamAsync(id, updateRequest.teamDto, updateRequest.teamMemberIdsToDel);

            if (ok)
            {
                return Ok(await _uow.TeamRepository.GetTeamDtoAsync(id));
            }
            else
            {
                return BadRequest("Failed to update team");
            }
        }

        [Authorize(Roles = "113, 138, 139")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteTeamAsync(int id)
        {
            var ok = await _uow.TeamRepository.DeleteTeamAsync(id);

            if (ok)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest("Failed to delete team");
            }
        }
    }

    public class TeamUpdateRequest
    {
        public TeamDto teamDto { get; set; }
        public List<int> teamMemberIdsToDel { get; set; } // ids of teamMebers to delete
    }
}