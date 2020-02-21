using MebelTelegramBot.Users;
using MihaZupan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MebelTelegramBot {
    class Program {
        private static ITelegramBotClient botClient;

        const long BORIS_ID = 193369564;

        static Dictionary<long, IUser> users = new Dictionary<long, IUser>();

        static EmployeeManager employeeManager;

        static void Main(string[] args) {
            employeeManager = new EmployeeManager();

            //var proxy = new HttpToSocks5Proxy("103.194.171.156", 8888);
            //proxy.ResolveHostnamesLocally = true;

            //botClient = new TelegramBotClient("755174490:AAGr0dyLPskL3UL3HyUeJbqXVXpijKj7hCI", proxy) {

            botClient = new TelegramBotClient("755174490:AAGr0dyLPskL3UL3HyUeJbqXVXpijKj7hCI") {
                Timeout = TimeSpan.FromSeconds(10) 
            };

            users.Add(BORIS_ID, new Admin(botClient, BORIS_ID));

            var me = botClient.GetMeAsync().Result;
            Console.WriteLine($"Bot Id: {me.Id}. Bot Name: {me.FirstName}");

            botClient.OnMessage += BotClient_OnMessage;

            botClient.StartReceiving();

            Console.ReadKey();
        }

        //const long TEST_BOT_ID = -352873331;
        //const int EUGENIY_ID = 425552425;

        //Диана - 788918728
        //Вован - 355373744
        //Я - 193369564
        //Женек - 425552425
        //Рукс - 262531255


        //pack://application:,,,/Fonts/#Digital-7 Mono

        private async static void BotClient_OnMessage(object sender, MessageEventArgs e) {
            UpdateUsers();

            var text = e?.Message?.Text;
            var id = e.Message.Chat.Id;
            //var id = e.Message.From.Id;
            //var messageID = e?.Message?.MessageId;

            if (text == null)
                return;

            if (users.ContainsKey(id)) {
                await users[id].ProcessMessage(text);
            }
            else {
                var anon = new Anonymous(botClient, id);
                await anon.ProcessMessage(text);
            }

            //if (e?.Message?.Chat.Id == 193369564) {
            //    await botClient.SendTextMessageAsync(
            //    chatId: 193369564,
            //    text: $"Борис только что написал: '{text}'"
            //    ).ConfigureAwait(false);

            //    await botClient.SendTextMessageAsync(
            //    chatId: e?.Message?.Chat.Id,
            //    text: $"You said '{text}'",
            //    replyMarkup: markupBoris
            //    ).ConfigureAwait(false);
            //}
        }

        private static void UpdateUsers() {
            var registeredUsers = employeeManager.GetEmployees();

            var managers = users.Where(u => u.Value.GetType() == typeof(Manager)).Select(u => u.Value).ToList();
            managers.ForEach(m => users.Remove(m.Id));
            registeredUsers.ForEach(m => users.Add(m.ChatID, new Manager(botClient, m.ChatID)));
        }
    }
}
