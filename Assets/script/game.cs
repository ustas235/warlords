using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class game : MonoBehaviour
{
    // Start is called before the first frame update
    data_game data;
    List<gamer> gamer_list = new List<gamer>();//������ �������
    List<GameObject> city_obj_list = new List<GameObject>();//������ ������ �������
    public GameObject city_prefab;//������ ������
    public GameObject unit_prefab;//������ �����
    public GameObject flag_prefab;//������ �����
    public GameObject army_prefab;//������ �����
    public GameObject butt_end;//���� ������ ����� ����
    mouse obj_mouse;//������ � ��������� ����
    public int num_tek_igrok = 1;
    Sprite[] spr_unit_grey, spr_unit_bel, spr_unit_dark, spr_unit_zel, spr_unit_orange, spr_unit_off;//������ �������� ������
    Sprite[] spr_flag_grey, spr_flag_bel, spr_flag_dark, spr_flag_zel, spr_flag_orange;//������ �������� ������
    Sprite[] spr_city_grey, spr_city_bel, spr_city_dark, spr_city_zel, spr_city_orange;//������ �������� �������
    List<Sprite[]> spr_list_city = new List<Sprite[]>();//������ �� �������� ������� �� ������ ������
    List<Sprite[]> spr_list_unit = new List<Sprite[]>();//������ �� �������� ������ �� ������ ������
    List<Sprite[]> spr_list_unit_off = new List<Sprite[]>();//������ �� �������� ����������� ������
    int index_unit = 0;//������� ��� �������� ������
    public int id = 0;
    
    void Start()
    {
        GameObject obj_player = GameObject.Find("land");
        //� ������� �������� ���� ������ ���� ���
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
        obj_mouse = obj_player.GetComponent(typeof(mouse)) as mouse;
        //���������� ������� ������
        {
            spr_unit_grey = Resources.LoadAll<Sprite>("sprite/army/grey");//������� ���������
            spr_unit_bel = Resources.LoadAll<Sprite>("sprite/army/bel");//������� ����� 1
            spr_unit_dark = Resources.LoadAll<Sprite>("sprite/army/dark");//������� ����� 2
            spr_unit_zel = Resources.LoadAll<Sprite>("sprite/army/zel");//������� ����� 3
            spr_unit_orange = Resources.LoadAll<Sprite>("sprite/army/orange");//������� ����� 4
            spr_unit_off = Resources.LoadAll<Sprite>("sprite/army/desel");//������� ����������� ������
            spr_list_unit.Add(spr_unit_grey);
            spr_list_unit.Add(spr_unit_bel);
            spr_list_unit.Add(spr_unit_dark);
            spr_list_unit.Add(spr_unit_zel);
            spr_list_unit.Add(spr_unit_orange);
            //���������� ������� � �������
            spr_flag_grey = Resources.LoadAll<Sprite>("sprite/flags/flag_grey");//����� ���������
            spr_flag_bel = Resources.LoadAll<Sprite>("sprite/flags/flag_bel");//����� ����� 1
            spr_flag_dark = Resources.LoadAll<Sprite>("sprite/flags/flag_dark");//����� ����� 2
            spr_flag_zel = Resources.LoadAll<Sprite>("sprite/flags/flag_zel");//����� ����� 3
            spr_flag_orange = Resources.LoadAll<Sprite>("sprite/flags/flag_orange");//����� ����� 4
            //���������� ������� �������
            spr_city_grey = Resources.LoadAll<Sprite>("sprite/city/grey_city");//������� ���������
            spr_city_bel = Resources.LoadAll<Sprite>("sprite/city/bel_city");//������� ����� 1
            spr_city_dark = Resources.LoadAll<Sprite>("sprite/city/dark_city");//������� ����� 2
            spr_city_zel = Resources.LoadAll<Sprite>("sprite/city/gren_city");//������� ����� 3
            spr_city_orange = Resources.LoadAll<Sprite>("sprite/city/orange_city");//������� ����� 4
            spr_list_city.Add(spr_city_grey);
            spr_list_city.Add(spr_city_bel);
            spr_list_city.Add(spr_city_dark);
            spr_list_city.Add(spr_city_zel);
            spr_list_city.Add(spr_city_orange);
        }
        //������� �������
        create_gamers();
        gamer_list[1].active = true;//��� ������� ������
        data.set_activ_igrok(gamer_list[1]);//�������� ��������� ������

        initial_place();//������� � ����������� ������
        //���������� ������ � ����� ������� ������
        data.set_activ_army(data.get_activ_igrok().s_army_list[0]);//�������� ���� 
        data.move_cam(data.get_activ_igrok().city_list[0].koordinat);

    }

    // Update is called once per frame
    void Update()
    {
        data.get_activ_igrok().get_move_to_target();//��������� ����� ����������� � ��������� ������
    }
    public void end_turn()//��������� ������ ����� ����
    {
        if (!data.get_flag_army_is_move())//���� ��� ����� � ���� ������ ��������� ���
        {
            foreach (GameObject p in data.spisok_puti) Destroy(p);//��������� ������ ����

            int next = data.get_activ_igrok().id;
            int count = 0;//������������ ���������� ������
            while (count < 20)//���� ���������� ������
            {
                next++;
                count++;
                if (next >= gamer_list.Count) next = 1;
                if (gamer_list[next].still_play)
                {
                    gamer_list[next].active = true;
                    num_tek_igrok = next;
                    //���� ������ ������ �� ��������
                    foreach (s_army army in gamer_list[next].s_army_list)
                        army.reboot_count_hod();//��������� ���� � ������
                                                //�������� ������������ ������ �� ���� ������� ������
                    foreach (city c in gamer_list[next].city_list) c.create_unit();
                    break;
                }

            }
            print("����� �����  " + num_tek_igrok);
            butt_end.GetComponentInChildren<Text>().text = num_tek_igrok.ToString();//���� ������� ����� ��������� ������
            data.set_activ_igrok(gamer_list[num_tek_igrok]);//�������� � ���� �������� ������
                                                             //��������� ������ �� ����� ������
            if (data.get_activ_igrok().obj_army_list.Count > 0)
            {
                //Vector3 tmp_vect=data.tek_activ_igrok.unit_list[0]
                data.move_cam(data.get_activ_igrok().s_army_list[0].koordinat);
                data.set_activ_army(data.get_activ_igrok().s_army_list[0]);
                index_unit = 0;
            }//���� �� ������ �����
            else
            {
                if (data.get_activ_igrok().city_list.Count > 0) data.move_cam(data.get_activ_igrok().city_list[0].koordinat);
            }
            if (data.get_activ_igrok().bot_flag) data.get_activ_igrok().action_bot();//���� ��� �� ����� �� ��� ������
        }
    }
    //����!!!
    
    public void deselect_button()// ��������� ������ ��� ��������� �����
    {
        data.set_activ_army(null);
        obj_mouse.kursor.gameObject.SetActive(false);// ������ �������
        foreach (GameObject p in data.spisok_puti) Destroy(p);//��������� ������ ����
    }
    public void next_unit_button()// ��������� ������ ������� ������
    {
        
        // ����� ���������� ����� ��� ��������� 
        if (data.get_activ_igrok().obj_army_list.Count > 0)
        {
            index_unit++;
            if (index_unit >= data.get_activ_igrok().obj_army_list.Count) index_unit = 0;
            data.move_cam(data.get_activ_igrok().s_army_list[index_unit].koordinat);
            data.set_activ_army(data.get_activ_igrok().s_army_list[index_unit]);
            foreach (GameObject p in data.spisok_puti) Destroy(p);//��������� ������ ����
            obj_mouse.kursor.gameObject.SetActive(false);// ������ �������
        }//���� �� ������ �����
    }
    public void create_gamers()//�������� �������
    {
        for (int i = 0; i < (data.get_count_players()+1); i++)
        {
            gamer tmp_gamer;
            //���� ������ �� ������� ������� �� �������
            if (i > 2) tmp_gamer = new gamer(i, true);
            else tmp_gamer = new gamer(i, false);
            gamer_list.Add(tmp_gamer);
            gamer_list[i].spr_city = spr_list_city[i][0];//����� ���������� ������ ������ ������
        }
    }
    public void initial_place()//��������� ����������� ������ � �������
    {
        obj_mouse.kursor.SetActive(true);//��� ���� ����� �������� ����� ����� � move
        obj_mouse.kursor.SetActive(false);
        Vector2[] list_city_kor = new Vector2[9];
        list_city_kor[0] = new Vector2(-2.8f, 2.8f);
        list_city_kor[1] = new Vector2(0f, 2.8f);
        list_city_kor[2] = new Vector2(2.8f, 2.8f);
        list_city_kor[3] = new Vector2(-2.8f, 0f);
        list_city_kor[4] = new Vector2(0f, 0f);
        list_city_kor[5] = new Vector2(2.8f, 0f);
        list_city_kor[6] = new Vector2(-2.8f, -2.8f);
        list_city_kor[7] = new Vector2(0f, -2.8f);
        list_city_kor[8] = new Vector2(2.8f, -2.8f);
        for (int i = 0; i < 9; i++)
        {
            Vector3 koor = new Vector3(list_city_kor[i].x, list_city_kor[i].y, -2.0f);
            GameObject obj_city = (GameObject)Instantiate(city_prefab, koor, Quaternion.identity);
            city tmp_city = obj_city.GetComponent(typeof(city)) as city;
            tmp_city.koordinat = koor;//������ ������ ������ ��� ����������
            tmp_city.vladelec = gamer_list[0];//���� ��� ������ ������� �������
            city_obj_list.Add(obj_city);

        }
        //������� ������ �������
        change_city_player(0, 1);
        change_city_player(0, 3);
        change_city_player(0, 4);
        change_city_player(0, 5);
        change_city_player(0, 7);
        change_city_player(1, 0);
        change_city_player(2, 2);
        if (data.get_count_players() > 2) change_city_player(3, 6);
        if (data.get_count_players() > 3) change_city_player(4, 8);
        //���� ��������� ����� �������
        for (int i = 0; i < (data.get_count_players() + 1); i++)
        {
            //���������� ����� ��� ������ �������� ����� �����
            //���������� ������ ������ � ���������� ���� ��������� �����
            foreach (city c in gamer_list[i].city_list)
            {
                Vector3 koor_unit = new Vector3(c.koordinat.x - 0.2f, c.koordinat.y + 0.2f, c.koordinat.z);
                GameObject unit_tmp_obj = (GameObject)Instantiate(unit_prefab, koor_unit, Quaternion.identity);//������� ������ �����
                unit tmp_unit_s = unit_tmp_obj.GetComponent(typeof(unit)) as unit;
                tmp_unit_s.obj_unit = unit_tmp_obj;//���� �������� ���� ������
                tmp_unit_s.obj_unit.SetActive(false);//��� ����� �� ������, ����� ������ �����
                tmp_unit_s.id_unit = data.id_unit_count++;
                tmp_unit_s.set_koordinat(koor_unit);
                tmp_unit_s.set_unit(data.start_unit, gamer_list[i], get_sprite_unit(i, data.start_unit), spr_unit_off);//����������� ����
                
                create_new_army(tmp_unit_s);//������� ����� �� ������ �����

            }
        }
    }
    public void change_city_player(int num_igrok, int num_gorod)//����� ������� ������ ��������� � ������
    {
        city tmp_city = city_obj_list[num_gorod].GetComponent(typeof(city)) as city;
        tmp_city.change_vladelec(gamer_list[num_igrok]);//�������� ������ 1 � �����
    }
    //����� �������� ����� ������

    public void create_new_army(unit u)//�������� ����� ����� �� �����
    {
        
        Vector3 koor_unit = u.koordinat;//���������� �������� ����� ����� ����������� �����
        GameObject army_tmp_obj = (GameObject)Instantiate(army_prefab, koor_unit, Quaternion.identity);//������� ������ �����
        s_army tmp_army_s = army_tmp_obj.GetComponent(typeof(s_army)) as s_army; ;//������� ������ �����
        tmp_army_s.id = data.id_army_count++; //�������� id �����
        tmp_army_s.set_koordinat(koor_unit);//����� �������� ���� ���������
        tmp_army_s.set_obj(army_tmp_obj);//����� �������� ������
        tmp_army_s.vladelec = u.vladelec;
        GameObject flag_tmp_obj = (GameObject)Instantiate(flag_prefab, koor_unit, Quaternion.identity);//������� ������ �����
        tmp_army_s.army_flag = flag_tmp_obj;//���� ���������� ���� ����
        tmp_army_s.flags_sprites = get_sprite_flag(tmp_army_s.vladelec.id);

        //�������� ����� ������
        //unit_tmp_obj.GetComponent<SpriteRenderer>().sprite = u.spr_unit;//���������� ������
        tmp_army_s.add_unit(u);//������� � ����� ����� ����
        gamer_list[u.vladelec.id].obj_army_list.Add(army_tmp_obj);//������ ��������
        gamer_list[u.vladelec.id].s_army_list.Add(tmp_army_s);//�������� � ��������
        tmp_army_s.set_army();
        

    }
    public Sprite get_sprite_unit(int num_igrok, int num_type)//��������� ������� ����� �� ������ ������ � ������ �����
    {
        return spr_list_unit[num_igrok][num_type];
    }
    public Sprite[] get_sprite_flag(int num_igrok)//��������� �������� ����� �� ������ ������
    {
        Sprite[] tmp_spr;
        switch(num_igrok)
        {
            case 0:
                tmp_spr = spr_flag_grey;
                break;
            case 1:
                tmp_spr = spr_flag_bel;
                break;
            case 2:
                tmp_spr = spr_flag_dark;
                break;
            case 3:
                tmp_spr = spr_flag_zel;
                break;
            case 4:
                tmp_spr = spr_flag_orange;
                break;
            default:
                tmp_spr = spr_flag_grey;
                break;
        }
        return tmp_spr;
    }
    public Sprite[] get_sprite_unit_off()
    {
        return spr_unit_off;
    }
    public void finih_atack(List<unit> unit_list_atack, List<bool> f_a, List<unit> unit_list_def, List<bool> f_d)
    {//����� ����������� ���������� ��� �� �����
        //������ ������ �����
        for (int i= unit_list_atack.Count-1; i>-1 ;i--)
        {
            if (!f_a[i])
            {
                unit_list_atack[i].sc_army.sub_unit_destroy(unit_list_atack[i]);//���� ���� ���� ������ ����
                f_a.RemoveAt(i);
            }
        }
        for (int i = unit_list_def.Count-1; i > -1; i--)
        {
            if (!f_d[i])
            {
                unit_list_def[i].sc_army.sub_unit_destroy(unit_list_def[i]);//���� ���� ���� ������ ����
                unit_list_def.Remove(unit_list_def[i]);//�.�. ������ ���������� ��� ������ ������ ������ � ���
                f_d.RemoveAt(i);

            }
        }
        //���� ��� ��������� ���� � ���� ������ ������, �� ����� ���������� ������ ���������
        if ((unit_list_def.Count<1) & (data.get_activ_army().get_target_city() != null))
            data.get_activ_army().get_target_city().change_vladelec(unit_list_atack[0].vladelec);
        //data.set_def_city(null);//������� ������ �� ��� �����
        //unit_list_atack.Clear();
        //unit_list_def.Clear();
        data.setting_panel_unit();//�������� ������ � �������
    }
    public List<city> get_city_list()
    {//����� ��������� ������ �������
        List<city> tmp_city_list = new List<city>();
        foreach (GameObject city_obj in city_obj_list)
        {
            tmp_city_list.Add(city_obj.GetComponent(typeof(city)) as city);
        }
        return tmp_city_list;
    }
    public List<city> get_city_list_other()
    {//����� ��������� ������ ������� ������ ������� (������������� �������-���������� ��������� �������)
        List<city> tmp_city_list = new List<city>();
        foreach (GameObject city_obj in city_obj_list)
        {
            city tmp_city = city_obj.GetComponent(typeof(city)) as city;
            if (tmp_city.vladelec.id!= data.get_activ_igrok().id)
                tmp_city_list.Add(tmp_city);
        }
        return tmp_city_list;
    }
    public List <item_cell> get_put_cell(Vector3 start, Vector3 finish)
    {//����� ������ ���� ����� ����� ������, ���������� ������ ����� �� ������� �������� ���������� ����
        List<item_cell> put_cell_list;//������ ������ ����
        data.set_st_f_point(start, finish);//(vector2int)  data.st_p, data.fin_p ������� ������� ���������� � ������� �����
        item_cell tek_cel = data.kletki[data.st_p.x, data.st_p.y];//������� ������
        item_cell finish_cell = data.kletki[data.fin_p.x, data.fin_p.y];//�������� ������
        List<item_cell> open_list;//������� ������ ������
        List<item_cell> close_list = new List<item_cell>();//������� ������ ������
        bool end_put = true;
        while (end_put)//���� �� ������ ����� ����
        {
            open_list = create_open_list(tek_cel);//������� ��������� �������� ������
            //���� ��������� ������� ������ �������� �������� ������ �� �� ����� ��� ������ ����
            if (open_list.Contains(finish_cell))
            {
                //close_list.Add(finish_cell); //�������� ������ ���� ������� � ������ ����
                end_put = true;
                break;
            }
            tek_cel = find_new_tek_cell(open_list);
            close_list.Add(tek_cel);
        }
        put_cell_list = close_list;
        return put_cell_list;
        //�������� ��������� ������ ������
        List<item_cell> create_open_list(item_cell tek_cel_old)
        {

            List<item_cell> open_list_new = new List<item_cell>();// ����� ������� ������ ������
            int min_indx_x = tek_cel_old.idx_kor.x - 1;
            int max_indx_x = tek_cel_old.idx_kor.x + 1;
            int min_indx_y = tek_cel_old.idx_kor.y - 1;
            int max_indx_y = tek_cel_old.idx_kor.y + 1;
            //�������� ����� �� ��������� �� ���� ������� ������
            if (min_indx_x < data.min_kletka_x) min_indx_x = 0;
            if (min_indx_y < data.min_kletka_y) min_indx_y = 0;
            if (max_indx_x > data.max_kletka_x) max_indx_x = 17;
            if (max_indx_y > data.max_kletka_y) max_indx_y = 17;
            for (int i = min_indx_x; i <= max_indx_x; i++)
                for (int j = min_indx_y; j <= max_indx_y; j++)
                {
                    item_cell cell = data.kletki[i, j];//�������� ��������� ������
                    if (close_list.Contains(cell)) continue;//��� ������ � �������� ������, ���������� ���������� �������
                    if (cell.id == tek_cel_old.id) continue;//��� ������ �������, ���������� ���������� �������
                    else
                    {
                        cell.set_aproxim(finish_cell);//����������� ���������� ��� ������
                        cell.set_weight();//��������� ��� ������
                        open_list_new.Add(cell);//��������� � �������� ������
                    }
                }
            return open_list_new;
            //throw new NotImplementedException();
        }
        //����� ����� ������� ������
        item_cell find_new_tek_cell(List<item_cell> list_op_c)
        {
            item_cell tek_cel_new = list_op_c[0];
            int min_weigth = tek_cel_new.weight;
            foreach (item_cell cell in list_op_c)
            {
                if (cell.weight <= min_weigth)
                {
                    tek_cel_new = cell;//����� ��������� ������� ������ � ����������� �����
                    min_weigth = cell.weight;//������� �������;
                }
            }

            return tek_cel_new;
            //throw new NotImplementedException();
        }
    }
    public void bot_atack_target(Vector3 target)
    {
        bot_klick_mouse(3,target);//��������� ���� ����� � ����� ����� �� �����
        bot_klick_kursor();
        
    }
    public void bot_move_target(Vector3 target)
    { //1-�����������, 2-�����, 3-����� ������
        bot_klick_mouse(1, target);//��������� ���� ����� � ����� ����������� � �����
        bot_klick_kursor();

    }
    public void bot_klick_mouse(int type_event, Vector3 koor_clk)
    {//�������� ����� ����� �����, ����� ��������� ������
        data.type_event = type_event;//��� ������� ��� ���������� ���������
        koor_clk.z = 2.1f;
        obj_mouse.kursor.transform.position = data.get_grid_step(koor_clk);//���������� ������
        obj_mouse.mouse_event(type_event);
    }
    public void bot_klick_kursor()
    {//�������� ����� ����� �����, ����� ������������ � �������
        GameObject kr = obj_mouse.kursor;
        move mv= kr.GetComponent(typeof(move)) as move;
        mv.start_move();//����� ��������
    }
    public void set_sprite_army_flag(gamer igrok,Vector3 k)
    {//����� ������� ����� ������� ������������ ���������� ������� ����� � ����� �� �������� �����������
        List<s_army> army_koor_lists = new List<s_army>();//������ ����� �� ����� �����������
        s_army army_max_strengh = data.get_activ_army(); ;//����� � ����� ������� ������
        int strenght = 0;
        int count_unit = 0;//���������� ������ � ������;
        army_koor_lists.Clear();
        foreach (s_army a in igrok.s_army_list)
        {//�������� ��� ����� � ���� ������
            if (a.check_koordinat(k))
            {
                army_koor_lists.Add(a);
                count_unit = count_unit + a.get_unit_list().Count;//������� ���������� ������ � ������
            }
        }
        if (army_koor_lists.Count() > 0)
        {
            foreach (s_army a in army_koor_lists)
            {
                a.army_flag.SetActive(false);//��������� ��� ����� � �����
                a.get_obj().SetActive(false);//��������� ������� ���� �����
            }
            foreach (s_army a in army_koor_lists)
            {
                if (a.get_max_unit_str() >= strenght)
                {
                    strenght = a.get_max_unit_str();
                    army_max_strengh = a;
                }
            }
            if (data.get_activ_army() != null)
            {//���� ���� �������� ������
                if (data.get_activ_army().check_koordinat(k))//� ��� �� ����� ������ �� ������� ��
                {//���� � ������ ���� �������� �����, �� ������ �� ������ ����� ����� ������� �����
                    army_max_strengh = data.get_activ_army();
                }
            }
            army_max_strengh.get_obj().SetActive(true);//������ ���������� � ����� ����� ������ ������/�������� �����
            army_max_strengh.army_flag.GetComponent<SpriteRenderer>().sprite = army_max_strengh.flags_sprites[count_unit - 1];//������ ����� ������� �� �������� �����
            army_max_strengh.army_flag.SetActive(true);
        }

    }
}
