using System;
using System.Collections.Generic;
using System.Linq;

namespace TheV_Logger
{
    class Vloger
    {
        public string Name { get; set; }
        public int Followers { get; set; }
        public int Followings { get; set; }

        class Program
        {
            static void Main(string[] args)
            {
                Dictionary<string, List<string>> vlogersWithFollowers = new Dictionary<string, List<string>>();
                Dictionary<string, List<string>> vlogersWithFollowings = new Dictionary<string, List<string>>();

                while (true)
                {
                    string input = Console.ReadLine();

                    if (input == "Statistics")
                    {
                        break;
                    }

                    string[] command = input
                        .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    string vloger = command[0];

                    if (command[1] == "joined")
                    {
                        if (!vlogersWithFollowers.ContainsKey(vloger))
                        {
                            vlogersWithFollowers[vloger] = new List<string>();
                            vlogersWithFollowings[vloger] = new List<string>();
                        }
                    }
                    else if (command[1] == "followed")
                    {
                        string followedVloger = command[2];

                        if (vlogersWithFollowers.ContainsKey(vloger) && vlogersWithFollowers.ContainsKey(followedVloger) && vloger != followedVloger)
                        {
                            if (!vlogersWithFollowers[followedVloger].Contains(vloger))
                            {
                                vlogersWithFollowers[followedVloger].Add(vloger);
                                vlogersWithFollowings[vloger].Add(followedVloger);
                            }
                        }
                    }
                }
                int vlogersCount = vlogersWithFollowers.Count;
                Console.WriteLine($"The V-Logger has a total of {vlogersCount} vloggers in its logs.");

                foreach (var vloger in vlogersWithFollowers.OrderByDescending(f => f.Value.Count))
                {
                    string topVloger = vloger.Key;
                    int followersCount = vloger.Value.Count;
                    int topVlogerFollowings = 0;

                    foreach (var vlogerFollowing in vlogersWithFollowings)
                    {
                        if (vlogerFollowing.Key == topVloger)
                        {
                            topVlogerFollowings = vlogerFollowing.Value.Count;
                        }
                    }

                    Console.WriteLine($"1. {topVloger} : {followersCount} followers, {topVlogerFollowings} following");
                    List<string> followers = vloger.Value;
                    followers.Sort();
                    foreach (var follower in followers)
                    {
                        Console.WriteLine($"*  {follower}");
                    }
                    vlogersWithFollowers.Remove(topVloger);
                    break;
                }
                int count = 2;
                List<Vloger> vlogersInClass = new List<Vloger>();

                foreach (var kvp in vlogersWithFollowers)
                {
                    string vloger = kvp.Key;
                    int followersCount = kvp.Value.Count;
                    int vlogerFollowings = 0;


                    foreach (var vlogerFollowing in vlogersWithFollowings)
                    {
                        if (vlogerFollowing.Key == vloger)
                        {
                            vlogerFollowings = vlogerFollowing.Value.Count;
                        }
                    }


                    var vlogerClass = new Vloger()
                    {
                        Name = vloger,
                        Followers = followersCount,
                        Followings = vlogerFollowings
                    };

                    vlogersInClass.Add(vlogerClass);
                }
                foreach (var vloger in vlogersInClass.OrderByDescending(v => v.Followers).ThenBy(f => f.Followings))
                    {
                    Console.WriteLine($"{count}. {vloger.Name} : {vloger.Followers} followers, {vloger.Followings} following");
                    count++;
                    
                        
                }
            }
        }
    }
}
