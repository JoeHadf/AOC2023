using System.Text;

string input = File.ReadAllText(@"C:\Users\Josep\Documents\AOC2023\Day15Input.txt");

input = input.Replace("\n", "");

string[] splitInput = input.Split(",");

Dictionary<int, List<Lens>> boxes = new Dictionary<int, List<Lens>>(256);

for (int i = 0; i < splitInput.Length; i++)
{
    string currentStep = splitInput[i];

    (string label, char operation, int focalLength) = SplitStep(currentStep);

    int hash = GetHash(label);

    if (!boxes.ContainsKey(hash))
    {
        boxes[hash] = new List<Lens>();
    }

    if (operation == '-')
    {
        List<Lens> lenses = boxes[hash];
        for (int j = 0; j < lenses.Count; j++)
        {
            Lens lens = lenses[j];

            if (lens.label == label)
            {
                boxes[hash].RemoveAt(j);
                break;
            }
        }
    }
    else if (operation == '=')
    {
        List<Lens> lenses = boxes[hash];

        bool lensFound = false;
        for (int j = 0; j < lenses.Count; j++)
        {
            Lens lens = lenses[j];

            if (lens.label == label)
            {
                boxes[hash][j] = new Lens(label, focalLength);
                lensFound = true;
                break;
            }
        }

        if (!lensFound)
        {
            boxes[hash].Add(new Lens(label, focalLength));
        }
    }
}

int totalFocusingPower = 0;

foreach (KeyValuePair<int, List<Lens>> box in boxes)
{
    List<Lens> lenses = box.Value;
    for (int i = 0; i < lenses.Count; i++)
    {
        totalFocusingPower += (box.Key + 1) * (i + 1) * (lenses[i].focalLength);
    }
}

Console.WriteLine($"Total Focusing Power: {totalFocusingPower}");

(string label, char operation, int focalLength) SplitStep(string step)
{
    if (step[^1] == '-')
    {
        return (step.Substring(0, step.Length - 1), '-', -1);
    }

    string[] data = step.Split("=");

    return (data[0], '=', Int32.Parse(data[1]));
}

int GetHash(string label)
{
    byte[] asciiValues = Encoding.ASCII.GetBytes(label);
    int currentHash = 0;

    for (int j = 0; j < asciiValues.Length; j++)
    {
        int currentAscii = asciiValues[j];
        currentHash += currentAscii;
        currentHash *= 17;
        currentHash %= 256;
    }

    return currentHash;
}

struct Lens
{
    public string label;
    public int focalLength;

    public Lens(string label, int focalLength)
    {
        this.label = label;
        this.focalLength = focalLength;
    }
}