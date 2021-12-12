using System.Collections.Generic;
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
    public GameObject butt_end;//���� ������ ����� ����
    mouse obj_mouse;//������ � ��������� ����
    public int num_tek_igrok = 1;
    Sprite[] spr_unit_grey, spr_unit_bel, spr_unit_dark, spr_unit_zel, spr_unit_orange, spr_unit_off;//������ �������� ������
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
        data.tek_activ_igrok = gamer_list[1];//�������� ��������� ������

        initial_place();//������� � ����������� ������
        //���������� ������ � ����� ������� ������
        data.set_activ_army(data.tek_activ_igrok.s_army_list[0]);//�������� ���� 
        data.move_cam(data.tek_activ_igrok.city_list[0].koordinat);

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void end_turn()//��������� ������ ����� ����
    {
        foreach (GameObject p in data.spisok_puti) Destroy(p);//��������� ������ ����

        int next = data.tek_activ_igrok.id;
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
                foreach (s_army army in gamer_list[next].s_army_list) army.tek_hod = army.max_hod;
                //�������� ������������ ������ �� ���� ������� ������
                foreach (city c in gamer_list[next].city_list) c.create_unit();
                break;
            }
            next++;
            count++;
        }
        print("����� �����  " + num_tek_igrok);
        butt_end.GetComponentInChildren<Text>().text = num_tek_igrok.ToString();//���� ������� ����� ��������� ������
        data.tek_activ_igrok = gamer_list[num_tek_igrok];//�������� � ���� �������� ������
        //��������� ������ �� ����� ������
        if (data.tek_activ_igrok.obj_army_list.Count > 0)
        {
            //Vector3 tmp_vect=data.tek_activ_igrok.unit_list[0]
            data.move_cam(data.tek_activ_igrok.s_army_list[0].koordinat);
            data.set_activ_army(data.tek_activ_igrok.s_army_list[0]);
            index_unit = 0;
        }//���� �� ������ �����
        else
        {
            if (data.tek_activ_igrok.city_list.Count > 0) data.move_cam(data.tek_activ_igrok.city_list[0].koordinat);
        }

    }
    public void deselect_button()// ��������� ������ ��� ��������� �����
    {
        data.set_activ_army(null);
        obj_mouse.kursor.gameObject.SetActive(false);// ������ �������
        foreach (GameObject p in data.spisok_puti) Destroy(p);//��������� ������ ����
    }
    public void next_unit_button()// ��������� ������ ������� ������
    {
        // ����� ���������� ����� ��� ��������� 
        if (data.tek_activ_igrok.obj_army_list.Count > 0)
        {
            index_unit++;
            if (index_unit >= data.tek_activ_igrok.obj_army_list.Count) index_unit = 0;
            data.move_cam(data.tek_activ_igrok.s_army_list[index_unit].koordinat);
            data.set_activ_army(data.tek_activ_igrok.s_army_list[index_unit]);
            foreach (GameObject p in data.spisok_puti) Destroy(p);//��������� ������ ����
            obj_mouse.kursor.gameObject.SetActive(false);// ������ �������
        }//���� �� ������ �����
    }
    public void create_gamers()//�������� �������
    {
        for (int i = 0; i < 5; i++)
        {
            gamer_list.Add(new gamer(i));
            gamer_list[i].spr_city = spr_list_city[i][0];//����� ���������� ������ ������ ������
        }
    }
    public void initial_place()//��������� ����������� ������ � �������
    {
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
        change_city_player(1, 0);
        change_city_player(2, 2);
        change_city_player(3, 6);
        change_city_player(4, 8);
        //���� ��������� ����� �������
        for (int i = 1; i < 5; i++)
        {
            gamer_list[i].spr_city = spr_list_city[i][0];//����� ���������� ������ ������ ������
            //���������� ����� ��� ������ �������� ����� �����
            Vector3 koor_unit = new Vector3(gamer_list[i].city_list[0].koordinat.x - 0.2f, gamer_list[i].city_list[0].koordinat.y + 0.2f, gamer_list[i].city_list[0].koordinat.z);
            unit tmp_unit_s = new unit();
            tmp_unit_s.set_koordinat(koor_unit);
            tmp_unit_s.set_unit(2, gamer_list[i], get_sprite_unit(i, 2), spr_unit_off);//����������� ����
            create_new_army(tmp_unit_s);//������� ����� �� ������ �����



        }
    }
    public void change_city_player(int num_igrok, int num_gorod)//����� ������� ������ ��������� � ������
    {
        city tmp_city = city_obj_list[num_gorod].GetComponent(typeof(city)) as city;
        tmp_city.change_vladelec(gamer_list[num_igrok]);//�������� ������ 1 � �����
        gamer_list[num_igrok].city_list.Add(tmp_city);//��������� ����� ������
    }
    //����� �������� ����� ������

    public void create_new_army(unit u)//�������� ����� ����� �� �����
    {
        Vector3 koor_unit = u.koordinat;//���������� �������� ����� ����� ����������� �����
        GameObject army_tmp_obj = (GameObject)Instantiate(unit_prefab, koor_unit, Quaternion.identity);
        s_army tmp_army_s = army_tmp_obj.GetComponent(typeof(s_army)) as s_army;//�� ������
        tmp_army_s.set_koordinat(koor_unit);//����� �������� ���� ���������
        tmp_army_s.set_obj(army_tmp_obj);//����� �������� ������
        tmp_army_s.vladelec = u.vladelec;
        //�������� ����� ������
        army_tmp_obj.GetComponent<SpriteRenderer>().sprite = u.spr_unit;//���������� ������
        tmp_army_s.unit_list.Add(u);//������� � ����� ����� ����
        tmp_army_s.set_army();
        u.sc_army = tmp_army_s;//���� ���������� ���� �����
        gamer_list[u.vladelec.id].obj_army_list.Add(army_tmp_obj);//������ ��������
        gamer_list[u.vladelec.id].s_army_list.Add(tmp_army_s);//�������� � ��������
        
    }
    public Sprite get_sprite_unit(int num_igrok, int nim_type)//��������� ������� ����� �� ������ ������ � ������ �����
    {
        return spr_list_unit[num_igrok][nim_type];
    }
    public Sprite[] get_sprite_unit_off()
    {
        return spr_unit_off;
    }
}
