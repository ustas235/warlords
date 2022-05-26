using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class s_main_panel : MonoBehaviour
{
    data_game data;
    GameObject txt_count_gold;//����� � ����������� ������
    GameObject txt_delta_gold;//����� � ����������� ����� ������ ������
    // Start is called before the first frame update
    private void Awake()
    {
        //txt_count_gold = new GameObject();//����� � ����������� ������
        //txt_delta_gold = new GameObject();//����� � ����������� ����� ������ ������
        GameObject obj_player = GameObject.Find("land");
        //� ������� �������� ���� ������ ���� ���

        data = obj_player.GetComponent(typeof(data_game)) as data_game;
        txt_count_gold = this.transform.Find("txt_count_gold").gameObject;
        txt_delta_gold = this.transform.Find("txt_delta_gold").gameObject;
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void set_main_panel(float count, float delta)
    {//��������� ������� ������
        txt_count_gold.GetComponent<Text>().text = ((int)count).ToString();
        txt_delta_gold.GetComponent<Text>().text = ((int)delta).ToString();
    }
    public void OnMouseDown()
    {
        Debug.Log("���� �� ������");
    }
}
