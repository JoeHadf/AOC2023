string input = File.ReadAllText(@"C:\Users\Josep\Documents\AOC2023\Day11Input.txt");

string[] inputLines = input.Split("\r\n");

int inputHeight = inputLines.Length;
int inputWidth = inputLines[0].Length;

List<GalaxyPos> galaxyPositions = new List<GalaxyPos>();

bool[] rowsWithGalaxy = new bool[inputHeight];
bool[] colsWithGalaxy = new bool[inputWidth];

for (int i = 0; i < inputHeight; i++)
{
    for (int j = 0; j < inputWidth; j++)
    {
        if (inputLines[i][j] == '#')
        {
            rowsWithGalaxy[i] = true;
            colsWithGalaxy[j] = true;
            
            galaxyPositions.Add(new GalaxyPos(i, j));
        }
    }
}

for (int i = 0; i < rowsWithGalaxy.Length; i++)
{
    bool rowHasGalaxy = rowsWithGalaxy[i];
    if (!rowHasGalaxy)
    {
        for (int j = 0; j < galaxyPositions.Count; j++)
        {
            GalaxyPos currentGalaxyPos = galaxyPositions[j];
            if (currentGalaxyPos.initialRow > i)
            {
                currentGalaxyPos.rowExpansion++;
                galaxyPositions[j] = currentGalaxyPos;
            }
        }
    }
}

for (int i = 0; i < colsWithGalaxy.Length; i++)
{
    bool colHasGalaxy = colsWithGalaxy[i];
    if (!colHasGalaxy)
    {
        for (int j = 0; j < galaxyPositions.Count; j++)
        {
            GalaxyPos currentGalaxyPos = galaxyPositions[j];
            if (currentGalaxyPos.initialCol > i)
            {
                currentGalaxyPos.colExpansion++;
                galaxyPositions[j] = currentGalaxyPos;
            }
        }
    }
}

long lengthSum = 0;
for (int i = 0; i < galaxyPositions.Count; i++)
{
    GalaxyPos position1 = galaxyPositions[i];
    for (int j = i + 1; j < galaxyPositions.Count; j++)
    {
        GalaxyPos position2 = galaxyPositions[j];
        long rowDiff = Math.Abs(position2.GetRow() - position1.GetRow());
        long colDiff = Math.Abs(position2.GetCol() - position1.GetCol());

        long distance = rowDiff + colDiff;
        lengthSum += distance;
    }
}

Console.WriteLine($"Length Sum : {lengthSum}");

struct GalaxyPos
{
    public int initialRow;
    public int initialCol;

    public int rowExpansion;
    public int colExpansion;

    public GalaxyPos(int row, int col)
    {
        this.initialRow = row;
        this.initialCol = col;
    }

    public long GetRow()
    {
        return initialRow + (999999 * rowExpansion);
    }

    public long GetCol()
    {
        return initialCol + (999999 * colExpansion);
    }
}