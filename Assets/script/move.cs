using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class move : MonoBehaviour
{
    public GameObject kursor;
    item_cell next_cell, prev_cell;
    Vector3 MousePos = new Vector3(-2.5f, -1.7f, 0f);
    bool flag_collision_enemy = false;//���� ��� ��������� �� ������ �����
    public data_game data;//����� ��� ���� �������� ��� ������ ����
    // Start is called before the first frame update
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
    private void OnMouseDown()
    {
        if (!data.get_flag_army_is_move())
        {//���� ��� ����� � ��������
            if ((!EventSystem.current.IsPointerOverGameObject()) & (data.get_activ_army() != null))
            {
                start_move();
            }
        }
    }
    public void start_move()
    {
        GameObject obj_player = GameObject.Find("land");
        //� ������� �������� ���� ������ ���� ���
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
        if (data.can_move_cell_list.Count > 0)
        {
            foreach (GameObject p in data.spisok_puti) Destroy(p);
            data.set_flag_army_is_move(true);//��������� ���� �������� �����
            StartCoroutine(start_move_coroutine());
        }
    }
    IEnumerator start_move_coroutine()
    {//������������ �������� (���� ��� ��������)
        // yield �� ����� ���������� YieldInstruction, ������� ���� 5 ������.
        kursor.transform.position = new Vector3(kursor.transform.position.x, kursor.transform.position.y, -5.0f);//�������� ������
        foreach (item_cell c in data.can_move_cell_list)
        {
            prev_cell = next_cell;
            next_cell = c;
            Vector3 old_koordinat = data.get_activ_army().koordinat;//�������� ���������� ���������� �����
            data.get_activ_army().move_army(next_cell.koordint3x);//���������� ����� � ��������� ������
            data.game_s.set_sprite_army_flag(data.get_activ_army().vladelec,next_cell.koordint3x);//�������� ����������� ����� � ����� � ����� ������
            data.game_s.set_sprite_army_flag(data.get_activ_army().vladelec, old_koordinat);//�������� ����������� ����� � ����� � ������ ������
            //����������� ����
            int delta_hod = c.get_cost_move();//��������� ����������� ����
            data.get_activ_army().update_count_hod(delta_hod);//������� ���������� ����� � ������ � �����
            data.get_activ_army().set_army();//������� ��������� �����*/
            data.setting_panel_unit(); //����������� ������ � �������
            //��� ����� ��������
            yield return new WaitForSeconds(0.2f);
            //���� � ��� ������ ����������������� ����� ���� ���� - ������ ���
            flag_collision_enemy = false;
            foreach (gamer g in data.game_s.get_gamer_list())
            {//���������� ���� �������
                if (data.get_activ_igrok().id!=g.id)
                {//������� ������ ����� �������
                    foreach (s_army a in g.s_army_list)
                    {//���������� ����� ������� ������
                        if (data.get_activ_army().check_koordinat(a.koordinat))
                        {
                            flag_collision_enemy = true;//���� ��� ��������� �� ������ �����
                            data.def_army = a;//�������� ����� �����
                            break;
                        }
                    }
                }
                if (flag_collision_enemy) break;
            }
            //���������� ��������
            if (flag_collision_enemy)
            {
                data.type_event = 2;//������� ��� �������
                break;//��������� ������������
            }
           
        }
        kursor.gameObject.SetActive(false);
        data.move_cam(data.get_activ_army().koordinat);//���������� ������
        //data.setting_panel_unit(); //����������� ������ � �������
        /*int delta_hod = data.get_activ_army().tek_hod - data.get_activ_army().tek_hod_tmp;//��������� ����������� ����
        data.get_activ_army().update_count_hod(delta_hod);//������� ���������� ����� � ������ � �����
        data.get_activ_army().set_army();//������� ��������� �����*/
        //� ����� �������� ������� ������ ��������
        data.can_move_cell_list.Clear();
        
        //data.get_activ_army().tek_hod = data.get_activ_army().tek_hod_tmp;//������� ������� ����� ����� �����������
        switch (data.type_event)
        {//�������� � ����������� �� ���� ��������
            case 1://������ ��������, ����� ��� ���������
                if (data.get_activ_army().check_koordinat(kursor.transform.position))
                {  //���� ��������� �� ����� ����������
                    if (data.get_activ_army().get_status()==4) //���� ������ ����� ��� ����� � ���� ����� �� ������� �� ���������
                        data.get_activ_army().set_status(0);//������ ����� ����������� ���������
                    data.get_activ_army().flag_old_target = false;//������� ���� ������� ������ ����
                }
                data.set_flag_army_is_move(false);//����� ��������� �������� ����� �� ��� �����
                break;
            case 2:// ����� �� ������ �����
                if ((data.get_activ_army().check_koordinat(kursor.transform.position))||(flag_collision_enemy))
                {  //���� ��������� �� ���������� �������� ���
                    data.get_activ_army().flag_old_target = false;//������� ���� ������� ������ ����
                    data.get_activ_army().attack_event_army(); //���������� ����� � ��������� ������� - ������� ���
                    flag_collision_enemy = false;
                    
                }

                else data.set_flag_army_is_move(false);//���� �� ����� �� ���� ����������� ����� ��������� �������� ����� �� ��� �����
                break;
            case 3:// ����� �� �����
                if (data.get_activ_army().check_koordinat(kursor.transform.position))
                {//���� ��������� �� ���������� �������� ���
                    data.get_activ_army().set_status(3);
                    data.get_activ_army().flag_old_target = false;//������� ���� ������� ������ ����
                    data.get_activ_army().attack_event_city();//�������� ����� �� ������ �����
                }
                else data.set_flag_army_is_move(false);//���� �� ����� �� ���� ����������� ����� ��������� �������� ����� �� ��� �����
                break;
            default://�� ���
                data.set_flag_army_is_move(false);//����� ��������� �������� ����� �� ��� �����
                break;
        }
        data.type_event = 0;
    }
   
}
