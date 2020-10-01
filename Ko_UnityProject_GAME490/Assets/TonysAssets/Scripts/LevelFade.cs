using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFade : MonoBehaviour
{ 
    public Animator animator;

    private MainMenu mainMenu;

    private PauseMenu01 pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        mainMenu = FindObjectOfType<MainMenu>();

        pauseMenu = FindObjectOfType<PauseMenu01>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FadeOutOfMainMenu()
    {
        animator.SetTrigger("FadeOutMain");
    }

    public void FadeOutOfLevel()
    {
        animator.SetTrigger("FadeOutLevel");
        Time.timeScale = 1f;
    }

    public void OnFadeCompleteForMain()
    {
        mainMenu.NewGame();
    }

    public void OnFadeCompleteForPause()
    {
        pauseMenu.QuitToMain();
    }
}
