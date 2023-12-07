string input = File.ReadAllText(@"C:\Users\Josep\Documents\AOC2023\Day3Input.txt");

string[] lines = input.Split("\n");

int ratioSum = 0;

for (int currentLine = 0; currentLine < lines.Length; currentLine++)
{
    for (int currentPos = 0; currentPos < lines[currentLine].Length; currentPos++)
    {
        Char currentChar = lines[currentLine][currentPos];

        if (currentChar == '*')
        {
            ratioSum += GetGearRatio(currentLine, currentPos);
        }
    }
}

Console.WriteLine($"Ratio Sum : {ratioSum}");

int GetGearRatio(int starLine, int starPos)
{
    int ratio = 0;
    
    List<int> foundNumbers = new List<int>();

    int currentLine = starLine;
    int currentPos = starPos - 1;
    
    if (IsValidAndDigit(currentLine, currentPos))
    {
        foundNumbers.Add(ExtrapolateNumber(currentLine, currentPos, out int firstPos, out int lastPos));
    }

    currentPos = starPos + 1;

    if (IsValidAndDigit(currentLine, currentPos))
    {
        foundNumbers.Add(ExtrapolateNumber(currentLine, currentPos, out int firstPos,out int lastPos));
    }

    List<int> topCapNumbers = GetNumbersFromCap(starLine - 1, starPos);

    List<int> bottomCapNumbers = GetNumbersFromCap(starLine + 1, starPos);

    foreach (int newNum in topCapNumbers)
    {
        foundNumbers.Add(newNum);
    }

    foreach (int newNum in bottomCapNumbers)
    {
        foundNumbers.Add(newNum);
    }

    if (foundNumbers.Count == 2)
    {
        ratio = foundNumbers[0] * foundNumbers[1];
    }

    return ratio;
}

List<int> GetNumbersFromCap(int capLine, int starPos)
{
    List<int> capNumbers = new List<int>();
    
    if (IsValidAndDigit(capLine, starPos - 1))
    {
        capNumbers.Add(ExtrapolateNumber(capLine, starPos - 1, out int firstPos,out int lastPos));

        if (lastPos == starPos)
        {
            if(IsValidAndDigit(capLine, starPos + 1))
            {
                capNumbers.Add(ExtrapolateNumber(capLine, starPos + 1, out int first,out int last));
            }
        }
    }
    else if(IsValidAndDigit(capLine, starPos + 1))
    {
        capNumbers.Add(ExtrapolateNumber(capLine, starPos + 1, out int firstPos,out int lastPos));
        
    }
    else if (IsValidAndDigit(capLine, starPos))
    {
        capNumbers.Add(ExtrapolateNumber(capLine, starPos, out int firstPos,out int lastPos));
    }

    return capNumbers;
}

int ExtrapolateNumber(int line, int pos,out int firstPos, out int lastPos)
{
    int last = pos;

    while (last < lines[line].Length && Char.IsDigit(lines[line][last]))
    {
        last++;
    }
    
    int first = pos;

    while (first >= 0 && Char.IsDigit(lines[line][first]))
    {
        first--;
    }

    string numberString = lines[line].Substring(first + 1, last - (first + 1));
    int foundNumber = Int32.Parse(numberString);
    lastPos = last;
    firstPos = first;
    return foundNumber;
}

bool IsValidAndDigit(int lineIndex, int stringIndex)
{
    if (IsValidIndex(lineIndex, stringIndex))
    {
        if (Char.IsDigit(lines[lineIndex][stringIndex]))
        {
            return true;
        }
    }

    return false;
}

bool IsValidIndex(int lineIndex, int stringIndex)
{
    bool lineValidBounds = 0 <= lineIndex && lineIndex < lines.Length;
    bool stringValidBounds = lineValidBounds && 0 <= stringIndex && stringIndex < lines[lineIndex].Length;
    return stringValidBounds;
}
