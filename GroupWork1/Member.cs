using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projektarbete
{
    class Member
    {
        public string name { get; set; }
        public Gender gender { get; set; }

        public Member(string name)
        {
            this.name = name;
            this.gender = Gender.Unknown;
        }

        public Member(string name, Gender gender)
        {
            this.name = name;
            this.gender = gender;
        }
    }
}
