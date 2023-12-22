string[] input = File.ReadAllLines(@"C:\Users\Josep\Documents\AOC2023\Day22Input.txt");

BrickStack stack = new BrickStack();

for (int i = 0; i < input.Length; i++)
{
    string[] vectors = input[i].Split("~");

    string[] startString = vectors[0].Split(",");
    string[] endString = vectors[1].Split(",");

    int[] start = new int[3];
    int[] end = new int[3];

    for (int j = 0; j < 3; j++)
    {
        start[j] = Int32.Parse(startString[j]);
        end[j] = Int32.Parse(endString[j]);
    }

    Brick currentBrick = new Brick(i, new Vector(start), new Vector(end));
    stack.Add(currentBrick);
}

stack.FallStack();

int fallenBricksSum = 0;

foreach (Brick brick in stack.bricksById.Values)
{
    int fallenBricks = 0;
    
    HashSet<int> destroyedBricks = new HashSet<int>();
    destroyedBricks.Add(brick.id);
    
    List<int> bricksToConsider = new List<int>();
    if (stack.bricksAbove.TryGetValue(brick.id, out HashSet<int>? startingBricks))
    {
        bricksToConsider.AddRange(startingBricks);
    }

    while (bricksToConsider.Count > 0)
    {
        int currentBrick = bricksToConsider[0];
        bricksToConsider.RemoveAt(0);

        if (!destroyedBricks.Contains(currentBrick))
        {
            int supportCount = stack.bricksBelow[currentBrick].Count;

            foreach (int below in stack.bricksBelow[currentBrick])
            {
                if (destroyedBricks.Contains(below))
                {
                    supportCount--;
                }
            }

            if (supportCount == 0)
            {
                destroyedBricks.Add(currentBrick);

                if (stack.bricksAbove.TryGetValue(currentBrick, out HashSet<int>? above))
                {
                    bricksToConsider.AddRange(above);
                }
                
                fallenBricks++;
            }
        }
    }

    fallenBricksSum += fallenBricks;
}

Console.WriteLine($"Fallen Bricks Sum : {fallenBricksSum}");

class BrickStack
{
    public Dictionary<int, Brick> bricksById = new Dictionary<int, Brick>();
    public Dictionary<Vector, Brick> brickLocations = new Dictionary<Vector, Brick>();
    public List<int> idsByHeight = new List<int>();

    public Dictionary<int, HashSet<int>> bricksBelow = new Dictionary<int, HashSet<int>>();
    public Dictionary<int, HashSet<int>> bricksAbove = new Dictionary<int, HashSet<int>>();

    public void Add(Brick brick)
    {
        bricksById.Add(brick.id, brick);
        
        InsertToHeightList(brick);
        
        for (int i = 0; i <= brick.magnitude; i++)
        {
            brickLocations.Add(brick.start + (i * brick.direction), brick);
        }
    }

    public void FallStack()
    {
        for (int i = 0; i < idsByHeight.Count; i++)
        {
            int id = idsByHeight[i];
            Brick brick = bricksById[id];
            Vector[] below = brick.GetSpacesBelow();

            int fallDistance = 0;
            bool isFalling = true;
            HashSet<int> brickSupports = new HashSet<int>();
            
            Console.WriteLine($"Falling Brick {id}");
            Console.WriteLine($"({brick.start.x}, {brick.start.y}, {brick.start.z}), ({brick.direction.x}, {brick.direction.y}, {brick.direction.z}), {brick.magnitude}");

            while (isFalling)
            {
                bool hasHit = false;

                if (below[0].z == 0)
                {
                    hasHit = true;
                    Console.WriteLine($"Brick has hit floor");
                }
                else
                {
                    for (int j = 0; j < below.Length; j++)
                    {
                        if (brickLocations.ContainsKey(below[j]))
                        {
                            hasHit = true;
                            brickSupports.Add(brickLocations[below[j]].id);
                            RegisterBrickOnTop(brickLocations[below[j]], brick);
                            
                            Console.WriteLine($"Brick has hit brick {brickLocations[below[j]].id}");
                        }

                        below[j] += Vector.Down;
                    }
                }

                if (hasHit)
                {
                    isFalling = false;
                }
                else
                {
                    fallDistance++;
                }
            }
            
            Console.WriteLine($"Brick has fallen {fallDistance}");

            if (fallDistance > 0)
            {
                FallBrick(brick, fallDistance);
            }
            
            bricksBelow.Add(brick.id, brickSupports);
        }
    }

