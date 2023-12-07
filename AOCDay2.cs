string input = File.ReadAllText(@"C:\Users\Josep\Documents\AOC2023\Day2Input.txt");

char[] subsetSeparators = new char[] { ':', ';' };

StringReader stringReader = new StringReader(input);

int powerSum = 0;

for (string game = stringReader.ReadLine(); game != null; game = stringReader.ReadLine())
{
    bool gameIsInvalid = false;
    string[] cubeSubsets = game.Split(subsetSeparators);

    Dictionary<string, int> stringToMinValue = new Dictionary<string, int>()
    {
        { "red", 0 },
        { "green", 0 },
        { "blue", 0 }
    };

    for (int i = 1; i < cubeSubsets.Length; i++)
    {
        string currentGame = cubeSubsets[i];

        string[] ballValuePairs = currentGame.Split(",");

        for (int j = 0; j < ballValuePairs.Length; j++)
        {
            string currentPair = ballValuePairs[j];

            string[] valuesAndBalls = currentPair.Split(" ");

            string valueString = valuesAndBalls[1];
            string ball = valuesAndBalls[2];

            int value = Int32.Parse(valueString);

            if (value > stringToMinValue[ball])
            {
                stringToMinValue[ball] = value;
            }
        }
    }

    Dictionary<string, int>.ValueCollection valueCollection = stringToMinValue.Values;
    int power = 1;
    foreach (int value in valueCollection)
    {
        power *= value;
    }

    powerSum += power;
}

Console.WriteLine($"PowerSum: {powerSum}");