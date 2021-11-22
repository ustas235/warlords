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
    public gamer vladelec;//владелец города
    public int id_spr = 0;//номер спрайта города, чтобы автоматически находить его спрайт
    public Vector3 koordinat;
    public int id_unit=1;//номер производимого юнита 1- легкая пехота, 2- тяжелая, 3- рыцарь
    public int count_hod = 1, count_hod_start=1;//количество ходов до завершения строительства
    void Start()
    {
        GameObject obj_player = GameObject.Find("land");
        //к объекту привязан свой скрипт ищем его
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
        game_s = obj_player.GetComponent(typeof(game)) as game;
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
    private void OnMouseDown()
    {
        //this.GetComponent<SpriteRenderer>().sprite = spr_city_bel[0];
        if (data.tek_activ_igrok.id == vladelec.id)
        {
            data.activ_city = this;//сохраним себя в активном городе
            data.city_window.SetActive(true);//если активный игрок владелец города показать панель
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
        count_hod--;
        if (count_hod<=0)
        {
            count_hod = count_hod_start;//одновим счетчик
            //создадим юнита
            Vector3 koor_unit = new Vector3(koordinat.x - 0.2f, koordinat.y + 0.2f, koordinat.z);//координаты создания юнита
            GameObject unit_tmp = (GameObject)Instantiate(unit_prefab, koor_unit, Quaternion.identity);
            unit tmp = unit_tmp.GetComponent(typeof(unit)) as unit;//на скрипт
            tmp.set_koordinat(koor_unit);//юнит запомнит свой координат
            game_s.change_unit_player(vladelec.id, unit_tmp, id_unit);//настраиваем юнит
        }
    }
    public void setting_activ_city(int num_unit)//метод настройки производсвта гороа
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
}
