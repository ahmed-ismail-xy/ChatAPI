using AutoMapper;
using ChatAPI.Application.Contracts;
using ChatAPI.Application.Featuers.ResponseHandler;
using ChatAPI.Application.RepositoryDTOs.ChatRepository;
using ChatAPI.Application.RepositoryDTOs.ChatRepository.GetListAllChats;
using ChatAPI.Application.RepositoryDTOs.UserRepository;
using ChatAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatAPI.Persistence.Repositories
{
    public class ChatRepository : IChatRepository
    {
        protected readonly ChatDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ChatRepository> _logger;

        public ChatRepository(ChatDbContext dbContext, IMapper mapper, ILogger<ChatRepository> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<APIResponse<Guid>> CreateChatAsync(CreateChatDTO.Request request)
        {
            APIResponse<Guid> response = new APIResponse<Guid>();

            try
            {
                ChatMember existingChatMember = await GetExistingChatMemberAsync(request.SenderId, request.ReceiverId);

                if (existingChatMember != null)
                {
                    Chat existingChat = await GetChatByChatMemberIdAsync(existingChatMember.ChatMemberId);
                    response.Data = existingChat.ChatId;
                    return response;
                }

                ChatMember newChatMember = CreateChatMember(request.SenderId, request.ReceiverId);
                Chat newChat = await CreateChatAsync(newChatMember.ChatMemberId);

                await SendWelcomeMessage(newChat.ChatId, request.SenderId);

                response.Data = newChat.ChatId;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogInformation("Request INFO:", request.ToString());
                response.Message = "Unexpected Error Occurred.";
                response.AddError(10);
                return response;
            }
           
        }
        private async Task<ChatMember> GetExistingChatMemberAsync(Guid senderId, Guid receiverId)
        {
            return await _dbContext.ChatMembers
                .FirstOrDefaultAsync(cm => (cm.SenderId == senderId || cm.ReceiverId == senderId)
                && (cm.ReceiverId == receiverId || cm.SenderId == receiverId));
        }

        private async Task<Chat> GetChatByChatMemberIdAsync(Guid chatMemberId)
        {
            return await _dbContext.Chats
                .FirstOrDefaultAsync(c => c.ChatMemberId == chatMemberId);
        }

        private ChatMember CreateChatMember(Guid senderId, Guid receiverId)
        {
            ChatMember chatMember = new ChatMember
            {
                ReceiverId = receiverId,
                SenderId = senderId
            };
            _dbContext.ChatMembers.Add(chatMember);
            return chatMember;
        }

        private async Task<Chat> CreateChatAsync(Guid chatMemberId)
        {
            Chat chat = new Chat
            {
                ChatMemberId = chatMemberId
            };
            await _dbContext.Chats.AddAsync(chat);
            await _dbContext.SaveChangesAsync();

            return chat;
        }
        private async Task SendWelcomeMessage(Guid chatIs, Guid senderId)
        {
            Message message = new Message
            {
                ChatId = chatIs,
                SenderId = senderId,
                FileLists = null,
                IsStarred = false,
                MessageStateId = Guid.Parse("fc07c939-594c-4ca0-8bf8-ccdd81a3f33b"),
                MessageTypeId = Guid.Parse("4a1030fb-2903-4b59-9409-9aca22cbb82c"),
                MessageText = "Welcome",
                MessageTime = DateTime.Now,
                MessageTimeString = DateTime.Now.ToString()
            };
            await _dbContext.Messages.AddAsync(message);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<APIResponse<List<ChatDTO>>> GetListAllChatsAsync(Guid userId)
        {
            var result = new APIResponse<List<ChatDTO>>();
            try
            {
                var chatMembers = _dbContext.ChatMembers
                                   .AsNoTracking()
                                   .Where(cm => cm.SenderId == userId || cm.ReceiverId == userId)
                                   .ToList();

                if (chatMembers == null || !chatMembers.Any())
                {
                    result.AddError(12);
                    result.Message = "No chat found for the user.";
                    return result;
                }

                var chatDTOs = new List<ChatDTO>();
                foreach (var chatMember in chatMembers)
                {
                    var receiverId = chatMember.SenderId == userId ? chatMember.ReceiverId : chatMember.SenderId;
                    var receiver = _dbContext.Users
                                    .FirstOrDefault(u => u.UserId == receiverId);

                    if (receiver == null)
                    {
                        result.Errors.Add(13);
                        result.Message = "No receiver found for the chat member.";
                        continue;
                    }

                    var chat = _dbContext.Chats
                                .FirstOrDefault(c => c.ChatMemberId == chatMember.ChatMemberId);
                    if (chat == null)
                    {
                        result.Errors.Add(14);
                        result.Message = "No chat found for the chat member.";
                        continue;
                    }

                    var lastMessage = _dbContext.Messages
                                       .Where(m => m.ChatId == chat.ChatId)
                                       .OrderByDescending(m => m.MessageTime)
                                       .FirstOrDefault();

                    var chatDTO = new ChatDTO
                    {
                        UserId = receiverId,
                        ChatId = chat.ChatId,
                        ChatImage = receiver.UserImage,
                        ChatName = $"{receiver.FirstName} {receiver.LastName}",
                        FireToken = receiver.FireToken,
                        PhoneNumber = receiver.PhoneNumber,
                        LastMessageDto = new LastMessageDto
                        {
                            MessageId = lastMessage.MessageId,
                            MessageStateId = lastMessage.MessageStateId,
                            MessageText = lastMessage.MessageText,
                            MessageTime = lastMessage.MessageTime,
                            MessageTimeString = lastMessage.MessageTimeString,
                            MessageTypeId = lastMessage.MessageTypeId,
                            SenderId = lastMessage.SenderId,
                        }
                    };
                    chatDTOs.Add(chatDTO);
                }

                result.Data = chatDTOs;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                result.Errors.Add(15);
                result.Message = "An error occurred while retrieving the chat list: " + ex.Message;
                return result;
            }

        }

    }
}
