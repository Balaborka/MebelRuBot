using MebelTelegramBot.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace MebelTelegramBot.Managers {
    public class MebelRuBotContext {
        public static List<Employee> Employees { get; private set; }

        //private static AppContext instance;

        //private AppContext() {  }

        //public static AppContext getInstance() {
        //    if (instance == null)
        //        instance = new AppContext();
        //    return instance;
        //}

        public static void Init() {
            Employees = new List<Employee>();
        }
    }
}
