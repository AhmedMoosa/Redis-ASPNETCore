using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisDemo.Data
{
    public class Patient
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
