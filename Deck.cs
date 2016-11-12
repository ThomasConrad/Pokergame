﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;


//Let's fucking go yo
namespace Poker
{
    

    public class Card
    {
        private Int32 face;
        private Int32 suit;

        public Card(Int32 cardFace, Int32 cardSuit)
        {
            face = cardFace;
            suit = cardSuit;
        }

        public override string ToString()
        {
            return face + "," + suit;
        }
    }

    //Easy way to do a collection of cards
    public class Deck
    {
        String[] faceName = { "Ace", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King" };
        String[] suit = { "spades", "hearts", "clubs", "diamonds" };
        private Card[] deck;
        private int currentCard;
        public const int cardAmount = 52;
        private Random ranCard;

        public Deck()
        {

            Int32[] faces = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
            Int32[] suits = { 1, 2, 3, 4 };

           
            deck = new Card[cardAmount];
            currentCard = 0;
            ranCard = new Random();
            for (int count = 0; count < deck.Length; count++)//Creates deck
            {
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
                deck[i] = deck[j]; //shuffles a random card to iterating spot
                deck[j] = temp; //shuffles iterating card to random spot
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

            for (int i = 0; i < playerAmount; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    hands[i, j] = GenerateCard(playerAmount*2);
                }
            }
            return hands;
        }
    }

    public class Player
    {
        public Int32 currency;
        
        public Player(Int32 initialMoney)
        {
            currency = initialMoney;
        } 
    }

    public class Program
    {
        public static void Main()
        {
            Deck mainDeck = new Deck();
            mainDeck.Shuffle();
            Card[,] hands = mainDeck.Hand(4); //TODO: Make dependant on actual amount of players

            string hando = hands[1, 1].ToString();
            string[] s0 = hando.Split(',');
            Console.WriteLine("{0}", string.Join(", ", s0));
            Console.ReadKey();
            /*
            for (int i = 0; i < Deck.cardAmount; i++)
            {
                Console.Write("{0,-20}", mainDeck.GenerateCard());
                if ((i + 1) % 4 == 0)
                    Console.WriteLine();
            }
            Console.ReadKey();
            */
            foreach (Card element in hands)
            {
                Console.WriteLine(element);
            }
            Console.ReadKey();
            
            









































            //Menu options
            Console.WriteLine("1. Join Game\n2. Host Game");
            string pick;
            pick = Console.ReadLine();
            Console.Clear();
            IPAddress ip;
            Console.WriteLine("Pick a name: ");
            string name = Console.ReadLine();
            Console.ReadLine();
            Client connection = new Client(name);
            switch (pick)
            {
                //Joining game
                case "1":
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
                        goto case "1";
                    }
                    Console.Clear();
                    Console.WriteLine("Connection successful! Waiting for host to start.");







                    break;
                //Hosting game
                case "2":
                    Console.WriteLine("Your local IP: " + NetTools.getLocalIP().ToString());
                    while (true)
                    {
                        Console.WriteLine("Enter IP to host on: ");
                        string ipstring = Console.ReadLine();
                        if (IPAddress.TryParse(ipstring, out ip))
                        {
                            break;
                        }
                    }
                    Server server = new Server(ip, 31415);
                    server.listen();
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("Server is running! Press ENTER to update player list.");
                        pick = Console.ReadLine();
                        if (pick == "")
                        {
                            for (Int32 s = 0; s<server.maxplayers; s++)
                            {
                                if (server.sockets[s] == null)
                                {
                                    try
                                    {
                                        server.acceptPlayer(s);
                                        Console.WriteLine(server.sockets);
                                    }
                                    catch (SocketException) { }
                                }

                            }
                        }

                    }
                    



                    break;
            }
            ///////////////////////////////////
            //REMEMBER TO DELETE THIS SECTION//
            ///////////////////////////////////

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
        public Socket[] sockets = new Socket[8];
        public Int32 maxplayers = 8;
        public Int32 initialMoney = 1000;
        Socket s;
        
        public Server(IPAddress ip, Int32 port)
        {
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endpoint = new IPEndPoint(ip, port);
            s.Bind(endpoint);
        }

        public void acceptPlayer(Int32 slot)
        {
            sockets[slot] = s.Accept();
            playerCount++;
        }

        public void listen()
        {
            s.Listen(10);
        }

        public void closePlayer(Int32 slot)
        {
            sockets[slot].Close();
            playerCount--;
        }

        public void sendString(string message, Int32 slot)
        {
            sockets[slot].Send(encoder.GetBytes(message));
        }

        public void sendInt(Int32 message, Int32 slot)
        {
            sockets[slot].Send(encoder.GetBytes(message.ToString()));
        }

        public Int32 receiveInt(Int32 slot)
        {
            byte[] bytes = new byte[64];
            sockets[slot].Receive(bytes);
            return Convert.ToInt32(encoder.GetString(bytes));
        }

        public string receiveString(Int32 slot, Int32 length = 64)
        {
            byte[] bytes = new byte[length];
            sockets[slot].Receive(bytes);
            return encoder.GetString(bytes);
        }
    }

    public class Client
    {
        System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
        Socket s;
        public string name;

        public Client(string name = "Trump")
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

        public Int32 receiveInt()
        {
            byte[] bytes = new byte[1024];
            s.Receive(bytes);
            return Convert.ToInt32(encoder.GetString(bytes));
        }

        public string receiveString(Int32 length = 64)
        {
            byte[] bytes = new byte[length];
            s.Receive(bytes);
            return encoder.GetString(bytes);
        }
    }
}