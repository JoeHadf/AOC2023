string input = File.ReadAllText(@"C:\Users\Josep\Documents\AOC2023\Day7Input.txt");

string[] inputLines = input.Split("\n");

Dictionary<HandType, List<Tuple<string, int>>> sortedLines = new Dictionary<HandType, List<Tuple<string, int>>>();

for(int i = 0; i <= 6; i++)
{
    sortedLines[(HandType)i] = new List<Tuple<string, int>>();
}

for (int i = 0; i < inputLines.Length; i++)
{
    string[] handBidStrings = inputLines[i].Split(" ");
    string hand = handBidStrings[0];
    string bidString = handBidStrings[1];
    int bid = Int32.Parse(bidString);

    string convertedHand = ConvertHand(hand);

    Tuple<string, int> handBidPair = new Tuple<string, int>(convertedHand, bid);
    HandType handType = DetermineHandType(hand);
    
    sortedLines[handType].Add(handBidPair);
}

long winnings = 0;
int rank = 1;

for (int i = 0; i <= 6; i++)
{
    HandType currentHandType = (HandType)i;

    List<Tuple<string, int>> handsOfType = sortedLines[currentHandType];

    Tuple<string, int>[] orderedHands = handsOfType.OrderBy(x => x.Item1).ToArray();

    for (int j = 0; j < orderedHands.Length; j++)
    {
        winnings += orderedHands[j].Item2 * rank;
        rank++;
    }
}

Console.WriteLine($"Winnings {winnings}");

HandType DetermineHandType(string hand)
{
    Dictionary<char, int> characterCounts = new Dictionary<char, int>();

    for (int i = 0; i < hand.Length; i++)
    {
        char currentChar = hand[i];

        if (characterCounts.ContainsKey(currentChar))
        {
            characterCounts[currentChar]++;
        }
        else
        {
            characterCounts.Add(currentChar,1);
        }
    }

    if (characterCounts.TryGetValue('J', out int jCount))
    {
        characterCounts.Remove('J');

        if (characterCounts.Count == 0)
        {
            characterCounts.Add('A', 5);
        }
        else
        {
            char currentMax = ' ';
        
            foreach (KeyValuePair<char, int> pair in characterCounts)
            {
                if (currentMax == ' ')
                {
                    currentMax = pair.Key;
                }
                else if(characterCounts[currentMax] < characterCounts[pair.Key])
                {
                    currentMax = pair.Key;
                }
            }

            characterCounts[currentMax] += jCount;
        }
    }

    Dictionary<char, int>.ValueCollection characterCountsValues = characterCounts.Values;
    int[] duplicateValues = characterCountsValues.Order().ToArray();
    int duplicateCount = duplicateValues.Length;

    switch (duplicateCount)
    {
        case 1:
            return HandType.FiveOfAKind;
        case 2:
            if (duplicateValues[0] == 1)
            {
                return HandType.FourOfAKind;
            }
            return HandType.FullHouse;
        case 3:
            if (duplicateValues[1] == 2)
            {
                return HandType.TwoPair;
            }
            return HandType.ThreeOfAKind;
        case 4:
            return HandType.OnePair;
        default:
            return HandType.HighCard;
    }
}

string ConvertHand(string hand)
{
    string converted = "";

    for (int i = 0; i < hand.Length; i++)
    {
        converted += GetAltChar(hand[i]);
    }

    return converted;
}

char GetAltChar(char cardChar)
{
    switch (cardChar)
    {
        case 'A':
            return 'M';
        case 'K':
            return 'L';
        case 'Q':
            return 'K';
        case 'J':
            return 'A';
        case 'T':
            return 'J';
        case '9':
            return 'I';
        case '8':
            return 'H';
        case '7':
            return 'G';
        case '6':
            return 'F';
        case '5':
            return 'E';
        case '4':
            return 'D';
        case '3':
            return 'C';
        default:
            return 'B';
    }
}

enum HandType
{
    HighCard = 0,
    OnePair = 1,
    TwoPair = 2,
    ThreeOfAKind = 3,
    FullHouse = 4,
    FourOfAKind = 5,
    FiveOfAKind = 6
}