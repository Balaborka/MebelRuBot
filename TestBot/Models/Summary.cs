using System;
using System.Collections.Generic;
using System.Text;

namespace MebelTelegramBot.Models {
    public class Summary {

        //Правила заполнения:
        //Лиды  > КЭВ Назначил = false
        //КЭВ Назначил < КЭВ Проведенных = false
        //Сделок > КЭВ Проведенных = false
        //Сделок < КЭВ Проведенных = false

        public int Lead { get; set; }
        public int Appointed { get; set; }
        public int Committed { get; set; }
        public int Deal { get; set; }
        public long Id { get; set; }
        public DateTime Date { get; set; }

        string result;

        public Summary(long id, List<int> summaryes) {
            Id = id;
            Date = DateTime.Now;
            Lead = summaryes[0]; Appointed = summaryes[1]; Committed = summaryes[2]; Deal = summaryes[3];

            //Method Validate
        }

        public string Validate() {
            result = null;

            if (Lead > Appointed)
                result = $"{Lead} Лидов больше чем {Appointed} КЭВ назначенных." + Environment.NewLine; 
            if (Committed > Appointed)
                result = result + $"{Committed} КЭВ проведенных больше чем {Appointed} КЭВ назначенных." + Environment.NewLine;
            if (Deal > Appointed)
                result = result + $"{Deal} Сделок больше чем {Appointed} КЭВ назначенных." + Environment.NewLine;
            if (Committed > Deal)
                result = result + $"{Committed} КЭВ проведенных больше чем {Deal} сделок." + Environment.NewLine;

            return result;
        }
    }
}
