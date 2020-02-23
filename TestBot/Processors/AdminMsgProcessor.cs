using MebelTelegramBot.Managers;
using MebelTelegramBot.Models;
using MebelTelegramBot.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace MebelTelegramBot {
    public class AdminMsgProcessor : IMsgProcessor {
        public Admin Employee { get; set; }

        ITelegramBotClient botClient;
        string textReturn = "Return";
        string textGet_Employees = "Get Employees";
        string textAdd_User = "Add User";
        string textRemove_User = "Remove User";
        string textResults = "Results";
        string textConfirm = "Confirm";

        public AdminMsgProcessor(ITelegramBotClient botClient, Employee employee) {
            this.botClient = botClient;
            Employee = (Admin)employee;
        }

        public async Task ProcessMessage(string text) {
            if (text == textReturn) {
                await AdminStart();
            }
            else if (text == textGet_Employees) {
                await GetEmployeesCommad();
            }
            else if (Employee.State == AdminState.Start) {
                if (text == textAdd_User) {
                    await AddEmployeeMessage();
                }
                else if (text == textRemove_User) {
                    await RemoveEmployeeMessage();
                }
                else if (text == textResults) {
                    await ShowResults();
                    await AdminStart();
                }
                else await AdminStart();
            }
            else if (Employee.State == AdminState.Add) {
                await ValidateName(text);
            }
            else if (Employee.State == AdminState.ConfirmAdd) {
                await AddEmployeeCommand(text);
            }
            else if (Employee.State == AdminState.Remove) {
                await ValidateName(text);
            }
            else if (Employee.State == AdminState.ConfirmRemove) {
                await RemoveEmployeeCommand(text);
            }
        }
        async Task GetEmployeesCommad() {
            List<Employee> list = new EmployeeManager().Get();
            var employees = list.Select(i => i.Name).Aggregate((i, j) => i + Environment.NewLine + j);
            if (!string.IsNullOrEmpty(employees)) {
                await botClient.SendTextMessageAsync(
                      chatId: Employee.Id,
                      text: employees
                ).ConfigureAwait(false);
            }
            else {
                await botClient.SendTextMessageAsync(
                      chatId: Employee.Id,
                      text: "Сотрудников нет"
                ).ConfigureAwait(false);
            }
            await AdminStart();
        }

        async Task RemoveEmployeeCommand(string text) {
            if (text == textConfirm) {
                Employee.State = AdminState.Start;
                await botClient.SendTextMessageAsync(
                      chatId: Employee.Id,
                      text: "Вы удалили сотрудника"
                ).ConfigureAwait(false);
                await AdminStart();
            }
            else await RemoveEmployeeMessage();
        }

        async Task AddEmployeeCommand(string text) {
            if (text == textConfirm) {
                Employee.State = AdminState.Start;
                await botClient.SendTextMessageAsync(
                      chatId: Employee.Id,
                      text: "Вы добавили нового сотрудника"
                ).ConfigureAwait(false);
                await AdminStart();
            }
            else await AddEmployeeMessage();
        }

        async Task ValidateName(string text) {
            int countWords = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
            if (Employee.State == AdminState.Add) {
                if (countWords == 3) {
                    new EmployeeManager().Add(new Employee() { Name = text });
                    await ConfirmAddEmployeeMessage(text);
                }
                else await AddEmployeeMessage();
            }
            else if (Employee.State == AdminState.Remove) {
                if (countWords == 3) {
                    new EmployeeManager().Remove(text);
                    await ConfirmRemoveEmployeeMessage(text);
                }
                else await RemoveEmployeeMessage();
            }
        }

        async Task ConfirmRemoveEmployeeMessage(string text) {
            Employee.State = AdminState.ConfirmRemove;

            var markupConfirmReturn = new ReplyKeyboardMarkup(new List<KeyboardButton>() {
                new KeyboardButton() { Text = "Confirm" },
                new KeyboardButton() { Text = "Return" } }, true
            );
            await botClient.SendTextMessageAsync(
                  chatId: Employee.Id,
                  text: $"Удалить сотрудника '{text}'?",
                  replyMarkup: markupConfirmReturn
            ).ConfigureAwait(false);
        }

        async Task AdminStart() {
            Employee.State = AdminState.Start;

            var markupAdminStart = new ReplyKeyboardMarkup(new List<KeyboardButton>() {
                new KeyboardButton() { Text = textAdd_User,  },
                new KeyboardButton() { Text = textRemove_User },
                new KeyboardButton() { Text = textResults },
                new KeyboardButton() { Text = textGet_Employees },
                }, true
             );

            await botClient.SendTextMessageAsync(
                  chatId: Employee.Id,
                  text: "Выберете команду",
                  replyMarkup: markupAdminStart
            ).ConfigureAwait(false);
        }

        async Task AddEmployeeMessage() {
            Employee.State = AdminState.Add;

            var markupReturn = new ReplyKeyboardMarkup(new List<KeyboardButton>() {
                new KeyboardButton() { Text = textResults } }, true
             );

            await botClient.SendTextMessageAsync(
                  chatId: Employee.Id,
                  text: "Введите ФИО нового сотрудника",
                  replyMarkup: markupReturn
            ).ConfigureAwait(false);
        }
        async Task ConfirmAddEmployeeMessage(string text) {
            Employee.State = AdminState.ConfirmAdd;

            var markupConfirmReturn = new ReplyKeyboardMarkup(new List<KeyboardButton>() {
                new KeyboardButton() { Text = textConfirm },
                new KeyboardButton() { Text = textReturn } }, true
            );
            await botClient.SendTextMessageAsync(
                  chatId: Employee.Id,
                  text: $"Добавить сотрудника '{text}'?",
                  replyMarkup: markupConfirmReturn
            ).ConfigureAwait(false);
        }

        async Task RemoveEmployeeMessage() {
            Employee.State = AdminState.Remove;

            var markupReturn = new ReplyKeyboardMarkup(new List<KeyboardButton>() {
                new KeyboardButton() { Text = textReturn } }, true
             );

            await botClient.SendTextMessageAsync(
                  chatId: Employee.Id,
                  text: "Введите ФИО сотрудника для удаления",
                  replyMarkup: markupReturn
            ).ConfigureAwait(false);
        }

        async Task ShowResults() {
            Employee.State = AdminState.Start;
        }
    }
}
