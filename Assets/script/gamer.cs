using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamer : MonoBehaviour//
{
    public List<city> city_list = new List<city>();//������ �������
    public List<GameObject> obj_army_list = new List<GameObject>();//������ �������� ������
    //public List<s_army> skript_army_list = new List<s_army>();//������ ��������� ������
    public List<s_army> s_army_list = new List<s_army>();//������ ��������� �����
    public data_game data;//����� ��� ���� �������� ��� ������ ����
    int money = 0;
    public int id = 0;//id ������, �� �� �����
    public bool still_play = true;//���� ��� ����� ��� ������
    public bool active = false;//���� ��� ��� ������
    public Sprite spr_city;//������ �� ����� ������ ������
    // Start is called before the first frame update
    void Start()
    {
        GameObject obj_player = GameObject.Find("land");
        //� ������� �������� ���� ������ ���� ���
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
        money = data.start_money;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public gamer(int num)//����������� ��������� �����
    {
        id = num;
        
    }
}
