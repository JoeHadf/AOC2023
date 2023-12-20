string[] input = File.ReadAllLines(@"C:\Users\Josep\Documents\AOC2023\Day20Input.txt");
char[] deliminators = new char[] { ' ', '-', '>', ',' };

Dictionary<string, Module> modules = new Dictionary<string, Module>(input.Length);

for (int i = 0; i < input.Length; i++)
{
    string[] inputData = input[i].Split(deliminators, StringSplitOptions.RemoveEmptyEntries);
    
    Module currentModule;
    string name;
    
    if (inputData[0][0] == '%')
    {
        currentModule = new FlipFlop();
        name = inputData[0][1..];

    }
    else if (input[i][0] == '&')
    {
        currentModule = new Conjunction();
        name = inputData[0][1..];
    }
    else
    {
        currentModule = new Broadcaster();
        name = inputData[0];
    }

    List<string> moduleDestinations = new List<string>(inputData.Length - 1);

    for (int j = 1; j < inputData.Length; j++)
    {
        moduleDestinations.Add(inputData[j]);
    }

    currentModule.destinations = moduleDestinations;
    currentModule.name = name;
    
    modules.Add(name, currentModule);
}

foreach(KeyValuePair<string, Module> module in modules)
{
    if (module.Value is Conjunction conjunction)
    {
        foreach (KeyValuePair<string, Module> inputModule in modules)
        {
            if (inputModule.Value.destinations.Contains(module.Key))
            {
                conjunction.rememberedPulses.Add(inputModule.Key, PulseType.Low);
            }
        }
    }
}

List<Pulse> pulsesToDo = new List<Pulse>();

int lowCount = 0;
int highCount = 0;

bool hasFoundButtonPressCount = false;

string[] parentModules = new string[] {"lh", "fk", "ff", "mm"};

Dictionary<string, int> pressesToReceiveLow = new Dictionary<string, int>();

int pressCount = 1;

while(!hasFoundButtonPressCount)
{
    pulsesToDo.Add(new Pulse(PulseType.Low,"button" ,"broadcaster"));

    while (pulsesToDo.Count > 0)
    {
        Pulse currentPulse = pulsesToDo[0];
        pulsesToDo.RemoveAt(0);

        if (currentPulse.type == PulseType.Low)
        {
            lowCount++;
        }
        else
        {
            highCount++;
        }

        string pulseDestination = currentPulse.destination;

        if (modules.TryGetValue(pulseDestination, out Module? currentModule))
        {
            pulsesToDo.AddRange(currentModule.ReceivePulse(currentPulse.inputModule, currentPulse.type));
        }

        if (currentPulse.type == PulseType.Low && parentModules.Contains(currentPulse.destination))
        {
            Console.WriteLine($"Low pulse received at {currentPulse.destination} after {pressCount} presses");
            pressesToReceiveLow.TryAdd(currentPulse.destination, pressCount);
        }
    }

    bool allParentsPressesFound = true;
    for (int i = 0; i < parentModules.Length; i++)
    {
        if (!pressesToReceiveLow.ContainsKey(parentModules[i]))
        {
            allParentsPressesFound = false;
        }
    }

    hasFoundButtonPressCount = allParentsPressesFound;

    pressCount++;
}

long pressMultiplied = 1;

for (int i = 0; i < parentModules.Length; i++)
{
    pressMultiplied *= pressesToReceiveLow[parentModules[i]];
}

Console.WriteLine($"Single Low Pulse To rx : {pressMultiplied}");

class Module
{
    public string name;
    
    public List<string> destinations = new List<string>();

    private protected List<Pulse> GetPulseList(PulseType type)
    {
        List<Pulse> pulses = new List<Pulse>(destinations.Count);
        
        for (int i = 0; i < destinations.Count; i++)
        {
            pulses.Add(new Pulse(type, name,destinations[i]));
        }

        return pulses;
    }

    public virtual List<Pulse> ReceivePulse(string inputModule, PulseType type)
    {
        return new List<Pulse>();
    }
}

class FlipFlop : Module
{
    private bool isOn = false;
    
    public override List<Pulse> ReceivePulse(string inputModule, PulseType type)
    {
        if (type == PulseType.Low)
        {
            isOn = !isOn;
            PulseType typeToSend = (isOn) ? PulseType.High: PulseType.Low;
            return GetPulseList(typeToSend);
        }

        return new List<Pulse>();
    }
}

class Conjunction : Module
{
    public Dictionary<string, PulseType> rememberedPulses = new Dictionary<string, PulseType>();
    public override List<Pulse> ReceivePulse(string inputModule, PulseType type)
    {
        rememberedPulses[inputModule] = type;

        bool allHigh = true;

        foreach (PulseType rememberedPulse in rememberedPulses.Values)
        {
            if (rememberedPulse == PulseType.Low)
            {
                allHigh = false;
            }
        }

        PulseType typeToSend = (allHigh) ? PulseType.Low : PulseType.High;

        return GetPulseList(typeToSend);
    }
}

class Broadcaster : Module
{
    public override List<Pulse> ReceivePulse(string inputModule, PulseType type)
    {
        return GetPulseList(type);
    }
}

struct Pulse
{
    public PulseType type;
    public string inputModule;
    public string destination;

    public Pulse(PulseType type, string inputModule, string destination)
    {
        this.type = type;
        this.inputModule = inputModule;
        this.destination = destination;
    }
}

enum PulseType
{
    Low,
    High
}