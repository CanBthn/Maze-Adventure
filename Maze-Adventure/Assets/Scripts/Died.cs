using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Died : MonoBehaviour{
    void OnEnable()
    {
    Cursor.visible = true; //Visible mouse
    Cursor.lockState = CursorLockMode.None;
    }

    public void RestartGame(){
        SceneManager.LoadScene("SampleScene");
    }
    public void QuitGame(){
        SceneManager.LoadScene("Start");
    }
}