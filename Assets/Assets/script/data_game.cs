using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// в данном классе мы храним все данные игры
public class data_game : MonoBehaviour
{
    // Start is called before the first frame update
    //setting game--------------------------
    public float start_gold = 10f;//стартовый капитал
    public int min_city_dohod = 16;//vbybvfkmys доход города
    public int max_city_dohod = 32;//максимальный доход города
    public float koef_cost = 0.5f;//стоимость содержания воиск 0.5 от стоимости
    int count_player = 2;//количесвто игроков
    public int start_unit = 1;//старотовые юниты всех игроков 0 легкая пехота, 1 тяжедая, 2 рыцыри
    //-----------------------
    private unit activ_unit;//активный юнит
    private bool is_army_move = false;//флаг что армия находится в движении
    private s_army activ_army;//активная армия
    public city activ_city;//активный город
    public game game_s;//класс со скриптом игры
    public s_army def_army;//защищиающаяся армия
    public int type_event = 1;//текущее событие 1-перемещение, 2 атака, 3 атака города
    gamer tek_activ_igrok;
    public item_cell can_move_cell;//ячейка, до которой юниту хватит очков хода
    public List<item_cell> can_move_cell_list = new List<item_cell>();//список ячеек куда может пойти юнит
    public List<GameObject> spisok_puti;//список объектов пути
    public Camera Cam;//камера
    public GameObject city_window;//окно города
    public GameObject attack_window;//окно атаки общее
    public GameObject attack_window_8;//окно атаки на 8 юниов
    public GameObject attack_window_16;//окно атаки
    public GameObject attack_window_24;//окно атаки
    public GameObject attack_window_32;//окно атаки
    public s_panel_unit units_panel_s;//скрипт панели с юнитами
    public s_panel_city city_panel_s;//скрипт панели с городом
    public s_panel_attack atack_panel_s;//скрипт напнели сатакой
    public s_panel_attack atack_panel_s_8;//скрипт напнели сатакой на 8 юнитов
    public s_panel_attack atack_panel_s_16;//скрипт напнели сатакой на 16 юнитов
    public s_panel_attack atack_panel_s_24;//скрипт напнели сатакой на 24 юнитов
    public s_panel_attack atack_panel_s_32;//скрипт напнели сатакой на 32 юнитов
    public s_main_panel main_panel;//скрипт главноей панели
    public Vector2Int st_p, fin_p;//индексы координат х и у в массиве координат grid_x и grid_y
    public float[] grid_x=new float[18];
    public float[] grid_y = new float[18];
    public int max_kletka_x= 17;
    public int min_kletka_x = 0;
    public int max_kletka_y = 17;
    public int min_kletka_y = 0;
    public int id_unit_count = 0;//счетчик индефикаторов юниов
    public int id_army_count = 0;//счетчик индефикаторов армий
    public item_cell[,] kletki;//двумерный массив с объектами клеток
    void Start()
    {
        start_gold = 10f;
        GameObject obj_player = GameObject.Find("land");
        game_s = obj_player.GetComponent(typeof(game)) as game;//найдем главный скрипт с игрой
        //надем скрипт с панелью юнитов
        units_panel_s = GameObject.Find("Panel_unit").GetComponent(typeof(s_panel_unit)) as s_panel_unit;//найдем скрипт панели юнитов
        city_panel_s = GameObject.Find("Panel_city").GetComponent(typeof(s_panel_city)) as s_panel_city;//найдем главный скрипт панели города
        atack_panel_s_8= GameObject.Find("Panel_attack_8").GetComponent(typeof(s_panel_attack)) as s_panel_attack;//найдем скрипт панели с атакой
        atack_panel_s_16 = GameObject.Find("Panel_attack_16").GetComponent(typeof(s_panel_attack)) as s_panel_attack;//найдем скрипт панели с атакой 16
        atack_panel_s_24 = GameObject.Find("Panel_attack_24").GetComponent(typeof(s_panel_attack)) as s_panel_attack;//найдем скрипт панели с атакой 24
        atack_panel_s_32 = GameObject.Find("Panel_attack_32").GetComponent(typeof(s_panel_attack)) as s_panel_attack;//найдем скрипт панели с атакой 32
        main_panel = GameObject.Find("main_panel").GetComponent(typeof(s_main_panel)) as s_main_panel;//найдем скрипт панели главной панелью
        attack_window_8.SetActive(false);
        attack_window_16.SetActive(false);
        attack_window_24.SetActive(false);
        attack_window_32.SetActive(false);
        city_window.SetActive(false);//скроем панели;
        
        //двумерный массив с объектами клеток
        kletki = new item_cell[max_kletka_x + 1, max_kletka_y + 1];
        int count = 0;
        //заполнение сетки
        for (int i=0;i<18;i++)
        {
            grid_x[i] = -3.4f + i * 0.4f;
            grid_y[i] = 3.4f - i * 0.4f;
        }
        for (int i=0; i<18;i++)
            for (int j = 0; j < 18; j++)
            {
                GameObject gameObject = new GameObject("item_cell");
                item_cell ic = gameObject.AddComponent<item_cell>();
                ic.set_indx(new Vector2Int(i, j));
                kletki[i, j] = ic;
                kletki[i, j].set_kordinat(new Vector2(grid_x[i], grid_y[j]));//заполняем индексы и координаты клеток
                kletki[i, j].set_cost_move(2);//стоимость движения по клеткам 2х
                kletki[i, j].id = count;//у каждой клетки совй номер
                if ((j == 2) & (((i > 2) & (i < 8)) || ((i > 9) & (i < 15)))) kletki[i, j].set_cost_move(1);//дорога
                if ((j == 9) & (((i > 2) & (i < 8)) || ((i > 9) & (i < 15)))) kletki[i, j].set_cost_move(1);//дорога
                if ((j == 16) & (((i > 2) & (i < 8)) || ((i > 9) & (i < 15)))) kletki[i, j].set_cost_move(1);//дорога
                if ((i == 2) & (((j > 2) & (j < 8)) || ((j > 9) & (j < 15)))) kletki[i, j].set_cost_move(1);//дорога
                if ((i == 9) & (((j > 2) & (j < 8)) || ((j > 9) & (j < 15)))) kletki[i, j].set_cost_move(1);//дорога
                if ((i == 15) & (((j > 2) & (j < 8)) || ((j > 9) & (j < 15)))) kletki[i, j].set_cost_move(1);//дорога
                count++;//счетчик клеток
            }
    }
    public void set_activ_untit(unit u)//установка активного юнита
    {
        activ_unit = u;
        if (u != null)
        {
            setting_panel_unit();//настроим панель с юнитами
            set_activ_army(u.sc_army);
        }
    }
    public void set_activ_army(s_army a)//установка активного игрока
    {
        activ_army = a;
        
        if (a != null)
        {
            setting_panel_unit();//настроим панель с юнитами
        }
        
    }
    public unit get_activ_unit()//получение активного юнита
    {
        
        return activ_unit;
    }
    public s_army get_activ_army()//получение активного юнита
    {
        return activ_army;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    //метод получения ближайшей точки
    public Vector3 get_grid_step(Vector3 point)
    {
        float x_p = point.x;
        float y_p = point.y;
        float z_p = point.z;
        float x_g=0, y_g=0, z_g = z_p;
        float min_x = 100, min_y = 100;
        for (int i=0;i<18;i++)//перебираем массивы сточкми
        {
            if (Math.Abs(grid_x[i]-x_p)<= min_x)//ищем минимальное расхождение по х
            { 
                min_x = Math.Abs(grid_x[i] - x_p);
                x_g = grid_x[i];
            }
            if (Math.Abs(grid_y[i] - y_p)<= min_y)//ищем минимальное расхождение по у
            {
                min_y = Math.Abs(grid_y[i] - y_p);
                y_g = grid_y[i];
            }
        }
        return new Vector3(x_g, y_g, z_p);
    }
    public void set_st_f_point_activ_army( Vector3 point_f)//функкця установки стартовой финишной точки для поиска пути, получает координаты старотовой точки и запоминает их индекс
    {//для активной армии
        Vector3 point_s = get_activ_army().transform.position;
        for (int i = 0; i < 18; i++)//перебираем массивы сточкми
        {
            if (grid_x[i] == point_s.x) 
                st_p.x = i;//ищем старт х
            if (grid_x[i] == point_f.x) 
                fin_p.x = i;//ищем финиш х
            if (grid_y[i] == point_s.y) 
                st_p.y = i;//ищем старт y
            if (grid_y[i] == point_f.y) 
                fin_p.y = i;//ищем финиш y
        }

    }
    public void set_st_f_point(Vector3 start, Vector3 finish)//функкця установки стартовой финишной точки для поиска пути, получает координаты старотовой и финишной точки 
    {//для любой пары точек
        for (int i = 0; i < 18; i++)//перебираем массивы сточкми
        {
            if (Math.Abs(grid_x[i] - start.x)<0.01)
                st_p.x = i;//ищем старт х
            if (Math.Abs(grid_x[i] - finish.x) < 0.01)
                fin_p.x = i;//ищем финиш х
            if (Math.Abs(grid_y[i] - start.y) < 0.01)
                st_p.y = i;//ищем старт y
            if (Math.Abs(grid_y[i] - finish.y) < 0.01)
                fin_p.y = i;//ищем финиш y
       }

    }
    //перемещение камеры к нужному объекту
    public void move_cam(Vector3 k)
    {
        Vector3 tmp = k;
        k.z = -5.0f;//камера будет выше всех
        Cam.transform.position = k;
    }
    public void setting_panel_unit()//метод настройкт панели с юнитам
    {
        List<unit> point_unit_list = new List<unit>();//список юнитов в точке сактивным юнитом
        //сразу занесем в список юниты активной армии
        foreach (unit tmp_unit in activ_army.get_unit_list()) point_unit_list.Add(tmp_unit);
        //перебираем все армии игрока
        
        foreach (s_army tmp_army in tek_activ_igrok.s_army_list)
        {
            if (!tmp_army.Equals(activ_army))
            {
                if ((activ_army.koordinat.x == tmp_army.koordinat.x) &
                        (activ_army.koordinat.y == tmp_army.koordinat.y))
                //если армия в тех же координатах что и активная
                {
                    foreach (unit tmp_unit in tmp_army.get_unit_list())//перебираем все второй армии
                    {
                        //если таких юнитов там еще не было занесем в спиок
                        point_unit_list.Add(tmp_unit);
                        
                    }
                }
            }
        }
        units_panel_s.set_panel_unit(point_unit_list);
    }
    public void set_count_players(int n)
    {
        count_player = n;
    }
    public int get_count_players()
    {
        return count_player;
    }


    public bool get_flag_army_is_move()
    {//получить флаг состоятния движения армий
        return is_army_move;
    }
    public void set_flag_army_is_move(bool f)
    {//установить флаг состоятния движения армий
        is_army_move=f;
    }
    public gamer get_activ_igrok()
    {
        return tek_activ_igrok;
    }
    public void set_activ_igrok(gamer g)
    {
        tek_activ_igrok = g;
    }
}
