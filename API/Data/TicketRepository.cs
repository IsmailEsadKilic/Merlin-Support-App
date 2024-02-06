using System.Text.Json;
using API.Dtos;
using API.DTOs;
using API.Entities;
using API.Enums;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    // public class Ticket
    // {
    //     public int Id { get; set; }
    //     public string RowGuid { get; set; }
    //     public int CustomerTicketId { get; set; } // ticket counter
    //     public DateTime RecordDate { get; set; }
    //     public DateTime ClosedDate { get; set; }
    //     public DateTime FirstResponseTime { get; set; }
    //     public string Subject { get; set; }
    //     public string Description { get; set; }
    //     public Product Product { get; set; }
    //     public int ProductId { get; set; }
    //     public Team Team { get; set; }
    //     public int TeamId { get; set; }
    //     public Priority Priority { get; set; }
    //     public int PriorityId { get; set; }
    //     public User RecordUser { get; set; }
    //     public int RecordUserId { get; set; }
    //     public Customer Customer { get; set; }
    //     public int CustomerId { get; set; }
    //     public int TicketStateId { get; set; }
    // }
    // public class TicketDto
    //     public int Id { get; set; }
    //     public string RowGuid { get; set; }
    //     public int CustomerTicketId { get; set; }
    //     public DateTime RecordDate { get; set; }
    //     public DateTime ClosedDate { get; set; }
    //     public DateTime FirstResponseTime { get; set; }
    //     public string Subject { get; set; }
    //     public string Description { get; set; }
    //     public int ProductId { get; set; }
    //     public string ProductName { get; set; }
    //     public int TeamId { get; set; }
    //     public string TeamName { get; set; }
    //     public int PriorityId { get; set; }
    //     public string PriorityName { get; set; }
    //     public int RecordUserId { get; set; }
    //     public string RecordUserName { get; set; }
    //     public int CustomerId { get; set; }
    //     public string CustomerName { get; set; }
    //     public int TicketStateId { get; set; }
    //     public string TicketStateName { get; set; }
    // }
    public class TicketRepository
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IConfiguration _config;

        public TicketRepository(DataContext context, IMapper mapper, IConfiguration config)
        {
            _mapper = mapper;
            _context = context;
            _config = config;
        }

        //ticket

        public async Task<IEnumerable<TicketDto>> GetTicketDtosAsync()
        {
            // get names from related tables
            var v = await _context.Tickets
                .Include(t => t.Product)
                .Include(t => t.Team)
                .Include(t => t.Priority)
                .Include(t => t.User)
                .Include(t => t.Customer)
                .Include(t => t.TicketType)
                .ProjectTo<TicketDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            //get ticketstatenames
            var ticketStateHelper = new TicketStateHelper();
            var ticketStateNames = TicketStateHelper.GetTicketStatesDict();

            foreach (var item in v)
            {
                item.TicketStateName = ticketStateNames[item.TicketStateId.ToString()];
            }
            
            ticketStateHelper = null;

            //get teamMemberDtos

            foreach (var item in v)
            {
                var ticketOwnerDetails = await _context.TicketOwnerDetails
                    .Where(tod => tod.TicketId == item.Id)
                    .ToListAsync();
                
                var userIds = new List<long>();

                foreach (var tod in ticketOwnerDetails)
                {
                    if (tod.UserId != 0 && !userIds.Contains(tod.UserId))
                    {
                        userIds.Add(tod.UserId);
                    }
                }

                //from teamMembers, get ones that are in userIds

                var teamMemberDtos = await _context.TeamMembers
                    .Where(tm => userIds.Contains(tm.UserId))
                    .Where(tm => tm.TeamId == item.TeamId)
                    .ProjectTo<TeamMemberDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                //get names
                foreach (var teamMemberDto in teamMemberDtos)
                {
                    var user = await _context.Users.FindAsync(teamMemberDto.UserId);
                    teamMemberDto.Name = user.UserName;
                }

                item.TeamMemberDtos = teamMemberDtos;
            }

            return v;
        }

        public async Task<TicketDto> GetTicketDtoAsync(int id)
        {
            var v = await _context.Tickets
                .Where(t => t.Id == id)
                .Include(t => t.Product)
                .Include(t => t.Team)
                .Include(t => t.Priority)
                .Include(t => t.User)
                .Include(t => t.Customer)
                .Include(t => t.TicketType)
                .ProjectTo<TicketDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            //get ticketstatename
            var ticketStateHelper = new TicketStateHelper();
            var ticketStateNames = TicketStateHelper.GetTicketStatesDict();

            v.TicketStateName = ticketStateNames[v.TicketStateId.ToString()];

            ticketStateHelper = null;

            //get teamMemberDtos

            var ticketOwnerDetails = await _context.TicketOwnerDetails
                .Where(tod => tod.TicketId == id)
                .ToListAsync();
            
            var userIds = new List<long>();

            foreach (var tod in ticketOwnerDetails)
            {
                if (tod.UserId != 0 && !userIds.Contains(tod.UserId))
                {
                    userIds.Add(tod.UserId);
                }
            }

            //from teamMembers, get ones that are in userIds

            var teamMemberDtos = await _context.TeamMembers
                .Where(tm => userIds.Contains(tm.UserId))
                .Where(tm => tm.TeamId == v.TeamId)
                .ProjectTo<TeamMemberDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            //get names
            foreach (var teamMemberDto in teamMemberDtos)
            {
                var user = await _context.Users.FindAsync(teamMemberDto.UserId);
                teamMemberDto.Name = user.UserName;
            }
            

            v.TeamMemberDtos = teamMemberDtos;

            return v;

        }

        public async Task<int> AddTicketAsync(Ticket ticket, IList<TeamMemberDto> teamMemberDtos)
        {
            Console.WriteLine("UserId on the ticket: " + ticket.UserId);
            Console.WriteLine(JsonSerializer.Serialize(ticket));
            Console.WriteLine("teamMemberDtos: " + JsonSerializer.Serialize(teamMemberDtos));

            // other 2 dates are 1/1/1

            var customer = await _context.Customers
                .Include(c => c.Tickets)
                .Where(c => c.Id == ticket.CustomerId)
                .SingleOrDefaultAsync();

            if (customer == null) return 0;

            //shenanigans to get the ticket id
            {
                var inc = 1;
                var start = 0;
                var postfix = "";
                //get increment from config
                if (_config != null)
                {
                    var incStr = _config["TicketIdInc"];
                    var startStr = _config["TicketIdStart"];

                    // "TicketIdStart": "[year]0" or "TicketIdStart": "0"
                    // "TicketIdInc": "1"

                    if (startStr != null)
                    {
                        if (startStr.Contains("[year]"))
                        {
                            postfix = DateTime.Now.Year.ToString();
                            startStr = startStr.Replace("[year]", "");

                            if (startStr != null)
                            {
                                start = int.Parse(startStr);
                            }
                        }
                        else
                        {
                            start = int.Parse(startStr);
                        }
                    }

                    if (incStr != null)
                    {
                        inc = int.Parse(incStr);
                    }

                }
                start += inc;

                if (customer.Tickets.Count > 0)
                {
                    var max = customer.Tickets.Max(t => t.CustomerTicketId);
                    if (postfix != "")
                    {
                        string numberString = max.ToString();
                        
                        Console.WriteLine("numberString: " + numberString);

                        // Extract substring excluding the last 4 characters
                        string result = numberString.Substring(0, numberString.Length - 4);
                        
                        // Convert the substring back to an integer

                        int resultInteger = int.Parse(result) | 0;

                        resultInteger += inc;

                        result = resultInteger.ToString();

                        ticket.CustomerTicketId = int.Parse(result + postfix);
                    }

                    else
                    {
                        ticket.CustomerTicketId = max + inc;
                    }
                }
                else
                {
                    if (postfix != "")
                    {
                        string numberStr = start.ToString() + postfix;
                        ticket.CustomerTicketId = int.Parse(numberStr);
                    }
                    else
                    {
                        ticket.CustomerTicketId = start;
                    }
                }

            }

            ticket.RecordDate = DateTime.Now;

            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();

            foreach (var teamMemberDto in teamMemberDtos)
            {
                var ticketOwnerDetail = new TicketOwnerDetail
                {
                    TicketId = ticket.Id,
                    UserId = teamMemberDto.UserId
                };

                await _context.TicketOwnerDetails.AddAsync(ticketOwnerDetail);
            }

            await _context.SaveChangesAsync();

            return ticket.Id;
        }

        public async Task<bool> UpdateTicketAsync(int id, TicketDto ticketDto)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null) return false;

            ticket.Subject = ticketDto.Subject;
            ticket.PriorityId = ticketDto.PriorityId;
            ticket.TicketStateId = ticketDto.TicketStateId;
            ticket.TicketTypeId = ticketDto.TicketTypeId;

            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTicketAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null) {
                return false;
            }

            //delete ticketOwnerDetails
            var ticketOwnerDetails = await _context.TicketOwnerDetails
                .Where(tod => tod.TicketId == id)
                .ToListAsync();

            foreach (var tod in ticketOwnerDetails)
            {
                _context.TicketOwnerDetails.Remove(tod);
            }

            await _context.SaveChangesAsync();

            //delete ticketNodes

            var ticketNodes = await _context.TicketNodes
                .Where(tn => tn.TicketId == id)
                .ToListAsync();

            foreach (var tn in ticketNodes)

            {
                // _context.TicketNodes.Remove(tn);
                //instead of deleting, set the isDeleted flag to true
                tn.IsDeleted = true;
            }

            await _context.SaveChangesAsync();

            _context.Tickets.Remove(ticket);

            await _context.SaveChangesAsync();

            return true;
        }

        //////////////////////////////////////////////////////////tickeNode
        
        public async Task<IEnumerable<TicketNodeDto>> GetTicketNodeDtosByTicketIdAsync(int ticketId)
        {
            long ticketIdLong = ticketId;
            var v = await _context.TicketNodes
                .Where(tn => tn.TicketId == ticketIdLong)
                .Where(tn => tn.IsDeleted == false)
                .ProjectTo<TicketNodeDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            //get record user name

            foreach (var item in v)
            {
                var user = await _context.Users.FindAsync(item.RecordUserId);
                item.RecordUserName = user.UserName;
            }

            return v;
        }

        public async Task<TicketNodeDto> GetTicketNodeDtoAsync(long id)
        {
            var v = await _context.TicketNodes
                .Where(tn => tn.Id == id)
                .Where(tn => tn.IsDeleted == false)
                .ProjectTo<TicketNodeDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            //get record user name

            var user = await _context.Users.FindAsync(v.RecordUserId);
            v.RecordUserName = user.UserName;

            return v;
        }

        public async Task<long> AddTicketNodeAsync(TicketNode ticketNode)
        {
            await _context.TicketNodes.AddAsync(ticketNode);
            await _context.SaveChangesAsync();
            return ticketNode.Id;
        }

        public async Task<bool>DeleteTicketNodeAsync(long id)
        {
            var ticketNode = await _context.TicketNodes.FindAsync(id);

            if (ticketNode == null) {
                return false;
            }

            // _context.TicketNodes.Remove(ticketNode);

            //instead of deleting, set the isDeleted flag to true
            ticketNode.IsDeleted = true;

            await _context.SaveChangesAsync();

            return true;
        }

        /////////////////////////////////////////////////////////////ticketType

        public async Task<IEnumerable<TicketTypeDto>> GetTicketTypeDtosAsync()
        {
            return await _context.TicketTypes
                .ProjectTo<TicketTypeDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<TicketTypeDto> GetTicketTypeDtoAsync(int id)
        {
            return await _context.TicketTypes
                .Where(t => t.Id == id)
                .ProjectTo<TicketTypeDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<int> AddTicketTypeAsync(TicketType ticketType)
        {
            await _context.TicketTypes.AddAsync(ticketType);
            await _context.SaveChangesAsync();
            return ticketType.Id;
        }

        public async Task<bool> UpdateTicketTypeAsync(int id, TicketTypeDto ticketTypeDto)
        {
            var ticketType = await _context.TicketTypes.FindAsync(id);

            if (ticketType == null) return false;

            ticketType.Name = ticketTypeDto.Name;
            ticketType.ListIndex = ticketTypeDto.ListIndex;

            _context.TicketTypes.Update(ticketType);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTicketTypeAsync(int id)
        {
            var ticketType = await _context.TicketTypes.FindAsync(id);

            if (ticketType == null) {
                return false;
            }

            _context.TicketTypes.Remove(ticketType);
            await _context.SaveChangesAsync();

            return true;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////priority

        public async Task<IEnumerable<PriorityDto>> GetPriorityDtosAsync()
        {
            return await _context.Priorities
                .ProjectTo<PriorityDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<PriorityDto> GetPriorityDtoAsync(int id)
        {
            return await _context.Priorities
                .Where(p => p.Id == id)
                .ProjectTo<PriorityDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<int> AddPriorityAsync(Priority priority)
        {
            await _context.Priorities.AddAsync(priority);
            await _context.SaveChangesAsync();
            return priority.Id;
        }

        public async Task<bool> UpdatePriorityAsync(int id, PriorityDto priorityDto)
        {
            var priority = await _context.Priorities.FindAsync(id);

            if (priority == null) return false;

            priority.PriorityName = priorityDto.PriorityName;
            priority.TimeType = (enums.TimeType)priorityDto.TimeType;
            priority.Time = priorityDto.Time;
            priority.ListIndex = priorityDto.ListIndex;

            _context.Priorities.Update(priority);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeletePriorityAsync(int id)
        {
            var priority = await _context.Priorities.FindAsync(id);

            if (priority == null) {
                return false;
            }

            _context.Priorities.Remove(priority);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

    // public class TicketType
    // {
    //     public int Id { get; set; }
    //     public string RowGuid { get; set; }
    //     public string Name { get; set; }
    //     public int ListIndex { get; set; }

    // }

    //     public class Priority
    // {
    //     public int Id { get; set; }
    //     public string RowGuid { get; set; }
    //     public string PriorityName { get; set; }

    //     public TimeType TimeType { get; set; }
    //     public int Time { get; set; }
    //     public int ListIndex { get; set; }

    // }