using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class unit : MonoBehaviour
{
    // Start is called before the first frame update
    
    public data_game data;//класс где буду хранится все данные игры
    
    mouse obj_mouse;//объект с скриптами мыши
    public int max_hod=8;//количество ходов максимальное
    public int tek_hod = 8;//количество ходов текущее
    public int tek_hod_tmp = 8;//количество ходов временное, после перемещения данное значение переместится в тек ход
    public gamer vladelec;//владелец юнита
    public int strength = 2;//сила
    public int price = 3;//цена
    public int num_spr = 0;//номер спрайта юнита, чтобы автоматически находить его спрайт
    public Sprite spr_unit;//спрайт унита
    public Vector3 koordinat;//координаты юнита
    void Start()
    {
        GameObject obj_player = GameObject.Find("land");
        //к объекту привязан свой скрипт ищем его
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
        obj_mouse = obj_player.GetComponent(typeof(mouse)) as mouse;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {

        if (data.tek_activ_igrok.id == this.vladelec.id)//если юнит принадлежит активному игроку
        {
            data.set_activ_untit(this);//при клике юнита он передает в данные себя
            this.transform.position = data.get_grid_step(this.transform.position);//выровним позицию по сетке
            data.move_cam(koordinat);
        }
        else
        {
            if (data.get_activ_unit()!=null)//клик по другому юниту - возможно попытка атаки
            {
                obj_mouse.mouse_event(2);//вызываем метод перемещения с атакой
                data.attack_untit = this;//сохраним себя в атакуемом юните
                data.type_event = 2; //сохраним тип события бля дальнейшей обработки
            }
        }
        //Debug.Log("Стал активным в "+ this.transform.position);

        //set_sprite(1, 1);
    }
    public void set_unit(int num, gamer v, Sprite spr)
    //установка юнита
    //0 - легкая пехота, 1 тяжелая, 2 -рыцарь
    {
        vladelec = v;
        num_spr = num;
        spr_unit = spr;
        switch (num)
        {
            case 0:
                max_hod = 0;
                tek_hod = max_hod;
                strength = 2;
                price = 3;
                break;
            case 1:
                max_hod = 6;
                tek_hod = max_hod;
                strength = 3;
                price = 6;
                break;
            case 2:
                max_hod = 12;
                tek_hod = max_hod;
                strength = 6;
                price = 9;
                break;
            default:
                max_hod = 12;
                tek_hod = max_hod;
                strength = 2;
                price = 3;
                break;
        }

    }
    public void move_unit(Vector3 k)
    {
        this.transform.position = k;
        koordinat = k;
    }
    public void set_koordinat(Vector3 k)
    {
        koordinat = k;
    }
    public void attack_event()
    {//метод рсчета боя
        unit winn;
        if (data.attack_untit.strength >= data.get_activ_unit().strength) winn = data.attack_untit;
        else winn = data.get_activ_unit();
        //найдем скрипт связанный с панелью ататки
        s_panel_attack win_panel_script = data.attack_window.GetComponent(typeof(s_panel_attack)) as s_panel_attack;
        win_panel_script.winner.GetComponent<Image>().sprite = winn.spr_unit;//спрайт победителя
        data.attack_window.SetActive(true);//покажем окно
    }
}
