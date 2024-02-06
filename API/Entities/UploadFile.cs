using System.ComponentModel.DataAnnotations;

public class UploadRequest
{
    // public IFormFile FormFile { get; set; }
    public string FileName { get; set; }
    public IFormFile FormFile { get; set; }
    public string ForTicketId { get; set; }
    public string ForTicketNodeId { get; set; }
}

public class UploadFile
 {
     
     public long Id { get; set; }

     [StringLength(500)]
     public string FileName { get; set; }

     [StringLength(500)]
     public string OrginalFileName { get; set; }

     public string UploadFilePath { get; set; }

     public long? TicketId { get; set; }

     public decimal? FileSize { get; set; }

     public DateTime? RecordDatetime { get; set; }

     public long? UserId { get; set; }

     public long? TicketNodeId { get; set; }

     public bool IsMessageFile { get; set; }     

 }
