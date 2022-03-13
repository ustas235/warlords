using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class unit : MonoBehaviour 
{
    // Start is called before the first frame update
    
    public data_game data;//����� ��� ���� �������� ��� ������ ����
    
    mouse obj_mouse;//������ � ��������� ����
    public GameObject flag;//����� �� ������������� ����
    public Sprite[] flags_sprites = new Sprite[8];//������ �� �������� ������
    public int max_hod=1;//���������� ����� ������������
    public int tek_hod = 1;//���������� ����� �������
    public int tek_hod_tmp = 1;//���������� ����� ���������, ����� ����������� ������ �������� ������������ � ��� ���
    public gamer vladelec;//�������� �����
    public int strength = 2;//����
    public int price = 3;//����
    public int num_spr = 0;//����� ������� �����, ����� ������������� �������� ��� ������
    public Sprite spr_unit;//������ �����
    public Sprite spr_unit_off;//������ ����� ������������
    public Vector3 koordinat;//���������� �����
    public s_army sc_army;//������ �� �����
    public int id_unit = 0;//���������� ����� �����
    public bool flag_life = true;//���� ��� ���� �����
    public int status_untit = 0;//������ ����� 0 ��������, 1 - � ���������, 2 -  � ����� �����
    private void Awake()
    {
        
    }
    void Start()
    {
        
        


    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    public void set_unit(int num, gamer v, Sprite spr, Sprite[] srp_off)
    //��������� �����
    //0 - ������ ������, 1 �������, 2 -������
    {
        

        vladelec = v;
        num_spr = num;
        spr_unit = spr;
        
        switch (num)
        {
            case 0:
                max_hod = 8;
                tek_hod = max_hod;
                tek_hod_tmp = tek_hod;
                strength = 2;
                price = 3;
                spr_unit_off = srp_off[2];//������ ������������ �����
                break;
            case 1:
                max_hod = 6;
                tek_hod = max_hod;
                tek_hod_tmp = tek_hod;
                strength = 3;
                price = 6;
                spr_unit_off = srp_off[1];//������ ������������ �����
                break;
            case 2:
                max_hod = 12;
                tek_hod = max_hod;
                tek_hod_tmp = tek_hod;
                strength = 6;
                price = 9;
                spr_unit_off = srp_off[4];//������ ������������ �����
                break;
            default:
                max_hod = 12;
                tek_hod = max_hod;
                tek_hod_tmp = tek_hod;
                strength = 2;
                price = 3;
                spr_unit_off = srp_off[2];//������ ������������ �����
                break;
        }

    }
    /*
    public void move_unit(Vector3 k)
    {
        this.transform.position = k;
        koordinat = k;
    }
    */
    public void set_koordinat(Vector3 k)
    {
        koordinat = k;
    }
    public void attack_event()
    {//����� ������ ���
        
    }
    public unit ()
    {
        GameObject obj_player = GameObject.Find("land");
        //� ������� �������� ���� ������ ���� ���
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
        obj_mouse = obj_player.GetComponent(typeof(mouse)) as mouse;
        id_unit = data.id_unit_count++;
        //Debug.Log(id_unit.ToString());
    }
    public bool contains_to_list(List<unit> l)//����� ��������, ��� ���� ����������� � ������
    {
        if (l==null) return false;
        bool tmp = false;
        for(int i=0;i<l.Count;i++)
        {
            if (id_unit == l[i].id_unit) tmp = true;
        }
        return tmp;
    }
    public void remove_unit(List<unit> u_list)
    {//�������� ����� �� ������
        int num = -1;
        for (int i = 0; i < u_list.Count; i++) if (id_unit == u_list[i].id_unit) 
                num = i;
        if (num >=0) 
            u_list.RemoveAt(num);
    }
    public void destroy_unit()
    {//������ �������� ������� ������
        Destroy(flag);
        Destroy(this);
    }
}
