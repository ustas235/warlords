using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class s_main_menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void start_game()
    {
        SceneManager.LoadScene("scene_kvadrat");
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void OnMouseDown()
    {
        Debug.Log("клик по майн панели");
    }
   
}
