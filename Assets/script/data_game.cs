using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// � ������ ������ �� ������ ��� ������ ����
public class data_game : MonoBehaviour
{
    // Start is called before the first frame update
    //setting game--------------------------
    public int start_money = 100;//��������� �������
    public int count_player=4;//���������� �������
    //-----------------------
    private unit activ_unit;//�������� ����
    public city activ_city;//�������� �����
    public game game_s;//����� �� �������� ����
    public unit attack_untit;//���� �� ������� ���� �����
    public int type_event = 1;//������� ������� 1-�����������, 2 �����
    public gamer tek_activ_igrok;
    public item_cell can_move_cell;//������, �� ������� ����� ������ ����� ����
    public List<GameObject> spisok_puti;//������ �������� ����
    public Camera Cam;//������
    public GameObject city_window;//���� ������
    public GameObject attack_window;//���� ������
    public Vector2Int st_p, fin_p;//������� ��������� � � � � ������� ��������� grid_x � grid_y
    public float[] grid_x=new float[18];
    public float[] grid_y = new float[18];
    public int max_kletka_x= 17;
    public int min_kletka_x = 0;
    public int max_kletka_y = 17;
    public int min_kletka_y = 0;
    public item_cell[,] kletki;//��������� ������ � ��������� ������
    void Start()
    {
        GameObject obj_player = GameObject.Find("land");
        game_s = obj_player.GetComponent(typeof(game)) as game;//������ ������� ������ � �����
        attack_window.SetActive(false);
        kletki = new item_cell[max_kletka_x + 1, max_kletka_y + 1];//��������� ������ � ��������� ������
        int count = 0;
        //���������� �����
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
                kletki[i, j].set_kordinat(new Vector2(grid_x[i], grid_y[j]));//��������� ������� � ���������� ������
                kletki[i, j].set_cost_move(2);//��������� �������� �� ������� 20
                kletki[i, j].id = count;//� ������ ������ ���� �����
                if ((j == 2) & (((i > 2) & (i < 8)) || ((i > 9) & (i < 15)))) kletki[i, j].set_cost_move(1);//������
                if ((j == 9) & (((i > 2) & (i < 8)) || ((i > 9) & (i < 15)))) kletki[i, j].set_cost_move(1);//������
                if ((j == 16) & (((i > 2) & (i < 8)) || ((i > 9) & (i < 15)))) kletki[i, j].set_cost_move(1);//������
                if ((i == 2) & (((j > 2) & (j < 8)) || ((j > 9) & (j < 15)))) kletki[i, j].set_cost_move(1);//������
                if ((i == 9) & (((j > 2) & (j < 8)) || ((j > 9) & (j < 15)))) kletki[i, j].set_cost_move(1);//������
                if ((i == 15) & (((j > 2) & (j < 8)) || ((j > 9) & (j < 15)))) kletki[i, j].set_cost_move(1);//������
                count++;//������� ������
            }
    }
    public void set_activ_untit(unit u)//��������� ��������� ������
    {
        activ_unit = u;
    }
    public unit get_activ_unit()//��������� ��������� �����
    {
        return activ_unit;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    //����� ��������� ��������� �����
    public Vector3 get_grid_step(Vector3 point)
    {
        float x_p = point.x;
        float y_p = point.y;
        float z_p = point.z;
        float x_g=0, y_g=0, z_g = z_p;
        float min_x = 100, min_y = 100;
        for (int i=0;i<18;i++)//���������� ������� �������
        {
            if (Math.Abs(grid_x[i]-x_p)<= min_x)//���� ����������� ����������� �� �
            { 
                min_x = Math.Abs(grid_x[i] - x_p);
                x_g = grid_x[i];
            }
            if (Math.Abs(grid_y[i] - y_p)<= min_y)//���� ����������� ����������� �� �
            {
                min_y = Math.Abs(grid_y[i] - y_p);
                y_g = grid_y[i];
            }
        }
        return new Vector3(x_g, y_g, z_p);
    }
    public void set_st_f_point( Vector3 point_f)//������� ��������� ��������� �������� ����� ��� ������ ����, �������� ���������� ���������� ����� � ���������� �� ������
    {
        Vector3 point_s = get_activ_unit().transform.position;
        for (int i = 0; i < 18; i++)//���������� ������� �������
        {
            if (grid_x[i] == point_s.x) 
                st_p.x = i;//���� ����� �
            if (grid_x[i] == point_f.x) 
                fin_p.x = i;//���� ����� �
            if (grid_y[i] == point_s.y) 
                st_p.y = i;//���� ����� y
            if (grid_y[i] == point_f.y) 
                fin_p.y = i;//���� ����� y
        }

    }
    //����������� ������ � ������� �������
    public void move_cam(Vector3 k)
    {
        Vector3 tmp = k;
        k.z = -5.0f;//������ ����� ���� ����
        Cam.transform.position = k;
    }
}
