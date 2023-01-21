namespace TelegramAnki
{
    public class DeckOutputModel
    {
        public string Name { get; private set; }
        public List<CardOutputModel> CardsList { get; private set; }

        public DeckOutputModel(string Name, List<CardOutputModel> cards)
        {
            this.Name = Name;
            CardsList = cards;
        }
    }
}
