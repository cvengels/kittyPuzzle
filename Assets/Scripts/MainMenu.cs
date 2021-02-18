using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public void StartGame()
    {
        AudioManager.current.Play("MM_PLAY", playVariablePitch: false);
        EventManager.current.PlayerReachedGoal();


        // BITTE NACH RELEASE ANDERS LÖSEN!!!!!!!!!!!!!!!!!!!!!!!!!!!

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
