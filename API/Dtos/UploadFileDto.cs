public class UploadFileDto
 {
     
     public long Id { get; set; }

     public string FileName { get; set; }

     public string OrginalFileName { get; set; }

    public string UploadFilePath { get; set; }

     public long? TicketId { get; set; }

     public decimal? FileSize { get; set; }

     public DateTime? RecordDatetime { get; set; }

     public long? UserId { get; set; }

     public long? TicketNodeId { get; set; }

     public bool IsMessageFile { get; set; }     

 }
