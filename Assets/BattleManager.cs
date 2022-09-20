using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleManager : MonoBehaviour
{
    public float turnTimer = 0;
    public float turnTime = 1f;

    public List<EnemyInBattle> enemies;

    public UIDocument ui;
    public ProgressBar progressBar;
    public Button fightButton;

    public LayerMask enemyLayer;

    public GameObject player;

    public EnemyInBattle selectedEnemy;
    public GameObject selectionArrow;

    public IDictionary<EnemyType, BattleData> datas = new Dictionary<EnemyType, BattleData>();

    IEnumerator AttackAnimation(GameObject attacker, EnemyInBattle defender)
    {
        var startPos = attacker.transform.position;
        var endPos = defender.transform.position;
        float totalTime = 0.5f;

        for (float time = 0f; time < totalTime; time += Time.deltaTime)
        {
            var t = time / totalTime;
            t = t * t * t;
            attacker.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return 0;
        }

        defender.Damage(10, attacker);

        for (float time = 0f; time < totalTime; time += Time.deltaTime)
        {
            var t = time / totalTime;
            t = (1 - t);
            t = t * t * t;
            t = (1 - t);
            
            attacker.transform.position = Vector3.Lerp(endPos, startPos, t);
            yield return 0;
        }
    }

    public static void Fight()
    {
        if (instance.turnTimer >= instance.turnTime)
        {
            instance.turnTimer = 0f;
            instance.fightButton.SetEnabled(false);

            instance.StartCoroutine(instance.AttackAnimation(instance.player, instance.selectedEnemy));
        }
    }

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        foreach (var bm in Object.FindObjectsOfType<BattleManager>())
        {
            bm.Init();
        }
    }

    private static BattleManager _instance;
    public static BattleManager instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<BattleManager>();
            }
            return _instance;
        }
    }

    public void Init()
    {
        // don't run in editor
        if (!Application.isPlaying) { return; }

        datas.Add(EnemyType.Cyclops, Resources.Load<BattleData>("Enemy/Cyclops"));

        progressBar = ui.rootVisualElement.Q<ProgressBar>("TurnProgress");
        progressBar.lowValue = 0;
        progressBar.highValue = turnTime;

        fightButton = ui.rootVisualElement.Q<Button>("Fight");
        fightButton.SetEnabled(false);

        for (int i = 0; i < DB.instance.currentBattle.nofEnemies; i++)
        {
            enemies[i].gameObject.SetActive(true);
            enemies[i].GetComponent<EnemyInBattle>().data = Instantiate(datas[EnemyType.Cyclops]);
        }

        for (int i = DB.instance.currentBattle.nofEnemies; i < enemies.Count; i++)
        {
            enemies[i].gameObject.SetActive(false);
        }
    }

    void Start()
    {
        Init();
    }

    void Update()
    {

        if (!selectedEnemy)
        {
            selectedEnemy = enemies[0];
            selectionArrow.transform.position = selectedEnemy.transform.position;
        }

        turnTimer += Time.deltaTime;
        progressBar.value = turnTimer;

        if (turnTimer >= turnTime)
        {
            instance.fightButton.SetEnabled(true);
        }


        if (Input.GetMouseButtonUp(0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var mp = new Vector2(mousePos.x, mousePos.y);
            var hit = Physics2D.Raycast(mp, Vector2.zero, enemyLayer);

            if (hit)
            {
                selectedEnemy = hit.collider.gameObject.GetComponent<EnemyInBattle>();
                selectionArrow.transform.position = selectedEnemy.transform.position;
            }
        }
    }
}
