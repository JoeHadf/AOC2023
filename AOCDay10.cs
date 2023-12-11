string input = File.ReadAllText(@"C:\Users\Josep\Documents\AOC2023\Day10Input.txt");

string[] inputLines = input.Split("\r\n");

Pipe sPipe = FindSPipe();
(Pipe adjacentPipe1, Pipe adjacentPipe2) = GetAdjacentPipes(sPipe);
char sType = DetermineSPipeType(sPipe, adjacentPipe1, adjacentPipe2);

inputLines[sPipe.Row] = inputLines[sPipe.Row].Replace('S', sType);
sPipe.type = sType;

HashSet<Pipe> pipeSystem = new HashSet<Pipe>();

pipeSystem.Add(sPipe);

Pipe currentPipeInSystem = adjacentPipe1;
Pipe previousPipeInSystem = sPipe;
bool hasFoundS = false;

while (!hasFoundS)
{
    pipeSystem.Add(currentPipeInSystem);
    
    (PipePosition nextPos1, PipePosition nextPos2) = currentPipeInSystem.GetNextPipePositions();

    Pipe newPreviousPipe = currentPipeInSystem;

    if (!nextPos1.Equals(previousPipeInSystem.position))
    {
        currentPipeInSystem = GetPipeAtPosition(nextPos1);
    }
    else
    {
        currentPipeInSystem = GetPipeAtPosition(nextPos2);
    }
    previousPipeInSystem = newPreviousPipe;
        
    hasFoundS = currentPipeInSystem.Equals(sPipe);
}

int pipesInside = 0;

for (int i = 0; i < inputLines.Length; i++)
{
    bool isTopHalfInside = false;
    bool isBottomHalfInside = false;
    
    for (int j = 0; j < inputLines[i].Length; j++)
    {
        Pipe currentPipe = GetPipeAtIndex(i, j);
        
        if (pipeSystem.Contains(currentPipe))
        {
            (bool topBar, bool botBar) = currentPipe.GetBars();

            if (topBar)
            {
                isTopHalfInside = !isTopHalfInside;
            }

            if (botBar)
            {
                isBottomHalfInside = !isBottomHalfInside;
            }
        }
        else if (isTopHalfInside && isBottomHalfInside)
        {
            pipesInside++;
        }
    }
}

Console.WriteLine($"Pipes Inside: {pipesInside}");

Pipe FindSPipe()
{
    for (int i = 0; i < inputLines.Length; i++)
    {
        for (int j = 0; j < inputLines[i].Length; j++)
        {
            if (inputLines[i][j] == 'S')
            {
                return new Pipe(i, j, 'S');
            }
        }
    }

    return new Pipe();
}

(Pipe adjacent1, Pipe adjacent2) GetAdjacentPipes(Pipe s)
{
    bool foundPipe1 = false;
    Pipe adjacent1 = new Pipe();
    Pipe adjacent2 = new Pipe();
    
    for (int i = -1; i <= 1; i++)
    {
        for (int j = -1; j <= 1; j++)
        {
            if(i != 0 || j != 0)
            {
                Pipe pipe = GetPipeAtIndex(s.Row + i, s.Col + j);
            
                if (pipe.IsPipe())
                {
                    (PipePosition nextPipe1, PipePosition nextPipe2) = pipe.GetNextPipePositions();

                    if ((nextPipe1.Equals(s.position)) || nextPipe2.Equals(s.position))
                    {
                        if (!foundPipe1)
                        {
                            adjacent1 = pipe;
                            foundPipe1 = true;
                        }
                        else
                        {
                            adjacent2 = pipe;
                        }
                    }
                }
            }
        }
    }

    return (adjacent1, adjacent2);
}

char DetermineSPipeType(Pipe s, Pipe adjacent1, Pipe adjacent2)
{
    PipePosition displacement1 = adjacent1.position - s.position;
    PipePosition displacement2 = adjacent2.position - s.position;

    PipePosition sum = displacement1 + displacement2;

    char type = '.';

    if (sum.row == 0 && sum.col == 0)
    {
        type = (displacement1.row == 0) ? '-' : '|';
    }
    else if (sum.row == -1 && sum.col == -1)
    {
        type = 'J';
    }
    else if (sum.row == -1 && sum.col == 1)
    {
        type = 'L';
    }
    else if (sum.row == 1 && sum.col == -1)
    {
        type = '7';
    }
    else if (sum.row == 1 && sum.col == 1)
    {
        type = 'F';
    }

    return type;
}

