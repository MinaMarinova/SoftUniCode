using System;
using System.Linq;

namespace ParkingFeud
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] inputParking = Console.ReadLine().Split().Select(int.Parse).ToArray();

            int parkRows = inputParking[0];
            int parkCols = inputParking[1];

            int totalDistance = 0;
            int distanceSam = 0;
            int distanceEnemy = 0;

            char[,] matrix = new char[(parkRows * 2) - 1, parkCols + 2];

            int SamEntrence = int.Parse(Console.ReadLine());
            int SamEnterenceMatrix = (SamEntrence * 2) - 1;

            while (true)
            {
                string[] spotsToPark = Console.ReadLine().Split();
                string samSpot = spotsToPark[SamEntrence - 1];

                int enemyCarEntrence = -1;

                for (int i = 0; i < spotsToPark.Length; i++)
                {
                    if (spotsToPark[i] == samSpot)
                    {
                        if (i != SamEntrence - 1)
                        {
                            enemyCarEntrence = i + 1;
                        }
                    }
                }

                
                char spotCol = samSpot[0];
                int spotColMatrix = GetSpotColMatrix(spotCol);

                int spotRow = samSpot[1] - '0';
                int spotRowMatrix = spotRow * 2 - 2;

                if (enemyCarEntrence == -1)
                {
                    int difference = spotRowMatrix - SamEnterenceMatrix;

                    distanceSam = GetDistance(difference, parkCols, matrix, spotColMatrix);
                    totalDistance += distanceSam;
                    Console.WriteLine($"Parked successfully at {samSpot}.");
                    Console.WriteLine($"Total Distance Passed: {totalDistance}");
                    return;
                }
                else
                {
                    int differenceSam = spotRowMatrix - SamEnterenceMatrix;
                    distanceSam = GetDistance(differenceSam, parkCols, matrix, spotColMatrix);

                    int enemyEnterenceMatrix = (enemyCarEntrence * 2) - 1;
                    int differenceEnemy = spotRowMatrix - enemyEnterenceMatrix;
                    distanceEnemy = GetDistanceEnemy(differenceEnemy, parkCols, matrix, spotColMatrix, distanceEnemy);

                    if (distanceEnemy >= distanceSam)
                    {
                        totalDistance += distanceSam;
                        Console.WriteLine($"Parked successfully at {samSpot}.");
                        Console.WriteLine($"Total Distance Passed: {totalDistance}");
                        return;
                    }
                    else
                    {
                        totalDistance = totalDistance + distanceSam * 2;
                        continue;
                    }
                }
            }
        }

        private static int GetDistance(int difference, int parkCols, char[,] matrix, int spotColMatrix)
        {
            int distanceSam = 0;
            int zigzags = Math.Abs(difference / 2);

            for (int i = 0; i < zigzags; i++)
            {
                distanceSam += parkCols + 3;
            }

            if (zigzags % 2 != 0)
            {
                distanceSam += matrix.GetLength(1) - 1 - spotColMatrix;
            }
            else
            {
                distanceSam += spotColMatrix;
            }
            return distanceSam;
        }

        private static int GetDistanceEnemy(int difference, int parkCols, char[,] matrix, int spotColMatrix, int distanceEnemy)
        {
            int zigzags = Math.Abs(difference / 2);

            for (int i = 0; i < zigzags; i++)
            {
                distanceEnemy += parkCols + 3;
            }

            if (zigzags % 2 != 0)
            {
                distanceEnemy += matrix.GetLength(1) - 1 - spotColMatrix;
            }
            else
            {
                distanceEnemy += spotColMatrix;
            }
            return distanceEnemy;
        }

        public static int GetSpotColMatrix(char spotCol)
        {
            switch (spotCol)
            {
                case 'A': return 1;

                case 'B': return 2;

                case 'C': return 3;

                case 'D': return 4;

                case 'E': return 5;

                case 'F': return 6;

                case 'G': return 7;

                case 'H': return 8;

                case 'I': return 9;

                case 'J': return 10;

                default: return 0;
            }
        }
    }
}

