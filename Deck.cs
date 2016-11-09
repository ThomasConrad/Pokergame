using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplication1
{
    public class Card
    {
        private string face;
        private string suit;

        public Card(string cardFace, string cardSuit)
        {
            face = cardFace;
            suit = cardSuit;
        }

        public override string ToString()
        {
            return face + " of " + suit;
        }
    }

    public class Deck
    {
        private Card[] deck;
        private int currentCard;
        public const int cardAmount = 52;
        private Random ranCard;

        public Deck()
        {
            String[] faces = { "Ace", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King" };
            String[] suits = { "spades", "hearts", "clubs", "diamonds" };
            deck = new Card[cardAmount];
            currentCard = 0;
            ranCard = new Random();
            for (int count = 0; count < deck.Length; count++) //Creates deck
                deck[count] = new Card(faces[count % 13], suits[count / 13]);
        }

        public void Shuffle()
        {
            currentCard = 0;
            for (int i = 0; i < deck.Length; i++)
            {
                int j = ranCard.Next(cardAmount);
                Card temp = deck[i];
                deck[i] = deck[j]; //shuffles a random card to ilterating spot
                deck[j] = temp; //shuffles ilterating card to random spot
            }

        }

        public Card DealCard()
        {
            if (currentCard < deck.Length)
                return deck[currentCard++];
            else
                return null;
        }
    }

    public class Dealer
    {
        public static void Main()
        {
            Deck mainDeck = new Deck();
            mainDeck.Shuffle();
            for (int i = 0; i < 52; i++)
            {
                Console.Write("{0,-20}",mainDeck.DealCard());
                if ((i + 1) % 4 == 0)
                    Console.WriteLine();
            }
            Console.ReadLine();
        }
    }
}