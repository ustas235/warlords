using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_cell : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    int cost_move = 0;//���������� ��������
    int aproxim = 0;//������������� ����������� �� �������� ������
    public int weight = 0;//��� ������
    public Vector2Int idx_kor = new Vector2Int();//������� ���������
    Vector2 kordinat = new Vector2();
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
        cost_move = c * 10;
    }
    public void set_aproxim(item_cell f_cell)//���������� ����������� �� ������
    {
        aproxim = (Math.Abs(f_cell.idx_kor.x - idx_kor.x) + Math.Abs(f_cell.idx_kor.y - idx_kor.y)) * 10;//(������ � +������ y) *10
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
    }
}
