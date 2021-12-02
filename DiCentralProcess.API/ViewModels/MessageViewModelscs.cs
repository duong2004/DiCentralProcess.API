using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DiCentralProcess.API.ViewModels
{
    public class MessageViewModelscs
    {
        public Guid Id { get; set; }
        [Required]
        public string UserIdTo { get; set; }
        [Required]
        public string UserIdFrom { get; set; }
        public string Message1 { get; set; }
        public DateTime? Createdon { get; set; }
    }
    public class MessageCount
    {
        public string UserMessage { get; set; }
    }
}
