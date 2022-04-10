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
    int profit = 0;//����� �� ������
    public int id_spr = 0;//����� ������� ������, ����� ������������� �������� ��� ������
    public Vector3 koordinat;//���������� ������
    public Vector3 koordinat_garnizon;//���������� ��� ������� ����� ���������
    public Vector3 koordinat_atack;//���������� ��� ������� ����� �����
    public Vector3 min_kkor;//���������� ��������� �����, ����� ��� ������� � �����
    public int id_unit = -1;//����� ������������� ����� 0- ������ ������, 1- �������, 2- ������
    public int count_hod = 1, count_hod_start = -1;//���������� ����� �� ���������� �������������
    public List<unit> garnison = new List<unit>();//����� ���������� �����
    public bool[] can_build_flag;//������ ���������� ������������ � ���� ������
    public int bot_num_unit_build=-1;//����� ����� ������� ���� ����� ������ � ���� ������

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
    private void OnMouseEnter()
    {
        //kursor.GetComponent<SpriteRenderer>().sprite= spr_attack;
        //this.GetComponent<SpriteRenderer>().sprite = spr_city_grey[0];
    }
    private void OnMouseDown()
    {
        //this.GetComponent<SpriteRenderer>().sprite = spr_city_bel[0];
        if (!data.get_flag_army_is_move())
        {//���� ��������� ����� �� ��������� �� ���� �����
            if ((!EventSystem.current.IsPointerOverGameObject()))
            {
                if (data.get_activ_igrok().id == vladelec.id)//������ ������� ��������
                {
                    if (data.get_activ_army() == null)//��� �������� ������ ������� ������ ������
                    {
                        data.activ_city = this;//�������� ���� � �������� ������
                        data.city_panel_s.set_panel(data.get_activ_igrok().id);
                        data.city_window.SetActive(true);//���� �������� ����� �������� ������ �������� ������
                    }
                    else
                    {
                        data.type_event = 1;//������� �����������
                        data.get_activ_army().set_target_city(this);//�������� ����� - ����������
                        obj_mouse.mouse_event(1);//���������� ���� ����
                    }

                }
                else //����� ����������� ������� ������, ������� ����� ������
                {
                    if (data.get_activ_army() != null)
                    {
                        obj_mouse.mouse_event(2);//�������� ����� ����������� � ������
                        data.get_activ_army().set_target_city(this);//�������� ���� � ���������� �����
                        data.type_event = 3; //�������� ��� ������� ��� ���������� ���������
                        data.get_activ_army().set_status(3);//������ ����� ����� �� �����
                    }
                }
            }
        }
    }
    public void change_vladelec(gamer vlad)
    {
        if (vladelec != null) vladelec.city_list.Remove(this);//������� �� ������ ������� ������
        vladelec = vlad;//��������� ���������
        spr_city = vlad.spr_city;//����� ����� ������ ���������
        this.GetComponent<SpriteRenderer>().sprite = spr_city;
        vlad.city_list.Add(this);//��������� ����� ������ � ������
        count_hod_start = -1;//������� ������������
        id_unit = -1;//������� ������������
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
                unit tmp_unit_s = new unit(data.id_unit_count++);
                tmp_unit_s.set_koordinat(koor_unit);
                tmp_unit_s.set_unit(id_unit, vladelec, game_s.get_sprite_unit(vladelec.id, id_unit), game_s.get_sprite_unit_off());
                game_s.create_new_army(tmp_unit_s);

            }
        }
    }
    public void setting_activ_city(int num_unit)//����� ��������� ������������ ������
    {
        id_unit = num_unit;
        switch (num_unit)
        {
            case -1://������
                count_hod_start = -1;
                count_hod = 1;
                break;
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
        foreach (s_army a in vladelec.s_army_list)
        {
            if (is_garnison(a))
            {
                foreach (unit u in a.get_unit_list()) garnison.Add(u);
            }
        }
        return garnison;
    }
    public int get_profit()
    {
        return profit;
    }
    public void collect_profit()
    {//���� ������ � ������
        vladelec.change_gold(profit);
    }
    public void set_profit(int p)
    {
        profit = p;
        
    }
    public void set_can_build(int f)
    {//��������� ���������� ������������
     //0 ����� ������ ������� �������, 1 ������ � �������, 2-���� �������, -1-������
        for (int i = 0; i < f+1; i++) can_build_flag[i] = true;
    }
    public void start_setup_set(int n)
    {//��������� �������������, n -���� ����� �����, �������� ����� ������� ����
        can_build_flag = new bool[3];//������ ���������� ������������ � ���� ������
        for (int i = 0; i <= n; i++) can_build_flag[i] = true;//�������� ��� n=1 � ������ ����� ������ ������ ������(0), ������� ������ (1), ��������� (2) ������
        GameObject obj_player = GameObject.Find("land");
        //� ������� �������� ���� ������ ���� ���
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
        game_s = obj_player.GetComponent(typeof(game)) as game;
        obj_mouse = obj_player.GetComponent(typeof(mouse)) as mouse;
        count_hod_start = -1;//��� ������ �� -1 ����� ����� �� �����������
        id_unit = -1;
    }
    public bool can_any_build()
    {//�������� �� �� ��� ����� ����� ���� ���-�� ������� 
        bool flag = false;
        for (int i=0;i<can_build_flag.Length;i++)
        {
            if (can_build_flag[i]) flag = true;
        }
        return flag;
    }
}
