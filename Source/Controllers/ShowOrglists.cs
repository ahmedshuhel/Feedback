using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComplaintBox.Web.Controllers
{
    public class ShowOrglists
    {
        public string Organization { get; set; }
        public IList<Orglists> Orglists { get; set; }
    }
    public class Orglists
    {
        public string Orgname { get; set; }
        public string phoneNumber { get; set; }
        
    }
}
