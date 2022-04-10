using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class s_panel_city : MonoBehaviour
{
    public data_game data;//класс где буду хранится все данные игры
    GameObject txt_profit;//текст с доходом города
    List<unit> unit_list_s;//список юнитов
    List<GameObject> img_unit_obj_list = new List<GameObject>();//список изображений над кнопками
    List<GameObject> but_obj_list = new List<GameObject>();//список кнопок
    // Start is called before the first frame update
    private void Awake()
    {
        txt_profit = this.transform.Find("txt_profit").gameObject; ;//текст с количеством золота
        //настрока изображений кнопок
        for (int i = 0; i < 3; i++)
        {
            string tmp = "img_army_city_" + i.ToString();
            img_unit_obj_list.Add(this.transform.Find(tmp).gameObject);//добавим в массив объекты изображений
            tmp = "but_unit_" + i.ToString();
            but_obj_list.Add(this.transform.Find(tmp).gameObject);//добавим в массив объекты кнопок

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

        active_panel(-1); 
    }
    public void but1()
    {
        active_panel(0);
    }
    public void but2()
    {
        active_panel(1);
    }
    public void but3()
    {
        active_panel(2);
    }
    public void exit()
    {
        data.city_window.SetActive(false);
        data.activ_city=null;
    }
    public void set_panel(int num_igrok)
    {
        txt_profit.GetComponent<Text>().text = data.activ_city.get_profit().ToString();//покажем доход города
        for (int i=0;i< data.count_type_unit; i++)
        {
            Sprite tmp;
            if (i == data.activ_city.id_unit) tmp = data.game_s.get_sprite_unit(num_igrok, i);//если юнит у же проивзодится, то спрат включен
            else tmp = data.game_s.get_sprite_unit_off()[i];//спрафты не производимых юнитов выключены
            img_unit_obj_list[i].GetComponent<Image>().sprite = tmp;
            //настроим меню строительства
            if (data.activ_city.can_build_flag[i]) but_obj_list[i].transform.Find("text_" + i).gameObject.GetComponent<Text>().text = "build";
            else but_obj_list[i].transform.Find("text_" + i).gameObject.GetComponent<Text>().text = "Gold:"+data.get_cost_build()[i];
        }
    

    }
    void pay_new_build(int n)
    {//попытка купить новое строительство по номеру юнита
        if (data.get_activ_igrok().change_gold(0-data.get_cost_build()[n]))
        {//если денег хвататет
            but_obj_list[n].transform.Find("text_" + n).gameObject.GetComponent<Text>().text = "build";//меняем надпись на кнопке
            data.activ_city.can_build_flag[n] = true;//теперь этот юнит можно строить
        }
    }
    void active_panel(int n)
    {//производит дествия напанели в зависимости от нажатых кнопок
        Sprite tmp;
        int num_igrok = data.get_activ_igrok().id;
        int p = n;
        if (p >= 0)
        {
            if (data.activ_city.can_build_flag[p]) data.activ_city.setting_activ_city(p);
            else pay_new_build(p);
            for (int i = 0; i < data.count_type_unit; i++)
            {
                if (i == data.activ_city.id_unit) tmp = data.game_s.get_sprite_unit(num_igrok, i);//номер спрайт нажатой кнопки активен
                else tmp = data.game_s.get_sprite_unit_off()[i];//спрафты не производимых юнитов выключены
                img_unit_obj_list[i].GetComponent<Image>().sprite = tmp;

            }
        }
        else
        {
            data.activ_city.setting_activ_city(p);
            for (int i = 0; i < data.count_type_unit; i++)
            {
                tmp = data.game_s.get_sprite_unit_off()[i];//спрафты не производимых юнитов выключены
                img_unit_obj_list[i].GetComponent<Image>().sprite = tmp;
            }
        }
        
    }
}
