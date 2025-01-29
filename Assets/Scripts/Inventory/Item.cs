using UnityEngine;

public class Item : ScriptableObject, IStorable
{
    [SerializeField] private Sprite _sprite;
    [SerializeField, Min(1)] private int _maxCount;

    public Sprite Sprite => _sprite;
    public int MaxCount => _maxCount;
}
