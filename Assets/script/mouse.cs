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
    public GameObject point_put_x;//объект с точкой пути куда нехватает очков перемещения
    public Sprite spr_attack;
    public Sprite spr_incity;
    public Sprite spr_move;
    public data_game data;//класс где буду хранится все данные игры

    List<item_cell> open_list = new List<item_cell>();//откртый список клеток
    List<item_cell> close_list = new List<item_cell>();//закрыты список клеток
    List<Vector2Int> put_list = new List<Vector2Int>();//полученный путь
    item_cell tek_cel ;//текущая ячейка
    item_cell finish_cell ;//финишная ячейка
    //тест
    public int count_test = 0;//тест
    void Start()
    {
        kursor.gameObject.SetActive(false);//при старте курсор невидим
        GameObject obj_player = GameObject.Find("land");
        //к объекту привязан свой скрипт ищем его
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the left mouse button was clicked
        


    }
    //клик для указания конца пути
    public void OnMouseDown()
    {
        if (!data.get_flag_army_is_move())
        {
            
            if ((!IsPointerOverUIObject()) & (data.get_activ_army() != null))
            {
                data.game_s.move_kursor_clik();//перемещаем курсор и армия запомнит конечную точку
                data.type_event = 1;//1-перемещение сохраним тип события бля дальнейшей обработки
                data.get_activ_army().old_type_event = data.type_event;
                mouse_event(1);
            }
            if ((!IsPointerOverUIObject()) & (data.get_activ_army() == null))
            {//если клик по карте но нет активной армии, то переместим туда камеру
                MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                MousePos.z = -10f;
                data.move_cam(MousePos);//перемещаем камеру
            }
        }
        
    }
    //создание объектов  показывающих путь
    public void mouse_event(int type_event)
    {
        //метод отработки клика мышки
        //1-перемещение, 2-атака, 3-атака города
        if (type_event == 1) kursor.gameObject.GetComponent<SpriteRenderer>().sprite = spr_move;
        if (type_event == 2) kursor.gameObject.GetComponent<SpriteRenderer>().sprite = spr_attack;
        if (type_event == 3) kursor.gameObject.GetComponent<SpriteRenderer>().sprite = spr_attack;
        data.can_move_cell_list.Clear();
        //for (int i=0;i< spisok_puti.Count;i++)
        foreach (GameObject p in spisok_puti) Destroy(p);
        spisok_puti.Clear();
        kursor.gameObject.SetActive(true);
        //установка стартовой и конечной точки
        //если у игрока есть юниты
        if (data.get_activ_igrok().obj_army_list.Count > 0)
        {
            data.set_st_f_point_activ_army(kursor.transform.position);
            //data.set_st_f_point(data.get_activ_unit().transform.position, kursor.transform.position);
            create_put(data.st_p, data.fin_p);//создаем путь на основе стартовой и конечной точке
        }
    }

    public void create_put(Vector2Int st, Vector2Int f)
    {
        close_list = data.game_s.get_put_cell(data.get_activ_army().koordinat, kursor.transform.position);
        int count_hod = data.get_activ_army().tek_hod;//оставшееся количесвто ходов юнита
        bool can_hod = true;
        finish_cell = data.kletki[f.x, f.y];//финишная ячейка
        foreach (item_cell cell in close_list)
        {
            GameObject p;
            //spisok_puti.Add((GameObject)Instantiate(point_put, new Vector3(cell.kordinat.x, cell.kordinat.y, -2.0f), Quaternion.identity));
            if (count_hod >= cell.get_cost_move())
            {
                count_hod = count_hod - cell.get_cost_move();//вычитыаем перемещение
                data.can_move_cell = cell;//обновим ячейку куда можем добраться
                data.can_move_cell_list.Add(cell);
                data.get_activ_army().tek_hod_tmp = count_hod;
            }
            else can_hod = false;//на дальнейшее перемещение очков не хватает
            if (can_hod) p=(GameObject)Instantiate(point_put, new Vector3(cell.kordinat.x, cell.kordinat.y, -2.0f), Quaternion.identity);
            else p = (GameObject)Instantiate(point_put_x, new Vector3(cell.kordinat.x, cell.kordinat.y, -2.0f), Quaternion.identity);
            spisok_puti.Add(p);
        }
        data.spisok_puti = spisok_puti;//сохраним в дате ссылку на список чтобы после перемещения юнита его удалить
        //если ходы остались что бы добраться до финиша, то идем до финишка
        if (count_hod >= finish_cell.get_cost_move())
        {
            count_hod = count_hod - finish_cell.get_cost_move();//вычитыаем перемещение
            data.can_move_cell = finish_cell;
            data.can_move_cell_list.Add(finish_cell);
            data.get_activ_army().tek_hod_tmp = count_hod;
        }
    }
   
    public GameObject get_kursor()
    {//получение ссылки на курсор
        return kursor;
    }
    
    private static bool IsPointerOverUIObject()
    {//проверка на то что клик идет по UI
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
    







