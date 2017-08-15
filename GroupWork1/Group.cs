using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projektarbete
{
    class Group
    {
        public string name { get; set; }
        public List<Member> members { get; set; }
        public Member groupleader { get; set; }

        public Group(string name, List<Member> members, Member groupLeader)
        {
            this.name = name;
            this.members = members;
            this.groupleader = groupleader;
        }
    }
}
