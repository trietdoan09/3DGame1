using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyInfo
{
    public enum RankOfEnemy
    {
        God, // 0.4%
        Emperor, // 0.6%
        Lord, // 4%
        General, // 7%
        Commander, // 13%
        Soilder, // 25%
        Civilians // 50%
    }
    public enum TypeEnemy
    {
        Undead,
        Demon,
        Human,
        Beats,
        Demigod
    }
    public int idEnemy;
    public string nameEnemy;
    public TypeEnemy typeEnemy;
    public GameObject[] dropItem;
    public float soulDrop;
    public RankOfEnemy maxRank;
    public float enemyMaxHealth;
    public float enemyMaxBreakPoint;
    public float attackPoint;
}
