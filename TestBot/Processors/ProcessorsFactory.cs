using MebelTelegramBot.Models;
using MebelTelegramBot.Users;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace MebelTelegramBot.Processors {
    public class ProcessorsFactory {
        public IMsgProcessor GetProcessor(ITelegramBotClient botClient, Employee employee) {
            if (employee is Admin) {
                return new AdminMsgProcessor(botClient, employee);
            }
            if (employee is Manager) {
                return new ManagerMsgProcessor(botClient, employee);
            }
            return new AnonymousMsgProcessor(botClient, employee);
        } 
    }
}
