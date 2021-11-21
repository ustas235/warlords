using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamer : MonoBehaviour//
{
    public List<city> city_list = new List<city>();//список городов
    public List<GameObject> obj_unit_list = new List<GameObject>();//список объектов юнитов
    public List<unit> skript_unit_list = new List<unit>();//список скприптов юнитов
    public data_game data;//класс где буду хранится все данные игры
    int money = 0;
    public int id = 0;//id игрока, он же номер
    public bool still_play = true;//флаг что игрок еще играет
    public bool active = false;//флаг что ход игрока
    public Sprite spr_city;//ссылка на спрат своего города
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
    public gamer(int num)//конструктор принимает номер
    {
        id = num;
        
    }
}
