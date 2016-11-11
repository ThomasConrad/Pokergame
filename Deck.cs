using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Poker
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
            for (int count = 0; count < deck.Length; count++)
            {//Creates deck
                deck[count] = new Card(faces[count % 13], suits[count / 13]);
            }
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

        public Card GenerateCard(int amount = 52) //Valgfri mængde kort, standard = 52
        {
            if (currentCard < amount)
                return deck[currentCard++];
            else
                return null;
        }

        public Card[,] Hand(int playerAmount)
        {
            Card[,] hands = new Card[playerAmount, 2];

            for(int i = 0; i < playerAmount; i++)
            {
                for(int j = 0; j < 2; j++)
                {
                    hands[i, j] = GenerateCard(8);
                }
            }
            return hands;
        }


    }

    public class Program
    {
        public static void Main()
        {

            Deck mainDeck = new Deck();
            mainDeck.Shuffle();
            Card[,] hands = mainDeck.Hand(4);

            
            for (int i = 0; i < Deck.cardAmount; i++)
            {
                Console.Write("{0,-20}", mainDeck.GenerateCard());
                if ((i + 1) % 4 == 0)
                    Console.WriteLine();
            }
            Console.ReadKey();
            foreach(Card element in hands)
            {
                Console.WriteLine(element);
            }
            Console.ReadKey();
            











































            Console.WriteLine("1. Join Game\n2. Host Game");
            Int32 pick;
            while (true)
            {
                try
                {
                    pick = Convert.ToInt32(Console.ReadLine());
                    break;
                }
                catch
                {
                    continue;
                }
            }
            Console.Clear();
            IPAddress ip;
            Client connection = new Client();
            switch (pick)
            {
                case 1:
                    while (true)
                    {
                        Console.WriteLine("X: Back to menu\nEnter IP of game host: ");
                        string ipstring = Console.ReadLine();
                        Console.Clear();
                        if (IPAddress.TryParse(ipstring, out ip))
                        {
                            break;
                        }
                    }
                    try
                    {
                        Console.WriteLine("Attempting Connection...");
                        connection.connect(ip, 31415);
                    }
                    catch (SocketException)
                    {
                        Console.WriteLine("Connection timed out. Server is unavailable.\nPress ANY key to continue");
                        Console.ReadKey();
                        Console.Clear();
                        goto case 1;
                    }








                    break;
                case 2:
                    Console.WriteLine("Your local IP: " + NetTools.getLocalIP().ToString());
                    while (true)
                    {
                        Console.WriteLine("Enter IP to host on: ");
                        string ipstring = Console.ReadLine();

                        if (IPAddress.TryParse(ipstring, out ip))
                        {
                            break;
                        }
                        Console.Clear();
                    }
                    Server server = new Server(ip, 31415);




                    break;
            }


            Console.WriteLine("Program Over, remember to delete me");
            Console.ReadLine();
            
        }
    }

    public static class NetTools
    {
        public static IPAddress getLocalIP()
        {
            IPAddress ip;
            foreach (IPAddress i in Dns.GetHostAddresses(""))
            {
                if (i.AddressFamily == AddressFamily.InterNetwork)
                {
                    ip = i;
                    return ip;
                }
            }
            return null;
        }
    }

    public class Server
    {
        System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
        public Int32 playerCount = 0;
        public Socket[] players = new Socket[8];
        Socket s;
        
        public Server(IPAddress ip, Int32 port)
        {
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endpoint = new IPEndPoint(ip, port);
            s.Bind(endpoint);
        }

        public void acceptPlayer(Int32 slot)
        {
            players[slot] = s.Accept();
            playerCount++;
        }

        public void listen()
        {
            s.Listen(10);
        }

        public void closePlayer(Int32 slot)
        {
            players[slot].Close();
            playerCount--;
        }

        public void sendString(string message, Int32 slot)
        {
            players[slot].Send(encoder.GetBytes(message));
        }

        public void sendInt(Int32 message, Int32 slot)
        {
            players[slot].Send(encoder.GetBytes(message.ToString()));
        }

        public Int32 receiveInt(Int32 slot)
        {
            byte[] bytes = new byte[64];
            players[slot].Receive(bytes);
            return Convert.ToInt32(encoder.GetString(bytes));
        }

        public string receiveString(Int32 slot, Int32 length = 64)
        {
            byte[] bytes = new byte[length];
            players[slot].Receive(bytes);
            return encoder.GetString(bytes);
        }
    }

    public class Client
    {
        System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
        Socket s;
        public Client()
        {
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void connect(IPAddress ip, Int32 port)
        {
            s.Connect(ip, port);
        }

        public void sendString(string message)
        {
            s.Send(encoder.GetBytes(message));
        }

        public void sendInt(Int32 message)
        {
            s.Send(encoder.GetBytes(message.ToString()));
        }

        public Int32 receiveInt(Socket s)
        {
            byte[] bytes = new byte[1024];
            s.Receive(bytes);
            return Convert.ToInt32(encoder.GetString(bytes));
        }

        public string receiveString(Socket s, Int32 length = 64)
        {
            byte[] bytes = new byte[length];
            s.Receive(bytes);
            return encoder.GetString(bytes);
        }
    }
}