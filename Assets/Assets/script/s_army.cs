using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class s_army : MonoBehaviour
{
    // Start is called before the first frame update
    
    public data_game data;//����� ��� ���� �������� ��� ������ ����
    GameObject obj_army;//������ �� ������ � ������� ����������� �����
    public int id;//����� �����
    mouse obj_mouse;//������ � ��������� ����
    int strength;//����
    int max_unit_strenght;//���� ������������� �����
   // public int max_hod;//���������� ����� ������������
    public int tek_hod;//���������� ����� �������
    public int tek_hod_tmp;//���������� ����� ���������, ����� ����������� ������ �������� ������������ � ��� ���
    public gamer vladelec;//�������� �����
    public Sprite spr_army;//������ �����
    public GameObject army_flag;//c������ �� ���� �����
    public Sprite[] flags_sprites = new Sprite[8];//������ �� �������� ������
    public Vector3 koordinat;//���������� �����
    List<unit> unit_list=new List<unit>();//������ ������ � �����
    //��� ����
    int status_army=0;//������ ����� 0 -��������, 1- � ���������,2 ���������� ��� �����, 3 ���� � ����� �����, 4 ���� � ���� �� ����� �������, 5 ����� �� ������ �����
    city target_city;//�����, ������� ����/������ ��������� �����
    Vector3 target_koordinat;//��������, ��� ������ ��������� �����
    private void Awake()
    {
       
        GameObject obj_player = GameObject.Find("land");
        //� ������� �������� ���� ������ ���� ���
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
        obj_mouse = obj_player.GetComponent(typeof(mouse)) as mouse;
    }

    void Start()
    {
        

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
            //max_hod = 100;//���������� ����� ������������
            tek_hod = 100;//���������� ����� �������
            tek_hod_tmp = 100;//���������� ����� ���������, ����� ����������� ������ �������� ������������ � ��� ���
            koordinat = unit_list[0].koordinat;
            for (int i=0; i< unit_list.Count;i++)
            {
                /*if (i == 0)//���� ����� � ������� �����
                {
                    unit_list[i].flag.SetActive(true);//����� ������ ������ ����
                    unit_list[i].flag.GetComponent<SpriteRenderer>().sprite = unit_list[i].flags_sprites[unit_list.Count - 1];//������ ����� ������� �� �������� �����
                }*/
                //else unit_list[i].flag.SetActive(false);//����� ������ ������ ����

                if (unit_list[i].strength >= strength)//���� ������ �������� �����
                {
                    strength = unit_list[i].strength;//������ ����� �� ������ �������� �����
                    max_unit_strenght = unit_list[i].strength;
                    spr_army = unit_list[i].spr_unit;//������� ������ �����
                    obj_army.GetComponent<SpriteRenderer>().sprite = spr_army;
                }
                /*if (unit_list[i].max_hod <= max_hod)//���� ��� �� ������������ �� ����
                {
                    max_hod = unit_list[i].max_hod;
                }*/
                if (unit_list[i].get_tek_hod() <= tek_hod)//��� ��� ��� �� ������������ �� ����
                {
                    tek_hod = unit_list[i].get_tek_hod();
                }
                if (unit_list[i].tek_hod_tmp <= tek_hod_tmp)//��� ��� ��� �� ������������ �� ����
                {
                    tek_hod_tmp = unit_list[i].tek_hod_tmp;
                }
            }
            data.game_s.set_sprite_army_flag(vladelec, koordinat);//�������� ��������� �����
        }
    }
       
    public bool add_unit(unit u)//�������� ���� � ����� �� ������
    {
        bool check = false;
        if (unit_list.Count < 8)
        {
            //������ ���� �� ������ �����
            if (u.sc_army != null)
            {
                u.sc_army.sub_unit_destroy(u);
            }
            //������� ���� � ����� �����
            unit_list.Add(u);
            u.sc_army = this;
            set_army();//�������� �����
            check = true;
        }
        else check = false;
        return check;
    }
    public void sub_unit_create(unit u)//������� ���� �� ����� � ��������� ����� ������
    {
        u.remove_unit(unit_list);
        u.sc_army = null;//������ ���������� ������� �����
        data.game_s.create_new_army(u);//������� ����� �����
        set_army();//�������� ����� ����� ���������
    }
    public void sub_unit_destroy(unit u)//������� ���� � ��������� �����
    {
        //u.remove_unit(unit_list);
        unit_list.Remove(u);
        if (unit_list.Count == 0)
        {
            vladelec.obj_army_list.Remove(obj_army);//������ ��������
            vladelec.s_army_list.Remove(this);//�������� � ��������
            Destroy(army_flag);
            Destroy(obj_army);
        }
    }
    public void unit_destroy(unit u)//����� ����
    {
        unit_list.Remove(u);
        u.destroy_unit();
        if (unit_list.Count == 0)
        {
            vladelec.obj_army_list.Remove(obj_army);//������ ��������
            vladelec.s_army_list.Remove(this);//�������� � ��������
            Destroy(army_flag);
            Destroy(obj_army);
        }
    }
    public void move_army(Vector3 k)
    {
        koordinat = k;//����� ������ ���� ����������
        foreach (unit u in unit_list)
        {
            u.koordinat = k;//����� � ����� ���� ������ ���������� �����
        }
        army_flag.transform.position = k;
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
        gamer oth_vl = data.def_army.vladelec;//���������� ����� ������� �� �����
        List<unit> def_unit = new List<unit>();
        city def_city = null;
        foreach (city c in oth_vl.city_list) if (c.is_garnison(data.def_army))
            {
                flag_g = true;
                def_city = c;//�������� �����
                target_city=def_city;
                break;
            }
        //���� ����� ���� �� ��������, �������� ��� �� � ������ ��� �����
        if (flag_g)
        {
            
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
            foreach (unit u in data.def_army.unit_list) def_unit.Add(u);//���������� � ������ �������� �����
            calkulate_atack(unit_list, def_unit);//������ ������ ����� � ������ ����

        }
    }
    public void attack_event_city()
    {//����� ������ ����� ������ (���� �� ������)
        //��������, �� ���� �� ����� �� �������� ������
        gamer oth_vl = target_city.vladelec;//������������ �����
        //�������� ��� �� � ������ ��� �����
        List<unit> def_unit = new List<unit>();
        foreach (s_army a in oth_vl.s_army_list)//���������� ��� ����� ������������� ������
        {
            if (target_city.is_garnison(a))//���� ��������� ����� � ����� ������
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
        //Debug.Log("�������� ����");
        if (!EventSystem.current.IsPointerOverGameObject() )
        {
            if (data.get_activ_army() == null)
            {
                if (data.get_activ_igrok().id == this.vladelec.id)//���� ����� ����������� ��������� ������
                {
                    data.set_activ_army(this);//��� ����� ����� �� �������� � ������ �������� �����
                    this.transform.position = data.get_grid_step(this.transform.position);//�������� ������� �� �����
                    data.move_cam(koordinat);
                }
            }
            else
            {
                if (data.get_activ_igrok().id != this.vladelec.id)//���� �� ������ ����� - �������� ������� �����
                {
                    data.def_army = this;//�������� ���� � ���������� �����
                    data.type_event = 2; //�������� ��� ������� ��� ���������� ���������    
                    obj_mouse.mouse_event(2);//�������� ����� ����������� � ������
                    //obj_mouse.do_kursor();
                }
                else//���� ������ ���������� �� ���� ������
                { 
                    if (this.id != data.get_activ_army().id)
                    {//���� ���� �� ������ ������� �����
                        data.type_event = 1;//������� �����������
                        obj_mouse.mouse_event(1);//���������� ���� ������
                                                 //obj_mouse.do_kursor();
                    }
                }
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
    public GameObject get_obj()//��������� ������� �� ������
    {
        return obj_army;
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
        while ((max_count_round>0)&(army_d.Count>0))
        {
            tmp_unit_atack = army_a[i_a];
            tmp_unit_def = army_d[i_d];
            if(check_boy(tmp_unit_atack, tmp_unit_def))
            {//���� ������� ���� �����
                flags_d[i_d] = false;
                tmp_unit_def.flag_life = false;
                i_d++;//����� ��������� �� ������ ���� ������
                if (i_d >= army_d.Count) break;//���� ����� ������ ��������� ������� �� ���
            }
            else
            {//���� ������� ���� ������
                flags_a[i_a] = false;
                tmp_unit_atack.flag_life = false;
                i_a++;//����� ��������� �� ������ ���� ������
                if (i_a >= army_a.Count) break;//���� ����� ����� ��������� ������� �� ���
            }
            max_count_round--;//��������������, ���� ������� � ���������� �� ����� � ������ ����
        }
        //������� ������ � ����������� �� ����� �������
        int tmp_count = (army_d.Count-1) / 8;
        int count_unit_panel;
        switch (tmp_count)
        {
            case 0:
                data.atack_panel_s = data.atack_panel_s_8;
                data.attack_window = data.attack_window_8;
                count_unit_panel = 8;
                break;
            case 1:
                data.atack_panel_s = data.atack_panel_s_16;
                data.attack_window = data.attack_window_16;
                count_unit_panel = 16;
                break;
            case 2:
                data.atack_panel_s = data.atack_panel_s_24;
                data.attack_window = data.attack_window_24;
                count_unit_panel = 24;
                break;
            case 4:
                data.atack_panel_s = data.atack_panel_s_32;
                data.attack_window = data.attack_window_32;
                count_unit_panel = 32;
                break;
            default:
                data.atack_panel_s = data.atack_panel_s_32;
                data.attack_window = data.attack_window_32;
                count_unit_panel = 32;
                break;

        }
        data.atack_panel_s.set_panel_atack(unit_list, flags_a, army_d, flags_d, count_unit_panel);//������� ��������� ���
        data.attack_window.SetActive(true);//������� ����
        finih_atack(unit_list, flags_a, army_d, flags_d);//������ ������ �����
        
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
    public void update_count_hod(int delta_hod)
    {//����� ��������� ���������� ���������� ����� ��� ������
        foreach (unit u in unit_list) u.set_tek_hod(u.get_tek_hod() - delta_hod);

    }
    public void reboot_count_hod()//����� ���������� ����� ������
    {//����� ��������� ���������� ���������� ����� ��� ������
        foreach (unit u in unit_list)
        {
            u.set_tek_hod(u.get_max_hod());
            u.tek_hod_tmp = u.get_max_hod();
        }
        set_army();
    }
    public int get_strenght()
    {//����� ����������� ���� ����� � ��������� ��
        int str = 0;
        foreach (unit u in unit_list) str += u.strength;
        return str;
    }
    public void set_target_koordinat(Vector3 k)
    {
        target_koordinat = k;
    }
    public Vector3 get_target_koordinat()
    {
        return target_koordinat;
    }
    public void set_status(int st)
    {//����� ���������� ������ ����� � ���� �������� ������
        status_army = st;
        foreach (unit u in unit_list) u.status_untit = st;
    }
    public int get_status()
    {//����� ���������� ������ ����� � ���� �������� ������
        return status_army;
    }
    public void set_target_city(city c)
    {//��������� ����-������ ��� ����� ��� ������������
        target_city = c;
    }
    public city get_target_city()
    {//��������� ����-������ ��� ����� ��� ������������
        return target_city;
    }
    public void finih_atack(List<unit> unit_list_atack, List<bool> f_a, List<unit> unit_list_def, List<bool> f_d)
    {//����� ����������� ���������� ��� 
        //������ ������ �����
        for (int i = unit_list_atack.Count - 1; i > -1; i--)
        {
            if (!f_a[i])
            {
                unit_list_atack[i].sc_army.unit_destroy(unit_list_atack[i]);//���� ���� ���� ������ ����
                f_a.RemoveAt(i);
            }
        }

        for (int i = unit_list_def.Count - 1; i > -1; i--)
        {
            if (!f_d[i])
            {
                unit_list_def[i].sc_army.unit_destroy(unit_list_def[i]);//���� ���� ���� ������ ����
                unit_list_def.Remove(unit_list_def[i]);//�.�. ������ ���������� ��� ������ ������ ������ � ���
                f_d.RemoveAt(i);
            }
        }

        //���� ��� ��������� ���� � ���� ������ ������, �� ����� ���������� ������ ���������
        if ((unit_list_def.Count < 1) & (status_army == 3))
        {
            get_target_city().change_vladelec(vladelec);//������ ��������� ������
            set_target_city(null);//������� ������� �����
        }
        set_status(0);//������ ������ �����
        set_army();
        data.setting_panel_unit();//�������� ������ � �������
        vladelec.set_delta_gold();
    }
    public List<unit> get_unit_list()
    {
        return unit_list;
    }
    public int get_max_unit_str()
    {
        return max_unit_strenght;
    }
}
