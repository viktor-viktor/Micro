using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GW_Api
{
    public class Role
    {
        private Role(string value) { Value = value; }
        public string Value { get; set; }

        public static Role User { get { return new Role("user"); } }
        public static Role Dev { get { return new Role("dev"); } }
        public static Role Admin { get { return new Role("admin"); } }
    }
}
