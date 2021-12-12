using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class s_panel_unit : MonoBehaviour
{
    data_game data;
    public Sprite[] spr_on= new Sprite[8];
    public Sprite[] spr_off= new Sprite[8];
    List <GameObject> buttons_unit=new List<GameObject>();//������ �������� ������
    List<bool> flags = new List<bool>();//����� ��������� ������
    List <unit> s_unit_list;//������ ���������� �����
    // Start is called before the first frame update
    private void Awake()
    {
        spr_on = new Sprite[8];
        spr_off = new Sprite[8];
        for (int i = 0; i < 8; i++)
        {
            string tmp = "but_unit_" + i.ToString();
            buttons_unit.Add(this.transform.Find(tmp).gameObject);//������� � ������ 8 ������
            flags.Add(false);//���� ��� ����� �������
        }
        GameObject obj_player = GameObject.Find("land");
        //� ������� �������� ���� ������ ���� ���
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void button_0()
    {
        event_button(0);//��������� ����� �� ������ ������
    }
       
    public void button_1()
    {
        event_button(1);//��������� ����� �� ������ ������
    }
    public void button_2()
    {
        event_button(2);//��������� ����� �� ������ ������
    }
    public void button_3()
    {
        event_button(3);//��������� ����� �� ������ ������
    }
    public void button_4()
    {
        event_button(4);//��������� ����� �� ������ ������
    }
    public void button_5()
    {
        event_button(5);//��������� ����� �� ������ ������
    }
    public void button_6()
    {
        event_button(6);//��������� ����� �� ������ ������
    }
    public void button_7()
    {
        event_button(7);//��������� ����� �� ������ ������
    }
    void event_button(int num_button)//����� ��������� ������� ������ � ����������� �� �� ������
    {
        //�������� �� �� ����� ������� ��������� ������ ���� ����
        //������ ��������� ����� ��� ����� ���������
        flags[num_button] = !flags[num_button];
        if (flags.Contains(true))
        {
            if (flags[num_button]) buttons_unit[num_button].GetComponent<Image>().sprite = spr_on[num_button];
            else buttons_unit[num_button].GetComponent<Image>().sprite = spr_off[num_button];
            //����� ������� ������� ������� ���� �����
            //� ��������� ����� � ����� �����
            //����������������� ����
            //���� �������� ���� � �������� �����
            if (flags[num_button]) data.get_activ_army().add_unit(s_unit_list[num_button]);
            //����� ������ ���� �� ������� ����� c ��������� ����� �� ��� ������
            else data.get_activ_army().sub_unit_create(s_unit_list[num_button]);

        }
        else flags[num_button] = !flags[num_button];//������� �������
        //data.tek_activ_igrok.s_army_list.Add(tmp_army);//��������� ����� ����� � ������ �����
        //data.set_activ_army(tmp_army);//������� ����� ��������

    }
    public void set_panel_unit(List<unit> unit_list)//���� ��������� ������ ������
    {
        s_unit_list = unit_list;
        for (int i=0;i<8;i++)
        {
            if (i < unit_list.Count)//���� � ��������� ������ ������ ������ ��� ����� ������� ������
            {
                spr_on[i] = unit_list[i].spr_unit;
                spr_off[i] = unit_list[i].spr_unit_off;
                if (unit_list[i].contains_to_list(data.get_activ_army().unit_list))
                {
                    buttons_unit[i].GetComponent<Image>().sprite = spr_on[i];//��� �� �������� ����� ��������
                    flags[i] = true;
                }
                else
                {
                    buttons_unit[i].GetComponent<Image>().sprite = spr_off[i];
                    flags[i] = false;
                }
                buttons_unit[i].SetActive(true);//������� ������
            }
            else buttons_unit[i].SetActive(false);
        }
    }
}
