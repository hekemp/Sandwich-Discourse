using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InstructionsScreen : MonoBehaviour {

    public Button menu;

	// Use this for initialization
	void Start () {
        menu.onClick.AddListener(backToMenu);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void backToMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
