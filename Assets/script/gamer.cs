using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamer : MonoBehaviour//
{
    public List<city> city_list = new List<city>();//список городов
    public List<GameObject> obj_army_list = new List<GameObject>();//список объектов юнитов
    //public List<s_army> skript_army_list = new List<s_army>();//список скприптов юнитов
    public List<s_army> s_army_list = new List<s_army>();//список скприптов армий
    public data_game data;//класс где буду хранится все данные игры
    int money = 0;
    public int id = 0;//id игрока, он же номер
    public bool still_play = true;//флаг что игрок еще играет
    public bool active = false;//флаг что ход игрока
    public Sprite spr_city;//ссылка на спрат своего города
    //настройки бота
    public bool bot_flag = false;//флаг что играетбот
    public int bot_army_create_city = 2;//тип войск строимых ботом
    public int bot_min_garnison = 2;//гарнизон в городе
    // Start is called before the first frame update
    void Start()
    {
        GameObject obj_player = GameObject.Find("land");
        //к объекту привязан свой скрипт ищем его
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
        money = data.start_money;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public gamer(int num, bool flag_bot)//конструктор принимает номер и тип игроа бот/не бот
    {
        id = num;
        bot_flag = flag_bot;
    }
    //------------------------------------
    //действия бота
    public void set_bot(int level_bot)
    {//настройка бота в завимости от уровня
        switch (level_bot)
        {
            case 0:

                break;
            case 1:

                break;
            case 2:

                break;
            default:
                
                break;
        }
    }
    public void action_bot()
    {//дествие бота
        foreach (city c in city_list)
        {
            c.setting_activ_city(1);
            accumulation_garnison(c);//строим гарнизон
        }
    }
    //накопление гарнизонов городах
    public void accumulation_garnison(city c)
    {
        List<unit> unit_city_list = new List<unit>();//список всех юнитов города
        foreach (s_army a in s_army_list)//перебираем все армии игрока
        {
            if (c.is_garnison(a))//если очередная армия в нашем городе
            {
                foreach (unit u in a.unit_list) unit_city_list.Add(u);//записываем очередной юнит в список
            }
        }
        //набираем гарнизон
        if (unit_city_list.Count>= bot_min_garnison)
        {
            //если первый юнит состоял в большой армии, то создадим ему свою
            if (unit_city_list[0].sc_army.unit_list.Count > 1) unit_city_list[0].sc_army.sub_unit_create(unit_city_list[0]);
            s_army tmp_army_garnison= unit_city_list[0].sc_army;
            //юниты записываем в армию первого юнита
            for (int i=1;i< bot_min_garnison;i++)
            {
                tmp_army_garnison.add_unit(unit_city_list[i]);//записываем в армию гарнизона очередного юнита
            }
            tmp_army_garnison.move_army(c.koordinat_garnizon);//перемещаем в точку гарнизона
        }
        
    }
    //поиск целей
    public void search_target()
    {

    }
    //накопление армий к целям
    public void training_army()
    {

    }
    //поход к цели
    public void campaign_target()
    {

    }
}
