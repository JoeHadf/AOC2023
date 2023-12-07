string input = File.ReadAllText(@"C:\Users\Josep\Documents\AOC2023\Day1Input.txt");

Dictionary<string, char> stringNumberDictionary = new Dictionary<string, char>()
{
    {"one", '1'},
    {"two", '2'},
    {"three", '3'},
    {"four", '4'},
    {"five", '5'},
    {"six", '6'},
    {"seven", '7'},
    {"eight", '8'},
    {"nine", '9'}
};

StringReader stringReader = new StringReader(input);

int callibrationSum = 0;

for (string line = stringReader.ReadLine(); line != null; line = stringReader.ReadLine())
{
    bool hasFoundFirst = false;
    char firstNumber = ' ';
    char lastNumber = ' ';

    for (int i = 0; i < line.Length; i++)
    {
        if (TryGetNumberFromPosition(i, line, out char numberChar))
        {
            lastNumber = numberChar;

            if (!hasFoundFirst)
            {
                hasFoundFirst = true;
                firstNumber = numberChar;
            }
        }
    }

    char[] callibrationArray = new char[] { firstNumber, lastNumber };
    string callibrationString = new string(callibrationArray);
    int callibrationValue = Int32.Parse(callibrationString);

    callibrationSum += callibrationValue;
}

Console.WriteLine($"CallibrationSum : {callibrationSum}");

bool TryGetNumberFromPosition(int position, string line, out char numberChar)
{
    char charAtPos = line[position];
    if (Char.IsDigit(charAtPos))
    {
        numberChar = charAtPos;
        return true;
    }

    string substring = line.Substring(position);

    foreach (string numberString in stringNumberDictionary.Keys)
    {
        if (substring.StartsWith(numberString))
        {
            numberChar = stringNumberDictionary[numberString];
            return true;
        }
    }

    numberChar = ' ';
    return false;
}