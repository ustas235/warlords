using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class unit : MonoBehaviour
{
    // Start is called before the first frame update
    
    public data_game data;//����� ��� ���� �������� ��� ������ ����
    
    mouse obj_mouse;//������ � ��������� ����
    public int max_hod=8;//���������� ����� ������������
    public int tek_hod = 8;//���������� ����� �������
    public int tek_hod_tmp = 8;//���������� ����� ���������, ����� ����������� ������ �������� ������������ � ��� ���
    public gamer vladelec;//�������� �����
    public int strength = 2;//����
    public int price = 3;//����
    public int num_spr = 0;//����� ������� �����, ����� ������������� �������� ��� ������
    public Sprite spr_unit;//������ �����
    public Vector3 koordinat;//���������� �����
    void Start()
    {
        GameObject obj_player = GameObject.Find("land");
        //� ������� �������� ���� ������ ���� ���
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
        obj_mouse = obj_player.GetComponent(typeof(mouse)) as mouse;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {

        if (data.tek_activ_igrok.id == this.vladelec.id)//���� ���� ����������� ��������� ������
        {
            data.set_activ_untit(this);//��� ����� ����� �� �������� � ������ ����
            this.transform.position = data.get_grid_step(this.transform.position);//�������� ������� �� �����
            data.move_cam(koordinat);
        }
        else
        {
            if (data.get_activ_unit()!=null)//���� �� ������� ����� - �������� ������� �����
            {
                obj_mouse.mouse_event(2);//�������� ����� ����������� � ������
                data.attack_untit = this;//�������� ���� � ��������� �����
                data.type_event = 2; //�������� ��� ������� ��� ���������� ���������
            }
        }
        //Debug.Log("���� �������� � "+ this.transform.position);

        //set_sprite(1, 1);
    }
    public void set_unit(int num, gamer v, Sprite spr)
    //��������� �����
    //0 - ������ ������, 1 �������, 2 -������
    {
        vladelec = v;
        num_spr = num;
        spr_unit = spr;
        switch (num)
        {
            case 0:
                max_hod = 0;
                tek_hod = max_hod;
                strength = 2;
                price = 3;
                break;
            case 1:
                max_hod = 6;
                tek_hod = max_hod;
                strength = 3;
                price = 6;
                break;
            case 2:
                max_hod = 12;
                tek_hod = max_hod;
                strength = 6;
                price = 9;
                break;
            default:
                max_hod = 12;
                tek_hod = max_hod;
                strength = 2;
                price = 3;
                break;
        }

    }
    public void move_unit(Vector3 k)
    {
        this.transform.position = k;
        koordinat = k;
    }
    public void set_koordinat(Vector3 k)
    {
        koordinat = k;
    }
    public void attack_event()
    {//����� ������ ���
        unit winn;
        if (data.attack_untit.strength >= data.get_activ_unit().strength) winn = data.attack_untit;
        else winn = data.get_activ_unit();
        //������ ������ ��������� � ������� ������
        s_panel_attack win_panel_script = data.attack_window.GetComponent(typeof(s_panel_attack)) as s_panel_attack;
        win_panel_script.winner.GetComponent<Image>().sprite = winn.spr_unit;//������ ����������
        data.attack_window.SetActive(true);//������� ����
    }
}
