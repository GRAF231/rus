using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    Dictionary<int, Data.Monster> monsterStat = new Dictionary<int, Data.Monster>();
    public Transform[] spawnPoint;
    bool _isSpawning = false;

    public float _spawnTime = 0.4f;

    [SerializeField]
    GameObject[] _spawnUnit;

    [SerializeField]
    int _maxSpawnUnit = 80;
    PlayerStat _playerStat;

    public int enemyCount = 0;
    int timeLevel = 0;
    int TimeLevel { get { return timeLevel; } set { timeLevel = value; if (timeLevel >= 3) _spawnTime = 0.1f; } }
    public void AddEnemyCount(int value) { enemyCount += value; }
    private void Start()
    {
        monsterStat = Managers.Data.MonsterData;
        spawnPoint = GetComponentsInChildren<Transform>();
        _playerStat = Managers.Game.getPlayer().GetComponent<PlayerStat>();
        Managers.Game.OnSpawnEvent -= AddEnemyCount;
        Managers.Game.OnSpawnEvent += AddEnemyCount;
    }

    private void Update()
    {
        if ((timeLevel + 1) * 60 < Managers.GameTime)
        {
            timeLevel = (int)Managers.GameTime / 60;
            if (timeLevel <= 5)
            {
                Debug.Log($"{timeLevel}Boss Spawn!");
                SpawnBoss(timeLevel);
            }

        }
        if (!_isSpawning)
            StartCoroutine(SpawnMonster());
    }

    void SpawnBoss(int timeLevel)
    {
        GameObject Boss = null;
        int level = _playerStat.Level;
        if (timeLevel < 5)
        {
            Boss = Managers.Game.Spawn(Define.WorldObject.Enemy, "Monster/Enemy");
            Boss.GetOrAddComponent<EnemyController>().Init(monsterStat[timeLevel], level, Define.MonsterType.middleBoss);
        }
        else
        {
            Boss = Managers.Game.Spawn(Define.WorldObject.Enemy, "Monster/Boss");
        }
        if (Boss == null)
        {
            Debug.Log($"Boss Load Failed! level : {timeLevel}");
            return;
        }

        Boss.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;


    }


    IEnumerator SpawnMonster()
    {
        _isSpawning = true;
        int level;
        if (_playerStat.Level < 10)
        {
            level = (int)Managers.GameTime / 20;
        }
        else if (_playerStat.Level < 35)
        {
            level = (int)Managers.GameTime / 10;
        }
        else
        {
            level = (int)Managers.GameTime / 5;
        } 

        if (level <= 4)
        {
            _spawnTime = 0.8f;
            _maxSpawnUnit = 20;

        }
        else if (level <= 8)
        {
            _spawnTime = 0.6f;
            _maxSpawnUnit = 60;
        }
        else if (level <= 75)
        {
            _spawnTime = 0.15f;
            _maxSpawnUnit = 80;
        }
        else
        {
            _spawnTime = 0.05f;
            _maxSpawnUnit = 120;
        }

        if (enemyCount < _maxSpawnUnit)
        {
            int monsterType = SetRandomMonster(timeLevel);

            GameObject enemy = Managers.Game.Spawn(Define.WorldObject.Enemy, "Monster/Enemy");
            enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
            enemy.GetOrAddComponent<EnemyController>().Init(monsterStat[monsterType], level, Define.MonsterType.Enemy);
        }

        yield return new WaitForSeconds(_spawnTime);
        _isSpawning = false;
    }


    int SetRandomMonster(int timeLevel)
    {
        float rand1 = Random.Range(0, 100);
        float rand2 = Random.Range(0, 100);
        int rd = 1;
        if (rand1 < 50)
        {
            if (rand2 < 90 - (20 * timeLevel))
                rd = 1;
            else
                rd = 2;
        }
        else if (rand1 < 90)
        {
            if (rand2 < 90 - (20 * timeLevel))
                rd = 3;
            else
                rd = 4;
        }
        else
            rd = 5;

        return rd;
    }

}
