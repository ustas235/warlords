using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// � ������ ������ �� ������ ��� ������ ����
public class data_game : MonoBehaviour
{
    // Start is called before the first frame update
    private unit activ_unit;//�������� �����
    public Camera Cam;//������
    public Vector2Int st_p, fin_p;//������� ��������� � � � � ������� ��������� grid_x � grid_y
    public float[] grid_x=new float[18];
    public float[] grid_y = new float[18];
    public item_cell[,] kletki = new item_cell[18, 18];//��������� ������ � ��������� ������
    void Start()
    {
        //���������� �����
        for (int i=0;i<18;i++)
        {
            grid_x[i] = -3.4f + i * 0.4f;
            grid_y[i] = 3.4f - i * 0.4f;
        }
        for (int i=0; i<18;i++)
            for (int j = 0; j < 18; j++)
            {
                kletki[i, j] = new item_cell(new Vector2Int(i, j));
                kletki[i, j].set_kordinat(new Vector2(grid_x[i], grid_y[i]));//��������� ������� � ���������� ������
                kletki[i, j].set_cost_move(10);//��������� �������� �� ������� 10
            }
    }
    public void set_activ_untit(unit u)//��������� ��������� ������
    {
        activ_unit = u;
    }
    public unit get_activ_unit()//��������� ��������� ������
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
    public void set_st_f_point(Vector3 point_s, Vector3 point_f)//������� ��������� ��������� �������� ����� ��� ������ ����, �������� ���������� ���������� ����� � ���������� �� ������
    {
        for (int i = 0; i < 18; i++)//���������� ������� �������
        {
            if (grid_x[i] == point_s.x) st_p.x = i;//���� ����� �
            if (grid_x[i] == point_f.x) fin_p.x = i;//���� ����� �
            if (grid_y[i] == point_s.y) st_p.y = i;//���� ����� y
            if (grid_y[i] == point_f.y) fin_p.y = i;//���� ����� y
        }

    }
}
