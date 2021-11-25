using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class game : MonoBehaviour
{
    // Start is called before the first frame update
    data_game data;
    List<gamer> gamer_list = new List<gamer>();//список игроков
    List<GameObject> city_obj_list = new List<GameObject>();//список объект городов
    public GameObject city_prefab;//префаб города
    public GameObject unit_prefab;//префаб юнита
    public GameObject butt_end;//тест кнопка конца хода
    mouse obj_mouse;//объект с скриптами мыши
    public int num_tek_igrok = 1;
    Sprite[] spr_unit_grey, spr_unit_bel, spr_unit_dark, spr_unit_zel, spr_unit_orange;//наборы спрайтов юнитов
    Sprite[] spr_city_grey, spr_city_bel, spr_city_dark, spr_city_zel, spr_city_orange;//наборы спрайтов городов
    List<Sprite[]> spr_list_city = new List<Sprite[]>();//список со спратами городов по номеру игрока
    List<Sprite[]> spr_list_unit = new List<Sprite[]>();//список со спратами юнитов по номеру игрока
    int index_unit = 0;//счетчик для перебора юнитов
    void Start()
    {
        GameObject obj_player = GameObject.Find("land");
        //к объекту привязан свой скрипт ищем его
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
        obj_mouse = obj_player.GetComponent(typeof(mouse)) as mouse;
        //подгружаем спрайты юнитов
        {
            spr_unit_grey = Resources.LoadAll<Sprite>("sprite/army/grey");//спрайты нейтралов
            spr_unit_bel = Resources.LoadAll<Sprite>("sprite/army/bel");//спрайты игрок 1
            spr_unit_dark = Resources.LoadAll<Sprite>("sprite/army/dark");//спрайты игрок 2
            spr_unit_zel = Resources.LoadAll<Sprite>("sprite/army/zel");//спрайты игрок 3
            spr_unit_orange = Resources.LoadAll<Sprite>("sprite/army/orange");//спрайты игрок 4
            spr_list_unit.Add(spr_unit_grey);
            spr_list_unit.Add(spr_unit_bel);
            spr_list_unit.Add(spr_unit_dark);
            spr_list_unit.Add(spr_unit_zel);
            spr_list_unit.Add(spr_unit_orange);
            //подгружаем спрайты городов
            spr_city_grey = Resources.LoadAll<Sprite>("sprite/city/grey_city");//спрайты нейтралов
            spr_city_bel = Resources.LoadAll<Sprite>("sprite/city/bel_city");//спрайты игрок 1
            spr_city_dark = Resources.LoadAll<Sprite>("sprite/city/dark_city");//спрайты игрок 2
            spr_city_zel = Resources.LoadAll<Sprite>("sprite/city/gren_city");//спрайты игрок 3
            spr_city_orange = Resources.LoadAll<Sprite>("sprite/city/orange_city");//спрайты игрок 4
            spr_list_city.Add(spr_city_grey);
            spr_list_city.Add(spr_city_bel);
            spr_list_city.Add(spr_city_dark);
            spr_list_city.Add(spr_city_zel);
            spr_list_city.Add(spr_city_orange);
        }
        //создаем игроков
        create_gamers();
        gamer_list[1].active = true;//ход первому игроку
        data.tek_activ_igrok = gamer_list[1];//запмоним активного игрока
        
        initial_place();//создаем и расставляем города
        //переместим камеру в город первого игрока
        data.set_activ_untit(data.tek_activ_igrok.skript_unit_list[0]);//активный юнит 
        data.move_cam(data.tek_activ_igrok.city_list[0].koordinat);
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void end_turn()//обработка кнопки конца хода
    {
        foreach (GameObject p in data.spisok_puti) Destroy(p);//подчистим старые пути
        
        int next = data.tek_activ_igrok.id;
        int count = 0;//ограничитель количества кругов
        while (count < 20)//ищем очередного игрока
        {
            next++;
            count++;
            if (next >= gamer_list.Count) next = 1;
            if (gamer_list[next].still_play)
            {
                gamer_list[next].active = true;
                num_tek_igrok = next;
                //ходы юнитов игрока на максимум
                foreach (unit u in gamer_list[next].skript_unit_list) u.tek_hod = u.max_hod;
                //итерация производства юнитов во всех городах игрока
                foreach (city c in gamer_list[next].city_list) c.create_unit();
                break;
            }
            next++;
            count++;
        }
        print("Ходит игрок  " + num_tek_igrok);
        butt_end.GetComponentInChildren<Text>().text = num_tek_igrok.ToString();//тест покажем номер активного игрока
        data.tek_activ_igrok = gamer_list[num_tek_igrok];//сохраним в дате текущего игрока
        //переведем камеру на юныты игрока
        if (data.tek_activ_igrok.obj_unit_list.Count>0)
        {
            //Vector3 tmp_vect=data.tek_activ_igrok.unit_list[0]
            data.move_cam(data.tek_activ_igrok.skript_unit_list[0].koordinat);
            data.set_activ_untit(data.tek_activ_igrok.skript_unit_list[0]);
            index_unit = 0;
        }//либо на первый город
        else
        {
            if (data.tek_activ_igrok.city_list.Count > 0) data.move_cam(data.tek_activ_igrok.city_list[0].koordinat);
        } 
            
    }
    public void deselect_button()// обработка кнопки нет активного юнита
    {
        data.set_activ_untit(null);
        obj_mouse.kursor.gameObject.SetActive(false);// курсор невидим
        foreach (GameObject p in data.spisok_puti) Destroy(p);//подчистим старые пути
    }
    public void next_unit_button()// обработка кнопки перебор юнитов
    {
        // поиск следующего юнита для активации 
        if (data.tek_activ_igrok.obj_unit_list.Count > 0)
        {
            index_unit++;
            if (index_unit >= data.tek_activ_igrok.obj_unit_list.Count) index_unit = 0;
            data.move_cam(data.tek_activ_igrok.skript_unit_list[index_unit].koordinat);
            data.set_activ_untit(data.tek_activ_igrok.skript_unit_list[index_unit]);
            foreach (GameObject p in data.spisok_puti) Destroy(p);//подчистим старые пути
            obj_mouse.kursor.gameObject.SetActive(false);// курсор невидим
        }//либо на первый город
    }
    public void create_gamers()//создание игроков
    {
        for (int i = 0; i < 5; i++)
        {
            gamer_list.Add(new gamer(i));
            gamer_list[i].spr_city = spr_list_city[i][0];//игрок запоминает спратй своего гороа
        }
    }
    public void initial_place()//старотвая расстановка юнитов и городов
    {
        Vector2[] list_city_kor = new Vector2[9];
        list_city_kor[0] = new Vector2(-2.8f,2.8f);
        list_city_kor[1] = new Vector2(0f, 2.8f);
        list_city_kor[2] = new Vector2(2.8f, 2.8f);
        list_city_kor[3] = new Vector2(-2.8f, 0f);
        list_city_kor[4] = new Vector2(0f, 0f);
        list_city_kor[5] = new Vector2(2.8f, 0f);
        list_city_kor[6] = new Vector2(-2.8f, -2.8f);
        list_city_kor[7] = new Vector2(0f, -2.8f);
        list_city_kor[8] = new Vector2(2.8f, -2.8f);
        for (int i = 0; i < 9; i++)
        {
            Vector3 koor = new Vector3(list_city_kor[i].x, list_city_kor[i].y, -2.0f);
            GameObject obj_city = (GameObject)Instantiate(city_prefab, koor, Quaternion.identity);
            city tmp_city= obj_city.GetComponent(typeof(city)) as city;
            tmp_city.koordinat = koor;//скрипт города хранит его координаты
            tmp_city.vladelec = gamer_list[0];//пока все города владеет нейтрал
            city_obj_list.Add(obj_city);

        }
        //раздаем города игрокам
        change_city_player(1, 0);
        change_city_player(2, 2);
        change_city_player(3, 6);
        change_city_player(4, 8);
        //даем стартовые юниты игрокам
        for (int i = 1; i < 5; i++)
        {
            gamer_list[i].spr_city = spr_list_city[i][0];//игрок запоминает спратй своего гороа
            //координаты места где должен появится новый юниит
            Vector3 koor_unit = new Vector3(gamer_list[i].city_list[0].koordinat.x-0.2f, gamer_list[i].city_list[0].koordinat.y + 0.2f, gamer_list[i].city_list[0].koordinat.z);
            GameObject unit_tmp = (GameObject)Instantiate(unit_prefab, koor_unit, Quaternion.identity);
            unit tmp = unit_tmp.GetComponent(typeof(unit)) as unit;//на скрипт
            tmp.set_koordinat(koor_unit);//юнит запомнит свой координат
            //передаем юнита игроку
            change_unit_player(i, unit_tmp, 2);//меняем тип юнита и передаеи его игкроку


        }
    }
    public void change_city_player(int num_igrok, int num_gorod)//метод которые меняет владельца у игрока
    {
        city tmp_city = city_obj_list[num_gorod].GetComponent(typeof(city)) as city;
        tmp_city.change_vladelec(gamer_list[num_igrok]);//передает игрока 1 в город
        gamer_list[num_igrok].city_list.Add(tmp_city);//добавляем город игроку
    }
    //метод передачи юнита игроку
    public void change_unit_player(int num_igrok, GameObject u, int num_type_unit)
    {
        u.GetComponent<SpriteRenderer>().sprite = spr_list_unit[num_igrok][num_type_unit];//выставляем спрайт
        unit tmp_unit = u.GetComponent(typeof(unit)) as unit;//на скрипт
        tmp_unit.set_unit(num_type_unit, gamer_list[num_igrok], spr_list_unit[num_igrok][num_type_unit]);//настраиваем юнит
        gamer_list[num_igrok].obj_unit_list.Add(u);//список объектов
        gamer_list[num_igrok].skript_unit_list.Add(tmp_unit);//скриптов к объектам

    }
    
}
