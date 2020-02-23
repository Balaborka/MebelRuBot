using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace MebelTelegramBot {
    [XmlType("employee")]
    public class Employee {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("id")]
        public long Id { get; set; }
    }
}
