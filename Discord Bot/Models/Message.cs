using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot.Models
{
	public class Message
	{
		public int Id { get; set; }
		public User User { get; set; }
        public string Content { get; set; }
        public bool Files { get; set; }
    }
}
