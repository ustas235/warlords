using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unit : MonoBehaviour
{
    // Start is called before the first frame update
    
    public data_game data;//����� ��� ���� �������� ��� ������ ����
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
        
          
        data.set_activ_untit(this);//��� ����� ����� �� �������� � ������ ����
        this.transform.position = data.get_grid_step(this.transform.position);//�������� ������� �� �����
        data.Cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, data.Cam.transform.position.z);


        Debug.Log("���� ��������");
    }
}
