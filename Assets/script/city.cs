using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class city : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject kursor;
    public GameObject unit_prefab;//������ �����
    public Sprite spr_kursor_attack;
    public Sprite spr_kursor_in_city;
    public Sprite spr_city;
    public data_game data;//����� ��� ���� �������� ��� ������ ����
    public game game_s;//����� �� �������� ����
    mouse obj_mouse;//������ � ��������� ����
    public gamer vladelec;//�������� ������
    public int id_spr = 0;//����� ������� ������, ����� ������������� �������� ��� ������
    public Vector3 koordinat;//���������� ������
    public Vector3 koordinat_garnizon;//���������� ��� ������� ����� ���������
    public Vector3 koordinat_atack;//���������� ��� ������� ����� �����
    public Vector3 min_kkor;//���������� ��������� �����, ����� ��� ������� � �����
    public int id_unit=1;//����� ������������� ����� 0- ������ ������, 1- �������, 2- ������
    public int count_hod = 1, count_hod_start = -1;//���������� ����� �� ���������� �������������
    public List<unit> garnison = new List<unit>();//����� ���������� �����
    void Start()
    {
        GameObject obj_player = GameObject.Find("land");
        //� ������� �������� ���� ������ ���� ���
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
        game_s = obj_player.GetComponent(typeof(game)) as game;
        obj_mouse = obj_player.GetComponent(typeof(mouse)) as mouse;
        count_hod_start = -1;//��� ������ �� -1 ����� ����� �� �����������
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseEnter()
    {
        //kursor.GetComponent<SpriteRenderer>().sprite= spr_attack;
        //this.GetComponent<SpriteRenderer>().sprite = spr_city_grey[0];
    }
    private void OnMouseDown()
    {
        //this.GetComponent<SpriteRenderer>().sprite = spr_city_bel[0];
        if ((!EventSystem.current.IsPointerOverGameObject())) 
        {
            if (data.tek_activ_igrok.id == vladelec.id)//������ ������� ��������
            {
                if (data.get_activ_army() == null)//��� �������� ������ ������� ������ ������
                {
                    data.activ_city = this;//�������� ���� � �������� ������
                    data.city_panel_s.set_panel(data.tek_activ_igrok.id);
                    data.city_window.SetActive(true);//���� �������� ����� �������� ������ �������� ������
                }
                else
                {
                    data.type_event = 1;//������� �����������
                    obj_mouse.mouse_event(1);//���������� ���� ����
                }

            }
            else //����� ����������� ������� ������, ������� ����� ������
            {
                obj_mouse.mouse_event(2);//�������� ����� ����������� � ������
                data.set_def_city(this);//�������� ���� � ���������� �����
                data.type_event = 3; //�������� ��� ������� ��� ���������� ���������
            }
        }
    }
    public void change_vladelec(gamer vlad)
    {
        if (vladelec!=null) vladelec.city_list.Remove(this);//������� �� ������ ������� ������
        vladelec = vlad;//��������� ���������
        spr_city = vlad.spr_city;//����� ����� ������ ���������
        this.GetComponent<SpriteRenderer>().sprite = spr_city;
        vlad.city_list.Add(this);//��������� ����� ������ � ������
        count_hod_start = -1;//������� ������������
        koordinat_garnizon = new Vector3(koordinat.x + 0.2f, koordinat.y + 0.2f, koordinat.z);//�������� ����� ������������� � ������� ������� ����
        koordinat_atack = new Vector3(koordinat.x - 0.2f, koordinat.y - 0.2f, koordinat.z);//����� ����� ����� ������������� � ����� ������ ����
    }
    //���������� ������ ������
    public void create_unit()//����� �� �������� ������
    {
        if (count_hod_start > 0)//��� ������ �� ������ ����, ���� ����� �� ����������������� ����� �� ���������
        {
            count_hod--;
            if (count_hod <= 0)
            {
                count_hod = count_hod_start;//������� �������
                //�������� �����
                
                Vector3 koor_unit = new Vector3(koordinat.x - 0.2f, koordinat.y + 0.2f, koordinat.z);//���������� �������� �����
                unit tmp_unit_s = new unit();
                tmp_unit_s.set_koordinat(koor_unit);
                tmp_unit_s.set_unit(id_unit, vladelec, game_s.get_sprite_unit(vladelec.id, id_unit), game_s.get_sprite_unit_off());
                GameObject flag_tmp_obj = (GameObject)Instantiate(game_s.flag_prefab, koor_unit, Quaternion.identity);//������� ������ �����
                tmp_unit_s.flag = flag_tmp_obj;//����� ���������� ���� ����
                tmp_unit_s.flags_sprites = game_s.get_sprite_flag(tmp_unit_s.vladelec.id);
                game_s.create_new_army(tmp_unit_s);
            }
        }
    }
    public void setting_activ_city(int num_unit)//����� ��������� ������������ ������
    {
        id_unit = num_unit;
        switch (num_unit)
        {
            case 0://������ ������ ��������1 ���
                count_hod_start = 1;
                count_hod = 1;
                break;
            case 1://������� ������ �������� 2 ���
                count_hod_start = 2;
                count_hod = 2;
                break;
            case 2://������ �������� 3 ���
                count_hod_start = 3;
                count_hod = 3;
                break;
            default:
                count_hod_start = 1;
                count_hod = 1;
                break;

        }
    }
    public bool is_garnison(s_army sarmy)
    {//����� ��������� ����� �� ����� � ���� ������
        Vector3 k = sarmy.koordinat;
        float delta_x = Math.Abs(koordinat.x - k.x);
        float delta_y = Math.Abs(koordinat.y - k.y);
        if ((delta_x <= 0.25f) & (delta_y <= 0.25f))
        {//���� ��������� ����� ���� �������, � ��� ������� ���, ������ ���� ����� �� �����
            return true;
        }
        else return false;
    }
    public bool is_garnison(unit s_unit)
    {//����� ��������� ����� �� ���� � ���� ������
        Vector3 k = s_unit.koordinat;
        float delta_x = Math.Abs(koordinat.x - k.x);
        float delta_y = Math.Abs(koordinat.y - k.y);
        if ((delta_x <= 0.25f) & (delta_y <= 0.25f))
        {//���� ��������� ����� ���� �������, � ��� ������� ���, ������ ���� ����� �� �����
            return true;
        }
        else return false;
    }
    public List<unit> get_garnison_unit_list()
    {//����� ���������� ������ ������, ������� ����������
        List<unit> garnison = new List<unit>();
        foreach(s_army a in vladelec.s_army_list)
        {
            if (is_garnison(a))
            {
                foreach (unit u in a.unit_list) garnison.Add(u);
            }
        }
        return garnison;
    }
}
