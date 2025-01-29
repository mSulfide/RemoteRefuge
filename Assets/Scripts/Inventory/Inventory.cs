using System;
public class Inventory
{
    private readonly Slot[] _slots;

    public event Action OnChanged;

    public Inventory(int slotCount)
    {
        _slots = new Slot[slotCount];
        for (int i = 0; i < slotCount; i++)
            _slots[i] = new Slot(null);
    }

    public int Add(IStorable item, int count = 1)
    {
        int add = Add(item, count, 0);
        OnChanged?.Invoke();
        return add;
    }

    public int Remove(IStorable item, int count = int.MaxValue)
    {
        int remove = Remove(item, count, 0);
        OnChanged?.Invoke();
        return remove;
    }

    public int CountOf(IStorable item)
    {
        return CountOf(item, 0);
    }

    private int Add(IStorable item, int count, int start)
    {
        if (count <= 0 || start >= _slots.Length)
            return count;
        int index = Find(item, start);
        if (index != -1)
        {
            return Add(item, count - _slots[index].Add(count), index + 1);
        }
        else
        {
            index = Find(null);
            if (index != -1)
            {
                _slots[index].SetType(item);
                return Add(item, count - _slots[index].Add(count), index + 1);
            }
            else
            {
                return count;
            }
        }
    }

    private int Remove(IStorable item, int count, int start)
    {
        if (count <= 0 || start >= _slots.Length)
            return 0;
        int index = Find(item, start);
        if (index != -1)
        {
            int remove = _slots[index].Remove(count);
            return Remove(item, count - remove, index + 1) + remove;
        }
        else
        {
            return 0;
        }
    }

    private int CountOf(IStorable item, int start)
    {
        int index = Find(item, start);
        if (index != -1)
        {
            return _slots[index].Count + CountOf(item, index + 1);
        }
        else
        {
            return 0;
        }
    }

    private int Find(IStorable item, int start = 0)
    {
        for (int i = start; i < _slots.Length; i++)
            if (item.Equals(_slots[i].ItemType))
                return i;
        return -1;
    }
}