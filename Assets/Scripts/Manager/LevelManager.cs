using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager current;

    public int currentLevel;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        current = this;
    }

    private void Start()
    {
        FindNextLevelByName();
    }

    private void LevelFinished()
    {
        Debug.Log("Level gewonnen!");
        EventManager.current.DisablePlayerMovement();
        currentLevel++;
        AudioManager.current.Play("LevelWon", playVariablePitch: false);

        StartCoroutine(LoadNewLevel());
    }

    private string FindNextLevelByName(int index = 0)
    {
        if (GameObject.Find("NextLevel"))
        {
            Transform[] tmpChild = GameObject.Find("NextLevel").GetComponentsInChildren<Transform>();
            // Do we have a GameObject NextLevel with one child?
            if (tmpChild.Length > 0 && index <= tmpChild.Length)
            {
                string nameOfNextLevel = tmpChild[0].GetChild(index).name;
                Debug.Log($"Nächster Level: {nameOfNextLevel}");
                return nameOfNextLevel;
            }
            else
            {
                Debug.Log("Ungültiger Szenenname oder Index als nächster Level");
            }
        }
        return "";
    }

    internal static void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator LoadNewLevel()
    {
        yield return new WaitForSeconds(2.0f);

        if (FindNextLevelByName() != "")
        {
            SceneManager.LoadScene(FindNextLevelByName());
        }
        else
        {
            int nextSceneByIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextSceneByIndex - 1 <= SceneManager.sceneCount)
            {
                SceneManager.LoadScene(nextSceneByIndex);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    void OnEnable()
    {
        EventManager.current.onPlayerReachedGoal += LevelFinished;
    }

    void OnDisable()
    {
        EventManager.current.onPlayerReachedGoal -= LevelFinished;
    }
}
