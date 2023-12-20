string input = File.ReadAllText(@"C:\Users\Josep\Documents\AOC2023\Day19Input.txt");

string[] inputParts = input.Split("\r\n\r\n");

string[] stringWorkflows = inputParts[0].Split("\r\n");
string[] stringParts = inputParts[1].Split("\r\n");

Dictionary<string, Workflow> workflows = new Dictionary<string, Workflow>(stringWorkflows.Length);

char[] deliminators = new char[] { '{', '}', ',' };

for (int i = 0; i < stringWorkflows.Length; i++)
{
    string currentStringWorkflow = stringWorkflows[i];
    string[] rules = currentStringWorkflow.Split(deliminators, StringSplitOptions.RemoveEmptyEntries);

    List<Rule> workflowRules = new List<Rule>();

    for (int j = 1; j < rules.Length - 1; j++)
    {
        string currentRule = rules[j];

        string[] splitRule = currentRule.Split(":");
        
        workflowRules.Add(new Rule(splitRule[0][0], splitRule[0][1], long.Parse(splitRule[0][2..]), splitRule[1]));
    }

    Workflow currentWorkflow = new Workflow(workflowRules, rules[^1]);
    
    workflows.Add(rules[0], currentWorkflow);
}

List<BoundGroup> acceptedBounds = new List<BoundGroup>();

List<Workflow> foundWorkflows = new List<Workflow>(workflows.Count);
foundWorkflows.Add(workflows["in"]);

while (foundWorkflows.Count > 0)
{
    Workflow currentWorkflow = foundWorkflows[0];
    foundWorkflows.RemoveAt(0);

    BoundGroup defaultBoundGroup = new BoundGroup();
    defaultBoundGroup.Encapsulate(currentWorkflow.boundGroup);

    for (int i = 0; i < currentWorkflow.rules.Count; i++)
    {
        Rule currentRule = currentWorkflow.rules[i];
        if (workflows.TryGetValue(currentRule.nextWorkflow, out Workflow? nextWorkflow))
        {
            nextWorkflow.boundGroup.Encapsulate(currentWorkflow, defaultBoundGroup, currentRule);
            foundWorkflows.Add(nextWorkflow);
        }

        if (currentRule.nextWorkflow == "A")
        {
            BoundGroup currentBoundGroup = new BoundGroup();
            currentBoundGroup.Encapsulate(currentWorkflow, defaultBoundGroup, currentRule);
            
            acceptedBounds.Add(currentBoundGroup);
        }

        defaultBoundGroup.Encapsulate(currentRule.GetInvertedRule());
    }
    
    if (workflows.TryGetValue(currentWorkflow.defaultWorkflow, out Workflow? defaultWorkflow))
    {
        defaultWorkflow.boundGroup.Encapsulate(currentWorkflow, defaultBoundGroup, new Rule());
        foundWorkflows.Add(defaultWorkflow);
    }
    
    if (currentWorkflow.defaultWorkflow == "A")
    {
        BoundGroup currentBoundGroup = new BoundGroup();
        currentBoundGroup.Encapsulate(currentWorkflow, defaultBoundGroup, new Rule());
        
        acceptedBounds.Add(currentBoundGroup);
    }
}

long ratingCombinations = 0;

for (int i = 0; i < acceptedBounds.Count; i++)
{
    ratingCombinations += acceptedBounds[i].GetSize();
}

Console.WriteLine($"Rating Combinations : {ratingCombinations}");

class Workflow
{
    public List<Rule> rules;
    public string defaultWorkflow;

    public BoundGroup boundGroup;
    public Workflow(List<Rule> rules, string defaultWorkflow)
    {
        this.rules = rules;
        this.defaultWorkflow = defaultWorkflow;

        this.boundGroup = new BoundGroup();
    }
}

struct Rule
{
    public char category;
    public char inequality;
    public long bound;
    public string nextWorkflow;

    public Rule(char category, char inequality, long bound, string nextWorkflow)
    {
        this.category = category;
        this.inequality = inequality;
        this.bound = bound;
        this.nextWorkflow = nextWorkflow;
    }

    public Rule()
    {
        this.category = ' ';
        this.inequality = ' ';
        this.bound = -1;
        this.nextWorkflow = "";
    }

    public Rule GetInvertedRule()
    {
        if (inequality == '<')
        {
            return new Rule(category, '>', bound - 1, "");
        }

        if (inequality == '>')
        {
            return new Rule(category, '<', bound + 1, "");
        }

        return new Rule();
    }
}

class Bound
{
    public long lower = 0;
    public long upper = 4001;

    public void Encapsulate(Bound otherBound)
    {
        this.lower = Math.Max(lower, otherBound.lower);
        this.upper = Math.Min(upper, otherBound.upper);
    }

    public void Encapsulate(char inequality, long bound)
    {
        if (inequality == '<')
        {
            this.upper = Math.Min(upper, bound);
        }

        if (inequality == '>')
        {
            this.lower = Math.Max(lower, bound);
        }
    }
    
    public long GetSize()
    {
        return upper - lower - 1;
    }
}

class BoundGroup
{
    public Bound xBound = new Bound();
    public Bound mBound = new Bound();
    public Bound aBound = new Bound();
    public Bound sBound = new Bound();

    public void Encapsulate(BoundGroup otherWorkflow)
    {
        xBound.Encapsulate(otherWorkflow.xBound);
        mBound.Encapsulate(otherWorkflow.mBound);
        aBound.Encapsulate(otherWorkflow.aBound);
        sBound.Encapsulate(otherWorkflow.sBound);
    }

    public void Encapsulate(Rule rule)
    {
        switch (rule.category)
        {
            case 'x':
                xBound.Encapsulate(rule.inequality, rule.bound);
                break;
            case 'm':
                mBound.Encapsulate(rule.inequality, rule.bound);
                break;
            case 'a':
                aBound.Encapsulate(rule.inequality, rule.bound);
                break;
            case 's':
                sBound.Encapsulate(rule.inequality, rule.bound);
                break;
        }
    }
    
    public void Encapsulate(Workflow prevWorkflow, BoundGroup defaultBounds, Rule rule)
    {
        Encapsulate(prevWorkflow.boundGroup);
        Encapsulate(defaultBounds);
        Encapsulate(rule);
    }

    public long GetSize()
    {
        return xBound.GetSize() * mBound.GetSize() * aBound.GetSize() * sBound.GetSize();
    }
}