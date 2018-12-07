using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GiftCard
{
    class Program
    {
        /// <summary>
        /// Console app takes the following arguments.
        /// [Command] [File] [Balance] [GiftCount] - e.g find-pair prices.txt 2300 2
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Invalid argument list");
                return;
            }

            var command = args[0];
            var file = args[1];
            var balance = Convert.ToInt64(args[2]);
            var amountOfGifts = Convert.ToInt64(args[3]);

            if (command.Equals("find-pair"))
            {
                try
                {
                    var result = GetPairOfGiftsFast(ReadFile(file), balance, amountOfGifts);
                    //var result = GetPairOfGifts(ReadFile(file), balance);

                    if (result.Count > 0)
                    {
                        foreach (var gift in result)
                        {
                            Console.Write($"{gift.Item1} {gift.Item2},");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Not possible");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("find-pair command not found");
            }

            Console.ReadLine();
        }



        private static List<Tuple<string, long>> ReadFile(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);

            var items = new List<Tuple<string, long>>();

            foreach (var line in lines)
            {
                string[] col = line.Split(new char[] { ',' });

                items.Add(new Tuple<string, long>(col[0], Convert.ToInt64(col[1])));
            }

            return items;
        }

        private static IList<Tuple<string, long>> GetPairOfGiftsFast(List<Tuple<string, long>> gifts, long balance, long giftCount = 2)
        {
            List<Tuple<string, long>> selectedGifts = new List<Tuple<string, long>>();

            Queue<Tuple<string, long>> itemsSelected = new Queue<Tuple<string, long>>();

            long currentBalance = balance;

            for (int i = gifts.Count - 1; i > 0; i--)
            {
                if (gifts[i].Item2 < balance)
                {
                    if (itemsSelected.Sum(x => x.Item2) > balance)
                    {
                        itemsSelected.Dequeue();
                    }
                    else
                    {
                        itemsSelected.Enqueue(gifts[i]);
                    }
                }
            }

            foreach (var item in itemsSelected)
            {
                if (itemsSelected.Count > 0 && itemsSelected.Count <= giftCount)
                {
                    selectedGifts.Add(item);
                }
                else
                {
                    break;
                }
            }            

            return selectedGifts;
        }

        private static IList<Tuple<string, long>> GetPairOfGifts(List<Tuple<string, long>> gifts, long balance)
        {
            var selectedGifts = new List<List<Tuple<string, long>>>();

            int leftItem = 0;
            int rightItem = gifts.Count - 1;

            while (leftItem < rightItem)
            {
                if (gifts[leftItem].Item2 + gifts[rightItem].Item2 <= balance)
                {
                    selectedGifts.Add(new List<Tuple<string, long>>()
                    {
                        gifts[leftItem],
                        gifts[rightItem]
                    });

                    leftItem++;
                }
                else if(gifts[leftItem].Item2 + gifts[rightItem].Item2 < balance)
                {
                    leftItem++;
                }
                else
                {
                    rightItem--;
                }
            }

            // Get the closet gift pair to the balance.
            var selectedGiftPair = selectedGifts.First();

            foreach (var item in selectedGifts.Skip(1))
            {
                if (selectedGiftPair.Sum(c => c.Item2) < item.Sum(s => s.Item2))
                {
                    selectedGiftPair = item;
                }
            }

            return selectedGiftPair;
        }
    }
}
