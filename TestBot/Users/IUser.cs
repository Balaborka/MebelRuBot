using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MebelTelegramBot.Users {
    public interface IUser {
        long Id { get; set; }
        Task ProcessMessage(string text);
    }
}
