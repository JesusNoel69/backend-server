using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_Server.DTOs
{
    public class UserData
    {
        public int IdUser { get; set; }
        public string? UserName { get; set; }
        public string? UserAccount { get; set; }
        public int? TeamId { get; set; }
        public string? InformationAditional { get; set; }
    }
}