using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private bool levelFinished;
    public bool LevelFinished
    {
        get { return levelFinished; }
    }

    void Awake()
    {
        levelFinished = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            levelFinished = true;
            EventManager.current.PlayerReachedGoal();
        }
    }
}
