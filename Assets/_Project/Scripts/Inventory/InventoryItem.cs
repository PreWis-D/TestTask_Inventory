using System;
using UnityEngine;

public class InventoryItem
{
    public InventoryItemType Type;
    public string Name;
    public int Id;
    public int Stack;
    public int CurrentCount;
    public Sprite Icon;

    public event Action ValueAdded;
    public event Action ValueRemoved;

    public void Init(InventoryItemType type,
        string name,
        int id,
        int stack,
        int startValue,
        Sprite sprite)
    {
        Type = type;
        Name = name;
        Id = id;
        Stack = stack;
        CurrentCount = startValue;
        Icon = sprite;
    }

    public void AddValue(int value)
    {
        CurrentCount += value;

        if (CurrentCount > Stack)
            CurrentCount = Stack;

        ValueAdded?.Invoke();
    }

    public void RemoveValue(int value)
    {
        CurrentCount -= value;

        if (CurrentCount < 0)
            CurrentCount = 0;

        ValueRemoved?.Invoke();
    }
}