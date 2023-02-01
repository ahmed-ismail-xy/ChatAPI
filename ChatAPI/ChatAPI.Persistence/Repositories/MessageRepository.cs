using AutoMapper;
using Azure.Core;
using ChatAPI.Application.Contracts;
using ChatAPI.Application.Featuers.ResponseHandler;
using ChatAPI.Application.FilesHandler;
using ChatAPI.Application.RepositoryDTOs.MessageReposiory.SendMessage;
using ChatAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;

namespace ChatAPI.Persistence.Repositories
{
    public class MessageRepository : IMessageReposiory
    {
        protected readonly ChatDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<MessageRepository> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MessageRepository(ChatDbContext dbContext, IMapper mapper, ILogger<MessageRepository> logger, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<List<SendMessageDto.Response>> GetAllMessagesByChatIdAsync(Guid chatId)
        {
            throw new NotImplementedException();
        }

        public async Task<APIResponse<SendMessageDto.Response>> SendMessageAsync(SendMessageDto.Request request)
        {
            var response = new APIResponse<SendMessageDto.Response>();

            try
            {
                Message message = _mapper.Map<Message>(request);

                message.MessageTime = DateTime.Now;

                if (request.FileLists != null)
                {
                    List<FileList> files = await ProcessFiles(request);
                    message.FileLists = files;
                }

                await _dbContext.Messages.AddAsync(message);
                await _dbContext.SaveChangesAsync();

                var result = _mapper.Map<SendMessageDto.Response>(message);
                response.Data = result;
                return response;
            }
            catch (Exception ex)
            {
                var requestId = _httpContextAccessor.HttpContext.Response.Headers["Request-Id"];
                LogError(ex, requestId, request);
                response.Message = "Unexpected Error Occurred.";
                response.AddError(10);
                return response;
            }
           
        }
        private async Task<List<FileList>> ProcessFiles(SendMessageDto.Request request)
        {
            var files = new List<FileList>();
            foreach (var fileDto in request.FileLists)
            {
                try
                {
                    var file = new FileList
                    {
                        FilePath = await FileHandler.HandelChatsFiles(fileDto.File, request.ChatId),
                        FileSize = fileDto.FileSize,
                        FileName = fileDto.FileName,
                        IsRecord = fileDto.IsRecord
                    };
                    files.Add(file);
                }
                catch (Exception ex)
                {
                    var requestId = _httpContextAccessor.HttpContext.Response.Headers["Request-Id"];
                    LogError(ex, requestId, fileDto);
                }
            }
            return files;
        }

        private void LogError(Exception ex, string requestId, object data)
        {
            _logger.LogError($"Request ID: {requestId} : Error Message: {ex.Message}");
            _logger.LogInformation($"Request ID: {requestId} : Request Data: {data.ToString}");
        }
    }
}

