using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Vector3 koordinat;
    public int id_unit=1;//����� ������������� ����� 0- ������ ������, 1- �������, 2- ������
    public int count_hod = 1, count_hod_start = -1;//���������� ����� �� ���������� �������������
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
        if (data.tek_activ_igrok.id == vladelec.id)//������ ������� ��������
        {
            if (data.get_activ_unit() == null)//��� �������� ������ ������� ������ ������
            {
                data.activ_city = this;//�������� ���� � �������� ������
                data.city_window.SetActive(true);//���� �������� ����� �������� ������ �������� ������
            } 
            else
            {
                data.type_event = 1;//������� �����������
                obj_mouse.mouse_event(1);//���������� ���� ����
            }
                
        }
    }
    public void change_vladelec(gamer vlad)
    {
        vladelec = vlad;
        spr_city = vlad.spr_city;//����� ����� ������ ���������
        this.GetComponent<SpriteRenderer>().sprite = spr_city;
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
                GameObject unit_tmp = (GameObject)Instantiate(unit_prefab, koor_unit, Quaternion.identity);
                unit tmp = unit_tmp.GetComponent(typeof(unit)) as unit;//�� ������
                tmp.set_koordinat(koor_unit);//���� �������� ���� ���������
                game_s.change_unit_player(vladelec.id, unit_tmp, id_unit);//����������� ����
            }
        }
    }
    public void setting_activ_city(int num_unit)//����� ��������� ������������ �����
    {
        id_unit = num_unit;
        switch (num_unit)
        {
            case 1://������ ������ ��������1 ���
                count_hod_start = 1;
                count_hod = 1;
                break;
            case 2://������� ������ �������� 2 ���
                count_hod_start = 2;
                count_hod = 2;
                break;
            case 3://������ �������� 3 ���
                count_hod_start = 3;
                count_hod = 3;
                break;
            default:
                count_hod_start = 1;
                count_hod = 1;
                break;

        }
    }
}
