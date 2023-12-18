string[] input = File.ReadAllLines(@"C:\Users\Josep\Documents\AOC2023\Day18Input.txt");

long rowDiff = 0;
long colDiff = 0;

long boundary = 0;

List<Tuple<long, long>> corners = new List<Tuple<long, long>>();

for (int i = 0; i < input.Length; i++)
{
    string inputLine = input[i];
    string[] lineData = inputLine.Split(" ");

    char direction = lineData[2][7];
    long distance = Convert.ToInt64(lineData[2].Substring(2,5), 16);

    boundary += distance;

    for (int j = 0; j < distance; j++)
    {
        switch (direction)
        {
            case '3':
                rowDiff--;
                break;
            case '1':
                rowDiff++;
                break;
            case '2':
                colDiff--;
                break;
            case '0':
                colDiff++;
                break;
        }
    }
    corners.Add(new Tuple<long, long>(rowDiff, colDiff));
}

long twoArea = 0;

for (int i = 0; i < corners.Count; i++)
{
    Tuple<long, long> currentCorner = corners[i];
    Tuple<long, long> nextCorner = corners[(i + 1) % corners.Count];

    long determinant = currentCorner.Item1 * nextCorner.Item2 - currentCorner.Item2 * nextCorner.Item1;
    twoArea -= determinant;
}

double area = (double)twoArea / 2;

double interior = area - ((double)boundary / 2) + 1;

Console.WriteLine($"Hole Size : {interior + boundary}");