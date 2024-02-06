using System.Security.Claims;
using System.Text.Json;
using API.Data;
using API.Migrations;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// public class UploadRequest
// {
//     public IFormFile FormFile { get; set; }
    // public string ForTicketId { get; set; }
    // public string ForTicketNodeId { get; set; }
// }

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


namespace API.Controllers
{
    [AllowAnonymous]
    public class FileController : BaseApiController
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FileController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile()
        {
            var request = new UploadRequest();
            try
            {
                request.FormFile = Request.Form.Files[0];
                request.FileName = Request.Form.Files[0].FileName;
                request.ForTicketId = Request.Form["forTicketId"];
                request.ForTicketNodeId = Request.Form["forTicketNodeId"];
            }
            catch (Exception)
            {
                throw;
            }
            
            Console.WriteLine("Form data received:");
            Console.WriteLine(JsonSerializer.Serialize(request));

            long forTicketNodeId;
            long forTicketId;
            string fileName = request.FileName;
            try
            {
                forTicketId = long.Parse(request.ForTicketId);
                forTicketNodeId = long.Parse(request.ForTicketNodeId);
            }
            catch (Exception)
            {
                throw;
            }

            if (!(forTicketId > 0 || forTicketNodeId > 0))
            {
                return BadRequest("TicketId or TicketNodeId is required");
            }

            if (forTicketNodeId > 0 && !(forTicketId > 0))
            {
                return BadRequest("TicketId is required");
            }

            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId == 0)
            {
                return BadRequest("User Id is required");
            }

            var UploadFilePath = Path.Combine(Directory.GetCurrentDirectory(), "resources", "uploads");

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;

            // try to write the file into the upload folder

            var filePath = Path.Combine(UploadFilePath, uniqueFileName);

            var uploadFile = new UploadFile
            {
                FileName = uniqueFileName,
                OrginalFileName = fileName,
                UploadFilePath = UploadFilePath,
                RecordDatetime = DateTime.Now,
                TicketId = forTicketId,
                FileSize = request.FormFile.Length,
                TicketNodeId = forTicketNodeId,
                //get current user id
                UserId = userId,
                // if ticketNodeId is not null, then it is a message file
                IsMessageFile = forTicketNodeId > 0
            };

            var id = await _unitOfWork.UploadFileRepository.AddUploadFileAsync(uploadFile);

            if (id > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    try
                    { 
                        await request.FormFile.CopyToAsync(stream);
                    }
                    catch (System.Exception exception)
                    {
                        return BadRequest("Failed to upload file" + fileName + " to " + filePath + " (" + exception.Message + ")");
                    }
                }

                return Ok(JsonSerializer.Serialize(uploadFile));
            }
            else
            {
                return BadRequest("Failed to upload file");
            }
        }
        
        [HttpGet("ticket/{ticketId}")]
        public async Task<IActionResult> GetFilesByTicketId(long ticketId)
        {
            var files = await _unitOfWork.UploadFileRepository.GetFileDtosByTicketIdAsync(ticketId);

            if (files == null)
            {
                return Ok(false);
            }

            return Ok(files);
        }

        [HttpGet("ticketnode/{ticketNodeId}")]
        public async Task<IActionResult> GetFilesByTicketNodeId(long ticketNodeId)
        {
            var files = await _unitOfWork.UploadFileRepository.GetFileDtosByTicketNodeIdAsync(ticketNodeId);

            if (files == null)
            {
                return Ok(false);
            }

            return Ok(files);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(long id)
        {
            var result = await _unitOfWork.UploadFileRepository.DeleteUploadFileAsync(id);

            if (result)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest("Failed to delete file");
            }

        }

//           downloadFile(fileId: number) {
//     this.httpClient.get(this.baseUrl + '/download/' + fileId,
//       {observe: 'response', responseType: 'blob'}
//     ).subscribe(response => {
//       let fileName = response.headers.get('content-disposition')!.split(';')[1].split('=')[1] || 'file';
//       let blob: Blob = response.body as Blob;
//       let a = document.createElement('a');
//       a.download = fileName;
//       a.href = URL.createObjectURL(blob);
//       a.click();
//     });
//   }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadFile(long id)
        {
            var file = await _unitOfWork.UploadFileRepository.GetUploadFileDtoByIdAsync(id);

            if (file == null)
            {
                return Ok(false);
            }

            var filePath = Path.Combine(file.UploadFilePath, file.FileName);

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "application/octet-stream", file.OrginalFileName);
        }
    }
}
    //     const formData = new FormData();
    //     formData.append("file", file);
    //     formData.append("forTicketId", this.forTicketId.toString());
    //     formData.append("forTicketNodeId", this.forTicketNodeId.toString());
    //     const formDataUpload: FormDataUpload = formData as FormDataUpload;
    //     formDataUpload.fileName = file.name;
    //     formDataUpload.progress = 0;
    //     formDataUpload.formData = formData;
    //     this.formDataUploads.push(formDataUpload);
    //   });

    //   this.formDataUploads.forEach(formDataUpload => {

    //     const upload$ = this.http.post("/api/fileUpload", formDataUpload.formData, {
    //       reportProgress: true,
    //       observe: 'events'
    //     });

