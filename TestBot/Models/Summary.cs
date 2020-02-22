using System;
using System.Collections.Generic;
using System.Text;

namespace MebelTelegramBot.Models {
    public class Summary {

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
        }

        public string Validate() {
            result = null;

            if (Appointed > Lead)
                result = $"{Appointed} КЭВ назначенных > {Lead} Лидов." + Environment.NewLine; 
            if (Committed > Appointed)
                result = result + $"{Committed} КЭВ проведенных > {Appointed} КЭВ назначенных." + Environment.NewLine;
            if (Deal > Appointed)
                result = result + $"{Deal} Сделок > {Appointed} КЭВ назначенных." + Environment.NewLine;
            if (Deal > Committed)
                result = result + $"{Deal} Cделок > {Committed} КЭВ проведенных." + Environment.NewLine;

            return result;
        }
    }
}
