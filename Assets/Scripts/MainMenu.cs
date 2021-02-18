using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public void StartGame()
    {
        AudioManager.current.Play("MM_PLAY", playVariablePitch: false);
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
