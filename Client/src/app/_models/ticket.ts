import { TeamMember, TeamMemberAdd } from "./team";

export interface Ticket {
    id: number;
    rowGuid: string;
    customerTicketId: number;
    recordDate: string;
    closedDate: string;
    firstResponseTime: string;
    subject: string;
    description: string;
    productId: number;
    productName: string;
    teamId: number;
    teamName: string;

    teamMemberDtos: TeamMember[];

    priorityId: number;
    priorityName: string;
    
    recordUserName: string;
    recordUserId: number;

    customerId: number;
    customerName: string;

    ticketStateId: number;
    ticketStateName: string;

    ticketTypeId: number;
    ticketTypeName: string;
}

export interface TicketAdd {
    subject: string;
    description: string;
    productId: number;
    teamId: number;
    priorityId: number;
    customerId: number;
    ticketTypeId: number;
    ticketStateId: number;
    teamMemberDtos: TeamMemberAdd[];
}

    // public class Ticket
    // {
    //     public int Id { get; set; }
    //     public string RowGuid { get; set; }
    //     public int CustomerTicketId { get; set; }
    //     public DateTime RecordDate { get; set; }
    //     public DateTime ClosedDate { get; set; }
    //     public DateTime FirstResponseTime { get; set; }
    //     public string Subject { get; set; }
    //     public string Description { get; set; }
    //     public int ProductId { get; set; }
    //     public int TeamId { get; set; }
    //     public int PriorityId { get; set; }
    //     public int RecordUserId { get; set; }
    //     public int CustomerId { get; set; }
    //     public int TicketStateId { get; set; }
    // }

export interface TicketType {
    id: number;
    rowGuiid: string;
    name: string;
    listIndex: number;
}

export interface TicketTypeAdd {
    name: string;
    listIndex: number;
}