using System;
using UnityEngine;

public class InventoryItem
{
    public InventoryItemType Type { get; private set; }
    public string Name { get; private set; }
    public int Id { get; private set; }
    public int Stack { get; private set; }
    public int CurrentCount { get; private set; }
    public Sprite Icon { get; private set; }

    public event Action<int> ValueRemains;

    public InventoryItem(InventoryItemConfig config)
    {
        Type = config.ItemType;
        Name = config.Name;
        Id = config.Id;
        Stack = config.Stack;
        CurrentCount = config.StartValue;
        Icon = config.Icon;
    }

    public void AddValue(int value)
    {
        CurrentCount += value;

        if (CurrentCount > Stack)
            CurrentCount = Stack;
    }

    public void RemoveValue(int value)
    {
        CurrentCount -= value;

        if (CurrentCount < 0)
            CurrentCount = 0;
    }
}