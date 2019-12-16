using System;
using System.Collections.Generic;
using System.Linq;

namespace Ranking
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> contests = new Dictionary<string, string>();
            Dictionary<string, Dictionary<string, int>> usersDict = new Dictionary<string, Dictionary<string, int>>();

            while (true)
            {
                string inputFirst = Console.ReadLine();

                if (inputFirst == "end of contests")
                {
                    break;
                }

                string[] contestAndPassword = inputFirst
                    .Split(':', StringSplitOptions.RemoveEmptyEntries);

                string contest = contestAndPassword[0];
                string password = contestAndPassword[1];

                if(!contests.ContainsKey(contest))
                {
                    contests[contest] = password;
                }
            }
            while (true)
            {
                string inputSecond = Console.ReadLine();

                if (inputSecond == "end of submissions")
                {
                    break;
                }

                string[] usersContests = inputSecond
                    .Split("=>", StringSplitOptions.RemoveEmptyEntries);

                string contest = usersContests[0];
                string password = usersContests[1];
                string user = usersContests[2];
                int points = int.Parse(usersContests[3]);

                if (contests.ContainsKey(contest) && password == contests[contest])
                {
                    if (!usersDict.ContainsKey(user))
                    {
                        usersDict[user] = new Dictionary<string, int>();
                    }
                    if (!usersDict[user].ContainsKey(contest))
                    {
                         usersDict[user][contest] = points;
                    }
                    else
                    {
                        if (usersDict[user][contest] < points)
                        {
                            usersDict[user][contest] = points;
                        }
                    }
                }
            }
            
            string topUser = null;
            int totalPoints = 0;

            foreach (var user in usersDict)
            {
                string currentUser = null;
                int pointsChek = 0;
                
                Dictionary<string, int> contestPoint = user.Value;

                foreach (var contest in contestPoint)
                {
                    pointsChek += contest.Value;
                }
                if (pointsChek >= totalPoints)
                {
                    totalPoints = pointsChek;
                    topUser = user.Key;
                }
            }
            Console.WriteLine($"Best candidate is {topUser} with total {totalPoints} points.");
            Console.WriteLine("Ranking:");
            
            foreach (var user in usersDict.OrderBy(u => u.Key))
            {
                Console.WriteLine(user.Key);

                Dictionary<string, int> contestAndPoints = user.Value;
                foreach (var item in contestAndPoints.OrderByDescending(v => v.Value))
                {
                    Console.WriteLine($"#  {item.Key} -> {item.Value}");
                }
            }
        }
    }
}
