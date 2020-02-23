using MebelTelegramBot.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MebelTelegramBot.Models {
    public class Manager : Employee {
        public ManagerState State { get; set; }
    }
}
