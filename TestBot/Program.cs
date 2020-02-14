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

        static Dictionary<long, Admin> users = new Dictionary<long, Admin>();

        static void Main(string[] args) {

            //var proxy = new HttpToSocks5Proxy("45.55.230.207", 30405);
            //botClient = new TelegramBotClient("755174490:AAGr0dyLPskL3UL3HyUeJbqXVXpijKj7hCI", proxy)

            botClient = new TelegramBotClient("755174490:AAGr0dyLPskL3UL3HyUeJbqXVXpijKj7hCI") {
                Timeout = TimeSpan.FromSeconds(10)
            };

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
            var text = e?.Message?.Text;
            var id = e.Message.Chat.Id;
            //var id = e.Message.From.Id;
            //var messageID = e?.Message?.MessageId;

            if (text == null)
                return;

            if (!users.ContainsKey(id)) {
                users.Add(id, new Admin(botClient, id));
            }
            await users[id].ProcessMessage(text);

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
    }
}
