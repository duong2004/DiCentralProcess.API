using System;
using System.Collections.Generic;

#nullable disable

namespace DiCentralProcess.API.Models
{
    public partial class UserClient
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Psid { get; set; }
    }
}
