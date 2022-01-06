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

        if (data.can_move_cell_list.Count > 0)
        {
            StartCoroutine(start_move(0.2f));//запуск движения с задержкой
            foreach (GameObject p in data.spisok_puti) Destroy(p);
        }
       
    }
    IEnumerator start_move(float time)
    {//подпрограмма задержки
        // yield на новую инструкцию YieldInstruction, которая ждет 5 секунд.
        kursor.transform.position = new Vector3(kursor.transform.position.x, kursor.transform.position.y, -5.0f);//задвинем курсор
        foreach (item_cell c in data.can_move_cell_list)
        {
            yield return new WaitForSeconds(time);
            data.get_activ_army().move_army(c.koordint3x);//перемещаем армию в очередную ячейку
            
        }
        kursor.gameObject.SetActive(false);
        data.move_cam(data.get_activ_army().koordinat);//перемещаем камеру
        data.setting_panel_unit(); //настраиваем панель с юнитами
        int delta_hod = data.get_activ_army().tek_hod - data.get_activ_army().tek_hod_tmp;//посчитаем затраченные ходы
        data.get_activ_army().update_count_hod(delta_hod);//обновим количесвто ходов у юнитов в армии
        data.get_activ_army().set_army();//обновим настройки ирмии
        //в конце движения очистим списки движения
        data.can_move_cell_list.Clear();
        //data.get_activ_army().tek_hod = data.get_activ_army().tek_hod_tmp;//обновим остаток ходов после перемещения
        if (data.type_event == 2) //начинаем атаку на другую армию
        {
            //если добрались до противника начнется бой
            if (data.get_activ_army().check_koordinat(kursor.transform.position)) data.get_activ_army().attack_event_army(); //координаты армии и защитника совпали - начался бой
        }
        if (data.type_event == 3) //начинаем атаку на город
        {
            //если добрались до противника начнется бой
            if (data.get_activ_army().check_koordinat(kursor.transform.position)) data.get_activ_army().attack_event_city();//начинаем атаку на другой город
        }
    }
}
