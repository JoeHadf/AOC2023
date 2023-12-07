string input = File.ReadAllText(@"C:\Users\Josep\Documents\AOC2023\Day6Input.txt");

string[] splitInput = input.Split("\n");

string timeString = splitInput[0].Split(":")[1].Replace(" ", string.Empty);
string distanceString = splitInput[1].Split(":")[1].Replace(" ", string.Empty);

long time = long.Parse(timeString);
long recordDistance = long.Parse(distanceString);

double sqrtDiscriminant = Math.Sqrt(time * time - 4 * recordDistance) / 2;
double head = time / 2;

double solution1 = head - sqrtDiscriminant;
double solution2 = head + sqrtDiscriminant;

double lowest = Math.Ceiling(solution1);
double highest = Math.Floor(solution2);

double wins = highest - lowest + 1;

Console.WriteLine($"Wins {wins}");