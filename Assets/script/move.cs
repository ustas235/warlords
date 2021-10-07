using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public GameObject kursor;
    Vector3 MousePos = new Vector3(-2.5f, -1.7f, 0f);
    public data_game data;//класс где буду хранится все данные игры
    // Start is called before the first frame update
    void Start()
    {
        GameObject obj_player = GameObject.Find("land");
        //к объекту привязан свой скрипт ищем его
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        kursor.gameObject.SetActive(false);
        data.get_activ_unit().transform.position = kursor.transform.position;//перемещаем юнит

        //Debug.Log(transform.InverseTransformVector(MousePos));
        data.Cam.transform.position = new Vector3(data.get_activ_unit().transform.position.x, data.get_activ_unit().transform.position.y, data.Cam.transform.position.z);
    }
}
