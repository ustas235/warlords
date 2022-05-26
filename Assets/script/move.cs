using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class move : MonoBehaviour
{
    public GameObject kursor;
    item_cell next_cell, prev_cell;
    Vector3 MousePos = new Vector3(-2.5f, -1.7f, 0f);
    bool flag_collision_enemy = false;//флаг что нарвались на другую армию
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
        if (!data.get_flag_army_is_move())
        {//если нет армий в движении
            if ((!EventSystem.current.IsPointerOverGameObject()) & (data.get_activ_army() != null))
            {
                start_move();
            }
        }
    }
    public void start_move()
    {
        GameObject obj_player = GameObject.Find("land");
        //к объекту привязан свой скрипт ищем его
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
        if (data.can_move_cell_list.Count > 0)
        {
            foreach (GameObject p in data.spisok_puti) Destroy(p);
            data.set_flag_army_is_move(true);//установим флаг движения армии
            StartCoroutine(start_move_coroutine());
        }
    }
    IEnumerator start_move_coroutine()
    {//подпрограмма задержки (пока без задержки)
        // yield на новую инструкцию YieldInstruction, которая ждет 5 секунд.
        kursor.transform.position = new Vector3(kursor.transform.position.x, kursor.transform.position.y, -5.0f);//задвинем курсор
        foreach (item_cell c in data.can_move_cell_list)
        {
            prev_cell = next_cell;
            next_cell = c;
            Vector3 old_koordinat = data.get_activ_army().koordinat;//запомним предыдущие координаты армии
            data.get_activ_army().move_army(next_cell.koordint3x);//перемещаем армию в очередную ячейку
            data.game_s.set_sprite_army_flag(data.get_activ_army().vladelec,next_cell.koordint3x);//настроим отображение армми и флага в новой клетке
            data.game_s.set_sprite_army_flag(data.get_activ_army().vladelec, old_koordinat);//настроим отображение армми и флага в старой клетке
            //пересчитаем ходы
            int delta_hod = c.get_cost_move();//посчитаем затраченные ходы
            data.get_activ_army().update_count_hod(delta_hod);//обновим количесвто ходов у юнитов в армии
            data.get_activ_army().set_army();//обновим настройки армии*/
            data.setting_panel_unit(); //настраиваем панель с юнитами
            //тут нужна задержка
            yield return new WaitForSeconds(0.2f);
            //если в той клетке кудаперемистилась армия есть враг - начать бой
            flag_collision_enemy = false;
            foreach (gamer g in data.game_s.get_gamer_list())
            {//перебираем всех игроков
                if (data.get_activ_igrok().id!=g.id)
                {//смотрим только чужих игроков
                    foreach (s_army a in g.s_army_list)
                    {//перебираем армии другуго игрока
                        if (data.get_activ_army().check_koordinat(a.koordinat))
                        {
                            flag_collision_enemy = true;//флаг что нарвались на другую армию
                            data.def_army = a;//запомним чужую армию
                            break;
                        }
                    }
                }
                if (flag_collision_enemy) break;
            }
            //прекратить движение
            if (flag_collision_enemy)
            {
                data.type_event = 2;//поменям тип события
                break;//остановим передвижение
            }
           
        }
        kursor.gameObject.SetActive(false);
        data.move_cam(data.get_activ_army().koordinat);//перемещаем камеру
        //data.setting_panel_unit(); //настраиваем панель с юнитами
        /*int delta_hod = data.get_activ_army().tek_hod - data.get_activ_army().tek_hod_tmp;//посчитаем затраченные ходы
        data.get_activ_army().update_count_hod(delta_hod);//обновим количесвто ходов у юнитов в армии
        data.get_activ_army().set_army();//обновим настройки армии*/
        //в конце движения очистим списки движения
        data.can_move_cell_list.Clear();
        
        //data.get_activ_army().tek_hod = data.get_activ_army().tek_hod_tmp;//обновим остаток ходов после перемещения
        switch (data.type_event)
        {//дейсвтия в зависимости от типа движения
            case 1://просто движение, после его оконяания
                if (data.get_activ_army().check_koordinat(kursor.transform.position))
                {  //если добрались до точки назначения
                    if (data.get_activ_army().get_status()==4) //если статус армии был поход в свой город то сделаем ее свободной
                        data.get_activ_army().set_status(0);//статус армии становиттся свободным
                    data.get_activ_army().flag_old_target = false;//сбросим флаг наличия старой цели
                }
                data.set_flag_army_is_move(false);//после окончание движения дадим об это знать
                break;
            case 2:// атака на другую армию
                if ((data.get_activ_army().check_koordinat(kursor.transform.position))||(flag_collision_enemy))
                {  //если добрались до противника начнется бой
                    data.get_activ_army().flag_old_target = false;//сбросим флаг наличия старой цели
                    data.get_activ_army().attack_event_army(); //координаты армии и защитника совпали - начался бой
                    flag_collision_enemy = false;
                    
                }

                else data.set_flag_army_is_move(false);//если не дошли но ходы закончились после окончание движения дадим об это знать
                break;
            case 3:// атака на город
                if (data.get_activ_army().check_koordinat(kursor.transform.position))
                {//если добрались до противника начнется бой
                    data.get_activ_army().set_status(3);
                    data.get_activ_army().flag_old_target = false;//сбросим флаг наличия старой цели
                    data.get_activ_army().attack_event_city();//начинаем атаку на другой город
                }
                else data.set_flag_army_is_move(false);//если не дошли но ходы закончились после окончание движения дадим об это знать
                break;
            default://хз что
                data.set_flag_army_is_move(false);//после окончание движения дадим об это знать
                break;
        }
        data.type_event = 0;
    }
   
}
