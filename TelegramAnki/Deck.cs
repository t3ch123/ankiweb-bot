using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramAnki
{
    public class Deck
    {
        public string Name { get; private set; }
        public List<Cards> CardsList { get; private set; }

        public Deck(string Name, List<Cards> cards)
        {
            this.Name = Name;
            CardsList = new List<Cards>();
        }
    }
}

