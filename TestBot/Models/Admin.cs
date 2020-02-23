using System;
using System.Collections.Generic;
using System.Text;

namespace MebelTelegramBot.Models {
    public class Admin : Employee {
        public AdminState State { get; set; } = AdminState.Start;
    }
}
