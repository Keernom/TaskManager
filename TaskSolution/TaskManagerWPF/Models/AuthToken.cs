using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerWPF.Models
{
    public class AuthToken
    {
        public string Access_Token { get; set; }
        public string Username { get; set; }
    }
}
