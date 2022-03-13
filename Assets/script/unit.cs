using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class unit : MonoBehaviour 
{
    // Start is called before the first frame update
    
    public data_game data;//класс где буду хранится все данные игры
    
    mouse obj_mouse;//объект с скриптами мыши
    public GameObject flag;//ссыка на прикрепленный флаг
    public Sprite[] flags_sprites = new Sprite[8];//массив со спратами флагов
    public int max_hod=1;//количество ходов максимальное
    public int tek_hod = 1;//количество ходов текущее
    public int tek_hod_tmp = 1;//количество ходов временное, после перемещения данное значение переместится в тек ход
    public gamer vladelec;//владелец юнита
    public int strength = 2;//сила
    public int price = 3;//цена
    public int num_spr = 0;//номер спрайта юнита, чтобы автоматически находить его спрайт
    public Sprite spr_unit;//спрайт унита
    public Sprite spr_unit_off;//спрайт унита выключенного
    public Vector3 koordinat;//координаты юнита
    public s_army sc_army;//ссылка на армию
    public int id_unit = 0;//уникальный номер юнита
    public bool flag_life = true;//флаг что юнит живой
    public int status_untit = 0;//статус юнита 0 свободен, 1 - в нарнизоне, 2 -  в армии атаки
    private void Awake()
    {
        
    }
    void Start()
    {
        
        


    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    public void set_unit(int num, gamer v, Sprite spr, Sprite[] srp_off)
    //установка юнита
    //0 - легкая пехота, 1 тяжелая, 2 -рыцарь
    {
        

        vladelec = v;
        num_spr = num;
        spr_unit = spr;
        
        switch (num)
        {
            case 0:
                max_hod = 8;
                tek_hod = max_hod;
                tek_hod_tmp = tek_hod;
                strength = 2;
                price = 3;
                spr_unit_off = srp_off[2];//спрайт выключенного юнита
                break;
            case 1:
                max_hod = 6;
                tek_hod = max_hod;
                tek_hod_tmp = tek_hod;
                strength = 3;
                price = 6;
                spr_unit_off = srp_off[1];//спрайт выключенного юнита
                break;
            case 2:
                max_hod = 12;
                tek_hod = max_hod;
                tek_hod_tmp = tek_hod;
                strength = 6;
                price = 9;
                spr_unit_off = srp_off[4];//спрайт выключенного юнита
                break;
            default:
                max_hod = 12;
                tek_hod = max_hod;
                tek_hod_tmp = tek_hod;
                strength = 2;
                price = 3;
                spr_unit_off = srp_off[2];//спрайт выключенного юнита
                break;
        }

    }
    /*
    public void move_unit(Vector3 k)
    {
        this.transform.position = k;
        koordinat = k;
    }
    */
    public void set_koordinat(Vector3 k)
    {
        koordinat = k;
    }
    public void attack_event()
    {//метод рсчета боя
        
    }
    public unit ()
    {
        GameObject obj_player = GameObject.Find("land");
        //к объекту привязан свой скрипт ищем его
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
        obj_mouse = obj_player.GetComponent(typeof(mouse)) as mouse;
        id_unit = data.id_unit_count++;
        //Debug.Log(id_unit.ToString());
    }
    public bool contains_to_list(List<unit> l)//метод проверки, что юнит сордержится в списке
    {
        if (l==null) return false;
        bool tmp = false;
        for(int i=0;i<l.Count;i++)
        {
            if (id_unit == l[i].id_unit) tmp = true;
        }
        return tmp;
    }
    public void remove_unit(List<unit> u_list)
    {//удаление юнита из списка
        int num = -1;
        for (int i = 0; i < u_list.Count; i++) if (id_unit == u_list[i].id_unit) 
                num = i;
        if (num >=0) 
            u_list.RemoveAt(num);
    }
    public void destroy_unit()
    {//методу удаления мертвых юнитов
        Destroy(flag);
        Destroy(this);
    }
}
