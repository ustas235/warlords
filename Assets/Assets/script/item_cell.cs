using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_cell : MonoBehaviour
{
    public int id;//����������� ������
    int cost_move = 0;//���������� ��������
    int aproxim = 0;//������������� ����������� �� �������� ������
    public int weight = 0;//��� ������
    public Vector2Int idx_kor = new Vector2Int();//������� ���������
    public Vector2 kordinat = new Vector2();//���������� ������
    public Vector3 koordint3x;
    int basik_cost = 10;//������� ��������� �������� �� ������ 10 (����), �� ������ 5
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public item_cell()//����������� ��� ��������
    {
        idx_kor.x = 0;
        idx_kor.y = 0;
    }
    public item_cell(Vector2Int k)//����������� �������� ������ � ���������
    {
        idx_kor = k;
    }
    public void set_cost_move(int c)//���������� ��������� �������� �� ������
    {
        cost_move = c * basik_cost;
    }
    public int get_cost_move()//���������� ��������� �������� �� ������
    {
        int c = cost_move / basik_cost;
        return c;
    }
    public void set_aproxim(item_cell f_cell)//���������� ����������� �� ������
    {
        int delta_x = Math.Abs(f_cell.idx_kor.x - idx_kor.x);
        int delta_y = Math.Abs(f_cell.idx_kor.y - idx_kor.y);
        
        
        aproxim = Math.Max(delta_x,delta_y) * 20;//(������������ ������) *20
    }
    public void set_weight()//���������� ��������� �������� �� ������
    {
        weight = aproxim + cost_move;
    }
    public int get_weight()
    {
        return weight;
    }
    public void set_kordinat(Vector2 kor)
    {
        kordinat = kor;
        koordint3x = new Vector3(kordinat.x, kordinat.y, -2.0f);
    }
    public void set_indx(Vector2Int indx)
    {
        idx_kor = indx;
    }
}
