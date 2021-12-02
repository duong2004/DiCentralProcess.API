using AutoMapper;
using DiCentralProcess.API.Hubs;
using DiCentralProcess.API.Models;
using DiCentralProcess.API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiCentralProcess.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly DiCentralProcessDataContext _db;
        private readonly IHubContext<NotificationHub> _notificationHubContext;

        public MessageController(IMapper mapper,
            DiCentralProcessDataContext db,
            IHubContext<NotificationHub> notificationHubContext)
        {
            _mapper = mapper;
            _db = db;
            _notificationHubContext = notificationHubContext;
        }

        [HttpPost]
        [Route("AddMessage")]
        public async Task<IActionResult> AddMessage(MessageViewModelscs model)
        {
            model.Id = Guid.NewGuid();
            model.Createdon = DateTime.Now;
            if (CheckUserIsPage(model.UserIdTo))
            {
                model.UserIdTo = UserToId(model.UserIdFrom);
            }
            else
            {
                if (!CheckNewMessage(model.UserIdTo))
                {
                    model.UserIdFrom = GetIdUserClient();
                    await _notificationHubContext.Clients.All.SendAsync("SendNotificationMessage",model.UserIdTo,model.UserIdFrom);
                }
                else
                {
                    model.UserIdFrom = UserToFrom(model.UserIdTo);
                    await _notificationHubContext.Clients.All.SendAsync("SendNotificationMessage", model.UserIdTo, model.UserIdFrom);
                }
            }
            var message = _mapper.Map<Message>(model);
            _db.Messages.Add(message);
            await _db.SaveChangesAsync();
            return Ok();
        }
        // Get message of qtvA voi kh
        [HttpGet]
        [Route("GetMessage")]
        public async Task<IActionResult> GetMessage(string a, string b)
        {
            List<Message> messages = await _db.Messages
                .Where(x => x.UserIdTo == a && x.UserIdFrom == b)
                .ToListAsync();
            List<Message> messageBA = await _db.Messages
                .Where(x => x.UserIdTo == b && x.UserIdFrom == a)
                .ToListAsync();
            messages.AddRange(messageBA);
            return Ok(messages.OrderBy(x => x.Createdon));
        }
        [HttpGet]
        [Route("GetMessageBy/{id}")]
        public async Task<IActionResult> GetMessageById(string id)
        {
            List<MessageCount> messages = await _db.Messages.OrderByDescending(x => x.Createdon).Where(x => x.UserIdFrom == id).Select(x => new MessageCount()
            {
                UserMessage = x.UserIdTo
            }).Distinct().ToListAsync();
            //List<Message> messages = await _db.Messages.Where(x => x.UserIdFrom == id).OrderByDescending(x => x.Createdon).Distinct().ToListAsync();
            return Ok(messages);
        }
        // Check user sended have a page.
        public bool CheckUserIsPage(string userToId)
        {
            return _db.UserClients.Any(x => x.Psid == userToId);
        }
        // Check new message
        public bool CheckNewMessage(string userToId)
        {
            var messageExits = _db.Messages.Any(x => x.UserIdTo == userToId);
            return messageExits;
        }
        // get id when new message
        public string GetIdUserClient()
        {
            List<UserClient> userClients = _db.UserClients.ToList();
            int countUser = userClients.Count();
            Random random = new Random();
            var index = random.Next(countUser);
            return userClients.ElementAtOrDefault(index).Id.ToString();
        }
        // user send message
        public string UserToFrom(string userToId)
        {
            return _db.Messages.FirstOrDefault(x => x.UserIdTo == userToId).UserIdFrom;
        }
        public string UserToId(string userFromId)
        {
            return _db.Messages.FirstOrDefault(x => x.UserIdTo == userFromId).UserIdFrom;
        }
    }
}
