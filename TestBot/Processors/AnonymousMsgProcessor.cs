using MebelTelegramBot.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace MebelTelegramBot.Users {
    public class AnonymousMsgProcessor : IMsgProcessor {

        ITelegramBotClient botClient;
        EmployeeManager employeManager;

        public Employee Employee { get; set; }

        public AnonymousMsgProcessor(ITelegramBotClient botClient, Employee employee) {
            this.botClient = botClient;
            Employee = employee;
            employeManager = new EmployeeManager();
        }

        public async Task ProcessMessage(string text) {
            var sucsess = employeManager.Bind(Employee.Id, text);
            if (sucsess) {
                await ApproveNameMessage();
            }
            else await RequestNameMessage();
        }

        async Task RequestNameMessage() {
            await botClient.SendTextMessageAsync(
                      chatId: Employee.Id,
                      text: "Введите ФИО" + Environment.NewLine + "Если авторизация не удалась - свяжитесь с администратором."
                ).ConfigureAwait(false);
        }

        async Task ApproveNameMessage() {
            await botClient.SendTextMessageAsync(
                      chatId: Employee.Id,
                      text: "Вы авторизованы!"
                ).ConfigureAwait(false);
        }
    }
}
