using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class city : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject kursor;
    
    public Sprite spr_kursor_attack;
    public Sprite spr_kursor_in_city;
    public Sprite spr_city;
    public data_game data;//����� ��� ���� �������� ��� ������ ����
    mouse obj_mouse;//������ � ��������� ����
    public gamer vladelec;//�������� ������
    public int id = 0;//����� ������� ������, ����� ������������� �������� ��� ������
    public Vector3 koordinat;
    Sprite[] spr_city_grey, spr_city_bel, spr_city_dark, spr_city_zel, spr_city_orange;//������ �������� �������
    void Start()
    {
        GameObject obj_player = GameObject.Find("land");
        //� ������� �������� ���� ������ ���� ���
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
        obj_mouse=obj_player.GetComponent(typeof(mouse)) as mouse;
        spr_city_bel = Resources.LoadAll<Sprite>("sprite/city/bel_city");//������� ���������
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
        if (data.tek_activ_igrok.id == vladelec.id) data.city_window.SetActive(true);//���� �������� ����� �������� ������ �������� ������
    }
    public void change_vladelec(gamer vlad)
    {
        vladelec = vlad;
        spr_city = vlad.spr_city;//����� ����� ������ ���������
        this.GetComponent<SpriteRenderer>().sprite = spr_city;
    }
    //���������� ������ ������

}
