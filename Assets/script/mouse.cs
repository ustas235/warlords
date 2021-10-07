using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class mouse : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 MousePos=new Vector3(-2.5f, -1.7f, 0f);
    int count_clik = 0;//���������� ������ 1-�� ������ ������, 2-���������� ������
    public GameObject kursor;
    public GameObject point_put;
    public Sprite spr_attack;
    public Sprite spr_incity;
    public data_game data;//����� ��� ���� �������� ��� ������ ����

    List<item_cell> open_list = new List<item_cell>();//������� ������ ������
    List<item_cell> close_list = new List<item_cell>();//������� ������ ������
    List<Vector2Int> put_list = new List<Vector2Int>();//���������� ����
    item_cell tek_cel ;//������� ������
    item_cell finish_cell ;//�������� ������

    void Start()
    {
        kursor.gameObject.SetActive(false);//��� ������ ������ �������
        GameObject obj_player = GameObject.Find("land");
        //� ������� �������� ���� ������ ���� ���
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
    }

    // Update is called once per frame
    void Update()
    {

    }
    //���� ��� �������� ����� ����
    void OnMouseDown()
    {
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePos.z = -2.0f;
        kursor.transform.position = data.get_grid_step(MousePos);//���������� ������
        Debug.Log(data.get_grid_step(MousePos));
        kursor.gameObject.SetActive(true);
        data.set_st_f_point(data.get_activ_unit().transform.position, kursor.transform.position);

        create_put(data.st_p,data.fin_p);//������� ���� �� ������ ��������� � �������� �����
        //Debug.Log("klivk");
        //Instantiate(kursor, MousePos, Quaternion.identity);
    }
    //�������� ��������  ������������ ����
    void create_put(Vector2Int st, Vector2Int f)
    {
        item_cell tek_cel = new item_cell(st);//������� ������
        item_cell finish_cell = new item_cell(f);//�������� ������
        bool end_put = false;
        while (end_put)//���� �� ������ ����� ����
        {
            
            open_list = create_open_list(tek_cel);//������� ��������� �������� ������
            if (open_list.Contains(finish_cell)) { end_put = true;break; }//���� ��������� ������� ������ �������� �������� ������ �� �� ����� ��� ������ ����
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
        
        item_cell tek_cel_new = list_op_c.Max(item_cell.weight);//������� ������ �����
        return tek_cel_new;
    }

    private List<item_cell> create_open_list(item_cell tek_cel)
    {
        throw new NotImplementedException();
        List<item_cell> open_list_new = new List<item_cell>();// ����� ������� ������ ������
        int min_indx_x = tek_cel.idx_kor.x - 1;
        int max_indx_x = tek_cel.idx_kor.x + 1;
        int min_indx_y = tek_cel.idx_kor.y - 1;
        int max_indx_y = tek_cel.idx_kor.y + 1;
        for (int i= min_indx_x;i<= max_indx_x;i++)
            for (int j = min_indx_y; j <= max_indx_y; j++)
            {
                item_cell cell = data.kletki[i,j];//�������� ��������� ������
                if (close_list.Contains(cell)) continue;//��� ������ � �������� ������, ���������� ���������� �������
                if (cell.Equals(tek_cel)) continue;//��� ������ �������, ���������� ���������� �������
                else
                {
                    cell.set_aproxim(finish_cell);//����������� ���������� ��� ������
                    cell.set_weight();//��������� ��� ������
                    open_list_new.Add(cell);//��������� � �������� ������
                }
            }
         return open_list_new;

    }

    
}





