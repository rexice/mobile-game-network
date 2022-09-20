using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public struct Battle
{
    public EnemyType type;
    public int nofEnemies;
}

// database of all ... data
public class DB : MonoBehaviour
{
    private static DB _instance;
    public static DB instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<DB>();
            }
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public Battle currentBattle;

    public static void InitFightScene(Enemy enemy)
    {
        instance.currentBattle = new Battle();
        instance.currentBattle.nofEnemies = Random.Range(1, 3);
        instance.currentBattle.type = enemy.type;
    }
}
