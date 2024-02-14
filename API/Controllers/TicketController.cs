using System.Security.Claims;
using System.Text.Json;
using API.Data;
using API.Dtos;
using API.DTOs;
using API.Entities;
using API.enums;
using API.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TicketController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _uow;

        public TicketController(IMapper mapper, UnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;

        }

        [Authorize(Roles = "113, 134")]
        [HttpGet("ticket")]
        public async Task<ActionResult<TicketDto>> GetTicketsAsync()
        {
            return Ok(await _uow.TicketRepository.GetTicketDtosAsync());
        }

        [Authorize]
        [HttpGet("ticket/{id}")]
        public async Task<ActionResult<TicketDto>> GetTicketAsync(int id)
        {
            return Ok(await _uow.TicketRepository.GetTicketDtoAsync(id));
        }

        [Authorize(Roles = "113, 135")]
        [HttpPut("ticket/add")]
        public async Task<ActionResult<TicketDto>> AddTicketAsync(TicketDto ticketDto)
        {
            var teamMemberDtos = ticketDto.TeamMemberDtos;

            Ticket ticket = _mapper.Map<Ticket>(ticketDto);
            ticket.RowGuid = Guid.NewGuid().ToString();
            ticket.TicketStateId = 1;

            var UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (UserId > 0)
            {
                ticket.UserId = UserId;
            }
            else
            {
                return BadRequest("Failed to add ticket");
            }

            //check if product license is valid

            var products = await _uow.ProductRepository.GetProductDtosByCustomerIdAsync(ticket.CustomerId);

            if (products.Count() == 0)
            {
                return BadRequest("No valid license found for this customer");
            }

            if (products.Where(p => p.Id == ticket.ProductId).Count() == 0)
            {
                return BadRequest("No valid license found for this product");
            }
        

            var id = await _uow.TicketRepository.AddTicketAsync(ticket, teamMemberDtos);

            if (id > 0)
            {
                return Ok(await _uow.TicketRepository.GetTicketDtoAsync(id));
            }
            else
            {
                return BadRequest("Failed to add ticket");
            }
        }

        [Authorize(Roles = "113, 156")]
        [HttpPost("ticket/update/{id}")]
        public async Task<ActionResult<TicketDto>> UpdateTicketAsync(int id, TicketDto ticketDto)
        {
            Console.WriteLine(JsonSerializer.Serialize(ticketDto));

            var ok = await _uow.TicketRepository.UpdateTicketAsync(id, ticketDto);

            if (ok)
            {
                return Ok(await _uow.TicketRepository.GetTicketDtoAsync(id));
            }
            else
            {
                return BadRequest("Failed to update ticket");
            }
        }

        [Authorize(Roles = "113, 114")]
        [HttpDelete("ticket/{id}")]
        public async Task<ActionResult<bool>> DeleteTicketAsync(int id)
        {
            var ok = await _uow.TicketRepository.DeleteTicketAsync(id);

            if (ok)
            {
                //delete uploaded files
                await _uow.UploadFileRepository.DeleteTicketFilesAsync(id);

                return Ok(true);
            }
            else
            {
                return BadRequest("Failed to delete ticket");
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Authorize] 
        [HttpGet("ticketNodesByTicketId/{id}")]
        public async Task<ActionResult<TicketNodeDto>> GetTicketNodeAsync(int id)
        {
            return Ok(await _uow.TicketRepository.GetTicketNodeDtosByTicketIdAsync(id));
        }

        [Authorize]
        [HttpPut("ticketNode/add")]
        public async Task<ActionResult<TicketNodeDto>> AddTicketNodeAsync(TicketNodeDto ticketNodeDto)
        {
            TicketNode ticketNode = _mapper.Map<TicketNode>(ticketNodeDto);
            ticketNode.RowGuid = Guid.NewGuid().ToString();
            ticketNode.RecordDate = DateTime.Now;
            ticketNode.RecordUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var id = await _uow.TicketRepository.AddTicketNodeAsync(ticketNode);

            if (id > 0)
            {
                return Ok(await _uow.TicketRepository.GetTicketNodeDtoAsync(id));
            }
            else
            {
                return BadRequest("Failed to add ticket node");
            }
        }

        [Authorize]
        [HttpDelete("ticketNode/{id}")]
        public async Task<ActionResult<bool>> DeleteTicketNodeAsync(int id)
        {
            var ok = await _uow.TicketRepository.DeleteTicketNodeAsync(id);

            if (ok)
            {
                //delete uploaded files
                await _uow.UploadFileRepository.DeleteTicketNodeFilesAsync(id);

                return Ok(true);
            }
            else
            {
                return BadRequest("Failed to delete ticket node");
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Authorize]
        [HttpGet("ticketType")]
        public async Task<ActionResult<TicketTypeDto>> GetTicketTypesAsync()
        {
            return Ok(await _uow.TicketRepository.GetTicketTypeDtosAsync());
        }

        [Authorize]
        [HttpGet("ticketType/{id}")]
        public async Task<ActionResult<TicketTypeDto>> GetTicketTypeAsync(int id)
        {
            return Ok(await _uow.TicketRepository.GetTicketTypeDtoAsync(id));
        }

        [Authorize(Roles = "144, 113")]
        [HttpPut("ticketType/add")]
        public async Task<ActionResult<TicketTypeDto>> AddTicketTypeAsync(TicketTypeDto ticketTypeDto)
        {
            TicketType ticketType = _mapper.Map<TicketType>(ticketTypeDto);
            ticketType.RowGuid = Guid.NewGuid().ToString();

            var id = await _uow.TicketRepository.AddTicketTypeAsync(ticketType);
            
            if (id > 0)
            {
                return Ok(await _uow.TicketRepository.GetTicketTypeDtoAsync(id));
            }
            else
            {
                return BadRequest("Failed to add ticket type");
            }
        }

        [Authorize(Roles = "145, 113")]
        [HttpPut("ticketType/update/{id}")]
        public async Task<ActionResult<TicketTypeDto>> UpdateTicketTypeAsync(int id, TicketTypeDto ticketTypeDto)
        {
            var ok = await _uow.TicketRepository.UpdateTicketTypeAsync(id, ticketTypeDto);

            if (ok)
            {
                return Ok(await _uow.TicketRepository.GetTicketTypeDtoAsync(id));
            }
            else
            {
                return BadRequest("Failed to update ticket type");
            }
        }

        [Authorize(Roles = "146, 157, 113")]
        [HttpDelete("ticketType/{id}")]
        public async Task<ActionResult<bool>> DeleteTicketTypeAsync(int id)
        {
            var ok = await _uow.TicketRepository.DeleteTicketTypeAsync(id);

            if (ok)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest("Failed to delete ticket type");
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Authorize]
        [HttpGet("priority")]
        public async Task<ActionResult<PriorityDto>> GetPrioritiesAsync()
        {
            return Ok(await _uow.TicketRepository.GetPriorityDtosAsync());
        }

        [Authorize]
        [HttpGet("priority/{id}")]
        public async Task<ActionResult<PriorityDto>> GetPriorityAsync(int id)
        {
            return Ok(await _uow.TicketRepository.GetPriorityDtoAsync(id));
        }

        [Authorize(Roles = "152, 113")]
        [HttpPut("priority/add")]
        public async Task<ActionResult<PriorityDto>> AddPriorityAsync(PriorityDto priorityDto)
        {
            Priority priority = _mapper.Map<Priority>(priorityDto);
            priority.RowGuid = Guid.NewGuid().ToString();

            var id = await _uow.TicketRepository.AddPriorityAsync(priority);

            if (await _uow.Complete() || id > 0)
            {
                return Ok(await _uow.TicketRepository.GetPriorityDtoAsync(id));
            }
            else
            {
                return BadRequest("Failed to add priority");
            }
        }

        [Authorize(Roles = "153, 113")]
        [HttpPut("priority/update/{id}")]
        public async Task<ActionResult<PriorityDto>> UpdatePriorityAsync(int id, PriorityDto priorityDto)
        {
            var ok = await _uow.TicketRepository.UpdatePriorityAsync(id, priorityDto);

            if (ok)
            {
                return Ok(await _uow.TicketRepository.GetPriorityDtoAsync(id));
            }
            else
            {
                return BadRequest("Failed to update priority");
            }
        }

        [Authorize(Roles = "154, 155, 113")]
        [HttpDelete("priority/{id}")]
        public async Task<ActionResult<bool>> DeletePriorityAsync(int id)
        {
            var ok = await _uow.TicketRepository.DeletePriorityAsync(id);

            if (ok)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest("Failed to delete priority");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        [Authorize]
        [HttpGet("timeTypesDict")]
        public ActionResult<Dictionary<string, string>> GetTimeTypesDict()
        {
            var en = TimeTypeHelper.GetTimeTypesDict();
            return Ok(en);
        }

        [Authorize]
        [HttpGet("ticketStatesDict")]
        public ActionResult<Dictionary<string, string>> GetTicketStatesDict()
        {
            var en = TicketStateHelper.GetTicketStatesDict();
            return Ok(en);
        }
    }
}
// public partial class TicketOwnerDetail
// {
//      public long Id { get; set; }
//      public long TicketId { get; set; }
 
//      public long UserId { get; set; }
// }