using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


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
    int profit = 0;//доход от города
    public int id_spr = 0;//номер спрайта города, чтобы автоматически находить его спрайт
    public Vector3 koordinat;//координаты города
    public Vector3 koordinat_garnizon;//координаты для стоянки армии гарнизона
    public Vector3 koordinat_atack;//координаты для стоянки армии атаки
    public Vector3 min_kkor;//координаты ближайшей точки, нужны для расчета у ботов
    public int id_unit = -1;//номер производимого юнита 0- легкая пехота, 1- тяжелая, 2- рыцарь
    public int count_hod = 1, count_hod_start = -1;//количество ходов до завершения строительства
    List<unit> garnison = new List<unit>();//юниты охраняющие город
    public bool[] can_build_flag;//список возможного стрительства в виде флагов
    public int bot_num_unit_build=-1;//номер юнита который боту будет стрить в этом городе
    bool falg_create_neutral_garnison = false;
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
    private void OnMouseEnter()
    {
        //kursor.GetComponent<SpriteRenderer>().sprite= spr_attack;
        //this.GetComponent<SpriteRenderer>().sprite = spr_city_grey[0];
    }
    private void OnMouseDown()
    {
        //this.GetComponent<SpriteRenderer>().sprite = spr_city_bel[0];
        if (!data.get_flag_army_is_move())
        {//пока двигаются армии не реагируем на клик мышки
            if ((!EventSystem.current.IsPointerOverGameObject()))
            {
                
                if (data.get_activ_igrok().id == vladelec.id)//городу кликает владелец
                {
                    if (data.get_activ_army() == null)//нет активынх юнитов откроем панель города
                    {
                        data.activ_city = this;//сохраним себя в активном городе
                        data.city_panel_s.set_panel(data.get_activ_igrok().id);
                        data.city_window.SetActive(true);//если активный игрок владелец города показать панель
                    }
                    else
                    {
                        data.game_s.move_kursor_clik();//перемещаем курсор и армия запомнит конечную точку
                        data.type_event = 1;//1-перемещение сохраним тип события бля дальнейшей обработки
                        data.get_activ_army().old_type_event = data.type_event;
                        data.get_activ_army().set_target_city(this);//запомним город - назанчение
                        obj_mouse.mouse_event(1);//переместим туда юнит
                    }

                }
                else //город принадлежит другому игроку, попытка атаки города
                {
                    if (data.get_activ_army() != null)
                    {
                        data.game_s.move_kursor_clik();//перемещаем курсор и армия запомнит конечную точку
                        data.get_activ_army().set_target_city(this);//сохраним себя в защищаемом город
                        data.type_event = 3; //сохраним тип события бля дальнейшей обработки
                        data.get_activ_army().old_type_event = data.type_event;
                        data.get_activ_army().set_status(3);//сатаус армии атака на город
                        obj_mouse.mouse_event(2);//вызываем метод перемещения с атакой
                        
                    }
                }
            }
        }
    }
    public void change_vladelec(gamer vlad)
    {
        if (!vladelec.Equals(null))
        {
            vladelec.city_list.Remove(this);//удаляем из списка старого игрока
            //после смены владельца проверим не проиграл ли игрок
            //if (!data.game_s.check_gamer_lose(vladelec)) Debug.Log("Игрок " + vladelec.id + "проиграл");
        }
        
        vladelec = vlad;//обновляем владельца
        spr_city = vlad.spr_city;//город носит спрайт владельца
        this.GetComponent<SpriteRenderer>().sprite = spr_city;
        vlad.city_list.Add(this);//добавляем город игроку в список
        count_hod_start = -1;//сбросим производство
        id_unit = -1;//сбросим производство
        koordinat_garnizon = new Vector3(koordinat.x + 0.2f, koordinat.y + 0.2f, koordinat.z);//гарнизон будет распологаться в проавом верхнем углу
        koordinat_atack = new Vector3(koordinat.x - 0.2f, koordinat.y - 0.2f, koordinat.z);//армия атаки будет распологаться в левом нижнем углу
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
                unit tmp_unit_s = new unit(data.id_unit_count++);
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
            case -1://ничего
                count_hod_start = -1;
                count_hod = 1;
                break;
            case 0://легкая пехота делается1 ход
                count_hod_start = 1;
                count_hod = 1;
                break;
            case 1://тяжелая пехота делается 2 ход
                count_hod_start = 2;
                count_hod = 2;
                break;
            case 2://рыцари делается 3 ход
                count_hod_start = 3;
                count_hod = 3;
                break;
            default:
                count_hod_start = -1;
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
        {//если произошел вызов этой функции, и она вернула тру, значит идет атака на город
            return true;
        }
        else return false;
    }
    public bool is_garnison(unit s_unit)
    {//метод проверяет стоит ли юнит в этом городе
        Vector3 k = s_unit.koordinat;
        float delta_x = Math.Abs(koordinat.x - k.x);
        float delta_y = Math.Abs(koordinat.y - k.y);
        if ((delta_x <= 0.25f) & (delta_y <= 0.25f))
        {//если произошел вызов этой функции, и она вернула тру, значит идет атака на город
            return true;
        }
        else return false;
    }
    public List<unit> get_garnison_unit_list()
    {//метод возвращает список юнитов, стоящих горнизоном
        List<unit> tmp_garnison = new List<unit>();
        foreach (s_army a in vladelec.s_army_list)
        {
            if (is_garnison(a))
            {
                foreach (unit u in a.get_unit_list()) tmp_garnison.Add(u);
            }
        }
        return tmp_garnison;
    }
    public int get_profit()
    {
        return profit;
    }
    public void collect_profit()
    {//сбор дохода с города
        vladelec.change_gold(profit);
    }
    public void set_profit(int p)
    {
        profit = p;
        
    }
    public void set_can_build(int f)
    {//настройка возможного стрительства
     //0 можно стрить легкоую пехорту, 1 легкую и тяжелую, 2-даже рыцарей, -1-ничего
        for (int i = 0; i < f+1; i++) can_build_flag[i] = true;
    }
    public void start_setup_set(int n)
    {//стартовая инициализация, n -макс номер юнита, котороый можно мтроить разу
        can_build_flag = new bool[3];//список возможного стрительства в виде флагов
        for (int i = 0; i <= n; i++) can_build_flag[i] = true;//например при n=1 в городе можно стрить легкую пехоту(0), тяжедую пехоту (1), кавалерию (2) нельзя
        GameObject obj_player = GameObject.Find("land");
        //к объекту привязан свой скрипт ищем его
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
        game_s = obj_player.GetComponent(typeof(game)) as game;
        obj_mouse = obj_player.GetComponent(typeof(mouse)) as mouse;
        count_hod_start = -1;//при старте он -1 чтобы юниты не создавались
        id_unit = -1;
    }
    public bool can_any_build()
    {//проверка на то что город может хоть что-то строить 
        bool flag = false;
        for (int i=0;i<can_build_flag.Length;i++)
        {
            if (can_build_flag[i]) flag = true;
        }
        return flag;
    }
    public void create_neutral_unit()
    {//строителсьтво войска после удачной обороны нейтралами
        //тест уберем
        /*
        if (falg_create_neutral_garnison)
        {//если стоит флаг строительсва нейтрального гарнизона
            garnison = get_garnison_unit_list();//обновим гарнизон
            //посчитаем стоимость его содержания
            int cost_garnison = 0;
            foreach (unit u in garnison) cost_garnison = cost_garnison + u.price;
            cost_garnison = cost_garnison / 2;//стоимость содержания равнаполовине всей стоимости
            unit u_g = garnison[0];//предполагается что хоть один юнит в гарнизоне есть
            
            if ((profit - cost_garnison) >= (u_g.price / 2)) create_unit();//строим юнит
        }
        */
    }
    public bool get_flag_create_garnison()
    {//получение флага строительства нейтрального гарнизона
        return falg_create_neutral_garnison;
    }
    public void set_flag_create_garnison(bool f)
    {//установка флага строительства нейтрального гарнизона
        falg_create_neutral_garnison =f;
        if (f) setting_activ_city(data.num_neutral_unit_build);//если флаг ставится, то настравиваем стройку
        else setting_activ_city(-1);//ничего не строим
    }
    public bool check_koordinat(Vector3 k)
    {//метод сравнения координат
        double delta_x = koordinat.x - k.x;
        double delta_y = koordinat.y - k.y;
        if ((delta_x <= 0.2) & (delta_y <= 0.2)) return true;
        else return false;
    }
}
