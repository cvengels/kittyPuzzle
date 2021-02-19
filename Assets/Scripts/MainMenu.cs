using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public void StartGame()
    {
        AudioManager.current.Play("MM_PLAY", playVariablePitch: false);
        
        
        // BITTE NACH RELEASE ANDERS LÖSEN!!!!!!!!!!!!!!!!!!!!!!!!!!!
        EventManager.current.PlayerReachedGoal();


        StartCoroutine(LevelManager.current.LoadNewLevel());
    }

    public void PlayDefaultSoundInMenu()
    {
        AudioManager.current.Play("MM_Default", playVariablePitch: false);
    }

    public void QuitGame()
    {
        Debug.Log("SPIEL BEENDET!");
        AudioManager.current.Play("MM_Default", playVariablePitch: false);
        Application.Quit();
    }

}
