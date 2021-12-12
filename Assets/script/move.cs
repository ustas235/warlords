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
        data.get_activ_army().move_army(data.can_move_cell.koordint3x);//перемещаем армию
        //data.get_activ_unit().move_unit(data.can_move_cell.koordint3x);//перемещаем юнит
        //data.get_activ_unit().transform.position = data.can_move_cell.koordint3x;//перемещаем юнит

        //Debug.Log(transform.InverseTransformVector(MousePos));
        data.move_cam(data.get_activ_army().koordinat);//перемещаем камеру
        data.setting_panel_unit(); //настраиваем панель с юнитами
        
        foreach (GameObject p in data.spisok_puti) Destroy(p);
        data.get_activ_army().tek_hod = data.get_activ_army().tek_hod_tmp;//обновим остаток ходов после перемещения
        if (data.type_event == 2) //начинаем атаку на другую армию
        {
            //если добрались до противника начнется бой
            if (data.get_activ_army().koordinat == kursor.transform.position) data.get_activ_army().attack_event_army(); //координаты армии и защитника совпали - начался бой
        }
        if (data.type_event == 3) //начинаем атаку на город
        {
            //если добрались до противника начнется бой
            if (data.get_activ_army().koordinat == kursor.transform.position) data.get_activ_army().attack_event_city();//начинаем атаку на другой город
        }
    }
}
