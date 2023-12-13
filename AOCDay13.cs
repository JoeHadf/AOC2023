string input = File.ReadAllText(@"C:\Users\Josep\Documents\AOC2023\Day13Input.txt");

string[] splitInput = input.Split("\r\n\r\n");

int lineSum = 0;
for (int i = 0; i < splitInput.Length; i++)
{
    string currentData = splitInput[i];
    string[] grid = currentData.Split("\r\n");

    if (HasReflectionLine(grid, out int indexAboveLine))
    {
        lineSum += 100 * (indexAboveLine + 1);
        continue;
    }
    
    string[] horizGrid = CreateHorizontalGrid(grid);

    if (HasReflectionLine(horizGrid, out int indexLeftOfLine))
    {
        lineSum += indexLeftOfLine + 1;
    }
}

Console.WriteLine($"Line Sum : {lineSum}");

bool HasReflectionLine(string[] grid, out int indexBeforeLine)
{
    for (int i = 0; i < grid.Length - 1; i++)
    {
        int placesToCheck = Math.Min(i + 1, grid.Length - i - 1);

        bool hasFoundSmudge = false;
        bool hasFoundDifference = false;

        for (int j = 0; j < placesToCheck; j++)
        {
            string aboveLine = grid[i - j];
            string belowLine = grid[i + j + 1];

            int hammingDistance = GetHammingDistance(aboveLine, belowLine);
            
            if (hammingDistance == 1)
            {
                if (hasFoundSmudge)
                {
                    hasFoundDifference = true;
                    break;
                }
                else
                {
                    hasFoundSmudge = true;
                }
            }
            else if(hammingDistance > 1)
            {
                hasFoundDifference = true;
                break;
            }
            
        }

        if (!hasFoundDifference && hasFoundSmudge)
        {
            indexBeforeLine = i;
            return true;
        }
    }

    indexBeforeLine = -1;
    return false;
}

int GetHammingDistance(string string1, string string2)
{
    int distance = 0;
    for (int i = 0; i < string1.Length; i++)
    {
        if (string1[i] != string2[i])
        {
            distance++;
        }
    }

    return distance;
}

string[] CreateHorizontalGrid(string[] grid)
{
    string[] horizontalGrid = new string[grid[0].Length];
    
    for (int i = 0; i < grid[0].Length; i++)
    {
        string currentLine = "";

        for (int j = 0; j < grid.Length; j++)
        {
            currentLine += grid[j][i];
        }

        horizontalGrid[i] = currentLine;
    }

    return horizontalGrid;
}