Pipe GetPipeAtIndex(int row, int col)
{
    return new Pipe(row, col, inputLines[row][col]);
}

Pipe GetPipeAtPosition(PipePosition position)
{
    return GetPipeAtIndex(position.row, position.col);
}

struct Pipe
{
    public PipePosition position;
    public char type;
    public int Row => position.row;
    public int Col => position.col;

    public Pipe(PipePosition position, char type)
    {
        this.position = position;
        this.type = type;
    }

    public Pipe(int row, int col, char type) : this(new PipePosition(row, col) ,type) { }
    
    public Pipe() : this(0,0,'.') { }
    
    public bool IsPipe()
    {
        switch (type)
        {
            case '|':
            case '-':
            case 'L':
            case 'J':
            case '7':
            case 'F':
                return true;
            default:
                return false;
        }
    }
    
    public (PipePosition nextPipe1, PipePosition nextPipe2) GetNextPipePositions()
    {
        (PipePosition displacement1, PipePosition displacement2) = GetPipeDisplacements();
        return (position + displacement1, position + displacement2);
    }
    
    private (PipePosition displacement1, PipePosition displacement2) GetPipeDisplacements()
    {
        PipePosition displacement1;
        PipePosition displacement2;
        
        switch (type)
        {
            case '|':
                displacement1 = new PipePosition(-1, 0);
                displacement2 = new PipePosition(1, 0);
                break;
            case '-':
                displacement1 = new PipePosition(0, -1);
                displacement2 = new PipePosition(0, 1);
                break;
            case 'L':
                displacement1 = new PipePosition(0, 1);
                displacement2 = new PipePosition(-1, 0);
                break;
            case 'J':
                displacement1 = new PipePosition(0, -1);
                displacement2 = new PipePosition(-1, 0);
                break;
            case '7':
                displacement1 = new PipePosition(0, -1);
                displacement2 = new PipePosition(1, 0);
                break;
            case 'F':
                displacement1 = new PipePosition(0, 1);
                displacement2 = new PipePosition(1, 0);
                break;
            default:
                displacement1 = new PipePosition(0, 0);
                displacement2 = new PipePosition(0, 0);
                break;
        }

        return (displacement1, displacement2);
    }

    public (bool topBar, bool botBar) GetBars()
    {
        switch (type)
        {
            case '|':
                return (true, true);
            case '-':
                return (false, false);
            case 'L':
            case 'J':
                return (true, false);
            case '7':
            case 'F':
                return (false, true);
            default:
                return (false, false);
        }
    }  
    public override bool Equals(object? obj)
    {
        if (obj is Pipe otherPipe)
        {
            return Equals(otherPipe);
        }

        return false;
    }

    public bool Equals(Pipe otherPipe)
    {
        return position.Equals(otherPipe.position) && type == otherPipe.type;
    }

    public override int GetHashCode()
    {
        return (position, type).GetHashCode();
    }
}

struct PipePosition
{
    public int row;
    public int col;
    
    public PipePosition(int row, int col)
    {
        this.row = row;
        this.col = col;
    }

    public static PipePosition operator +(PipePosition position1, PipePosition position2)
    {
        return new PipePosition(position1.row + position2.row, position1.col + position2.col);
    }

    public static PipePosition operator -(PipePosition position1, PipePosition position2)
    {
        return new PipePosition(position1.row - position2.row, position1.col - position2.col);
    }

    public override bool Equals(object? obj)
    {
        if (obj is PipePosition otherPosition)
        {
            return Equals(otherPosition);
        }

        return false;
    }

    public bool Equals(PipePosition otherPosition)
    {
        return row == otherPosition.row && col == otherPosition.col;
    }
    
    public override int GetHashCode()
    {
        return (row, col).GetHashCode();
    }
}
