string[] input = File.ReadAllLines(@"C:\Users\Josep\Documents\AOC2023\Day14Input.txt");

char[,] grid = new char[input.Length, input[0].Length];

for (int i = 0; i < input.Length; i++)
{
    for (int j = 0; j < input[i].Length; j++)
    {
        grid[i, j] = input[i][j];
    }
}

Dictionary<string, long> gridEncounters = new Dictionary<string, long> { { ConvertToString(grid), 0 } };

long numberOfCycles = 1000000000;
bool foundRepeat = false;

for (long i = 1; i <= numberOfCycles; i++)
{
    Console.WriteLine(i);
    RollNorth();
    RollWest();
    RollSouth();
    RollEast();

    string gridString = ConvertToString(grid);

    if (!foundRepeat)
    {
        if (gridEncounters.ContainsKey(gridString))
        {
            foundRepeat = true;
            long firstEncounter = gridEncounters[gridString];

            long cycleLength = i - firstEncounter;
            long quotient = (numberOfCycles - i) / cycleLength;

            long closestToCycleCount = (quotient * cycleLength) + i;

            i = closestToCycleCount;
        }
        else
        {
            gridEncounters.Add(gridString, i);
        }
    }
}

int load = CalculateLoad();

Console.WriteLine($"Total Load : {load}");

void RollNorth()
{
    Roll(false, false);
}

void RollSouth()
{
    Roll(false, true);
}

void RollEast()
{
    Roll(true, true);
}

void RollWest()
{
    Roll(true, false);
}

void Roll(bool rows, bool backwards)
{
    int iRange = grid.GetLength(1);
    int jRange = grid.GetLength(0);
    
    if (rows)
    {
        iRange = grid.GetLength(0);
        jRange = grid.GetLength(1);
    }
    
    for (int i = 0; i < iRange; i++)
    {
        int iIndex = i;
        if (backwards)
        {
            iIndex = iRange - 1 - i;
        }

        int blockStart = (backwards) ? jRange : -1;
        int blockSize = 0;
        int rockCount = 0;

        string line = "";
    
        for (int j = 0; j < jRange; j++)
        {
            int jIndex = j;
            if (backwards)
            {
                jIndex = jRange - 1 - j;
            }
            
            char currentPlace = grid[jIndex,iIndex];
            if (rows)
            {
                currentPlace = grid[iIndex,jIndex];
            }
            
            if (currentPlace != '#')
            {
                blockSize ++;
                if (currentPlace == 'O')
                {
                    rockCount++;
                }
            }
            else
            {
                if (blockSize > 0)
                {
                    UpdateBlock(blockStart, iIndex, rockCount, blockSize, rows, backwards);

                    rockCount = 0;
                    blockSize = 0;
                }

                blockStart = jIndex;
            }
        }

        if (blockSize > 0)
        {
            UpdateBlock(blockStart, iIndex, rockCount, blockSize, rows, backwards);
        }
    }
}

void UpdateBlock(int blockStart, int staticIndex, int rockCount, int size, bool rows, bool backwards)
{
    int counter = 0;
    for (int i = 0; i < size ; i++)
    {
        int iIndex = (backwards) ? blockStart - 1 - i : blockStart + 1 + i;
        if (rows)
        {
            if (counter < rockCount)
            {
                grid[staticIndex, iIndex] = 'O';
                counter++;
            }
            else
            {
                grid[staticIndex, iIndex] = '.';
            }
        }
        else
        {
            if (counter < rockCount)
            {
                grid[iIndex, staticIndex] = 'O';
                counter++;
            }
            else
            {
                grid[iIndex, staticIndex] = '.';
            }
        }
    }
}

int CalculateLoad()
{
    int totalLoad = 0;
    
    for (int i = 0; i < grid.GetLength(0); i++)
    {
        for (int j = 0; j < grid.GetLength(1); j++)
        {
            if (grid[i,j] == 'O')
            {
                totalLoad += grid.GetLength(0) - i;
            }
        }
    }

    return totalLoad;
}

string ConvertToString(char[,] chars)
{
    string gridString = "";
    for (int i = 0; i < chars.GetLength(0); i++)
    {
        for (int j = 0; j < chars.GetLength(1); j++)
        {
            gridString += chars[i, j];
        }
    }

    return gridString;
}