using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace MebelTelegramBot.Users {
    public class Anonymous : IUser {

        ITelegramBotClient botClient;
        EmployeeManager employeManager;

        public long Id { get; set; }

        public Anonymous(ITelegramBotClient botClient, long id) {
            this.botClient = botClient;
            this.Id = id;
            employeManager = new EmployeeManager();
        }

        async Task RequestNameMessage() {
            await botClient.SendTextMessageAsync(
                      chatId: Id,
                      text: "Введите ФИО" + Environment.NewLine + "Если авторизация не удалась - свяжитесь с администратором."
                ).ConfigureAwait(false);
        }

        async Task ApproveNameMessage() {
            await botClient.SendTextMessageAsync(
                      chatId: Id,
                      text: "Вы авторизованы!"
                ).ConfigureAwait(false);
        }

        public async Task ProcessMessage(string text) {
            var sucsess = employeManager.Bind(Id, text);    
            if (sucsess) {
                await ApproveNameMessage();
            }
            else await RequestNameMessage();
        }
    }
}
