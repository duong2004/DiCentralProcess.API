using AutoMapper;
using DiCentralProcess.API.Models;
using DiCentralProcess.API.ViewModels;

namespace DiCentralProcess.API.Helpers
{
    public class MessageMapper : Profile
    {
        public MessageMapper()
        {
            CreateMap<Message, MessageViewModelscs>();
            CreateMap<MessageViewModelscs, Message>();
        }
    }
}
