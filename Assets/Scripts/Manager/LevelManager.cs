using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager current;

    Animator transition;


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
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainMenu"))
        {
            AudioManager.current.Play("LevelWon", playVariablePitch: false);
        }

        StartCoroutine(LoadNewLevel());
    }

    private string FindNextLevelByName(int index = 0)
    {
        if (GameObject.Find("NextLevel"))
        {
            Transform[] tmpChild = GameObject.Find("NextLevel").GetComponentsInChildren<Transform>();
            // Do we have a GameObject NextLevel with one child?
            if (tmpChild.Length > 1 && index <= tmpChild.Length)
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

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator LoadNewLevel(int loadScene = -1)
    {

        yield return new WaitForSeconds(1.5f);

        if (loadScene != -1)
        {
            ResetWinSubscription();
            SceneManager.LoadScene(loadScene);
        }
        else
        {
            if (FindNextLevelByName() != "")
            {
                ResetWinSubscription();
                SceneManager.LoadScene(FindNextLevelByName());
            }
            else
            {
                int nextSceneByIndex = SceneManager.GetActiveScene().buildIndex + 1;
                if (nextSceneByIndex - 1 <= SceneManager.sceneCount)
                {
                    ResetWinSubscription();
                    SceneManager.LoadScene(nextSceneByIndex);
                }
                else
                {
                    SceneManager.LoadScene(0);
                }
            }
        }
    }

    private void ResetWinSubscription()
    {
        EventManager.current.onPlayerReachedGoal -= LevelFinished;
        EventManager.current.onPlayerReachedGoal += LevelFinished;
    }



    void OnEnable()
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainMenu"))
        {
            EventManager.current.onPlayerReachedGoal += LevelFinished;
        }
    }

    void OnDisable()
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainMenu"))
        {
            EventManager.current.onPlayerReachedGoal -= LevelFinished;
        }
    }
}
