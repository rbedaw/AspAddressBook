using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AddressBook
{
    public partial class Contact
    {
        public string ContactFirst { get; set; }
        public string ContactLast { get; set; }
        public string ContactNo { get; set; }
        public string PhoneType { get; set; }

        public string PhoneTypeName { get; set; }
    }
}