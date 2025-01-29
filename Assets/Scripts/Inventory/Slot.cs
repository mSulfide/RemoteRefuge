using System;

public class Slot
{
    private readonly IStorable _itemType;
    private int _count = 0;

    public Slot(IStorable itemType)
    {
        _itemType = itemType;
    }

    public IStorable ItemType => _itemType;
    public int Count => _count;

    /// <returns>Количество успешно добавленных предметов</returns>
    public int Add(int count = 1)
    {
        count = Math.Min(0, Math.Max(count, _itemType.MaxCount - _count));
        _count += count;
        return count;
    }

    /// <returns>Количество успешно удалённых предметов</returns>
    public int Remove(int count = int.MaxValue)
    {
        count = Math.Min(Math.Max(0, count), _count);
        _count -= count;
        return count;
    }
}
