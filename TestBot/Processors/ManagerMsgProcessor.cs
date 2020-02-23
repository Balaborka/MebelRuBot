using MebelTelegramBot.Enums;
using MebelTelegramBot.Managers;
using MebelTelegramBot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace MebelTelegramBot.Users {
    public class ManagerMsgProcessor : IMsgProcessor {

        public Employee Employee { get; set; }

        ITelegramBotClient botClient;
        EmployeeManager employeManager;
        string textConfirm = "Confirm";
        string textReturn = "Return";
        ManagerState managerState = ManagerState.Start;
        Summary summary;

        public ManagerMsgProcessor(ITelegramBotClient botClient, Employee employee) {
            this.botClient = botClient;
            Employee = employee;
            employeManager = new EmployeeManager();
        }

        public async Task ProcessMessage(string text) {
            if (managerState == ManagerState.Start) {
                string[] arrayWords = text.Split(',');
                List<int> arrayNumbers = new List<int>();
                if (arrayWords.Length == 4) {
                    foreach (var num in arrayWords) {
                        int res;
                        bool isNum = Int32.TryParse(num, out res);
                        if (!isNum) {
                            await RequestSummarytMessage();
                            return;
                        }
                        arrayNumbers.Add(res);
                    }

                    summary = new Summary(Employee.Id, arrayNumbers);
                    var validateMessage = summary.Validate();

                    managerState = ManagerState.Confirm;
                    await ConfirmSendResultsMessage(validateMessage);
                }
                else await RequestSummarytMessage();
            }
            else if (managerState == ManagerState.Confirm) {
                if (text == textConfirm) {
                    await ConfirmSendResultsMessage(summary);
                }
                else if (text == textReturn) {
                    managerState = ManagerState.Start;
                    await RequestSummarytMessage();
                }
            }
        }

        async Task RequestSummarytMessage() {
            await botClient.SendTextMessageAsync(
                      chatId: Employee.Id,
                      text: "Введите сводку из четырех чисел: " + 
                            Environment.NewLine +
                            Environment.NewLine +
                            "Лиды," + 
                            Environment.NewLine + 
                            "КЭВ Назначенных," + 
                            Environment.NewLine + 
                            "КЭВ Проведенных," +
                            Environment.NewLine +
                            "Сделок." + 
                            Environment.NewLine + 
                            Environment.NewLine + 
                            "Вводите значения через запятую в соответствующем порядке.",
                      replyMarkup: null
                ).ConfigureAwait(false);
        }

        async Task ConfirmSendResultsMessage(string text) {
            managerState = ManagerState.Confirm;

            var markupConfirmReturn = new ReplyKeyboardMarkup(new List<KeyboardButton>() {
                new KeyboardButton() { Text = textConfirm },
                new KeyboardButton() { Text = textReturn } }, true
            );

            if (text == null) {
                await botClient.SendTextMessageAsync(
                      chatId: Employee.Id,
                      text: "Отправить результаты?",
                      replyMarkup: markupConfirmReturn
                ).ConfigureAwait(false);
            }
            else {
                await botClient.SendTextMessageAsync(
                      chatId: Employee.Id,
                      text: "Внимание!" + 
                            Environment.NewLine +
                            Environment.NewLine + 
                            text +
                            Environment.NewLine +
                            "Отправить эти результаты?",
                      replyMarkup: markupConfirmReturn
                ).ConfigureAwait(false);
            }
        }

        async Task ConfirmSendResultsMessage(Summary sum) {
            //new EmployeeManager().SendSummary(sum);

            managerState = ManagerState.Start;

            await botClient.SendTextMessageAsync(
                      chatId: Employee.Id,
                      text: $"Сводка от {DateTime.Now} успешно отправлены",
                      replyMarkup: null
                ).ConfigureAwait(false);
        }
    }
}
