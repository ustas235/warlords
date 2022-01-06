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
    //��������� ����
    public bool bot_flag = false;//���� ��� ���������
    public int bot_army_create_city = 2;//��� ����� �������� �����
    public int bot_min_garnison = 2;//�������� � ������
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
    public gamer(int num, bool flag_bot)//����������� ��������� ����� � ��� ����� ���/�� ���
    {
        id = num;
        bot_flag = flag_bot;
    }
    //------------------------------------
    //�������� ����
    public void set_bot(int level_bot)
    {//��������� ���� � ��������� �� ������
        switch (level_bot)
        {
            case 0:

                break;
            case 1:

                break;
            case 2:

                break;
            default:
                
                break;
        }
    }
    public void action_bot()
    {//������� ����
        foreach (city c in city_list)
        {
            c.setting_activ_city(1);
            accumulation_garnison(c);//������ ��������
        }
    }
    //���������� ���������� �������
    public void accumulation_garnison(city c)
    {
        List<unit> unit_city_list = new List<unit>();//������ ���� ������ ������
        foreach (s_army a in s_army_list)//���������� ��� ����� ������
        {
            if (c.is_garnison(a))//���� ��������� ����� � ����� ������
            {
                foreach (unit u in a.unit_list) unit_city_list.Add(u);//���������� ��������� ���� � ������
            }
        }
        //�������� ��������
        if (unit_city_list.Count>= bot_min_garnison)
        {
            //���� ������ ���� ������� � ������� �����, �� �������� ��� ����
            if (unit_city_list[0].sc_army.unit_list.Count > 1) unit_city_list[0].sc_army.sub_unit_create(unit_city_list[0]);
            s_army tmp_army_garnison= unit_city_list[0].sc_army;
            //����� ���������� � ����� ������� �����
            for (int i=1;i< bot_min_garnison;i++)
            {
                tmp_army_garnison.add_unit(unit_city_list[i]);//���������� � ����� ��������� ���������� �����
            }
            tmp_army_garnison.move_army(c.koordinat_garnizon);//���������� � ����� ���������
        }
        
    }
    //����� �����
    public void search_target()
    {

    }
    //���������� ����� � �����
    public void training_army()
    {

    }
    //����� � ����
    public void campaign_target()
    {

    }
}
