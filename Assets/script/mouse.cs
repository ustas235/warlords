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

    }
    //клик для указания конца пути
    void OnMouseDown()
    {
        if ((!EventSystem.current.IsPointerOverGameObject())&(data.get_activ_army()!=null))
        {
            mouse_event(1);
            data.type_event = 1;//1-перемещение сохраним тип события бля дальнейшей обработки
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
        open_list.Clear();
        close_list.Clear();
        //for (int i=0;i< spisok_puti.Count;i++)
        foreach (GameObject p in spisok_puti) Destroy(p);

        spisok_puti.Clear();
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePos.z = -2.0f;
        kursor.transform.position = data.get_grid_step(MousePos);//перемещаем курсор
        kursor.gameObject.SetActive(true);
        //установка стартовой и конечной точки
        //если у игрока есть юниты
        if (data.tek_activ_igrok.obj_army_list.Count > 0)
        {
            data.set_st_f_point(kursor.transform.position);
            //data.set_st_f_point(data.get_activ_unit().transform.position, kursor.transform.position);
            create_put(data.st_p, data.fin_p);//создаем путь на основе стартовой и конечной точке
        }
    }

    void create_put(Vector2Int st, Vector2Int f)
    {
        item_cell tek_cel = data.kletki[st.x,st.y];//текущая ячейка
        //Debug.Log("стартовая клетка - "+tek_cel.kordinat);
        finish_cell = data.kletki[f.x, f.y];//финишная ячейка
        bool end_put = true;
        while (end_put)//пока не найден конец пути
        {
            
            open_list = create_open_list(tek_cel);//создаем очередной открытый список
            //если очередной отрытый список содержит финишную ячейку ты мы нашли все ячейки пути
            if (open_list.Contains(finish_cell)) 
            { 
                //close_list.Add(finish_cell); //финишную клетку тоже добавим в список пути
                end_put = true;
                break; 
            }
            tek_cel = find_new_tek_cell(open_list);
            close_list.Add(tek_cel);
        }
        
        int count_hod = data.get_activ_army().tek_hod;//оставшееся количесвто ходов юнита
        bool can_hod = true;
        foreach (item_cell cell in close_list)
        {
            GameObject p;
            //spisok_puti.Add((GameObject)Instantiate(point_put, new Vector3(cell.kordinat.x, cell.kordinat.y, -2.0f), Quaternion.identity));
            if (count_hod >= cell.get_cost_move())
            {
                count_hod = count_hod - cell.get_cost_move();//вычитыаем перемещение
                data.can_move_cell = cell;//обновим ячейку куда можем добраться
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
            data.get_activ_army().tek_hod_tmp = count_hod;
        }
    }

    item_cell find_new_tek_cell(List<item_cell> list_op_c)
    {
        
        item_cell tek_cel_new= list_op_c[0];
        int min_weigth = tek_cel_new.weight;
        foreach(item_cell cell in list_op_c)
        {
            if (cell.weight <= min_weigth)
            {
                tek_cel_new = cell;//нашли очередную текущую ячейку с минимальным путем
                min_weigth = cell.weight;//обновим минимум;
            }
        }
        
        return tek_cel_new;
        throw new NotImplementedException();
    }

    List<item_cell> create_open_list(item_cell tek_cel_old)
    {
        
        List<item_cell> open_list_new = new List<item_cell>();// новый откртый список клеток
        int min_indx_x = tek_cel_old.idx_kor.x - 1;
        int max_indx_x = tek_cel_old.idx_kor.x + 1;
        int min_indx_y = tek_cel_old.idx_kor.y - 1;
        int max_indx_y = tek_cel_old.idx_kor.y + 1;
        //проверим чтобы не вывалится за края массива клеток
        if (min_indx_x < data.min_kletka_x) min_indx_x = 0;
        if (min_indx_y < data.min_kletka_y) min_indx_y = 0;
        if (max_indx_x > data.max_kletka_x) max_indx_x = 17;
        if (max_indx_y > data.max_kletka_y) max_indx_y = 17;
        for (int i= min_indx_x;i<= max_indx_x;i++)
            for (int j = min_indx_y; j <= max_indx_y; j++)
            {
                item_cell cell = data.kletki[i,j];//получаем очередную клетку
                if (close_list.Contains(cell)) continue;//эта клетка в закртыом списке, пропускаем продолжаме перебор
                if (cell.id== tek_cel_old.id) continue;//эта клетка текущая, пропускаем продолжаем перебор
                else
                {
                    cell.set_aproxim(finish_cell);//высчитываем апрокусиму для клетки
                    cell.set_weight();//высчитаем вес клетки
                    open_list_new.Add(cell);//добавляем в открытый список
                }
            }
         return open_list_new;
        throw new NotImplementedException();
    }
    
    
}





