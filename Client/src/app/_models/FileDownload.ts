export interface FileDownload {
    id: number;
    fileName: string;
    orginalFileName: string;
    uploadFilePath: string;
    ticketId: number;
    fileSize: number;
    recordDatetime: Date;
    userId: number;
    ticketNodeId: number;
    isMessageFile: boolean;
    }



// public class UploadFile
//  {
     
//      public long Id { get; set; }

//      [StringLength(500)]
//      public string FileName { get; set; }

//      [StringLength(500)]
//      public string OrginalFileName { get; set; }

//      public string UploadFilePath { get; set; }

//      public long? TicketId { get; set; }

//      public decimal? FileSize { get; set; }

//      public DateTime? RecordDatetime { get; set; }

//      public long? UserId { get; set; }

//      public long? TicketNodeId { get; set; }

//      public bool IsMessageFile { get; set; }     

//  }
