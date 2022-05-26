using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class mouse : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 MousePos=new Vector3(-2.5f, -1.7f, 0f);
    public GameObject kursor;
    List<GameObject> spisok_puti=new List<GameObject>();//������ �������� ����
    public GameObject point_put;//������ � ������ ����
    public GameObject point_put_x;//������ � ������ ���� ���� ��������� ����� �����������
    public Sprite spr_attack;
    public Sprite spr_incity;
    public Sprite spr_move;
    public data_game data;//����� ��� ���� �������� ��� ������ ����

    List<item_cell> open_list = new List<item_cell>();//������� ������ ������
    List<item_cell> close_list = new List<item_cell>();//������� ������ ������
    List<Vector2Int> put_list = new List<Vector2Int>();//���������� ����
    item_cell tek_cel ;//������� ������
    item_cell finish_cell ;//�������� ������
    //����
    public int count_test = 0;//����
    void Start()
    {
        kursor.gameObject.SetActive(false);//��� ������ ������ �������
        GameObject obj_player = GameObject.Find("land");
        //� ������� �������� ���� ������ ���� ���
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the left mouse button was clicked
        


    }
    //���� ��� �������� ����� ����
    public void OnMouseDown()
    {
        if (!data.get_flag_army_is_move())
        {
            
            if ((!IsPointerOverUIObject()) & (data.get_activ_army() != null))
            {
                data.game_s.move_kursor_clik();//���������� ������ � ����� �������� �������� �����
                data.type_event = 1;//1-����������� �������� ��� ������� ��� ���������� ���������
                data.get_activ_army().old_type_event = data.type_event;
                mouse_event(1);
            }
            if ((!IsPointerOverUIObject()) & (data.get_activ_army() == null))
            {//���� ���� �� ����� �� ��� �������� �����, �� ���������� ���� ������
                MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                MousePos.z = -10f;
                data.move_cam(MousePos);//���������� ������
            }
        }
        
    }
    //�������� ��������  ������������ ����
    public void mouse_event(int type_event)
    {
        //����� ��������� ����� �����
        //1-�����������, 2-�����, 3-����� ������
        if (type_event == 1) kursor.gameObject.GetComponent<SpriteRenderer>().sprite = spr_move;
        if (type_event == 2) kursor.gameObject.GetComponent<SpriteRenderer>().sprite = spr_attack;
        if (type_event == 3) kursor.gameObject.GetComponent<SpriteRenderer>().sprite = spr_attack;
        data.can_move_cell_list.Clear();
        //for (int i=0;i< spisok_puti.Count;i++)
        foreach (GameObject p in spisok_puti) Destroy(p);
        spisok_puti.Clear();
        kursor.gameObject.SetActive(true);
        //��������� ��������� � �������� �����
        //���� � ������ ���� �����
        if (data.get_activ_igrok().obj_army_list.Count > 0)
        {
            data.set_st_f_point_activ_army(kursor.transform.position);
            //data.set_st_f_point(data.get_activ_unit().transform.position, kursor.transform.position);
            create_put(data.st_p, data.fin_p);//������� ���� �� ������ ��������� � �������� �����
        }
    }

    public void create_put(Vector2Int st, Vector2Int f)
    {
        close_list = data.game_s.get_put_cell(data.get_activ_army().koordinat, kursor.transform.position);
        int count_hod = data.get_activ_army().tek_hod;//���������� ���������� ����� �����
        bool can_hod = true;
        finish_cell = data.kletki[f.x, f.y];//�������� ������
        foreach (item_cell cell in close_list)
        {
            GameObject p;
            //spisok_puti.Add((GameObject)Instantiate(point_put, new Vector3(cell.kordinat.x, cell.kordinat.y, -2.0f), Quaternion.identity));
            if (count_hod >= cell.get_cost_move())
            {
                count_hod = count_hod - cell.get_cost_move();//��������� �����������
                data.can_move_cell = cell;//������� ������ ���� ����� ���������
                data.can_move_cell_list.Add(cell);
                data.get_activ_army().tek_hod_tmp = count_hod;
            }
            else can_hod = false;//�� ���������� ����������� ����� �� �������
            if (can_hod) p=(GameObject)Instantiate(point_put, new Vector3(cell.kordinat.x, cell.kordinat.y, -2.0f), Quaternion.identity);
            else p = (GameObject)Instantiate(point_put_x, new Vector3(cell.kordinat.x, cell.kordinat.y, -2.0f), Quaternion.identity);
            spisok_puti.Add(p);
        }
        data.spisok_puti = spisok_puti;//�������� � ���� ������ �� ������ ����� ����� ����������� ����� ��� �������
        //���� ���� �������� ��� �� ��������� �� ������, �� ���� �� �������
        if (count_hod >= finish_cell.get_cost_move())
        {
            count_hod = count_hod - finish_cell.get_cost_move();//��������� �����������
            data.can_move_cell = finish_cell;
            data.can_move_cell_list.Add(finish_cell);
            data.get_activ_army().tek_hod_tmp = count_hod;
        }
    }
   
    public GameObject get_kursor()
    {//��������� ������ �� ������
        return kursor;
    }
    
    private static bool IsPointerOverUIObject()
    {//�������� �� �� ��� ���� ���� �� UI
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
    







