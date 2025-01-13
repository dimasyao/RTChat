using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserConnection
    {
        public int Id { get; set; } 
        public string ConnectionId { get; set; }
        public string UserName { get; set; }
        public string ChatRoom { get; set; }
    }
}
