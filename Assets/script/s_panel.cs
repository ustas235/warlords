using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class s_panel : MonoBehaviour
{
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
    public void but1()
    {

    }
    public void but2()
    {

    }
    public void but3()
    {

    }
    public void exit()
    {
        data.city_window.SetActive(false);
    }

}
