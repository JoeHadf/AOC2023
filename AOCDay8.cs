string input = File.ReadAllText(@"C:\Users\Josep\Documents\AOC2023\Day8Input.txt");

string[] splitInput = input.Split("\r\n\r\n");

string directions = splitInput[0];
string allNodesString = splitInput[1];

string[] nodeStrings = allNodesString.Split("\r\n");

Dictionary<string, Tuple<string, string>> nodes = new Dictionary<string, Tuple<string, string>>();

List<string> startingNodes = new List<string>(); 

for (int i = 0; i < nodeStrings.Length; i++)
{
    string currentNodeString = nodeStrings[i];

    string[] nodeData = currentNodeString.Split(new char[] {' ', '=', '(', ',', ')'}, StringSplitOptions.RemoveEmptyEntries);

    string nodeLabel = nodeData[0];
    if (nodeLabel[2] == 'A')
    {
        startingNodes.Add(nodeLabel);
    }
    Tuple<string, string> leftRightPair = new Tuple<string, string>(nodeData[1], nodeData[2]);
    nodes.Add(nodeLabel, leftRightPair);
}

int[] cycleLengths = new int[startingNodes.Count];

for (int i = 0; i < startingNodes.Count; i++)
{
    string currentNode = startingNodes[i];

    int counter = 0;
    bool hasFoundZEnd = false;

    while (!hasFoundZEnd)
    {
        char currentDirection = directions[counter % directions.Length];
        currentNode = (currentDirection == 'L') ? nodes[currentNode].Item1 : nodes[currentNode].Item2;
        
        counter++;

        if (currentNode[2] == 'Z')
        {
            hasFoundZEnd = true;
        }
    }

    cycleLengths[i] = counter;
}

int largestElement = int.MinValue;
for (int i = 0; i < cycleLengths.Length; i++)
{
    if (cycleLengths[i] > largestElement)
    {
        largestElement = cycleLengths[i];
    }
}

long lcm = 1;
for (int div = 2; div <= largestElement; div++)
{
    bool hasDivided = false;
    
    for (int i = 0; i < cycleLengths.Length; i++)
    {
        if (cycleLengths[i] % div == 0)
        {
            cycleLengths[i] /= div;
            hasDivided = true;
        }
    }

    if (hasDivided)
    {
        lcm *= div;
    }
}

Console.WriteLine($"Step Count: {lcm}");

