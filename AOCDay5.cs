var watch = new System.Diagnostics.Stopwatch();
watch.Start();

string text = File.ReadAllText(@"C:\Users\Josep\Documents\AOC2023\Day5Input.txt");

string[] mapSets = text.Split("\r\n\r\n");

string allSeedsString = mapSets[0].Split(":")[1];

string[] seedStrings = allSeedsString.Split(" ", StringSplitOptions.RemoveEmptyEntries);

List<long[]>[] mapSetData = new List<long[]>[mapSets.Length - 1];

for (int i = 1; i < mapSets.Length; i++)
{
    string currentMapSet = mapSets[i];
    string mapSetStringData = currentMapSet.Split(":")[1];
    string[] mapStrings = mapSetStringData.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

    mapSetData[i - 1] = new List<long[]>();
    for (int j = 0; j < mapStrings.Length; j++)
    {
        string currentMapString = mapStrings[j];
        string[] dataStrings = currentMapString.Split(" ");

        long destinationStart = long.Parse(dataStrings[0]);
        long sourceStart = long.Parse(dataStrings[1]);
        long sourceRange = long.Parse(dataStrings[2]);

        long sourceEnd = sourceStart + sourceRange - 1;

        long[] mapData = new long[]
        {
            destinationStart,
            sourceStart,
            sourceEnd
        };
        
        mapSetData[i-1].Add(mapData);
    }
}

List<long[]> sourceIntervals = new List<long[]>();

int numberOfSeedIntervals = seedStrings.Length / 2;

for (int i = 0; i < numberOfSeedIntervals; i++)
{
    string seedStartString = seedStrings[2 * i];
    string seedRangeString = seedStrings[2 * i + 1];

    long seedStart = long.Parse(seedStartString);
    long seedRange = long.Parse(seedRangeString);
    long seedEnd = seedStart + seedRange - 1;
    

    sourceIntervals.Add(new long[]{seedStart, seedEnd});
}

for (int i = 0; i < mapSetData.Length; i++)
{
    List<long[]> currentMapSet = mapSetData[i];
    
    List<long[]> destinationIntervals = new List<long[]>();
    
    for (int j = 0; j < sourceIntervals.Count; j++)
    {
        long[] currentInterval = sourceIntervals[j];

        List<long[]> leftovers = new List<long[]>();
        leftovers.Add(new long[]{currentInterval[0], currentInterval[1]});

        for (int k = 0; k < currentMapSet.Count; k++)
        {
            long[] currentMap = currentMapSet[k];

            long destinationStart = currentMap[0]; 
            long sourceStart = currentMap[1];
            long sourceEnd = currentMap[2];
            
            long[] sourceInterval = new long[] { sourceStart, sourceEnd };
            
            if (IntervalsOverlap(currentInterval, sourceInterval, out long[] overlapInterval))
            {
                long overlapStart = overlapInterval[0];
                long overlapEnd = overlapInterval[1];
                long overlapRange = overlapEnd - overlapStart;
                    
                long difference = overlapStart - sourceStart;

                long destIntStart = destinationStart + difference;
                long destIntEnd = destIntStart + overlapRange;
                
                destinationIntervals.Add(new long[]{destIntStart, destIntEnd});

                for (int l = 0; l < leftovers.Count; l++)
                {
                    long[] currentLeftoverInterval = leftovers[l];

                    if (IntervalsOverlap(overlapInterval, currentLeftoverInterval, out long[] overlapToRemove))
                    {
                        leftovers.RemoveAt(l);

                        long leftoverStart = currentLeftoverInterval[0];
                        long leftoverEnd = currentLeftoverInterval[1];

                        long toRemoveStart = overlapToRemove[0];
                        long toRemoveEnd = overlapToRemove[1];

                        if (leftoverStart == toRemoveStart && leftoverEnd == toRemoveEnd)
                        {
                            
                        }
                        else if (leftoverStart == toRemoveStart)
                        {
                            leftovers.Add(new long[]{toRemoveEnd + 1, leftoverEnd});
                        }
                        else if(leftoverEnd == toRemoveEnd)
                        {
                            leftovers.Add(new long[]{leftoverStart, toRemoveStart - 1});
                        }
                        else
                        {
                            leftovers.Add(new long[]{leftoverStart, toRemoveStart - 1});
                            leftovers.Add(new long[]{toRemoveEnd + 1, leftoverEnd});
                        }
                    }
                }
            }
        }
        
        destinationIntervals.AddRange(leftovers);
    }

    sourceIntervals = destinationIntervals;
}

long currentLowestLocation = long.MaxValue;

for (int i = 0; i < sourceIntervals.Count; i++)
{
    long intervalStart = sourceIntervals[i][0];
    if (intervalStart < currentLowestLocation)
    {
        currentLowestLocation = intervalStart;
    }
}
               

Console.WriteLine($"Lowest Location {currentLowestLocation}");

watch.Stop();

Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

bool IntervalsOverlap(long[] int1, long[] int2, out long[] overlapInt)
{
    long int1Start = int1[0];
    long int1End = int1[1];

    long int2Start = int2[0];
    long int2End = int2[1];

    if (int2End - int2Start < 0 || int1End - int1Start < 0)
    {
        overlapInt = Array.Empty<long>();
        return false;
    }

    if (int1Start <= int2Start && int2Start <= int1End)
    {
        overlapInt = new long[] { int2Start, Math.Min(int1End, int2End) };
        return true;
    }

    if (int2Start <= int1Start && int1Start <= int2End)
    {
        overlapInt = new long[] { int1Start, Math.Min(int1End, int2End) };
        return true;
    }

    overlapInt = Array.Empty<long>();
    return false;
}