    public void FallBrick(Brick brick, int distance)
    {
        for (int i = 0; i <= brick.magnitude; i++)
        {
            brickLocations.Remove(brick.start + (i * brick.direction));
        }
        
        brick.start += distance * Vector.Down;
        
        for (int i = 0; i <= brick.magnitude; i++)
        {
            brickLocations.Add(brick.start + (i * brick.direction), brick);
        }

        idsByHeight.Remove(brick.id);
        InsertToHeightList(brick);
    }

    private void InsertToHeightList(Brick brick)
    {
        int height = brick.start.z;
        
        if (idsByHeight.Count == 0)
        {
            idsByHeight.Add(brick.id);
        }
        else if (height < bricksById[idsByHeight[0]].start.z)
        {
            idsByHeight.Insert(0, brick.id);
        }
        else
        {
            bool addAtEnd = true;
            
            for (int i = 0; i < idsByHeight.Count; i++)
            {
                if (height < bricksById[idsByHeight[i]].start.z)
                {
                    idsByHeight.Insert(i, brick.id);
                    addAtEnd = false;
                    break;
                }
            }

            if (addAtEnd)
            {
                idsByHeight.Add(brick.id);
            }
        }
    }

    private void RegisterBrickOnTop(Brick below, Brick above)
    {
        if (!bricksAbove.ContainsKey(below.id))
        {
            bricksAbove[below.id] = new HashSet<int>();
        }

        bricksAbove[below.id].Add(above.id);
    }
}

class Brick
{
    public int id;
    
    public Vector start;
    public Vector direction;
    public int magnitude;

    public Brick(int id, Vector start ,Vector end)
    {
        this.id = id;

        Vector signedExtents = end - start;

        if (signedExtents.SingleEntryPositive(out int mag))
        {
            this.start = start;
            this.direction = (mag != 0) ? signedExtents / mag : Vector.Zero;
            this.magnitude = mag;

        }
        else
        {
            this.start = end;
            this.direction = (mag != 0) ? signedExtents / mag : Vector.Zero;
            this.magnitude = -mag;
        }
    }

    public Vector[] GetSpacesBelow()
    {
        if (direction.Equals(Vector.Up))
        {
            return new Vector[] {start + Vector.Down};
        }
        
        Vector[] below = new Vector[magnitude + 1];
            
        for (int i = 0; i <= magnitude; i++)
        {
            below[i] = (start + (i * direction)) + Vector.Down;
        }

        return below;
    }
    public Vector[] GetSpacesAbove()
    {
        if (direction.Equals(Vector.Up))
        {
            return new Vector[] {(start + (magnitude * direction)) + Vector.Up};
        }
        
        Vector[] above = new Vector[magnitude + 1];
            
        for (int i = 0; i <= magnitude; i++)
        {
            above[i] = (start + (i * direction)) + Vector.Up;
        }

        return above;
    }
}

struct Vector
{
    public int x;
    public int y;
    public int z;

    public Vector(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector(int[] vectorArray)
    {
        this.x = vectorArray[0];
        this.y = vectorArray[1];
        this.z = vectorArray[2];
    }

    public static Vector Zero => new Vector(0, 0, 0);

    public static Vector Up => new Vector(0, 0, 1);

    public static Vector Down => new Vector(0, 0, -1);
    
    public static Vector operator *(int k, Vector v)
    {
        return new Vector(k * v.x, k * v.y, k * v.z);
    }

    public static Vector operator -(Vector u, Vector v)
    {
        return new Vector(u.x - v.x, u.y - v.y, u.z - v.z);
    }
    
    public static Vector operator +(Vector u, Vector v)
    {
        return new Vector(u.x + v.x, u.y + v.y, u.z + v.z);
    }

    public static Vector operator /(Vector u, int d)
    {
        return new Vector(u.x / d, u.y / d, u.z / d);
    }

    public bool Equals(Vector other)
    {
        return x == other.x && y == other.y && z == other.z;
    }
    
    public bool SingleEntryPositive(out int magnitude)
    {
        if (x != 0)
        {
            magnitude = x;
            return y == 0 && z == 0 && x > 0;
        }

        if (y != 0)
        {
            magnitude = y;
            return z == 0 && y > 0;
        }

        if (z != 0)
        {
            magnitude = z;
            return z > 0;
        }

        magnitude = 0;
        return false;
    }
}