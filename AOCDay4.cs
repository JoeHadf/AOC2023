string input = File.ReadAllText(@"C:\Users\Josep\Documents\AOC2023\Day4Input.txt");

string[] cards = input.Split("\n");

int[] cardQuantities = new int[cards.Length];

for (int i = 0; i < cardQuantities.Length; i++)
{
    cardQuantities[i] = 1;
}

for (int i = 0; i < cards.Length; i++)
{
    int currentCardQuantity = cardQuantities[i];
    int cardWinnings = 0;
    
    string card = cards[i];
    string cardData = card.Split(":")[1];
    string[] splitNums = cardData.Split("|");

    string winningString = splitNums[0];
    string yourNumsString = splitNums[1];

    string[] splitWinningString = winningString.Split(" ", StringSplitOptions.RemoveEmptyEntries);
    string[] splitYourNumsString = yourNumsString.Split(" ", StringSplitOptions.RemoveEmptyEntries);

    int[] winningNumbers = new int[splitWinningString.Length];
    for (int j = 0; j < winningNumbers.Length; j++)
    {
        winningNumbers[j] = Int32.Parse(splitWinningString[j]);
    }

    for (int j = 0; j < splitYourNumsString.Length; j++)
    {
        int currentNumber = Int32.Parse(splitYourNumsString[j]);

        if (winningNumbers.Contains(currentNumber))
        {
            cardWinnings++;
        }
    }

    for (int j = 0; j < cardWinnings; j++)
    {
        cardQuantities[i + j + 1] += currentCardQuantity;
    }
}

int totalCards = 0;

for (int i = 0; i < cardQuantities.Length; i++)
{
    totalCards += cardQuantities[i];
}

Console.Write($"Total Cards {totalCards}");