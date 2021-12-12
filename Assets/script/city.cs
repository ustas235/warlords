using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class city : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject kursor;
    public GameObject unit_prefab;//префаб юнита
    public Sprite spr_kursor_attack;
    public Sprite spr_kursor_in_city;
    public Sprite spr_city;
    public data_game data;//класс где буду хранится все данные игры
    public game game_s;//класс со скриптом игры
    mouse obj_mouse;//объект с скриптами мыши
    public gamer vladelec;//владелец города
    public int id_spr = 0;//номер спрайта города, чтобы автоматически находить его спрайт
    public Vector3 koordinat;
    public int id_unit=1;//номер производимого юнита 0- легкая пехота, 1- тяжелая, 2- рыцарь
    public int count_hod = 1, count_hod_start = -1;//количество ходов до завершения строительства
    public List<unit> garnison = new List<unit>();//юниты охраняющие город
    void Start()
    {
        GameObject obj_player = GameObject.Find("land");
        //к объекту привязан свой скрипт ищем его
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
        game_s = obj_player.GetComponent(typeof(game)) as game;
        obj_mouse = obj_player.GetComponent(typeof(mouse)) as mouse;
        count_hod_start = -1;//при старте он -1 чтобы юниты не создавались
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseEnter()
    {
        //kursor.GetComponent<SpriteRenderer>().sprite= spr_attack;
        //this.GetComponent<SpriteRenderer>().sprite = spr_city_grey[0];
    }
    private void OnMouseUp()
    {
        //this.GetComponent<SpriteRenderer>().sprite = spr_city_bel[0];
        if (data.tek_activ_igrok.id == vladelec.id)//городу кликает владелец
        {
            if (data.get_activ_army() == null)//нет активынх юнитов откроем панель города
            {
                data.activ_city = this;//сохраним себя в активном городе
                data.city_panel_s.set_panel(data.tek_activ_igrok.id);
                data.city_window.SetActive(true);//если активный игрок владелец города показать панель
            } 
            else
            {
                data.type_event = 1;//событие перемещения
                obj_mouse.mouse_event(1);//переместим туда юнит
            }
                
        }
        else //город принадлежит другому игроку, попытка атаки города
        {
            obj_mouse.mouse_event(2);//вызываем метод перемещения с атакой
            data.def_city = this;//сохраним себя в защищаемом город
            data.type_event = 3; //сохраним тип события бля дальнейшей обработки
        }
    }
    public void change_vladelec(gamer vlad)
    {
        vladelec = vlad;
        spr_city = vlad.spr_city;//город носит спрайт владельца
        this.GetComponent<SpriteRenderer>().sprite = spr_city;
    }
    //выключение панели города
    public void create_unit()//метод по созданию юнитов
    {
        if (count_hod_start > 0)//при старте он меншье нуля, пока игрок не проинициализирует юниты не создаются
        {
            count_hod--;
            if (count_hod <= 0)
            {
                count_hod = count_hod_start;//одновим счетчик
                //создадим юнита
                
                Vector3 koor_unit = new Vector3(koordinat.x - 0.2f, koordinat.y + 0.2f, koordinat.z);//координаты создания юнита
                unit tmp_unit_s = new unit();
                tmp_unit_s.set_koordinat(koor_unit);
                tmp_unit_s.set_unit(id_unit, vladelec, game_s.get_sprite_unit(vladelec.id, id_unit), game_s.get_sprite_unit_off());
                game_s.create_new_army(tmp_unit_s);
            }
        }
    }
    public void setting_activ_city(int num_unit)//метод настройки производсвта города
    {
        id_unit = num_unit;
        switch (num_unit)
        {
            case 1://легкая пехота делается1 ход
                count_hod_start = 1;
                count_hod = 1;
                break;
            case 2://тяжелая пехота делается 2 ход
                count_hod_start = 2;
                count_hod = 2;
                break;
            case 3://рыцари делается 3 ход
                count_hod_start = 3;
                count_hod = 3;
                break;
            default:
                count_hod_start = 1;
                count_hod = 1;
                break;

        }
    }
    public bool is_garnison(s_army sarmy)
    {//метод проверяет стоит ли армия в этом городе
        Vector3 k = sarmy.koordinat;
        float delta_x = Math.Abs(koordinat.x - k.x);
        float delta_y = Math.Abs(koordinat.y - k.y);
        if ((delta_x <= 0.25f) & (delta_y <= 0.25f)) 
            return true;
        else return false;
    }
}
