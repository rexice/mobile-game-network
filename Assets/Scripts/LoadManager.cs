using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadManager : MonoBehaviour
{
    public string lastScene;
    public bool useClassesInsteadOfIf;

    private static LoadManager _instance;
    public static LoadManager instance
    {
        get {
            if (!_instance)
            {
                _instance = FindObjectOfType<LoadManager>();
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

    IEnumerator LoadFightSceneRoutine(Enemy enemy)
    {
        lastScene = SceneManager.GetActiveScene().name;
        DB.InitFightScene(enemy);
        SceneManager.LoadScene("FightScene");
        yield break;
    }

    IEnumerator ExitFightSceneRoutine()
    {
        yield return SceneManager.LoadSceneAsync(lastScene);
        yield return SceneManager.SetActiveScene(SceneManager.GetSceneByName(lastScene));
        yield break;
    }

    public static void LoadFightScene(Enemy enemy)
    {
        instance.StartCoroutine(instance.LoadFightSceneRoutine(enemy));
    }

    public static void ExitFightScene()
    {
        instance.StartCoroutine(instance.ExitFightSceneRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
