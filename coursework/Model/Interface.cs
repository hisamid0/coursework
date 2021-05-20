using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Model
{
    public class Interface
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MacAddress { get; set; }
        public List<Interface> Interfaces { get; set; }
        public Interface (string name, string macaddress)
        {
            Name = name;
            MacAddress = macaddress;
        }
        public Interface()
        {

        }

    }
}
