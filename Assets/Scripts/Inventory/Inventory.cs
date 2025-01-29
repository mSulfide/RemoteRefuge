using System.Collections.Generic;

public class Inventory
{
    private readonly List<Slot> _slots = new();

    public Inventory(int slotsCount)
    {
        for (int i = 0; i < slotsCount; i++)
        {
            _slots.Add(null);
        }
    }
}