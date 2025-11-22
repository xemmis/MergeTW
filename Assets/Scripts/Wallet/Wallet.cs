using System;
using UnityEngine;

public static class Wallet
{
    public static int CurrentMoney;
    public static Action<int> OnMoneyChanged;

    public static bool SpendMoney(int amount)
    {
        if (amount <= 0)
        {
            return false;
        }

        if (CurrentMoney >= amount)
        {
            CurrentMoney -= amount;
            OnMoneyChanged?.Invoke(CurrentMoney);
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void EarnMoney(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        CurrentMoney += amount;
        OnMoneyChanged?.Invoke(CurrentMoney);
    }

    public static bool CanAfford(int amount)
    {
        return CurrentMoney >= amount && amount > 0;
    }

    public static void SetMoney(int newAmount)
    {
        CurrentMoney = Mathf.Max(0, newAmount);
        OnMoneyChanged?.Invoke(CurrentMoney);
    }

    public static int ReturnMoney()
    {
        return CurrentMoney;
    }

    public static void Reset()
    {
        CurrentMoney = 0;
    }
}
