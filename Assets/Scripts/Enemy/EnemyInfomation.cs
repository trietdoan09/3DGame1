using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyInfomation : ScriptableObject
{
    public EnemyInfo[] enemysInfo;
    public int TotalEnemy
    {
        get
        {
            return enemysInfo.Length;
        }
    }
    public EnemyInfo GetInfoEnemy(int index)
    {
        return enemysInfo[index];
    }
}
