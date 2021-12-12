using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class s_panel_attack : MonoBehaviour
{
    public data_game data;//����� ��� ���� �������� ��� ������ ����
    public Sprite[] spr_atack = new Sprite[8];
    public Sprite[] spr_deff = new Sprite[8];
    List<bool> flags_atack ;//����� ��������� ���������
    List<bool> flags_def ;//����� ��������� ���������
    public List<GameObject> obj_img_list_atack = new List<GameObject>();// ������ �������� ����������� ��������� �����
    public List<GameObject> obj_img_list_def = new List<GameObject>();// ������ �������� ����������� ���������� �����

    // Start is called before the first frame update
    private void Awake()
    {
        //������ ������ �� ��� �������� ����
        spr_atack = new Sprite[8];
        spr_deff = new Sprite[8];
        for (int i = 0; i < 8; i++)
        {
            string tmp_atack = "img_atack_" + i.ToString();
            string tmp_def = "img_def_" + i.ToString();
            
            obj_img_list_atack.Add(this.transform.Find(tmp_atack).gameObject);//������� � ������ 8 ����������� �����
            obj_img_list_def.Add(this.transform.Find(tmp_def).gameObject);//������� � ������ 8 ����������� �����
            
        }

    }

    void Start()
    {
        GameObject obj_player = GameObject.Find("land");
        //� ������� �������� ���� ������ ���� ���
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void exit()
    {
        data.attack_window.SetActive(false);
    }
    public void set_panel_atack(List<unit> unit_list_atack,List<bool> f_a, List<unit> unit_list_def, List<bool> f_d)
    {//����� �������� ������ �����, �������� ��� ������ ������: ����� � ������
        flags_atack = f_a;
        flags_def = f_d;
        Sprite tmp_spr;
        for (int i=0;i<8;i++)
        {
            //������� ����������� ����� - �� ������
            int[] por = new int[8] {3,4,2,5,1,6,0,7};
            int j;
            for (i=0;i<8;i++)
            {
                j = por[i];//������� ������ ��� ��������� �� ������
                //�������� ����������� ���������
                if (i< unit_list_atack.Count)// ���� � ��������� ������ ������ ������ ��� ����� �����
                {
                    if(flags_atack[i]) tmp_spr= unit_list_atack[i].spr_unit;
                    else tmp_spr = unit_list_atack[i].spr_unit_off;//���� ���� �����, �� ������ ����� ��������
                    obj_img_list_atack[j].GetComponent<Image>().sprite = tmp_spr;//�������� ����������� �����
                    obj_img_list_atack[j].SetActive(true);//������� ����������� ������
                }
                else obj_img_list_atack[j].SetActive(false);//�� ������� ����������� ������
                
                //�������� ����������� ����������
                if (i < unit_list_def.Count)// ���� � ��������� ������ ������ ������ ��� ����� �����
                {
                    if(flags_def[i]) tmp_spr = unit_list_def[i].spr_unit;
                    else tmp_spr = unit_list_def[i].spr_unit_off;//���� ���� �����, �� ������ ����� ��������
                    obj_img_list_def[j].GetComponent<Image>().sprite = tmp_spr;//�������� ����������� �����
                    obj_img_list_def[j].SetActive(true);//������� ����������� ������
                }
                else obj_img_list_def[j].SetActive(false);//�� ������� ����������� ������

            }
        }
    }
}
