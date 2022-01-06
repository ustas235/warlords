using System.Collections.Generic;
using System.Linq;
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
    public GameObject flag_prefab;//префаб флага
    public GameObject butt_end;//тест кнопка конца хода
    mouse obj_mouse;//объект с скриптами мыши
    public int num_tek_igrok = 1;
    Sprite[] spr_unit_grey, spr_unit_bel, spr_unit_dark, spr_unit_zel, spr_unit_orange, spr_unit_off;//наборы спрайтов юнитов
    Sprite[] spr_flag_grey, spr_flag_bel, spr_flag_dark, spr_flag_zel, spr_flag_orange;//наборы спрайтов флагов
    Sprite[] spr_city_grey, spr_city_bel, spr_city_dark, spr_city_zel, spr_city_orange;//наборы спрайтов городов
    List<Sprite[]> spr_list_city = new List<Sprite[]>();//список со спратами городов по номеру игрока
    List<Sprite[]> spr_list_unit = new List<Sprite[]>();//список со спратами юнитов по номеру игрока
    List<Sprite[]> spr_list_unit_off = new List<Sprite[]>();//список со спратами выключенных юнитов
    int index_unit = 0;//счетчик для перебора юнитов
    public int id = 0;
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
            spr_unit_off = Resources.LoadAll<Sprite>("sprite/army/desel");//спрайты выключенных юнитов
            spr_list_unit.Add(spr_unit_grey);
            spr_list_unit.Add(spr_unit_bel);
            spr_list_unit.Add(spr_unit_dark);
            spr_list_unit.Add(spr_unit_zel);
            spr_list_unit.Add(spr_unit_orange);
            //подгружаем спрайты с флагами
            spr_flag_grey = Resources.LoadAll<Sprite>("sprite/flags/flag_grey");//флаги нейтралов
            spr_flag_bel = Resources.LoadAll<Sprite>("sprite/flags/flag_bel");//флаги игрок 1
            spr_flag_dark = Resources.LoadAll<Sprite>("sprite/flags/flag_dark");//флаги игрок 2
            spr_flag_zel = Resources.LoadAll<Sprite>("sprite/flags/flag_zel");//флаги игрок 3
            spr_flag_orange = Resources.LoadAll<Sprite>("sprite/flags/flag_orange");//флаги игрок 4
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
        data.set_activ_army(data.tek_activ_igrok.s_army_list[0]);//активный юнит 
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
                foreach (s_army army in gamer_list[next].s_army_list) 
                    army.reboot_count_hod();//обновляем ходы в армиях
                //итерация производства юнитов во всех городах игрока
                foreach (city c in gamer_list[next].city_list) c.create_unit();
                break;
            }
            
        }
        print("Ходит игрок  " + num_tek_igrok);
        butt_end.GetComponentInChildren<Text>().text = num_tek_igrok.ToString();//тест покажем номер активного игрока
        data.tek_activ_igrok = gamer_list[num_tek_igrok];//сохраним в дате текущего игрока
        //переведем камеру на юныты игрока
        if (data.tek_activ_igrok.obj_army_list.Count > 0)
        {
            //Vector3 tmp_vect=data.tek_activ_igrok.unit_list[0]
            data.move_cam(data.tek_activ_igrok.s_army_list[0].koordinat);
            data.set_activ_army(data.tek_activ_igrok.s_army_list[0]);
            index_unit = 0;
        }//либо на первый город
        else
        {
            if (data.tek_activ_igrok.city_list.Count > 0) data.move_cam(data.tek_activ_igrok.city_list[0].koordinat);
        }
        if (data.tek_activ_igrok.bot_flag) data.tek_activ_igrok.action_bot();//если бут то пусть он сам играет

    }
    public void deselect_button()// обработка кнопки нет активного юнита
    {
        data.set_activ_army(null);
        obj_mouse.kursor.gameObject.SetActive(false);// курсор невидим
        foreach (GameObject p in data.spisok_puti) Destroy(p);//подчистим старые пути
    }
    public void next_unit_button()// обработка кнопки перебор юнитов
    {
        // поиск следующего юнита для активации 
        if (data.tek_activ_igrok.obj_army_list.Count > 0)
        {
            index_unit++;
            if (index_unit >= data.tek_activ_igrok.obj_army_list.Count) index_unit = 0;
            data.move_cam(data.tek_activ_igrok.s_army_list[index_unit].koordinat);
            data.set_activ_army(data.tek_activ_igrok.s_army_list[index_unit]);
            foreach (GameObject p in data.spisok_puti) Destroy(p);//подчистим старые пути
            obj_mouse.kursor.gameObject.SetActive(false);// курсор невидим
        }//либо на первый город
    }
    public void create_gamers()//создание игроков
    {
        for (int i = 0; i < 5; i++)
        {
            gamer tmp_gamer;
            //боты играют за игроков начиная со второго
            if (i > 1) tmp_gamer = new gamer(i, true);
            else tmp_gamer = new gamer(i, false);
            gamer_list.Add(tmp_gamer);
            gamer_list[i].spr_city = spr_list_city[i][0];//игрок запоминает спратй своего города
        }
    }
    public void initial_place()//старотвая расстановка юнитов и городов
    {
        Vector2[] list_city_kor = new Vector2[9];
        list_city_kor[0] = new Vector2(-2.8f, 2.8f);
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
            city tmp_city = obj_city.GetComponent(typeof(city)) as city;
            tmp_city.koordinat = koor;//скрипт города хранит его координаты
            tmp_city.vladelec = gamer_list[0];//пока все города владеет нейтрал
            city_obj_list.Add(obj_city);

        }
        //раздаем города игрокам
        change_city_player(0, 1);
        change_city_player(0, 3);
        change_city_player(0, 4);
        change_city_player(0, 5);
        change_city_player(0, 7);
        change_city_player(1, 0);
        change_city_player(2, 2);
        change_city_player(3, 6);
        change_city_player(4, 8);
        //даем стартовые юниты игрокам
        for (int i = 0; i < 5; i++)
        {
            //координаты места где должен появится новый юниит
            //перебираем города игрока и закидываем туда стартовые юниты
            foreach (city c in gamer_list[i].city_list)
            {
                Vector3 koor_unit = new Vector3(c.koordinat.x - 0.2f, c.koordinat.y + 0.2f, c.koordinat.z);
                unit tmp_unit_s = new unit();
                tmp_unit_s.set_koordinat(koor_unit);
                tmp_unit_s.set_unit(2, gamer_list[i], get_sprite_unit(i, 2), spr_unit_off);//настраиваем юнит
                GameObject flag_tmp_obj = (GameObject)Instantiate(flag_prefab, koor_unit, Quaternion.identity);//создаем объект флага
                tmp_unit_s.flag = flag_tmp_obj;//армия запоминает свой флаг
                tmp_unit_s.flags_sprites = get_sprite_flag(tmp_unit_s.vladelec.id);
                create_new_army(tmp_unit_s);//создаем армию на основе юнита
            }
        }
    }
    public void change_city_player(int num_igrok, int num_gorod)//метод которые меняет владельца у игрока
    {
        city tmp_city = city_obj_list[num_gorod].GetComponent(typeof(city)) as city;
        tmp_city.change_vladelec(gamer_list[num_igrok]);//передает игрока 1 в город
    }
    //метод передачи юнита игроку

    public void create_new_army(unit u)//создание новой армии из юнита
    {
        Vector3 koor_unit = u.koordinat;//координаты создания армии равны координатам юнита
        GameObject army_tmp_obj = (GameObject)Instantiate(unit_prefab, koor_unit, Quaternion.identity);//создаем объект армии
        s_army tmp_army_s = army_tmp_obj.GetComponent(typeof(s_army)) as s_army;//на скрипт
        tmp_army_s.set_koordinat(koor_unit);//армия запомнит свой координат
        tmp_army_s.set_obj(army_tmp_obj);//армия запомнит объект
        tmp_army_s.vladelec = u.vladelec;
        
        
        //передаем юнита игроку
        army_tmp_obj.GetComponent<SpriteRenderer>().sprite = u.spr_unit;//выставляем спрайт
        tmp_army_s.unit_list.Add(u);//добавим в армию новый юнит
        tmp_army_s.set_army();
        u.sc_army = tmp_army_s;//унит запоминает свою армию
        gamer_list[u.vladelec.id].obj_army_list.Add(army_tmp_obj);//список объектов
        gamer_list[u.vladelec.id].s_army_list.Add(tmp_army_s);//скриптов к объектам
        
    }
    public Sprite get_sprite_unit(int num_igrok, int nim_type)//получение спрайта юнита по номеру игрока и номеру юнита
    {
        return spr_list_unit[num_igrok][nim_type];
    }
    public Sprite[] get_sprite_flag(int num_igrok)//получение спрайтов флага по номеру игрока
    {
        Sprite[] tmp_spr;
        switch(num_igrok)
        {
            case 0:
                tmp_spr = spr_flag_grey;
                break;
            case 1:
                tmp_spr = spr_flag_bel;
                break;
            case 2:
                tmp_spr = spr_flag_dark;
                break;
            case 3:
                tmp_spr = spr_flag_zel;
                break;
            case 4:
                tmp_spr = spr_flag_orange;
                break;
            default:
                tmp_spr = spr_flag_grey;
                break;
        }
        return tmp_spr;
    }
    public Sprite[] get_sprite_unit_off()
    {
        return spr_unit_off;
    }
    public void finih_atack(List<unit> unit_list_atack, List<bool> f_a, List<unit> unit_list_def, List<bool> f_d)
    {//метод обрабатывае результаты боя за город
        //удалим убитые юниты
        for (int i=0;i< unit_list_atack.Count;i++)
        {
            if (!f_a[i])
            {
                unit_list_atack[i].sc_army.sub_unit_destroy(unit_list_atack[i]);//если юнит убит удалим юнит
                f_a.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < unit_list_def.Count; i++)
        {
            if (!f_d[i])
            {
                unit_list_def[i].sc_army.sub_unit_destroy(unit_list_def[i]);//если юнит убит удалим юнит
                unit_list_def.Remove(unit_list_def[i]);//т.к. список защитников был сощдан отдель чистим и его
                f_d.RemoveAt(i);
                i--;
            }
        }
        //если все защитники пали и была защита города, то город передается новому владельцу
        if ((unit_list_def.Count<1) & (data.def_city != null)) data.def_city.change_vladelec(unit_list_atack[0].vladelec);
        data.def_city = null;//сбросим ссылку на защ город
        //unit_list_atack.Clear();
        //unit_list_def.Clear();
        data.setting_panel_unit();//настроим панель с юнитами
    }
    
}
