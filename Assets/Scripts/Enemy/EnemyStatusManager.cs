using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EnemyInfo;

public class EnemyStatusManager : MonoBehaviour
{

    [SerializeField] private GameObject enemyUI;
    private EnemyController enemyController;
    public int[] randomRate;
    public RankOfEnemy rank;
    private string TypeOfMonster;
    [SerializeField] private float enemyMaxHealth;
    [SerializeField] private float enemyMaxBreakPoint;
    [SerializeField] private float attackPoint;
    [SerializeField] private float enemyCurrentHealth;
    [SerializeField] private float enemyCurrentBreakPoint;
    public float GetEnemyMaxHealth() { return enemyMaxHealth; }
    public float GetEnemyCurrentHealth() { return enemyCurrentHealth; }
    public float GetEnemyMaxBreakPoint() { return enemyMaxBreakPoint; }
    public float GetEnemyCurrentBreakPoint() { return enemyCurrentBreakPoint; }
    public void SetEnemyCurrentBreakPoint(float value) { enemyCurrentBreakPoint = value; }
    public void IncreaseEnemyCurrentBreakPoint(float increaseBreakPoint) 
    {
        enemyCurrentBreakPoint = enemyCurrentBreakPoint + increaseBreakPoint >= enemyMaxBreakPoint ? enemyMaxBreakPoint : enemyCurrentBreakPoint + increaseBreakPoint;
    }
    public void DescreaseEnemyCurrentBreakPoint(float descreaseBreakPoint)
    {
        enemyCurrentBreakPoint -= descreaseBreakPoint;
    }
    [SerializeField] private EnemyInfomation enemyInfomation;
    [SerializeField] private int idEnemy;
    [SerializeField] private string nameEnemy;
    [SerializeField] private TypeEnemy typeEnemy;
    [SerializeField] private GameObject[] dropItem;
    [SerializeField] private float soulDrop;
    [SerializeField] private RankOfEnemy maxRank;
    [Header("enemy UI")]
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider breakBarLeft;
    [SerializeField] private Slider breakBarRight;
    private void Awake()
    {
        GetDefaultInfomation();
    }
    private void GetDefaultInfomation()
    {
        //lấy thông tin cơ bản của enemy từ database
        for (int i = 0; i< enemyInfomation.TotalEnemy; i++)
        {
            if(idEnemy == enemyInfomation.GetInfoEnemy(i).idEnemy)
            {
                nameEnemy = enemyInfomation.GetInfoEnemy(i).nameEnemy;
                typeEnemy = enemyInfomation.GetInfoEnemy(i).typeEnemy;
                dropItem = enemyInfomation.GetInfoEnemy(i).dropItem;
                soulDrop = enemyInfomation.GetInfoEnemy(i).soulDrop;
                maxRank = enemyInfomation.GetInfoEnemy(i).maxRank;
                enemyMaxHealth = enemyInfomation.GetInfoEnemy(i).enemyMaxHealth;
                enemyMaxBreakPoint = enemyInfomation.GetInfoEnemy(i).enemyMaxBreakPoint;
                attackPoint = enemyInfomation.GetInfoEnemy(i).attackPoint;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        //GenarateEnemyDefaultStatus();
        randomRate = new int[7] { 4 , 6, 40,70,130,250,500};
        RandomGetRankOfEnemy(); 
        GenarateEnemyDefaultStatus(rank);
    }
    private void GenarateEnemyDefaultStatus(RankOfEnemy rank)
    {
        // khởi tạo chỉ số của enemy dựa vào thông tin cơ bản và rank
        RankOfEnemy[] allValues = (RankOfEnemy[])System.Enum.GetValues(typeof(RankOfEnemy));
        enemyMaxHealth = enemyMaxHealth * (allValues.Length - ((int)rank));
        enemyMaxBreakPoint = enemyMaxBreakPoint * (allValues.Length - ((int)rank));
        attackPoint = attackPoint* (allValues.Length - ((int)rank));
        enemyCurrentHealth = enemyMaxHealth;
        enemyCurrentBreakPoint = 0;
        hpBar.maxValue = enemyMaxHealth;
        breakBarLeft.maxValue = enemyMaxBreakPoint;
        breakBarRight.maxValue = enemyMaxBreakPoint;
    }
    private void RandomGetRankOfEnemy()
    {
        // khởi tạo rank ngẫu nhiên cho enemy và tùy vào loại quái mà có mức rank tối đa khác nhau
        RankOfEnemy[] allValues = (RankOfEnemy[])System.Enum.GetValues(typeof(RankOfEnemy));
        float cumulativePercentage = 0f;
        for (int i=0; i<allValues.Length; i++)
        {
            cumulativePercentage += randomRate[i];
            var random = Random.Range(0f, 1000f);
            if(random < cumulativePercentage)
            {
                rank = allValues[i];
                Debug.Log(rank);
                rank = rank < maxRank ? maxRank : rank;
                return;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        EnemyUi();
        EnemyUiBar();
    }
    public float GetEnemyAttackPoint()
    {
        return attackPoint;
    }
    private void EnemyUi()
    {
        //giao diện enemy luôn hướng về player
        Vector3 directionUI = enemyController.targetTransform.position - enemyUI.transform.position;
        directionUI.y = 0f;
        Quaternion uiTargetRotation = Quaternion.LookRotation(directionUI);
        enemyUI.transform.rotation = uiTargetRotation;
    }
    //show thông tin enemy
    private void EnemyUiBar()
    {
        hpBar.value = enemyCurrentHealth;
        breakBarLeft.value = enemyCurrentBreakPoint;
        breakBarRight.value = enemyCurrentBreakPoint;
    }
}
