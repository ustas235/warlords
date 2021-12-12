using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class s_panel_unit : MonoBehaviour
{
    data_game data;
    public Sprite[] spr_on= new Sprite[8];
    public Sprite[] spr_off= new Sprite[8];
    List <GameObject> buttons_unit=new List<GameObject>();//список объектов кнопок
    List<bool> flags = new List<bool>();//флаги состо€ни€ кнопки
    List <unit> s_unit_list;//список переданных армий
    // Start is called before the first frame update
    private void Awake()
    {
        spr_on = new Sprite[8];
        spr_off = new Sprite[8];
        for (int i = 0; i < 8; i++)
        {
            string tmp = "but_unit_" + i.ToString();
            buttons_unit.Add(this.transform.Find(tmp).gameObject);//добавим в массив 8 кнопок
            flags.Add(false);//пока все флаги сбросим
        }
        GameObject obj_player = GameObject.Find("land");
        //к объекту прив€зан свой скрипт ищем его
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void button_0()
    {
        event_button(0);//запускаем метод от первой кнопки
    }
       
    public void button_1()
    {
        event_button(1);//запускаем метод от первой кнопки
    }
    public void button_2()
    {
        event_button(2);//запускаем метод от первой кнопки
    }
    public void button_3()
    {
        event_button(3);//запускаем метод от первой кнопки
    }
    public void button_4()
    {
        event_button(4);//запускаем метод от первой кнопки
    }
    public void button_5()
    {
        event_button(5);//запускаем метод от первой кнопки
    }
    public void button_6()
    {
        event_button(6);//запускаем метод от первой кнопки
    }
    public void button_7()
    {
        event_button(7);//запускаем метод от первой кнопки
    }
    void event_button(int num_button)//метот обработки нажати€ кнопки в зависимости от ее номера
    {
        //проверка на то чтобы осталс€ включеным хот€бы один флаг
        //нельза допустить чтобы все юниты выключили
        flags[num_button] = !flags[num_button];
        if (flags.Contains(true))
        {
            if (flags[num_button]) buttons_unit[num_button].GetComponent<Image>().sprite = spr_on[num_button];
            else buttons_unit[num_button].GetComponent<Image>().sprite = spr_off[num_button];
            //после каждого нажати€ обновим наши армии
            //и перепишем юниты в новую армию
            //определ€емстарший юнит
            //если добавили юнит в активную армию
            if (flags[num_button]) data.get_activ_army().add_unit(s_unit_list[num_button]);
            //иначе удалим юнит из акивной армии c созданием новую на его основе
            else data.get_activ_army().sub_unit_create(s_unit_list[num_button]);

        }
        else flags[num_button] = !flags[num_button];//вертаем обратно
        //data.tek_activ_igrok.s_army_list.Add(tmp_army);//добавл€ем новую армию в список армий
        //data.set_activ_army(tmp_army);//текуща€ арми€ активна€

    }
    public void set_panel_unit(List<unit> unit_list)//меод настройки панели юнитов
    {
        s_unit_list = unit_list;
        for (int i=0;i<8;i++)
        {
            if (i < unit_list.Count)//если в переданом списке юнитов больше чем номер текущей кнопки
            {
                spr_on[i] = unit_list[i].spr_unit;
                spr_off[i] = unit_list[i].spr_unit_off;
                if (unit_list[i].contains_to_list(data.get_activ_army().unit_list))
                {
                    buttons_unit[i].GetComponent<Image>().sprite = spr_on[i];//все из активной армии включены
                    flags[i] = true;
                }
                else
                {
                    buttons_unit[i].GetComponent<Image>().sprite = spr_off[i];
                    flags[i] = false;
                }
                buttons_unit[i].SetActive(true);//покажем кнопку
            }
            else buttons_unit[i].SetActive(false);
        }
    }
}
