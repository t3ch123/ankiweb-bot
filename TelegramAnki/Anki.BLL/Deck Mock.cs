using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramAnki
{
    public class DeckMock
    {
        public static Deck GetDecks()
        {
            string name = "Math";
            List<Cards> cards = new List<Cards>();
            return new Deck(name, cards);
        }
    }
}
