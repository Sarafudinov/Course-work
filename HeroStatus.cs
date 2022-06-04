using System.Collections.Generic;

public class HeroStatus
{
    public Stack<Memento> History { get; set; }
    public HeroStatus()
    {
        History = new Stack<Memento>();
    }

}

