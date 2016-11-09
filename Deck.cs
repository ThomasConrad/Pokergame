using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Card
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

        public int getCardAmount()
        {
            return cardAmount;
        }

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

        public Card GenerateCard()
        {
            if (currentCard < deck.Length)
                return deck[currentCard++];
            else
                return null;
        }
    }

    public class StartHand
    {
        int playerAmount = 4;
        public List<List<string>> Hands = new List<List<string>>();


        
    }

    public class Builder
    {
        public static void Main()
        {
            Deck mainDeck = new Deck();
            mainDeck.Shuffle();

            for (int i = 0; i < Deck.cardAmount; i++)
            {
                Console.Write("{0,-20}",mainDeck.GenerateCard());
                if ((i + 1) % 4 == 0)
                    Console.WriteLine();
            }
            Console.ReadLine();
        }
    }

    public static class Net
    {
        public static IPAddress getLocalIP()
        {
            IPAddress ip;
            foreach (IPAddress i in Dns.GetHostAddresses())
            {
                if (i.AddressFamily == AddressFamily.InterNetwork)
                {
                    ip = i;
                    break;
                }
            }
            return ip;
        }
        //Bare fordi det er lidt lettere at arbejde med:
        public static string getIPString()
        {
            return getIP().ToString();
        }

        public Int32 receiveInt(Socket s)
        {
            byte[] bytes;
            s.Receive(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public string receiveString(Socket s)
        {
            byte[] bytes;
            s.Receive(bytes);
            return BitConverter.ToString(bytes, 0);
        }
    }
    public class Server
    {
        public Int32 playerCount = 0;
        public Socket[] players = new Socket[8];
        Socket s;

        public Server(IPAddress ip, Int32 port)
        {
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endpoint = new IPEndPoint(ip, port);
            s.Bind(endpoint);
        }

        public void acceptPlayer()
        {

        }
    }
}