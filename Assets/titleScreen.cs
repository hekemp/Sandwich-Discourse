using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class titleScreen : MonoBehaviour {


    public Button play;
    public Button instructions;
    public Button quitGame;

    // Use this for initialization
    void Start () {
        play.onClick.AddListener(moveToGame);
        instructions.onClick.AddListener(moveToInstructions);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void moveToGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    void moveToInstructions()
    {
        SceneManager.LoadSceneAsync(2);
    }

    void moveToQuitGame()
    {
        Application.Quit();
        
    }
}
