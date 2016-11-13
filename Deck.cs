using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;


//Let's fucking go yo!
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
        public String[] faceNames = { "Ace", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King" };
        public String[] suitNames = { "spades", "hearts", "clubs", "diamonds" };
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

        public Card[,] PlayerHands(int playerAmount)
        {
            Card[,] hands = new Card[playerAmount, 2];

            for (int i = 0; i < playerAmount; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    hands[i, j] = GenerateCard(playerAmount * 2);
                }
            }
            return hands;
        }
        public Card[] Hand(int playerNum, Card[,] hands)
        {
            Card[] tempHand = new Card[2];

            for (int i = 0; i < 2; i++)
            {
                tempHand[i] = hands[playerNum - 1, i];
            }
            return tempHand;
        }
        public Int32[] CardToIntArray(Card element)
        {
            string hand0 = element.ToString();
            string[] hand1 = hand0.Split(',');
            Int32[] myInts = Array.ConvertAll(hand1, int.Parse);
            return myInts;
        }
        public string CardToName(Card element)
        {
            string hand0 = element.ToString();
            string[] hand1 = hand0.Split(',');
            Int32[] myInts = Array.ConvertAll(hand1, int.Parse);
            return faceNames[myInts[0] - 1] + " of " + suitNames[myInts[1] - 1];
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

            /*
            for (int i = 0; i < Deck.cardAmount; i++)
            {
                Console.Write("{0,-20}", mainDeck.GenerateCard());
                if ((i + 1) % 4 == 0)
                    Console.WriteLine();
            }
            Console.ReadKey();
            */












































            //Menu options
            Console.WriteLine("1. Join Game\n2. Host Game");
            string pick;
            pick = Console.ReadLine();
            Console.Clear();
            IPAddress ip;
            Int32 playeramount;
            Console.WriteLine("Pick a name: ");
            string name = Console.ReadLine();
            Console.Clear();
            //Console.ReadLine();
            Client connection = new Client(name);
            switch (pick)
            {
                case "1"://CLIENT
                    while (true)
                    {
                        Console.WriteLine("Enter IP of game host: ");
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
                        connection.connect(ip);
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
                    Console.WriteLine(connection.receiveString());
                    Console.WriteLine(connection.receiveString()); //skip this shit. kk pls fix
























                    break;
                case "2": //HOST
                    Deck mainDeck = new Deck();
                    mainDeck.Shuffle();

                    /*
                    string hando = hands[1, 1].ToString();
                    string[] s0 = hando.Split(',');
                    Console.WriteLine("{0}", string.Join(" ", s0));
                    Console.ReadKey();
                    
                    */





                    //Set IP address to host on
                    while (true)
                    {
                        Console.WriteLine("Your local IP: " + NetTools.getLocalIP().ToString());
                        Console.WriteLine("Enter IP to host on, 'local' for local ip: ");
                        string ipstring = Console.ReadLine();
                        Console.Clear();
                        if (ipstring == "local")
                        {
                            if (IPAddress.TryParse(NetTools.getLocalIP().ToString(), out ip))
                            {
                                break;
                            }
                        }
                        else if (IPAddress.TryParse(ipstring, out ip))
                        {
                            break;
                        }
                    }

                    //Set player amount
                    while (true)
                    {
                        Console.WriteLine("Enter amount of players (max 16): ");
                        string setplayers = Console.ReadLine();
                        Console.Clear();
                        try
                        {
                            playeramount = Convert.ToInt32(setplayers);
                            if (playeramount > 16 || playeramount < 2)
                            {
                                continue;
                            }
                            break;
                        }
                        catch (FormatException)
                        {
                            continue;
                        }
                    }

                    //Accepts players, by looping until everyone is in.
                    //Will look smooth when running, .acceptPlayer will wait for ~30 seconds before letting the program continue, unless someone tries to connect
                    Server server = new Server(ip);
                    server.listen();
                    connection.connect(ip);
                    while (true)
                    {
                        Console.Clear();
                        if (playeramount == server.players) //changing for testing
                        {
                            break;
                        }
                        Console.WriteLine("Your local IP: " + NetTools.getLocalIP().ToString());
                        Console.WriteLine("Waiting for " + (playeramount - server.players).ToString() + " more players.\nCurrent players:");
                        //Print names here, we need some way to receive and store them, but that's not important right now.
                        server.acceptPlayer(server.players);

                    }


                    //server.sendStringToAll("Server ready. Press enter to begin.");
                    //connection.receiveString();
                    Card[,] hands = mainDeck.PlayerHands(server.players);

                    //--------------------------------------
                    //Send hands to players
                    for (int i = 0; i < playeramount; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            string tempCard = Convert.ToString(hands[i, j]);
                            server.sendString(tempCard, i);
                        }
                    }

                    //--------------------------------------
                    //prints the servers hand
                    foreach (var element in mainDeck.Hand(1, hands))

                        Console.WriteLine(element);

                    //--------------------------------------
                    //Prints the hands of all players
                    int counter = 0;
                    foreach (Card element in hands)
                    {
                        if (counter % 2 == 0)
                        {
                            Console.WriteLine("Player {0}'s hand", counter / 2 + 1);
                        }
                        //Console.WriteLine(string.Join(",", mainDeck.CardToIntArray(element)));
                        Console.WriteLine(mainDeck.CardToName(element));
                        counter++;
                    }

                    //Console.ReadKey();
                    break;

            }
            ///////////////////////////////////
            //REMEMBER TO DELETE THIS SECTION//
            ///////////////////////////////////

            Console.WriteLine("Program Over, remember to delete me. ONEGAI DESU");
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
        public Socket[] sockets = new Socket[16];
        public Int32 players = 0;
        public Int32 initialMoney = 1000;
        Socket s;

        public Server(IPAddress ip, Int32 port = 31415)
        {
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endpoint = new IPEndPoint(ip, port);
            try
            {
                s.Bind(endpoint);
            }
            catch (SocketException)
            {
                Console.WriteLine("The IP you inserted is invalid. The program will now exit..");
                Console.ReadKey();
                System.Environment.Exit(1);
            }

        }

        public void acceptPlayer(Int32 slot)
        {
            sockets[slot] = s.Accept();
            players++;
        }

        public void listen(int time = 10)
        {
            s.Listen(time);
        }

        public void sendString(string message, Int32 slot)
        {
            sockets[slot].Send(encoder.GetBytes(message));
        }

        public void sendCard(Card message, Int32 slot)
        {
            sockets[slot].Send(encoder.GetBytes(message.ToString()));
        }

        public void sendStringToAll(string message)
        {
            for (Int32 i = 0; i < players; i++)
            {
                sockets[i].Send(encoder.GetBytes(message));
            }
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

        public void connect(IPAddress ip, Int32 port = 31415)
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
            Thread.Sleep(100);
            return encoder.GetString(bytes);
        }

        public string receiveCard(Int32 length = 64)
        {
            byte[] bytes = new byte[length];
            s.Receive(bytes);
            Thread.Sleep(100);
            return encoder.GetString(bytes);
        }
    }
}