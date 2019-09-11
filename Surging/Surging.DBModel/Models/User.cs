using System;
using System.Collections.Generic;

namespace Surging.DBModel.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
    }
}
