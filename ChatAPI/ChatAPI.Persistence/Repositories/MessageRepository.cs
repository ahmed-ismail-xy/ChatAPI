using AutoMapper;
using Azure.Core;
using ChatAPI.Application.Contracts;
using ChatAPI.Application.FilesHandler;
using ChatAPI.Application.RepositoryDTOs.MessageReposiory.SendMessage;
using ChatAPI.Domain.Entities;
using System.Collections.Generic;

namespace ChatAPI.Persistence.Repositories
{
    public class MessageRepository : IMessageReposiory
    {
        protected readonly ChatDbContext _dbContext;
        private readonly IMapper _mapper;

        public MessageRepository(ChatDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Task<List<SendMessageDto.Response>> GetAllMessagesByChatIdAsync(Guid chatId)
        {
            throw new NotImplementedException();
        }

        public async Task<SendMessageDto.Response> SendMessageAsync(SendMessageDto.Request request)
        {
            List<FileList> files = request.FileLists.Select(async file => new FileList
            {
                FilePath = await FileHandler.HandelChatsFiles(file.File, request.ChatId),
                FileSize = file.FileSize,
                FileName = file.FileName,
                IsRecord = file.IsRecord
            }).Select(t => t.Result).ToList();

            Message message = _mapper.Map<Message>(request);
            message.FileLists = files;
            _dbContext.Messages.Add(message);

            var result = _mapper.Map<SendMessageDto.Response>(message);
            return result;

        }
 
    }
}
