using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class move : MonoBehaviour
{
    public GameObject kursor;
    item_cell next_cell;
    Vector3 MousePos = new Vector3(-2.5f, -1.7f, 0f);
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
        if ((!EventSystem.current.IsPointerOverGameObject()) & (data.get_activ_army() != null))
        {
            start_move();
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
            StartCoroutine(start_move_coroutine());

        }
    }
    IEnumerator start_move_coroutine()
    {//������������ �������� (���� ��� ��������)
        // yield �� ����� ���������� YieldInstruction, ������� ���� 5 ������.
        kursor.transform.position = new Vector3(kursor.transform.position.x, kursor.transform.position.y, -5.0f);//�������� ������
        foreach (item_cell c in data.can_move_cell_list)
        {
            
            next_cell = c;
            data.get_activ_army().move_army(next_cell.koordint3x);//���������� ����� � ��������� ������
            //��� ����� ��������
            yield return new WaitForSeconds(0.2f);

        }
        kursor.gameObject.SetActive(false);
        data.move_cam(data.get_activ_army().koordinat);//���������� ������
        data.setting_panel_unit(); //����������� ������ � �������
        int delta_hod = data.get_activ_army().tek_hod - data.get_activ_army().tek_hod_tmp;//��������� ����������� ����
        data.get_activ_army().update_count_hod(delta_hod);//������� ���������� ����� � ������ � �����
        data.get_activ_army().set_army();//������� ��������� �����
        //� ����� �������� ������� ������ ��������
        data.can_move_cell_list.Clear();
        
        //data.get_activ_army().tek_hod = data.get_activ_army().tek_hod_tmp;//������� ������� ����� ����� �����������
        if (data.type_event == 2) //�������� ����� �� ������ �����
        {
            data.get_activ_army().set_status(0);//������ ����� ����������� ���������
            //���� ��������� �� ���������� �������� ���
            if (data.get_activ_army().check_koordinat(kursor.transform.position)) data.get_activ_army().attack_event_army(); //���������� ����� � ��������� ������� - ������� ���
            else data.set_flag_army_is_move(false);//���� �� ����� �� ���� ����������� ����� ��������� �������� ����� �� ��� �����
        }
        if (data.type_event == 3) //�������� ����� �� �����
        {
            //���� ��������� �� ���������� �������� ���
            data.get_activ_army().set_status(0);//������ ����� ����������� ���������
            if (data.get_activ_army().check_koordinat(kursor.transform.position)) data.get_activ_army().attack_event_city();//�������� ����� �� ������ �����
            else data.set_flag_army_is_move(false);//���� �� ����� �� ���� ����������� ����� ��������� �������� ����� �� ��� �����
        }
        if (data.type_event == 1)
            data.set_flag_army_is_move(false);//����� ��������� �������� ����� �� ��� �����
    }
   
}
