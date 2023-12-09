string input = File.ReadAllText(@"C:\Users\Josep\Documents\AOC2023\Day9Input.txt");

string[] sequenceStrings = input.Split("\r\n");

int extrapolationSum = 0;

for (int i = 0; i < sequenceStrings.Length; i++)
{
    string[] currentStringSequence = sequenceStrings[i].Split(" ");

    List<int> currentSequence = new List<int>();

    for (int j = 0; j < currentStringSequence.Length; j++)
    {
        currentSequence.Add(Int32.Parse(currentStringSequence[j]));
    }

    List<List<int>> differences = new List<List<int>>();
    differences.Add(currentSequence);

    bool allZeros = false;
    int differencesIndex = 0;

    while (!allZeros)
    {
        List<int> currentDifferences = differences[differencesIndex];

        List<int> nextDifferences = new List<int>();

        bool foundNonZero = false;

        for (int j = 0; j < currentDifferences.Count - 1; j++)
        {
            int currentValue = currentDifferences[j];
            int nextValue = currentDifferences[j + 1];

            int difference = nextValue - currentValue;
            
            if (difference != 0)
            {
                foundNonZero = true;
            }
            
            nextDifferences.Add(difference);
        }
        
        differences.Add(nextDifferences);

        allZeros = !foundNonZero;

        differencesIndex++;
    }
    
    differences[^1].Insert(0, 0);

    for (int j = differences.Count - 2; j >= 0; j--)
    {
        int nextValue = differences[j][0] - differences[j + 1][0];

        differences[j].Insert(0,nextValue);
    }

    extrapolationSum += differences[0][0];
}

Console.WriteLine($"Extrapolation Sum {extrapolationSum}");
