using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class s_panel_city : MonoBehaviour
{
    public data_game data;//класс где буду хранится все данные игры
    GameObject txt_profit;//текст с доходом города
    List<unit> unit_list_s;//список юнитов
    List<GameObject> img_unit_obj_list = new List<GameObject>();//список изображений кнопок
    // Start is called before the first frame update
    private void Awake()
    {
        txt_profit = this.transform.Find("txt_profit").gameObject; ;//текст с количеством золота
        //настрока изображений кнопок
        for (int i = 0; i < 3; i++)
        {
            string tmp = "img_army_city_" + i.ToString();
            img_unit_obj_list.Add(this.transform.Find(tmp).gameObject);//добавим в массив объекты изображений

        }
    }
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
    public void but0()
    {//ничего не строить

        data.activ_city.setting_activ_city(-1);
    }
    public void but1()
    {
        data.activ_city.setting_activ_city(0);
    }
    public void but2()
    {
        data.activ_city.setting_activ_city(1);
    }
    public void but3()
    {
        data.activ_city.setting_activ_city(2);
    }
    public void exit()
    {
        data.city_window.SetActive(false);
        data.activ_city=null;
    }
    public void set_panel(int num_igrok)
    {
        txt_profit.GetComponent<Text>().text = data.activ_city.get_profit().ToString();//покажем доход города
        for (int i=0;i<3;i++)
        {
            img_unit_obj_list[i].GetComponent<Image>().sprite = data.game_s.get_sprite_unit(num_igrok,i);
        }
    }
}
