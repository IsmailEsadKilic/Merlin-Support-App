// public class TicketNodeDto
// {
//     public long Id { get; set; }
//     public string RowGuid { get; set; }
//     public string Node { get; set; }
//     public long RecordUserId { get; set; }
//     public string RecordUserName { get; set; }
//     public long TicketId { get; set; }
//     public DateTime RecordDate { get; set; }
//     public bool IsDeleted { get; set; }
// }

export interface TicketNode {
    id: number;
    rowGuid: string;
    node: string;
    recordUserId: number;
    recordUserName: string;
    ticketId: number;
    recordDate: Date;
    isDeleted: boolean;
}

export interface TicketNodeAdd {
    node: string;
    ticketId: number;
}