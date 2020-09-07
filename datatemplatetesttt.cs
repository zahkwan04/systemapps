using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace systemapps
{
    public class datatemplatetesttt
    {
        public string Firstname { get; set; }
        public string lastname { get; set; }

        public int Age { get; set; }

        public Gender Gender { get; set; }

        public override string ToString()
        {
            return String.Format("{0} {1}", Firstname, lastname);
        }

    }

    public enum Gender
    {
        Male,
        Female
    }
}
