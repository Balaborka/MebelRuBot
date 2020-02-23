using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MebelTelegramBot.Users {
    public interface IMsgProcessor {
        Task ProcessMessage(string text);
    }
}
