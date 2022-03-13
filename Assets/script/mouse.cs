using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class mouse : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 MousePos=new Vector3(-2.5f, -1.7f, 0f);
    public GameObject kursor;
    List<GameObject> spisok_puti=new List<GameObject>();//список объектов пути
    public GameObject point_put;//объект с точкой пути
    public GameObject point_put_x;//объект с точкой пути куда нехватает очков перемещени€
    public Sprite spr_attack;
    public Sprite spr_incity;
    public Sprite spr_move;
    public data_game data;//класс где буду хранитс€ все данные игры

    List<item_cell> open_list = new List<item_cell>();//откртый список клеток
    List<item_cell> close_list = new List<item_cell>();//закрыты список клеток
    List<Vector2Int> put_list = new List<Vector2Int>();//полученный путь
    item_cell tek_cel ;//текуща€ €чейка
    item_cell finish_cell ;//финишна€ €чейка
    //тест
    public int count_test = 0;//тест
    void Start()
    {
        kursor.gameObject.SetActive(false);//при старте курсор невидим
        GameObject obj_player = GameObject.Find("land");
        //к объекту прив€зан свой скрипт ищем его
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
    }

    // Update is called once per frame
    void Update()
    {

    }
    //клик дл€ указани€ конца пути
    void OnMouseDown()
    {
        if ((!EventSystem.current.IsPointerOverGameObject())&(data.get_activ_army()!=null))
        {
            mouse_event(1);
            data.type_event = 1;//1-перемещение сохраним тип событи€ бл€ дальнейшей обработки
        }
        if ((!EventSystem.current.IsPointerOverGameObject()) & (data.get_activ_army() == null))
        {//если клик по карте но нет активной армии, то переместим туда камеру
            MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MousePos.z = -10f;
            data.move_cam(MousePos);//перемещаем камеру
        }
    }
    //создание объектов  показывающих путь
    public void mouse_event(int type_evrnt)
    {
        //метод отработки клика мышки
        //1-перемещение, 2-атака, 3-атака города
        if (type_evrnt == 1) kursor.gameObject.GetComponent<SpriteRenderer>().sprite = spr_move;
        if (type_evrnt == 2) kursor.gameObject.GetComponent<SpriteRenderer>().sprite = spr_attack;
        if (type_evrnt == 3) kursor.gameObject.GetComponent<SpriteRenderer>().sprite = spr_attack;
        data.can_move_cell_list.Clear();
        //for (int i=0;i< spisok_puti.Count;i++)
        foreach (GameObject p in spisok_puti) Destroy(p);
        spisok_puti.Clear();
        if (!data.tek_activ_igrok.bot_flag)
        {//если метод вызвал бот, то эти настройки он сделает сам
            MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MousePos.z = -2.1f;
            kursor.transform.position = data.get_grid_step(MousePos);//перемещаем курсор
        }
        kursor.gameObject.SetActive(true);
        //установка стартовой и конечной точки
        //если у игрока есть юниты
        if (data.tek_activ_igrok.obj_army_list.Count > 0)
        {
            data.set_st_f_point_activ_army(kursor.transform.position);
            //data.set_st_f_point(data.get_activ_unit().transform.position, kursor.transform.position);
            create_put(data.st_p, data.fin_p);//создаем путь на основе стартовой и конечной точке
        }
    }

    void create_put(Vector2Int st, Vector2Int f)
    {
        close_list = data.game_s.get_put_cell(data.get_activ_army().koordinat, kursor.transform.position);
        int count_hod = data.get_activ_army().tek_hod;//оставшеес€ количесвто ходов юнита
        bool can_hod = true;
        finish_cell = data.kletki[f.x, f.y];//финишна€ €чейка
        foreach (item_cell cell in close_list)
        {
            GameObject p;
            //spisok_puti.Add((GameObject)Instantiate(point_put, new Vector3(cell.kordinat.x, cell.kordinat.y, -2.0f), Quaternion.identity));
            if (count_hod >= cell.get_cost_move())
            {
                count_hod = count_hod - cell.get_cost_move();//вычитыаем перемещение
                data.can_move_cell = cell;//обновим €чейку куда можем добратьс€
                data.can_move_cell_list.Add(cell);
                data.get_activ_army().tek_hod_tmp = count_hod;
            }
            else can_hod = false;//на дальнейшее перемещение очков не хватает
            if (can_hod) p=(GameObject)Instantiate(point_put, new Vector3(cell.kordinat.x, cell.kordinat.y, -2.0f), Quaternion.identity);
            else p = (GameObject)Instantiate(point_put_x, new Vector3(cell.kordinat.x, cell.kordinat.y, -2.0f), Quaternion.identity);
            spisok_puti.Add(p);
        }
        data.spisok_puti = spisok_puti;//сохраним в дате ссылку на список чтобы после перемещени€ юнита его удалить
        //если ходы остались что бы добратьс€ до финиша, то идем до финишка
        if (count_hod >= finish_cell.get_cost_move())
        {
            count_hod = count_hod - finish_cell.get_cost_move();//вычитыаем перемещение
            data.can_move_cell = finish_cell;
            data.can_move_cell_list.Add(finish_cell);
            data.get_activ_army().tek_hod_tmp = count_hod;
        }
    }
    public void culkulate_nex_put()
    {//!!тест
        GameObject p;
        foreach (GameObject pt in spisok_puti) Destroy(pt);
        spisok_puti.Clear();
        count_test++;
        List<city> tmp_city_list = data.game_s.get_city_list();
        if (count_test >= tmp_city_list.Count) count_test = 0;
        data.tek_activ_igrok.calculate_put(data.get_activ_army(), tmp_city_list[count_test]);
        List<item_cell> cell_list = data.tek_activ_igrok.bot_put_cell_list;
        Vector3 tmp_v = tmp_city_list[count_test].min_kkor;
        kursor.gameObject.SetActive(true);
        kursor.transform.position = tmp_v;//перемещаем курсор
        MousePos = tmp_v;
        MousePos.z = -10f;
        data.move_cam(MousePos);//перемещаем камеру
        
        foreach (item_cell cell in cell_list)
        {
            p = (GameObject)Instantiate(point_put, new Vector3(cell.kordinat.x, cell.kordinat.y, -2.0f), Quaternion.identity);
            spisok_puti.Add(p);
        }
    }
    public GameObject get_kursor()
    {//получение ссылки на курсор
        return kursor;
    }


}





