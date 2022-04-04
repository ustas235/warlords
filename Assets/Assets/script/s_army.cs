using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class s_army : MonoBehaviour
{
    // Start is called before the first frame update
    
    public data_game data;//класс где буду хранится все данные игры
    GameObject obj_army;//ссылка на объект к которум присоединен скрип
    public int id;//номер армии
    mouse obj_mouse;//объект с скриптами мыши
    int strength;//сила
    int max_unit_strenght;//сила максимального юнита
   // public int max_hod;//количество ходов максимальное
    public int tek_hod;//количество ходов текущее
    public int tek_hod_tmp;//количество ходов временное, после перемещения данное значение переместится в тек ход
    public gamer vladelec;//владелец армии
    public Sprite spr_army;//спрайт армии
    public GameObject army_flag;//cссылка на флаг армии
    public Sprite[] flags_sprites = new Sprite[8];//массив со спратами флагов
    public Vector3 koordinat;//координаты армии
    List<unit> unit_list=new List<unit>();//список юнитов в армии
    //для бота
    int status_army=0;//статус армии 0 -свободна, 1- в гарнизоне,2 собирается для атаки, 3 идет в атаку город, 4 идет в один из своих городов, 5 атака на другую армию
    city target_city;//город, который идет/пойдет атаковать армия
    Vector3 target_koordinat;//координы, куа пойдет атаковать армия
    private void Awake()
    {
       
        GameObject obj_player = GameObject.Find("land");
        //к объекту привязан свой скрипт ищем его
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
        obj_mouse = obj_player.GetComponent(typeof(mouse)) as mouse;
    }

    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void set_army()//настройка армии после каждого изменения его состава
    {
        if (unit_list.Count>0)
        {
            strength = 0;//сила
            //max_hod = 100;//количество ходов максимальное
            tek_hod = 100;//количество ходов текущее
            tek_hod_tmp = 100;//количество ходов временное, после перемещения данное значение переместится в тек ход
            koordinat = unit_list[0].koordinat;
            for (int i=0; i< unit_list.Count;i++)
            {
                /*if (i == 0)//флаг армии у первого юнита
                {
                    unit_list[i].flag.SetActive(true);//виден только первый флаг
                    unit_list[i].flag.GetComponent<SpriteRenderer>().sprite = unit_list[i].flags_sprites[unit_list.Count - 1];//размер флага зависит от величины армии
                }*/
                //else unit_list[i].flag.SetActive(false);//виден только первый флаг

                if (unit_list[i].strength >= strength)//ищем самого сильного юнита
                {
                    strength = unit_list[i].strength;//спрайт армии по самому сильному юниту
                    max_unit_strenght = unit_list[i].strength;
                    spr_army = unit_list[i].spr_unit;//обновим спрайт армии
                    obj_army.GetComponent<SpriteRenderer>().sprite = spr_army;
                }
                /*if (unit_list[i].max_hod <= max_hod)//макс ход по минимальному из всех
                {
                    max_hod = unit_list[i].max_hod;
                }*/
                if (unit_list[i].get_tek_hod() <= tek_hod)//тек ход ход по минимальному из всех
                {
                    tek_hod = unit_list[i].get_tek_hod();
                }
                if (unit_list[i].tek_hod_tmp <= tek_hod_tmp)//тек ход ход по минимальному из всех
                {
                    tek_hod_tmp = unit_list[i].tek_hod_tmp;
                }
            }
            data.game_s.set_sprite_army_flag(vladelec, koordinat);//настроим видимость армии
        }
    }
       
    public bool add_unit(unit u)//добавить юнит в армию из другой
    {
        bool check = false;
        if (unit_list.Count < 8)
        {
            //удалим унит из старой армии
            if (u.sc_army != null)
            {
                u.sc_army.sub_unit_destroy(u);
            }
            //добавим юнит в новую армию
            unit_list.Add(u);
            u.sc_army = this;
            set_army();//настроим армию
            check = true;
        }
        else check = false;
        return check;
    }
    public void sub_unit_create(unit u)//удалить юнит из армии с созданием новой аврмии
    {
        u.remove_unit(unit_list);
        u.sc_army = null;//сотрем информацию остарой армии
        data.game_s.create_new_army(u);//создаем новую армию
        set_army();//настроим армию после изменений
    }
    public void sub_unit_destroy(unit u)//удалить юнит с удалением армии
    {
        //u.remove_unit(unit_list);
        unit_list.Remove(u);
        if (unit_list.Count == 0)
        {
            vladelec.obj_army_list.Remove(obj_army);//список объектов
            vladelec.s_army_list.Remove(this);//скриптов к объектам
            Destroy(army_flag);
            Destroy(obj_army);
        }
    }
    public void unit_destroy(unit u)//убить юнит
    {
        unit_list.Remove(u);
        u.destroy_unit();
        if (unit_list.Count == 0)
        {
            vladelec.obj_army_list.Remove(obj_army);//список объектов
            vladelec.s_army_list.Remove(this);//скриптов к объектам
            Destroy(army_flag);
            Destroy(obj_army);
        }
    }
    public void move_army(Vector3 k)
    {
        koordinat = k;//армия помнит свои координаты
        foreach (unit u in unit_list)
        {
            u.koordinat = k;//юниты в армии тоже помнят координаты армии
        }
        army_flag.transform.position = k;
        this.transform.position = k;
        
    }
    public void set_koordinat(Vector3 k)
    {
        koordinat = k;
    }
    public void attack_event_army()
    {//метод рсчета боя клик был повражескому юниту
        //проверим, не идет ли атака на гарнизон города
        bool flag_g = false;
        gamer oth_vl = data.def_army.vladelec;//защищаемый игрок получим из армии
        List<unit> def_unit = new List<unit>();
        city def_city = null;
        foreach (city c in oth_vl.city_list) if (c.is_garnison(data.def_army))
            {
                flag_g = true;
                def_city = c;//запомним город
                target_city=def_city;
                break;
            }
        //если атака идет на гарнизон, проверим нет ли в городе еще войск
        if (flag_g)
        {
            
            foreach (s_army a in oth_vl.s_army_list)//перебираем все армии защищающегося игрока
            {
                if (def_city.is_garnison(a))//если очередная армия в нашем городе
                {
                    foreach (unit u in a.unit_list) def_unit.Add(u);//записываем очередной юнит в гарнизон
                }
            }

            //data.atack_panel_s.set_panel_atack(unit_list, def_unit);//начинаем атаку на гарнизон
            //data.attack_window.SetActive(true);//покажем окно
            calkulate_atack(unit_list, def_unit);//делаем расчет атаки и покаже окно
        }
        else// если атака не на гарнизон
        {
            foreach (unit u in data.def_army.unit_list) def_unit.Add(u);//записываем в список защитную армии
            calkulate_atack(unit_list, def_unit);//делаем расчет атаки и покаже окно

        }
    }
    public void attack_event_city()
    {//метод рсчета атака города (клик по городу)
        //проверим, не идет ли атака на гарнизон города
        gamer oth_vl = target_city.vladelec;//защищающийся игрок
        //проверим нет ли в городе еще войск
        List<unit> def_unit = new List<unit>();
        foreach (s_army a in oth_vl.s_army_list)//перебираем все армии защищающегося игрока
        {
            if (target_city.is_garnison(a))//если очередная армия в нашем городе
            {
                foreach (unit u in a.unit_list) def_unit.Add(u);//записываем очередной юнит в гарнизон
            }
        }
        //data.atack_panel_s.set_panel_atack(unit_list, def_unit);//начинаем атаку на гарнизон
        //data.attack_window.SetActive(true);//покажем окно
        calkulate_atack(unit_list, def_unit);//делаем расчет атаки и покаже окно
       
    }
    
    private void OnMouseDown()
    {
        //Debug.Log("Сработал арми");
        if (!EventSystem.current.IsPointerOverGameObject() )
        {
            if (data.get_activ_army() == null)
            {
                if (data.get_activ_igrok().id == this.vladelec.id)//если армия принадлежит активному игроку
                {
                    data.set_activ_army(this);//при клике юнита он передает в данные активной армии
                    this.transform.position = data.get_grid_step(this.transform.position);//выровним позицию по сетке
                    data.move_cam(koordinat);
                }
            }
            else
            {
                if (data.get_activ_igrok().id != this.vladelec.id)//клик по другой армии - возможно попытка атаки
                {
                    data.def_army = this;//сохраним себя в защищаемой армии
                    data.type_event = 2; //сохраним тип события бля дальнейшей обработки    
                    obj_mouse.mouse_event(2);//вызываем метод перемещения с атакой
                    //obj_mouse.do_kursor();
                }
                else//юнит союзни переместим на него курсор
                { 
                    if (this.id != data.get_activ_army().id)
                    {//если клик по другой союзной армии
                        data.type_event = 1;//событие перемещения
                        obj_mouse.mouse_event(1);//переместим туда курсор
                                                 //obj_mouse.do_kursor();
                    }
                }
            }
        }
        //obj_mouse.mouse_event(1);//переместим туда курсор
        //Debug.Log("Стал активным в "+ this.transform.position);

        //set_sprite(1, 1);
    }
    
    public void set_obj(GameObject g)//установка ссылкни на объект
    {
        obj_army = g;
    }
    public GameObject get_obj()//получение ссылкни на объект
    {
        return obj_army;
    }

    public void calkulate_atack(List<unit> army_a, List<unit> army_d)
    {//метод расчета атаки
        int max_count_round = army_a.Count + army_d.Count;//максимальной количесвто боев
        int i_a = 0, i_d = 0;//индексы юнитов атаки и защиты
        List<bool> flags_a = new List<bool>();//флаги состояния юнитов атки после боя
        List<bool> flags_d = new List<bool>();//флаги состояния юнитов защиты после боя
        for (int i = 0; i < army_a.Count; i++) flags_a.Add(true);
        for (int i = 0; i < army_d.Count; i++) flags_d.Add(true);//перед боем все юниты живые
        unit tmp_unit_atack, tmp_unit_def;
        while ((max_count_round>0)&(army_d.Count>0))
        {
            tmp_unit_atack = army_a[i_a];
            tmp_unit_def = army_d[i_d];
            if(check_boy(tmp_unit_atack, tmp_unit_def))
            {//если выиграл юнит атаки
                flags_d[i_d] = false;
                tmp_unit_def.flag_life = false;
                i_d++;//берем следующий по списку юнит защиты
                if (i_d >= army_d.Count) break;//если юниты защиты кончились выходим из боя
            }
            else
            {//если выиграл юнит защиты
                flags_a[i_a] = false;
                tmp_unit_atack.flag_life = false;
                i_a++;//берем следующий по списку юнит защиты
                if (i_a >= army_a.Count) break;//если юниты атаки кончились выходим из боя
            }
            max_count_round--;//предохранитель, если ошиблсь с алгоритмом не уйдем в вечный круг
        }
        //выберем панель в зависимости от числа обороны
        int tmp_count = (army_d.Count-1) / 8;
        int count_unit_panel;
        switch (tmp_count)
        {
            case 0:
                data.atack_panel_s = data.atack_panel_s_8;
                data.attack_window = data.attack_window_8;
                count_unit_panel = 8;
                break;
            case 1:
                data.atack_panel_s = data.atack_panel_s_16;
                data.attack_window = data.attack_window_16;
                count_unit_panel = 16;
                break;
            case 2:
                data.atack_panel_s = data.atack_panel_s_24;
                data.attack_window = data.attack_window_24;
                count_unit_panel = 24;
                break;
            case 4:
                data.atack_panel_s = data.atack_panel_s_32;
                data.attack_window = data.attack_window_32;
                count_unit_panel = 32;
                break;
            default:
                data.atack_panel_s = data.atack_panel_s_32;
                data.attack_window = data.attack_window_32;
                count_unit_panel = 32;
                break;

        }
        data.atack_panel_s.set_panel_atack(unit_list, flags_a, army_d, flags_d, count_unit_panel);//покажем результат боя
        data.attack_window.SetActive(true);//покажем окно
        finih_atack(unit_list, flags_a, army_d, flags_d);//удалим убитые юниты
        
    }
    public bool check_boy(unit a, unit d)
    {//метод расчета боя, возвращает true победитель атакующий, false -защищающийся
        int hit_a = 2, hit_d = 2;//счетчики жизней, при достижении 0 юнит погибает
        int rnd;//рандомное значение
        int s_a,  s_d;
        while ((hit_a>0)&(hit_d>0))
        {
            rnd = Random.Range(1, 11);
            s_a = a.strength + rnd;
            rnd = Random.Range(1, 11);
            s_d = d.strength + rnd;
            if (s_a!=s_d)
            {
                if (s_a > s_d) hit_d--;//у атаки больше слиа - защитникам урон
                else hit_a--;//у защиты больше сила - атаке урон
            }
        }
        if (hit_a>0) return true;
        else return false;
    }
    public bool check_koordinat(Vector3 k)
    {//метод сравнения координат
        if ((koordinat.x == k.x) & (koordinat.y == k.y)) return true;
        else return false;
    }
    public void update_count_hod(int delta_hod)
    {//метод обновляет количество оставшихся ходов сех юнитов
        foreach (unit u in unit_list) u.set_tek_hod(u.get_tek_hod() - delta_hod);

    }
    public void reboot_count_hod()//сброс количества ходов юнитов
    {//метод обновляет количество оставшихся ходов сех юнитов
        foreach (unit u in unit_list)
        {
            u.set_tek_hod(u.get_max_hod());
            u.tek_hod_tmp = u.get_max_hod();
        }
        set_army();
    }
    public int get_strenght()
    {//метод высчитывает силу армии и возращает ее
        int str = 0;
        foreach (unit u in unit_list) str += u.strength;
        return str;
    }
    public void set_target_koordinat(Vector3 k)
    {
        target_koordinat = k;
    }
    public Vector3 get_target_koordinat()
    {
        return target_koordinat;
    }
    public void set_status(int st)
    {//метод выставляет статус армии и всех входящих юнитов
        status_army = st;
        foreach (unit u in unit_list) u.status_untit = st;
    }
    public int get_status()
    {//метод выставляет статус армии и всех входящих юнитов
        return status_army;
    }
    public void set_target_city(city c)
    {//установка цели-города для атаки или передвижения
        target_city = c;
    }
    public city get_target_city()
    {//получение цели-города для атаки или передвижения
        return target_city;
    }
    public void finih_atack(List<unit> unit_list_atack, List<bool> f_a, List<unit> unit_list_def, List<bool> f_d)
    {//метод обрабатывае результаты боя 
        //удалим убитые юниты
        for (int i = unit_list_atack.Count - 1; i > -1; i--)
        {
            if (!f_a[i])
            {
                unit_list_atack[i].sc_army.unit_destroy(unit_list_atack[i]);//если юнит убит удалим юнит
                f_a.RemoveAt(i);
            }
        }

        for (int i = unit_list_def.Count - 1; i > -1; i--)
        {
            if (!f_d[i])
            {
                unit_list_def[i].sc_army.unit_destroy(unit_list_def[i]);//если юнит убит удалим юнит
                unit_list_def.Remove(unit_list_def[i]);//т.к. список защитников был сощдан отдель чистим и его
                f_d.RemoveAt(i);
            }
        }

        //если все защитники пали и была защита города, то город передается новому владельцу
        if ((unit_list_def.Count < 1) & (status_army == 3))
        {
            get_target_city().change_vladelec(vladelec);//меняем владельца города
            set_target_city(null);//сбросим целевой город
        }
        set_status(0);//меняем статус армии
        set_army();
        data.setting_panel_unit();//настроим панель с юнитами
        vladelec.set_delta_gold();
    }
    public List<unit> get_unit_list()
    {
        return unit_list;
    }
    public int get_max_unit_str()
    {
        return max_unit_strenght;
    }
}
