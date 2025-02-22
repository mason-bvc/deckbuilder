namespace DeckBuilder
{
    public struct Player
    {
        public Deck Deck;
        public Hand Hand;

        public void Initialize(Deck deck, Hand hand)
        {
            Deck = deck;
            Hand = hand;
        }

        public readonly void DrawFromDeckIntoHand(int count) => Deck.Draw(count, Hand.Cards);
    }
}
