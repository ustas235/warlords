using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public GameObject kursor;
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

        if (data.can_move_cell_list.Count > 0)
        {
            StartCoroutine(start_move(0.2f));//������ �������� � ���������
            foreach (GameObject p in data.spisok_puti) Destroy(p);
        }
       
    }
    IEnumerator start_move(float time)
    {//������������ ��������
        // yield �� ����� ���������� YieldInstruction, ������� ���� 5 ������.
        kursor.transform.position = new Vector3(kursor.transform.position.x, kursor.transform.position.y, -5.0f);//�������� ������
        foreach (item_cell c in data.can_move_cell_list)
        {
            yield return new WaitForSeconds(time);
            data.get_activ_army().move_army(c.koordint3x);//���������� ����� � ��������� ������
            
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
            //���� ��������� �� ���������� �������� ���
            if (data.get_activ_army().check_koordinat(kursor.transform.position)) data.get_activ_army().attack_event_army(); //���������� ����� � ��������� ������� - ������� ���
        }
        if (data.type_event == 3) //�������� ����� �� �����
        {
            //���� ��������� �� ���������� �������� ���
            if (data.get_activ_army().check_koordinat(kursor.transform.position)) data.get_activ_army().attack_event_city();//�������� ����� �� ������ �����
        }
    }
}
