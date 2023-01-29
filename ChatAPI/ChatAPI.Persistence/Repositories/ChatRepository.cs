using AutoMapper;
using ChatAPI.Application.Contracts;
using ChatAPI.Application.Featuers.ResponseHandler;
using ChatAPI.Application.RepositoryDTOs.ChatRepository;
using ChatAPI.Application.RepositoryDTOs.ChatRepository.GetListAllChats;
using ChatAPI.Application.RepositoryDTOs.UserRepository;
using ChatAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ChatAPI.Persistence.Repositories
{
    public class ChatRepository : IChatRepository
    {
        protected readonly ChatDbContext _dbContext;
        private readonly IMapper _mapper;

        public ChatRepository(ChatDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
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
                
                SendFirstMessage(request.SenderId, newChat.ChatId);
                
                response.Data = newChat.ChatId;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = "Unexpected Error Occurred.";
                response.AddError(10);
                return response;
            }
           
        }
        private void SendFirstMessage(Guid senderId, Guid chatId)
        {
            Message message = new Message()
            {
                MessageTime = DateTime.Now,
                MessageStateId = Guid.Parse("0c8e6b62-15e7-4fb3-b97b-f8c0442ddcd9"),
                ChatId = chatId,
                MessageTypeId = Guid.Parse("4a1030fb-2903-4b59-9409-9aca22cbb82c"),
                MessageText = "Start Chating Now.",
                SenderId= senderId,
                MessageTimeString= DateTime.Now.ToString(),
                IsStarred= false,
            };
             _dbContext.Messages.Add(message);
            _dbContext.SaveChanges();
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
                    result.Message = "No chat members found for the user.";
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
                        result.Message = "No receiver found for the chat member.";
                        continue;
                    }

                    var chat = _dbContext.Chats
                                .FirstOrDefault(c => c.ChatMemberId == chatMember.ChatMemberId);
                    if (chat == null)
                    {
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
                        ChatName = receiver.FirstName + receiver.LastName,
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
                result.Message = "An error occurred while retrieving the chat list: " + ex.Message;
                return result;
            }

        }

    }
}
