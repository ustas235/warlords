using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_panel_attack : MonoBehaviour
{
    public data_game data;//класс где буду хранится все данные игры
    public Sprite spr_win;//српайт победителя
    public GameObject winner;//обьъект поббедителя победителя
    // Start is called before the first frame update
    void Start()
    {
        GameObject obj_player = GameObject.Find("land");
        //к объекту привязан свой скрипт ищем его
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void exit()
    {
        data.attack_window.SetActive(false);
    }
}
