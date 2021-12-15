using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class s_army : MonoBehaviour
{
    // Start is called before the first frame update
    
    public data_game data;//����� ��� ���� �������� ��� ������ ����
    GameObject obj_army;//������ �� ������ � ������� ����������� �����
    public GameObject unit_prefab;//������ �����
    mouse obj_mouse;//������ � ��������� ����
    public int strength;//����
    public int max_hod;//���������� ����� ������������
    public int tek_hod;//���������� ����� �������
    public int tek_hod_tmp;//���������� ����� ���������, ����� ����������� ������ �������� ������������ � ��� ���
    public gamer vladelec;//�������� �����
    public Sprite spr_army;//������ �����
    public Vector3 koordinat;//���������� �����
    public List<unit> unit_list=new List<unit>();//������ ������ � �����
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
    
    public void set_army()//��������� ����� ����� ������� ��������� ��� �������
    {
        if (unit_list.Count>0)
        {
            strength = 0;//����
            max_hod = 100;//���������� ����� ������������
            tek_hod = 100;//���������� ����� �������
            tek_hod_tmp = 100;//���������� ����� ���������, ����� ����������� ������ �������� ������������ � ��� ���
            koordinat = unit_list[0].koordinat;
            for (int i=0; i< unit_list.Count;i++)
            {
                if (unit_list[i].strength >= strength)//���� ������ �������� �����
                {
                    strength = unit_list[i].strength;//������ ����� �� ������ �������� �����
                    spr_army = unit_list[i].spr_unit;//������� ������ �����
                    obj_army.GetComponent<SpriteRenderer>().sprite = spr_army;
                }
                if (unit_list[i].max_hod <= max_hod)//���� ��� �� ������������ �� ����
                {
                    max_hod = unit_list[i].max_hod;
                }
                if (unit_list[i].tek_hod <= tek_hod)//��� ��� ��� �� ������������ �� ����
                {
                    tek_hod = unit_list[i].tek_hod;
                }
                if (unit_list[i].tek_hod_tmp <= tek_hod_tmp)//��� ��� ��� �� ������������ �� ����
                {
                    tek_hod_tmp = unit_list[i].tek_hod_tmp;
                }
            }
        }
    }
       
    public void add_unit(unit u)//�������� ���� � ����� �� ������
    {
        //������ ���� �� ������ �����
        u.sc_army.sub_unit_destroy(u);
        
        //������� ���� � ����� �����
        unit_list.Add(u);
        u.sc_army = this;
        set_army();//�������� �����
    }
    public void sub_unit_create(unit u)//������� ���� �� ����� � ��������� ����� ������
    {
        unit_list.Remove(u);
        set_army();//�������� ����� 
        data.game_s.create_new_army(u);//������� ����� �����
    }
    public void sub_unit_destroy(unit u)//������� ���� � ��������� �����
    {
        unit_list.Remove(u);
        if (unit_list.Count == 0)
        {
            data.tek_activ_igrok.obj_army_list.Remove(obj_army);//������ ��������
            data.tek_activ_igrok.s_army_list.Remove(this);//�������� � ��������
            Destroy(obj_army);
        }
    }
    public void move_army(Vector3 k)
    {
        koordinat = k;//����� ������ ���� ����������
        foreach (unit u in unit_list) u.koordinat = k;//����� � ����� ���� ������ ���������� �����
        this.transform.position = k;
    }
    public void set_koordinat(Vector3 k)
    {
        koordinat = k;
    }
    public void attack_event_army()
    {//����� ������ ��� ���� ��� ������������ �����
        //��������, �� ���� �� ����� �� �������� ������
        bool flag_g = false;
        gamer oth_vl= oth_vl = data.def_army.vladelec;//���������� ����� ������� �� �����
        city def_city = null;
            foreach (city c in oth_vl.city_list) if (c.is_garnison(data.def_army))
                {
                    flag_g = true;
                    def_city = c;//�������� �����
                }

        //���� ����� ���� �� ��������, �������� ��� �� � ������ ��� �����
        if (flag_g)
        {
            List<unit> def_unit = new List<unit>();
            foreach (s_army a in oth_vl.s_army_list)//���������� ��� ����� ������������� ������
            {
                if (def_city.is_garnison(a))//���� ��������� ����� � ����� ������
                {
                    foreach (unit u in a.unit_list) def_unit.Add(u);//���������� ��������� ���� � ��������
                }
            }

            //data.atack_panel_s.set_panel_atack(unit_list, def_unit);//�������� ����� �� ��������
            //data.attack_window.SetActive(true);//������� ����
            calkulate_atack(unit_list, def_unit);//������ ������ ����� � ������ ����
        }
        else// ���� ����� �� �� ��������
        {
            //data.atack_panel_s.set_panel_atack(unit_list, data.def_army.unit_list);//�������� �����
            //data.attack_window.SetActive(true);//������� ����
            calkulate_atack(unit_list, data.def_army.unit_list);//������ ������ ����� � ������ ����
        }
    }
    public void attack_event_city()
    {//����� ������ ����� ������ (���� �� ������)
        //��������, �� ���� �� ����� �� �������� ������
        gamer oth_vl = data.def_city.vladelec;//������������ �����
        //�������� ��� �� � ������ ��� �����
        List<unit> def_unit = new List<unit>();
        foreach (s_army a in oth_vl.s_army_list)//���������� ��� ����� ������������� ������
        {
            if (data.def_city.is_garnison(a))//���� ��������� ����� � ����� ������
            {
                foreach (unit u in a.unit_list) def_unit.Add(u);//���������� ��������� ���� � ��������
            }
        }
        //data.atack_panel_s.set_panel_atack(unit_list, def_unit);//�������� ����� �� ��������
        //data.attack_window.SetActive(true);//������� ����
        calkulate_atack(unit_list, def_unit);//������ ������ ����� � ������ ����
    }
    private void OnMouseDown()
    {
        Debug.Log("�������� ����");
        if (data.get_activ_army() == null)
        {
            if (data.tek_activ_igrok.id == this.vladelec.id)//���� ����� ����������� ��������� ������
            {
                data.set_activ_army(this);//��� ����� ����� �� �������� � ������ �������� �����
                this.transform.position = data.get_grid_step(this.transform.position);//�������� ������� �� �����
                data.move_cam(koordinat);
            }
        }
        else
        {
            if (data.tek_activ_igrok.id != this.vladelec.id)//���� �� ������ ����� - �������� ������� �����
                {
                data.def_army = this;//�������� ���� � ���������� �����
                data.type_event = 2; //�������� ��� ������� ��� ���������� ���������    
                obj_mouse.mouse_event(2);//�������� ����� ����������� � ������
                    //obj_mouse.do_kursor();
                }
            else//���� ������ ���������� �� ���� ������
                {
                    data.type_event = 1;//������� �����������
                    obj_mouse.mouse_event(1);//���������� ���� ������
                    //obj_mouse.do_kursor();
                }
        }
        //obj_mouse.mouse_event(1);//���������� ���� ������
        //Debug.Log("���� �������� � "+ this.transform.position);

        //set_sprite(1, 1);
    }
    public void set_obj(GameObject g)//��������� ������� �� ������
    {
        obj_army = g;
    }
    
   public void calkulate_atack(List<unit> army_a, List<unit> army_d)
    {//����� ������� �����
        int max_count_round = army_a.Count + army_d.Count;//������������ ���������� ����
        int i_a = 0, i_d = 0;//������� ������ ����� � ������
        List<bool> flags_a = new List<bool>();//����� ��������� ������ ���� ����� ���
        List<bool> flags_d = new List<bool>();//����� ��������� ������ ������ ����� ���
        for (int i = 0; i < army_a.Count; i++) flags_a.Add(true);
        for (int i = 0; i < army_d.Count; i++) flags_d.Add(true);//����� ���� ��� ����� �����
        unit tmp_unit_atack, tmp_unit_def;
        while (max_count_round>0)
        {
            tmp_unit_atack = army_a[i_a];
            tmp_unit_def = army_d[i_d];
            if(check_boy(tmp_unit_atack, tmp_unit_def))
            {//���� ������� ���� �����
                flags_d[i_d] = false;
                i_d++;//����� ��������� �� ������ ���� ������
                if (i_d >= army_d.Count) break;//���� ����� ������ ��������� ������� �� ���
            }
            else
            {
                flags_d[i_a] = false;
                i_a++;//����� ��������� �� ������ ���� ������
                if (i_a >= army_a.Count) break;//���� ����� ������ ��������� ������� �� ���
            }
            max_count_round--;//��������������, ���� ������� � ���������� �� ����� � ������ ����
        }
        data.atack_panel_s.set_panel_atack(unit_list, flags_a, army_d, flags_d);//������� ��������� ���
        data.attack_window.SetActive(true);//������� ����
    }
    public bool check_boy(unit a, unit d)
    {//����� ������� ���, ���������� true ���������� ���������, false -������������
        int hit_a = 2, hit_d = 2;//�������� ������, ��� ���������� 0 ���� ��������
        int rnd;//��������� ��������
        int s_a,  s_d;
        while ((hit_a>0)&(hit_d>0))
        {
            rnd = Random.Range(1, 11);
            s_a = a.strength + rnd;
            rnd = Random.Range(1, 11);
            s_d = d.strength + rnd;
            if (s_a!=s_d)
            {
                if (s_a > s_d) hit_d--;//� ����� ������ ���� - ���������� ����
                else hit_a--;//� ������ ������ ���� - ����� ����
            }
        }
        if (hit_a>0) return true;
        else return false;
    }
    public bool check_koordinat(Vector3 k)
    {//����� ��������� ���������
        if ((koordinat.x == k.x) & (koordinat.y == k.y)) return true;
        else return false;
    }
}
