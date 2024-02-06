using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UploadFileRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UploadFileRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<long> AddUploadFileAsync(UploadFile uploadFile)
        {
            _context.UploadFiles.Add(uploadFile);
            await _context.SaveChangesAsync();
            return uploadFile.Id;
        }

        public async Task<UploadFileDto> GetUploadFileDtoByIdAsync(long id)
        {
            return await _context.UploadFiles
            .ProjectTo<UploadFileDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<UploadFileDto>> GetFileDtosByTicketIdAsync(long ticketId)
        {
            return await _context.UploadFiles
            .Where(x => x.TicketId == ticketId)
            .Where(x => x.TicketNodeId == null || x.TicketNodeId == 0)
            .ProjectTo<UploadFileDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        }

        public async Task<List<UploadFileDto>> GetFileDtosByTicketNodeIdAsync(long ticketNodeId)
        {
            return await _context.UploadFiles.Where(x => x.TicketNodeId == ticketNodeId)
            .ProjectTo<UploadFileDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        }

        public async Task<bool> DeleteTicketNodeFilesAsync(long ticketNodeId)
        {
            var files = await _context.UploadFiles.Where(x => x.TicketNodeId == ticketNodeId).ToListAsync();
            if (files != null)
            {
                //delete the file from, resources/Uploads
                foreach (var file in files)
                {
                    Console.WriteLine("delete file");
                    Console.WriteLine(file.UploadFilePath + '\\' + file.FileName);
                    if (File.Exists(file.UploadFilePath + '\\' + file.FileName))
                    {
                        Console.WriteLine("file exists");
                        File.Delete(file.UploadFilePath + '\\' + file.FileName);
                    }
                }

                foreach (var file in files)
                {
                    var fileToDelete = await _context.UploadFiles.FirstOrDefaultAsync(x => x.Id == file.Id);
                    _context.UploadFiles.Remove(fileToDelete);
                    //why???????????????????????????????????????????????????
                }

                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<bool> DeleteTicketFilesAsync(long ticketId)
        {
            var files = await _context.UploadFiles.Where(x => x.TicketId == ticketId).ToListAsync();
            if (files != null)
            {
                //delete the file from, resources/Uploads
                foreach (var file in files)
                {
                    Console.WriteLine("delete file");
                    Console.WriteLine(file.UploadFilePath + '\\' + file.FileName);
                    if (File.Exists(file.UploadFilePath + '\\' + file.FileName))
                    {
                        Console.WriteLine("file exists");
                        File.Delete(file.UploadFilePath + '\\' + file.FileName);
                    }
                }

                foreach (var file in files)
                {
                    var fileToDelete = await _context.UploadFiles.FirstOrDefaultAsync(x => x.Id == file.Id);
                    _context.UploadFiles.Remove(fileToDelete);
                    //why??
                }

                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<bool> DeleteUploadFileAsync(long id)
        {
            var file = await _context.UploadFiles.FirstOrDefaultAsync(x => x.Id == id);
            if (file != null)
            {
                if (File.Exists(file.UploadFilePath + '\\' + file.FileName))
                {
                    File.Delete(file.UploadFilePath + '\\' + file.FileName);
                }
                _context.UploadFiles.Remove(file);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
        
    }
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
