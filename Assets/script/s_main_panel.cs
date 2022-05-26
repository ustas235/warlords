using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class s_main_panel : MonoBehaviour
{
    data_game data;
    GameObject txt_count_gold;//текст с количеством золота
    GameObject txt_delta_gold;//текст с содержанием армии золота золота
    // Start is called before the first frame update
    private void Awake()
    {
        //txt_count_gold = new GameObject();//текст с количеством золота
        //txt_delta_gold = new GameObject();//текст с содержанием армии золота золота
        GameObject obj_player = GameObject.Find("land");
        //к объекту привязан свой скрипт ищем его

        data = obj_player.GetComponent(typeof(data_game)) as data_game;
        txt_count_gold = this.transform.Find("txt_count_gold").gameObject;
        txt_delta_gold = this.transform.Find("txt_delta_gold").gameObject;
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void set_main_panel(float count, float delta)
    {//настройка главное панели
        txt_count_gold.GetComponent<Text>().text = ((int)count).ToString();
        txt_delta_gold.GetComponent<Text>().text = ((int)delta).ToString();
    }
    public void OnMouseDown()
    {
        Debug.Log("Клип оп панели");
    }
}
