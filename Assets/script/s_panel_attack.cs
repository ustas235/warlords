using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_panel_attack : MonoBehaviour
{
    public data_game data;//����� ��� ���� �������� ��� ������ ����
    public Sprite spr_win;//������ ����������
    public GameObject winner;//������� ����������� ����������
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
    public void exit()
    {
        data.attack_window.SetActive(false);
    }
}
