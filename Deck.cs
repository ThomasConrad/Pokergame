﻿using System;
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
        private Card[] deck;
        private Int32 currentCard;
        public const Int32 cardAmount = 52;
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
                Int32 j = ranCard.Next(cardAmount);
                Card temp = deck[i];
                deck[i] = deck[j]; //shuffles a random card to an iterating spot
                deck[j] = temp; //shuffles an iterating card to a random spot
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
    }

    public class Program
    {
        static string CardToName(Card element)
        {
            String[] suitNames = { "spades", "hearts", "clubs", "diamonds" };
            String[] faceNames = { "Ace", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King" };
            string hand0 = element.ToString();
            string[] hand1 = hand0.Split(',');
            Int32[] tempInts = Array.ConvertAll(hand1, int.Parse);
            return faceNames[tempInts[0]-1] + " of " + suitNames[tempInts[1]-1];
        }

        static void PrintHandFromArray(Card[,] hands, Int32 slot = 0)
        {
            string[] tempHand = new string[2];
            Console.WriteLine("Your hand:");
            for (int i = 0; i < 2; i++)
            {
                tempHand[i] = CardToName(hands[slot-1, i]);
            }
            Console.WriteLine(tempHand[0] + " and " + tempHand[1]);
        }

        static void PrintHand(Card[] hands)
        {
            string[] tempHand = new string[2];
            //Console.WriteLine("Your hand:");
            for (int i = 0; i < 2; i++)
            {
                tempHand[i] = CardToName(hands[i]);
            }
            Console.WriteLine("Your hand: " + tempHand[0] + " and " + tempHand[1]);
        }

        
        


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
                    Console.Clear();
                    Console.WriteLine("Your hand:");

                    string[] tempCard = new string[2];
                    for (int i = 0; i < 2; i++)
                    {
                        tempCard[i] = CardToName(connection.receiveCard());
                    }
                    Console.WriteLine(tempCard[0] + " and " + tempCard[1]);
                    Console.ReadLine(); 
























                    break;
                case "2": //HOST
                    Int32 playerAmount;
                    Int32 initialMoney;
                    Int32 dead = 0;
                    Int32 round = 1;
                    Int32[] bets;
                    Int32 pool = 0;
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
                        Console.WriteLine("Your local IP: " + Game.getLocalIP().ToString());
                        Console.WriteLine("Enter IP to host on, 'local' for local ip: ");
                        string ipstring = Console.ReadLine();
                        Console.Clear();
                        if (ipstring == "local")
                        {
                            if (IPAddress.TryParse(Game.getLocalIP().ToString(), out ip))
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
                            playerAmount = Convert.ToInt32(setplayers);
                            if (playerAmount > 16 || playerAmount < 2)
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

                    //Set amount of money you start with
                    while (true)
                    {
                        Console.WriteLine("Enter initial amount of money: ");
                        string setmoney = Console.ReadLine();
                        Console.Clear();
                        try
                        {
                            initialMoney = Convert.ToInt32(setmoney);
                            if (initialMoney > 1000000000 || initialMoney < 1000)
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

                    //Creates a tracking array for the players, noting the amount of money the have as well as being used to keep general statistics for running the game.
                    //We really should've made a class for these things >:(
                    //[player, 0] for money, [player, 1] for status flags, [player, 2] for associated socket to tie it into the network system
                    Int32[,] players = new Int32[playerAmount+1,3];
                    for (Int32 i = 0; i < playerAmount; i++)
                    {
                        players[i,0] = initialMoney;

                        //Flag for status checks, big blind, small blind and bankruptcy
                        //0 = nothing, 1 = small blind, 2 = big blind, 3 = shit creek without a paddle, 4 = in-round
                        players[i, 1] = 0;
                    }

                    //Initial big/small blind
                    players[0, 1] = 1;
                    players[1, 1] = 2;

                    //Ghost player to make handling of players easier later on, sits outside the visible range of for loops, because they run towards the playerAmount
                    players[playerAmount, 0] = 0;
                    players[playerAmount, 1] = 3;

                    //Accepts players, by looping until everyone is in.
                    //Will look smooth when running, .acceptPlayer will wait for ~30 seconds before letting the program continue, unless someone tries to connect
                    Server server = new Server(ip);
                    server.listen();
                    connection.connect(ip);
                    while (true)
                    {
                        Console.Clear();
                        if (playerAmount == server.players)
                        {
                            break;
                        }
                        Console.WriteLine("Your local IP: " + Game.getLocalIP().ToString());
                        Console.WriteLine("Waiting for " + (playerAmount - server.players).ToString() + " more players.\nCurrent players:");
                        //Print names here, we need some way to receive and store them, but that's not important right now.
                        server.acceptPlayer(server.players);

                    }

                    Card[,] hands = mainDeck.PlayerHands(server.players);
                    string[] names = new string[playerAmount];

                    ////////////////////////////
                    /// RUNS THE ACTUAL GAME ///
                    ////////////////////////////

                    bool running = true;
                    while (running)
                    {
                        //Kills bankrupt players
                        for (Int32 i = 0; i < dead; i++)
                        {
                            for (Int32 j = 0; j < playerAmount; j++)
                            {
                                if (players[j, 1] == 3)
                                {
                                    players[j, 0] = players[j + 1, 0];
                                    players[j, 1] = players[j + 1, 1];
                                    players[j, 2] = players[j + 1, 2];
                                }
                            }
                        }

                        //Reduce playerAmount by amount of players that got killed
                        playerAmount -= dead;
                        dead = 0;

                        //Is this when we want to do it? Because I don't think so..
                        //---------------------------------------------------------

                        //Send hands to players

                        for (int i = 1; i < playerAmount; i++)
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                server.sendCard(hands[i, j], i);
                            }
                        }

                        //Prints the servers hand
                        Console.Clear();
                        PrintHandFromArray(hands, 1);
                        Console.ReadLine();

                        //---------------------------------------------------------

                        //Assigns big and small blinds
                        for (Int32 i = 0; i < playerAmount; i++)
                        {
                            if (players[i, 1] == 1)
                            {
                                players[i, 1] = 2;
                                if (players[i + 1, 1] == 3)
                                {
                                    players[0, 1] = 1;
                                } 
                                players[i + 1, 1] = 1;
                            }
                            else if (players[i,1] == 2)
                            {
                                players[i, 1] = 0;
                            }
                        }

                        /////////////////////////
                        /// ROUNDS BEGIN HERE ///
                        /////////////////////////

                        bets = new Int32[playerAmount];

                        //Big blind and small blind
                        for (Int32 i = 0; i < playerAmount; i++)
                        {
                            if (players[i, 1] == 1)
                            {
                                players[i, 0] -= 100;
                                bets[i] = 100;
                            }
                            if (players[i, 1] == 2)
                            {
                                players[i, 0] -= 200;
                                bets[i] = 200;
                            }
                        }

                        for (Int32 i = 0; i < playerAmount; i++)
                        {
                            players[i, 1] = 4;
                        }

                        while (round != 5)
                        {
                            for (Int32 cP = 0; cP < playerAmount; cP++)
                            {
                                server.sendString("TURN", players[cP, 2]);

                                //Tager imod input fra spillere, går ud fra at clients'ne ved hvad de laver så serveren ikke behøver at sende beskeder frem og tilbage indtil den får et gyldigt svar
                                string answer = server.receiveString(players[cP, 2], 4);
                                Int32 temp;
                                switch (answer)
                                {
                                    case "FOLD":
                                        players[cP, 1] = 0;
                                        break;
                                    case "RAIS":
                                        temp = server.receiveInt(players[cP, 2]);
                                        bets[cP] += temp;
                                        pool += temp;
                                        break;
                                    case "CALL":
                                        temp = server.receiveInt(players[cP, 2]);
                                        bets[cP] += temp;
                                        pool += temp;
                                        break;
                                    case "CHCK":
                                        break;
                                }
                                //Update board for everyone here. Somehow. We've gotta invent a format for a string that informs the changes...
                                //
                                //
                                //
                            }
                        }
                        //Check for who won somewhere in here, and update player on that information
                        //
                        //
                        //

                        //Check for bankruptcy and pay winner
                        for (Int32 i = 0; i < playerAmount; i++)
                        {
                            if (players[i, 0] < 200 && players[i, 1] == 1)
                            {
                                players[i, 1] = 3; //RIP
                                dead += 1;
                            }
                            else if (players[i, 0] < 100)
                            {
                                players[i, 1] = 3; //RIP
                                dead += 1;
                            }
                            else if (players[i, 1] == 4)
                            {
                                players[i, 1] = 0;
                            }
                            else if (players[i, 1] == 5)
                            {
                                players[i, 0] += pool; //Big money, ayyy
                            }
                        }
                    }


                    

                    
                    



                    
                    //--------------------------------------
                    /*Prints the hands of all players
                    Int32counter = 0;
                    foreach (Card element in hands)
                    {
                        if (counter % 2 == 0)
                        {
                            Console.WriteLine("Player {0}'s hand", counter / 2 + 1);
                        }
                        Console.WriteLine(CardToName(element));
                        counter++;
                    }
                    */
                    break;

            }
            ///////////////////////////////////
            //REMEMBER TO DELETE THIS SECTION//
            ///////////////////////////////////

            Console.WriteLine("Program Over, remember to delete me. ONEGAI DESU");
            Console.ReadLine();

        }

    }
  
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    public static class Game
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
        static Boolean isFlush(Card[] cards)
        {
            int[] suits = new int[7];
            int count = 0;
            foreach(var element in cards) //splits the suits from data type Card[]
            {
                string tempCard = element.ToString();
                string[] tempStrings = tempCard.Split(',');
                int[] tempInts = Array.ConvertAll(tempStrings, s => int.Parse(s));
                suits[count] = tempInts[1];
                count++; 
            }
            
            var groups = suits.GroupBy(item => item);
            foreach (var group in groups)
            {
                if (group.Count() == 5) //if there are five of the same suit, you have a flush
                {
                    return true;

                }
            }
            return false;
        }

        static Boolean isStraight(Card[] cards)
        {
            int[] ranks = new int[7];
            int count = 0;
            foreach (var element in cards)//splits the ranks from data type Card[]
            {
                string tempCard = element.ToString();
                string[] tempStrings = tempCard.Split(',');
                int[] tempInts = Array.ConvertAll(tempStrings, s => int.Parse(s));
                ranks[count] = tempInts[0];
                count++;
            }
            Array.Sort(ranks);
            Int32[] tempHand1 = new Int32[7];
            Int32[] tempHand = new Int32[7];
            Int32[] tempHand0 = new Int32[7];
            Int32 count0 = 0;
            var groups = ranks.GroupBy(item => item);
            if (groups.Count() < 5) { return false; } //if you don't have 5 different cards
            if (ranks.Max() == 13 && ranks.Min() == 1)
            {
                foreach (var group in groups)
                {
                    tempHand1[count0] = group.Key;
                    count0++;
                }
                Array.Sort(tempHand1);
                if(tempHand1[groups.Count()-1] == 13 && tempHand1[groups.Count()-2] == 12 && tempHand1[groups.Count()-3] == 11 && tempHand1[groups.Count()-4] == 10)
                {
                    return true;
                }
                    
                count0 = 0;
            }
            if (groups.Count() == 5) //if you have 5 different cards
            {
                foreach (var group in groups)
                {
                    tempHand[count0] = group.Key;
                }
                Array.Sort(tempHand);
                if (tempHand.Sum() / 5 == tempHand[2]) { return true; }
                return false;
            }
            if (groups.Count() == 6) // if you have 6 different cards
            {
                foreach (var group in groups)
                {
                    tempHand[count0] = group.Key;
                }
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        tempHand0[j] = tempHand[j + i];
                    }
                    if (tempHand0.Sum() / 5 == tempHand0[2]) { return true; }

                }
                return false;
            }
            if (groups.Count() == 7)// if you have 7 different cards
            {
                foreach (var group in groups)
                {
                    tempHand[count0] = group.Key;
                }
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        tempHand0[j] = tempHand[j + i];
                    }
                    if (tempHand0.Sum() / 5 == tempHand0[2]) { return true; }
                }
                return false;
            }
            return false;
        }
        
        static Boolean isStraightFlush(Card[] cards)
        {
            if(isStraight(cards) && isFlush(cards)) { return true; } // if it's a straight and a flush, it's a straight flush
            else { return false; }
        }

        static Boolean isRoyalFlush(Card[] cards)
        {
            if(isStraightFlush(cards)) //checks if it's a straight flush
            {
                int[] ranks = new int[7];
                int count = 0;
                foreach (var element in cards)
                {
                    string tempCard = element.ToString();
                    string[] tempStrings = tempCard.Split(',');
                    int[] tempInts = Array.ConvertAll(tempStrings, s => int.Parse(s));
                    ranks[count] = tempInts[0];
                    count++;
                }
                Array.Sort(ranks);
                Int32[] tempHand1 = new Int32[7];
                Int32 count0 = 0;
                var groups = ranks.GroupBy(item => item);
                if (ranks.Max() == 13 && ranks.Min() == 1) //checks if the lowest card is an ace, and the highest a king
                {
                    foreach (var group in groups)
                    {
                        tempHand1[count0] = group.Key;
                        count0++;
                    }
                    Array.Sort(tempHand1);//checks if you have the cards for a royal flush
                    if (tempHand1[groups.Count() - 1] == 13 && tempHand1[groups.Count() - 2] == 12 && tempHand1[groups.Count() - 3] == 11 && tempHand1[groups.Count() - 4] == 10)
                    {
                        return true;
                    }

                    else { return false; }
                }
                else { return false; }
            }
            else { return false; }
        }

        static Boolean isFullHouse(Card[] cards)
        {
            int[] rank = new int[7];
            int count = 0;
            int key = 0;
            foreach (var element in cards)
            {
                string tempCard = element.ToString();
                string[] tempStrings = tempCard.Split(',');
                int[] tempInts = Array.ConvertAll(tempStrings, s => int.Parse(s));
                rank[count] = tempInts[0];
                count++;
            }

            var groups = rank.GroupBy(item => item);
            foreach (var group in groups) //do you have three of one card
            {
                if (group.Count() >=3)
                {
                    key = group.Key;
                    break;
                }
            }
            foreach (var group in groups) // do you have two of one card, that isn't the one you had three of?
            {
                if (group.Count() >= 2 && group.Key != key && key != 0)
                {
                    return true;
                }
            }
            return false;
        }

        static Boolean isFourOfAKind(Card[] cards)
        {
            int[] rank = new int[7];
            int count = 0;
            foreach (var element in cards)
            {
                string tempCard = element.ToString();
                string[] tempStrings = tempCard.Split(',');
                int[] tempInts = Array.ConvertAll(tempStrings, s => int.Parse(s));
                rank[count] = tempInts[0];
                count++;
            }

            var groups = rank.GroupBy(item => item);
            foreach (var group in groups)
            {
                if (group.Count() >= 4)
                {
                    return true;
                }
            }
            return false;
        }

        static Boolean isThreeOfAKind(Card[] cards)
        {
            int[] rank = new int[7];
            int count = 0;
            foreach (var element in cards)
            {
                string tempCard = element.ToString();
                string[] tempStrings = tempCard.Split(',');
                int[] tempInts = Array.ConvertAll(tempStrings, s => int.Parse(s));
                rank[count] = tempInts[0];
                count++;
            }

            var groups = rank.GroupBy(item => item);
            foreach (var group in groups) // do you have three cards with the same rank
            {
                if (group.Count() >= 3)
                {
                    return true;
                }
            }
            return false;
        }

        static Boolean isTwoPair(Card[] cards)
        {
            int[] rank = new int[7];
            int count = 0;
            int key = 0;
            foreach (var element in cards)
            {
                string tempCard = element.ToString();
                string[] tempStrings = tempCard.Split(',');
                int[] tempInts = Array.ConvertAll(tempStrings, s => int.Parse(s));
                rank[count] = tempInts[0];
                count++;
            }

            var groups = rank.GroupBy(item => item); // do you have two of the same card
            foreach (var group in groups)
            {
                if (group.Count() >= 2)
                {
                    key = group.Key;
                    break;
                }
            }
            foreach (var group in groups)// do you have two of the same card, that isn't the same type of cards as the one you just had
            {
                if (group.Count() >= 2 && group.Key != key && key != 0)
                {
                    return true;
                }
            }
            return false;
        }

        static Boolean isPair(Card[] cards)
        {
            int[] rank = new int[7];
            int count = 0;
            foreach (var element in cards)
            {
                string tempCard = element.ToString();
                string[] tempStrings = tempCard.Split(',');
                int[] tempInts = Array.ConvertAll(tempStrings, s => int.Parse(s));
                rank[count] = tempInts[0];
                count++;
            }

            var groups = rank.GroupBy(item => item); // do you have two cards of the same value
            foreach (var group in groups)
            {
                if (group.Count() >= 2)
                {
                    return true;
                }
            }
            return false;
        }

        static int highCard(Card[] cards)
        {
            int[] rank = new int[7];
            int count = 0;
            foreach (var element in cards)
            {
                string tempCard = element.ToString();
                string[] tempStrings = tempCard.Split(',');
                int[] tempInts = Array.ConvertAll(tempStrings, s => int.Parse(s));
                rank[count] = tempInts[0];
                count++;
            }
            Array.Sort(rank);
            if (rank.Min() == 1) // if you have an ace, the highest is 14
                return 14;
            else // else, it's just the highest card
            {
                return rank.Max();
            } 
        }

        static int deckValue(Card[] cards)
        {
            int highValue = highCard(cards);
            if (isRoyalFlush(cards)) { return 180 + highValue; }
            if (isStraightFlush(cards)) { return 160 + highValue; }
            if (isFourOfAKind(cards)) { return 140 + highValue; }
            if (isFullHouse(cards)) { return 120 + highValue; }
            if (isFlush(cards)) { return 100 + highValue; }
            if (isStraight(cards)) { return 80 + highValue; }
            if (isThreeOfAKind(cards)) { return 60 + highValue; }
            if (isTwoPair(cards)) { return 40 + highValue; }
            if (isPair(cards)) { return 20 + highValue; }
            else {return highValue; }
        }

        static void BestHand(Card[] playerHand, Card[] board)
        {
            int cardAmount = playerHand.Length + board.Length;
            Card[] cards = new Card[cardAmount];
            for (int i = 0; i < cardAmount; i++)
            {
                if (i < playerHand.Length) { cards[i] = playerHand[i]; }
                else { cards[i] = board[i]; }
            }

        }
    }

    public class Server
    {
        System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
        public Socket[] sockets = new Socket[16];
        public Int32 players = 0;
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
            Thread.Sleep(50);
        }

        public void sendCard(Card message, Int32 slot)
        {
            sockets[slot].Send(encoder.GetBytes(message.ToString()));
            Thread.Sleep(50);
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
            return encoder.GetString(bytes);
        }

        public Card receiveCard(Int32 length = 64)
        {
            byte[] bytes = new byte[length];
            s.Receive(bytes);
            string recievedCardString = encoder.GetString(bytes);
            string[] recievedCardStringArray = recievedCardString.Split(',');
            Int32[] recievedCardInts = Array.ConvertAll(recievedCardStringArray, int.Parse);

            Card recievedCard = new Card(recievedCardInts[0], recievedCardInts[1]);
            return recievedCard;
        }
    }
}