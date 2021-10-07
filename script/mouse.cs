using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class mouse : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 MousePos=new Vector3(-2.5f, -1.7f, 0f);
    int count_clik = 0;//количество кликов 1-ый ставит курсор, 2-перемещает объект
    public GameObject kursor;
    public GameObject point_put;
    public Sprite spr_attack;
    public Sprite spr_incity;
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
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePos.z = -2.0f;
        kursor.transform.position = data.get_grid_step(MousePos);//перемещаем курсор
        Debug.Log(data.get_grid_step(MousePos));
        kursor.gameObject.SetActive(true);
        data.set_st_f_point(data.get_activ_unit().transform.position, kursor.transform.position);

        create_put(data.st_p,data.fin_p);//создаем путь на основе стартовой и конечной точке
        //Debug.Log("klivk");
        //Instantiate(kursor, MousePos, Quaternion.identity);
    }
    //создание объектов  показывающих путь
    void create_put(Vector2Int st, Vector2Int f)
    {
        item_cell tek_cel = new item_cell(st);//текущая ячейка
        item_cell finish_cell = new item_cell(f);//финишная ячейка
        bool end_put = false;
        while (end_put)//пока не найден конец пути
        {
            
            open_list = create_open_list(tek_cel);//создаем очередной открытый список
            if (open_list.Contains(finish_cell)) { end_put = true;break; }//если очередной отрытый список содержит финишную ячейку ты мы нашли все ячейки пути
            tek_cel = find_new_tek_cell(open_list);
            close_list.Add(tek_cel);
        }
        //for (int i=data.st_p_ix;i<data.f_p_ix;i++)
        {
            //Instantiate(point_put, new Vector3(data.grid_x[i],data.grid_y[data.f_p_iy],-2.0f), Quaternion.identity);
        }
        //return put_list;
    }

    private item_cell find_new_tek_cell(List<item_cell> list_op_c)
    {
        throw new NotImplementedException();
        
        item_cell tek_cel_new = list_op_c.Max(item_cell.weight);//текущая ячейка новая
        return tek_cel_new;
    }

    private List<item_cell> create_open_list(item_cell tek_cel)
    {
        throw new NotImplementedException();
        List<item_cell> open_list_new = new List<item_cell>();// новый откртый список клеток
        int min_indx_x = tek_cel.idx_kor.x - 1;
        int max_indx_x = tek_cel.idx_kor.x + 1;
        int min_indx_y = tek_cel.idx_kor.y - 1;
        int max_indx_y = tek_cel.idx_kor.y + 1;
        for (int i= min_indx_x;i<= max_indx_x;i++)
            for (int j = min_indx_y; j <= max_indx_y; j++)
            {
                item_cell cell = data.kletki[i,j];//получаем очередную клетку
                if (close_list.Contains(cell)) continue;//эта клетка в закртыом списке, пропускаем продолжаме перебор
                if (cell.Equals(tek_cel)) continue;//эта клетка текущая, пропускаем продолжаем перебор
                else
                {
                    cell.set_aproxim(finish_cell);//высчитываем апрокусиму для клетки
                    cell.set_weight();//высчитаем вес клетки
                    open_list_new.Add(cell);//добавляем в открытый список
                }
            }
         return open_list_new;

    }

    
}





