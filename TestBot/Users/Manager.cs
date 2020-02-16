using MebelTelegramBot.Enums;
using MebelTelegramBot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace MebelTelegramBot.Users {
    public class Manager : IUser {

        public long Id { get; set; }

        ITelegramBotClient botClient;
        EmployeeManager employeManager;
        string textConfirm = "Confirm";
        string textReturn = "Return";
        ManagerState managerState = ManagerState.Start;
        Summary summary;

        public Manager(ITelegramBotClient botClient, long id) {
            this.botClient = botClient;
            this.Id = id;
            employeManager = new EmployeeManager();
        }

        public async Task ProcessMessage(string text) {
            //int countNumbers = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
            //if (countNumbers == 4)

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

                    summary = new Summary(Id, arrayNumbers);
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
                      chatId: Id,
                      text: "Введите сводку из четырех чисел: " + Environment.NewLine + "Лиды, КЭВ Назначенных, КЭВ Проведенных, Сделок." + Environment.NewLine + "Вводите значения через запятую в соответствующем порядке."
                ).ConfigureAwait(false);
        }

        async Task ConfirmSendResultsMessage(string text) {
            var markupConfirmReturn = new ReplyKeyboardMarkup(new List<KeyboardButton>() {
                new KeyboardButton() { Text = textConfirm },
                new KeyboardButton() { Text = textReturn } }, true
            );

            managerState = ManagerState.Start;

            if (text == null) {
                await botClient.SendTextMessageAsync(
                      chatId: Id,
                      text: "Отправить результаты?",
                      replyMarkup: markupConfirmReturn
                ).ConfigureAwait(false);
            }
            else {
                await botClient.SendTextMessageAsync(
                      chatId: Id,
                      text: "Внимание!" + Environment.NewLine + text + "Отправить эти результаты?",
                      replyMarkup: markupConfirmReturn
                ).ConfigureAwait(false);
            }
        }

        async Task ConfirmSendResultsMessage(Summary sum) {
            new EmployeeManager().SendSummary(sum);
            await botClient.SendTextMessageAsync(
                      chatId: Id,
                      text: $"Сводка от {DateTime.Now} успешно отправлены"
                ).ConfigureAwait(false);
        }
    }
}
