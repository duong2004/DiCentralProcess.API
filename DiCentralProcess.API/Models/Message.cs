using System;
using System.Collections.Generic;

#nullable disable

namespace DiCentralProcess.API.Models
{
    public partial class Message
    {
        public Guid Id { get; set; }
        public string UserIdTo { get; set; }
        public string UserIdFrom { get; set; }
        public string Message1 { get; set; }
        public DateTime? Createdon { get; set; }
    }
}
