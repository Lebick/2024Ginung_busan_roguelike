using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaveManager : Singleton<DataSaveManager>
{
    public SaveData currentData;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}

public class SaveData
{
    public int level;
    public int attackType;
    public float moveSpeedBuff;
    public bool isTPtoDash;
    public float dashDistance;
    public int dashReturnMPValue;
    public int attackDamage;
    public float attackCD;
    public float normalAttackRange;
    public float maxChargingTime;
    public int maxArrowThrough;
    public int minionAmount;

    public SaveData(int level, int attackType, float moveSpeedBuff, bool isTPtoDash, float dashDistance, int dashReturnMPValue,int attackDamage, float attackCD, float normalAttackRange, float maxChargingTime, int maxArrowThrough, int minionAmount)
    {
        this.level = level;
        this.attackType = attackType;
        this.moveSpeedBuff = moveSpeedBuff;
        this.isTPtoDash = isTPtoDash;
        this.dashDistance = dashDistance;
        this.dashReturnMPValue = dashReturnMPValue;
        this.attackDamage = attackDamage;
        this.attackCD = attackCD;
        this.normalAttackRange = normalAttackRange;
        this.maxChargingTime = maxChargingTime;
        this.maxArrowThrough = maxArrowThrough;
        this.minionAmount = minionAmount;
    }
}
