using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamer : MonoBehaviour//
{
    public List<city> city_list = new List<city>();//список городов
    public List<GameObject> obj_army_list = new List<GameObject>();//список объектов юнитов
    //public List<s_army> skript_army_list = new List<s_army>();//список скприптов юнитов
    public List<s_army> s_army_list = new List<s_army>();//список скприптов армий
    public List<s_army> s_army_to_target_list = new List<s_army>();//список армий которые пойдут в атаку
    bool start_move_to_target = false;//флаг по которому в update будет дано разрешение для начала движения всех арвий
    int num_tek_army_to_taget=0;//номер текущей арвии (для перебора черз update
    public data_game data;//класс где буду хранится все данные игры
    int money = 0;
    public int id = 0;//id игрока, он же номер
    public bool still_play = true;//флаг что игрок еще играет
    public bool active = false;//флаг что ход игрока
    public Sprite spr_city;//ссылка на спрат своего города
    //настройки бота
    public bool bot_flag = false;//флаг что играетбот
    public int bot_army_create_city = 1;//тип войск строимых ботом
    public int bot_min_garnison = 0;//гарнизон в городе
    public List<item_cell> bot_put_cell_list;//список ячеек пути
    List<item_cell> tmp_bot_put_cell_list;//временный список список ячеек пути (станет постоянным для ближайшего города)
    Vector3 tmp_target_koordinat;//временная точка татаки
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public gamer(int num, bool flag_bot)//конструктор принимает номер и тип игроа бот/не бот
    {
        id = num;
        bot_flag = flag_bot;
        GameObject obj_player = GameObject.Find("land");
        //к объекту привязан свой скрипт ищем его
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
        money = data.start_money;
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
        {//строим войска и формируем гарнизоны
            if (c.count_hod_start<0)
                c.setting_activ_city(bot_army_create_city);//строим юниты bot_army_create_city го уровня
            if (accumulation_garnison(c))//строим гарнизон
                accumulation_army(c);//если гарнизон готов, строим армию атаки

        }
        //ищем цели для каждой армии атаки
        s_army_to_target_list.Clear();
        foreach (s_army a in s_army_list)
        {
            switch (a.get_status())
            {//перебираем все армии
                case 2://если армия готовится к атаке
                    search_target(a);//ищем ближайший вражеский город, армия запомнит атакуемый город
                    if (training_army_compleed(a,a.get_target_city()))//проверяем хватит ли сил для атаки
                    {
                        s_army_to_target_list.Add(a);//добавляем в список
                    }
                    break;
                case 3://если идет в атаку
                    s_army_to_target_list.Add(a);//добавляем в список
                    break;
                case 4://если идет в свой город
                    s_army_to_target_list.Add(a);//добавляем в список
                    break;
                default:
                    break;
            }
            
        }
        if (s_army_to_target_list.Count>0)
        {
            data.set_flag_army_is_move(false);//флаг, что нет армии не законившей свое движение
            num_tek_army_to_taget = 0;//указатель на первую армию
            start_move_to_target = true;//старт движения армий из списка
        }
    }
    //накопление гарнизонов городах
    public bool accumulation_garnison(city c)
    {
        List<unit> unit_city_list = new List<unit>();//список всех юнитов города
        bool flag_garnison_ready = false;
        s_army tmp_army_garnison = new s_army();//гарнизон
        int count_ready_garnison_unit = 0;//количесто юнитов уже записаных в гарнизон
        List<unit> garnison_unit_list;//список юнитов в городе
        garnison_unit_list = c.get_garnison_unit_list();
        foreach (unit u in garnison_unit_list)
        {
            if (u.status_untit==0)  
                unit_city_list.Add(u);//записываем очередной юнит в список
            if (u.status_untit == 1)
            {
                count_ready_garnison_unit++;
                tmp_army_garnison = u.sc_army;
            }
        }

        //набираем гарнизон
        if (count_ready_garnison_unit >= bot_min_garnison)//если юниты уже были набраны, то не будем заново набирать гарнизон
        {
            flag_garnison_ready = true;
        }
        else
        {
            if (unit_city_list.Count >= bot_min_garnison)
            {
                //если первый юнит состоял в большой армии, то создадим ему свою
                tmp_army_garnison = unit_city_list[0].sc_army;
                unit_city_list[0].status_untit = 1;//поменяем статус юнита
                tmp_army_garnison.set_status(1);//поменяем статус армии
                    //юниты записываем в армию первого юнита
                for (int i = 1; i < bot_min_garnison; i++)
                {
                    tmp_army_garnison.add_unit(unit_city_list[i]);//записываем в армию гарнизона очередного юнита
                    unit_city_list[i].status_untit = 1;//юниты помнят что они в гарнизоне
                }
                tmp_army_garnison.move_army(c.koordinat_garnizon);//перемещаем в точку гарнизона
                tmp_army_garnison.set_status(1);
                if (tmp_army_garnison.get_unit_list().Count >= bot_min_garnison) flag_garnison_ready = true;
            }
        }
        return flag_garnison_ready;
    }
   
    public void accumulation_army(city c)
    { //накопление ударной армии в городе
        List<unit> unit_city_list = new List<unit>();//список всех юнитов города
        s_army tmp_army_atack=new s_army();//армия атаки
        bool flag_unit_atack_is = false;//флаг наличия в городе армии атаки
        List<unit> garnison_unit_list;//список юнитов в городе
        garnison_unit_list = c.get_garnison_unit_list();
        foreach (unit u in garnison_unit_list)
        {
            if (u.status_untit == 0) unit_city_list.Add(u);//записываем очередной юнит в список
            if (u.status_untit == 2)
            {
                tmp_army_atack = u.sc_army;
                flag_unit_atack_is = true;
            }
        }

        //набираем атакюющую армию
        if (unit_city_list.Count > 0)// если есть своюбодные юниты
        {
            //если в городе не было армии атаки, то создадим ее из первого юнита
            if (!flag_unit_atack_is)
            {
                tmp_army_atack = unit_city_list[0].sc_army;
                unit_city_list[0].status_untit = 2;//поменяем статус юнита
                tmp_army_atack.set_status(2); //поменяем статус армии
            }
            //юниты записываем в армию первого юнита
            for (int i = 0; i < unit_city_list.Count; i++)
            {
                if (unit_city_list[i].contains_to_list(tmp_army_atack.get_unit_list())) continue;//если юнит уже в армии то переходи к следующему
                if (tmp_army_atack.add_unit(unit_city_list[i]))//записываем в армию атаки очередного юнита если есть место
                    unit_city_list[i].status_untit = 2;//поменяем статус юнита
                else
                {//места в армии атаки нет
                    break;//заканчиваем набор
                }
            }
            tmp_army_atack.move_army(c.koordinat_atack);//перемещаем в точку армии атаки
            tmp_army_atack.set_status(2);
        }
    }

    //поиск целей для каждой свободной армии
    public void search_target(s_army arm)
    {//ищем ближайший вражеский город
        List<city> tmp_all_city_list = data.game_s.get_city_list();
        List<city> tmp_alien_city_list = new List<city>();
        
        foreach (city c in tmp_all_city_list)
        {//ищем вражеские города
            if (c.vladelec.id != id) tmp_alien_city_list.Add(c);
        }
        int min_point = 10000000;//минимальное количесвто ходов
        int next_min_pount;
        city cit_near;//ближайщий город
        foreach (city c in tmp_alien_city_list)
        {
            next_min_pount = calculate_put(arm,c);
            if (next_min_pount <= min_point)
            {
                min_point = next_min_pount;
                cit_near = c;//находим ближайший
                arm.set_target_city(c);//армия запомнит ближайший город для атаки
                arm.set_target_koordinat(tmp_target_koordinat);//армия запомнит минимальные координаты
                bot_put_cell_list = tmp_bot_put_cell_list;//бот запомни ближайший путь
                tmp_bot_put_cell_list.Clear();

            }
        }
    }
    //проверка армии, что хватит для атаки
    public bool training_army_compleed(s_army arm, city cit)
    {
        bool army_compleed = false;
        int other_strenght_total = 0;//сила вражеского гарнризона, равняеся сумме сил всех юнитов
        int this_strenght_total = 0;//сила гарнризона игрока, равняеся сумме сил всех юнитов
        List<unit> other_unit_list;//список юниов стоящих гарнизоном в другом городе
        other_unit_list = cit.get_garnison_unit_list();//получаем список юнито гарнизона
        foreach (unit u in other_unit_list) other_strenght_total += u.strength;
        foreach (unit u in arm.get_unit_list()) this_strenght_total += u.strength;
        if ((this_strenght_total > other_strenght_total)||(arm.get_unit_list().Count>7)) army_compleed = true;//условие начала похода на вражеский города
        return army_compleed;
    }
    //поход к цели
    public void atack_to_target(s_army arm)
    {
        arm.set_status(3);//армия в атаке
        data.set_activ_army(arm);//передает в данные активной армии
        data.move_cam(arm.koordinat);
        data.game_s.bot_atack_target(arm.get_target_koordinat());
    }
    public int check_target(s_army arm)
    {//проверка цели для атаки
        int status = 0;
        if (arm.get_target_city().vladelec.id == id)//если город уже наш, то отправим туда армию для накопления
        {
            arm.set_status(4);
            status = 4;
        }
        else status = 3;
        return status;
    }
    public void move_to_target(s_army arm)
    {
        arm.set_status(4);//армия в пути
        data.set_activ_army(arm);//передает в данные активной армии
        data.move_cam(arm.koordinat);
        data.game_s.bot_move_target(arm.get_target_koordinat());
    }
    //просчет ходов до города
    public int calculate_put(s_army arm, city c)
    {//стоимости пути
        //каждый город стоит на 4 клетках, найдем минимаьный путь до ближайшей
        List<Vector3> koor_city_list = new List<Vector3>();
        //кооринаты клеток находтся в полушаге от центра во все 4 сторны
        koor_city_list.Add(new Vector3(c.koordinat.x - 0.2f, c.koordinat.y - 0.2f, c.koordinat.z));
        koor_city_list.Add(new Vector3(c.koordinat.x + 0.2f, c.koordinat.y - 0.2f, c.koordinat.z));
        koor_city_list.Add(new Vector3(c.koordinat.x + 0.2f, c.koordinat.y + 0.2f, c.koordinat.z));
        koor_city_list.Add(new Vector3(c.koordinat.x - 0.2f, c.koordinat.y + 0.2f, c.koordinat.z));
        //найдем минимальный путь до каждой
        List<item_cell> tmp_cell_list;
        int min_count = 1000000000;
        int summ_cost_mov;
        Vector3 min_v_city_put;//координаты клектки для минимального пути
        foreach (Vector3 v in koor_city_list)
        {//перибиарем 4 точки
            summ_cost_mov = 0;
            tmp_cell_list = data.game_s.get_put_cell(arm.koordinat, v);//получим список пути
            foreach (item_cell ic in tmp_cell_list) summ_cost_mov += ic.get_cost_move();
            if (summ_cost_mov <= min_count)
            {//ближайшую
                min_count = summ_cost_mov;
                min_v_city_put = v;
                //запомним точку для атаки и путь
                //т.к. расчет выполняется для всех городов, а нужен нам ближайший
                //после определения ближайшего перенесем данные из tmp в армию
                tmp_target_koordinat = min_v_city_put;//армия запомнит минимальные координаты
                tmp_bot_put_cell_list = tmp_cell_list;
            }
        }
        return min_count;
    }
    //функция хода у активного игорка
    public void get_move_to_target()
    {
       
        if (start_move_to_target)//дано разрешение на движение армий
        {

            if (!data.get_flag_army_is_move())//если предыдущая армия закончила движение
            {
                
                if (num_tek_army_to_taget >= s_army_to_target_list.Count)//счетчик армий для движения превысил коичесвто армий для движения, двигатся больше некому
                {
                    start_move_to_target = false;//движение всех армий закончено снимем флаг
                    data.set_flag_army_is_move(false);//нет армий в движении
                    num_tek_army_to_taget = 0;
                }
                else
                {
                    data.set_flag_army_is_move(true);
                    if (check_target(s_army_to_target_list[num_tek_army_to_taget]) == 4)//проверка цели, если город уже захвачен то поменяем статус армии
                        move_to_target(s_army_to_target_list[num_tek_army_to_taget]);//и отправим ее в завоевоанный город
                    else
                        atack_to_target(s_army_to_target_list[num_tek_army_to_taget]);//иначе отправим этот город завоевывать
                    num_tek_army_to_taget++;//переставлем номер на следующую армию
                }
            }
        }
    }
}
