using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamer //
{
    public List<city> city_list = new List<city>();//?????? ???????
    public List<GameObject> obj_army_list = new List<GameObject>();//?????? ???????? ??????
    //public List<s_army> skript_army_list = new List<s_army>();//?????? ????????? ??????
    public List<s_army> s_army_list = new List<s_army>();//?????? ????????? ?????
    public List<s_army> s_army_to_target_list = new List<s_army>();//?????? ????? ??????? ?????? ? ?????
    bool start_move_to_target = false;//???? ?? ???????? ? update ????? ???? ?????????? ??? ?????? ???????? ???? ?????
    int num_tek_army_to_taget=0;//????? ??????? ????? (??? ???????? ???? update
    public data_game data;//????? ??? ???? ???????? ??? ?????? ????
    //????????? ??????
    float gold = 0;//?????????? ?????
    float delta_gold = 0;//????????? ???????????????
    float old_gold;
    public int id = 0;//id ??????, ?? ?? ?????
    public bool still_play = true;//???? ??? ????? ??? ??????
    public bool active = false;//???? ??? ??? ??????
    public Sprite spr_city;//?????? ?? ????? ?????? ??????
    //????????? ????
    public bool bot_flag = false;//???? ??? ?????????
    public int bot_army_create_city = 0;//??? ????? ???????? ?????
    public int bot_min_garnison = 0;//???????? ? ??????
    public List<item_cell> bot_put_cell_list;//?????? ????? ????
    List<item_cell> tmp_bot_put_cell_list;//????????? ?????? ?????? ????? ???? (?????? ?????????? ??? ?????????? ??????)
    Vector3 tmp_target_koordinat;//????????? ????? ??????
    // Start is called before the first frame update
    void Start()
    {
        GameObject obj_player = GameObject.Find("land");
        //? ??????? ???????? ???? ?????? ???? ???
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
        old_gold = data.get_start_gold();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void start_setup(int num, bool flag_bot)
    {//????????? ?????????
        id = num;
        bot_flag = flag_bot;
        
        GameObject obj_player = GameObject.Find("land");
        //? ??????? ???????? ???? ?????? ???? ???
        data = obj_player.GetComponent(typeof(data_game)) as data_game;
        gold = data.get_start_gold();
    }
    
    //------------------------------------
    //???????? ????
    public void set_bot(int level_bot)
    {//????????? ???? ? ????????? ?? ??????
        switch (level_bot)
        {
            case 0:

                break;
            case 1:

                break;
            case 2:

                break;
            default:
                
                break;
        }
    }
    public void action_bot()
    {//??????? ????
        foreach (city c in city_list)
        {//?????????? ?????? ?????? ?????? ? ????????? ?????????
            if (check_upgrate_city(c))//???????? ?? ?? ??? ????? ???????? ??-?? ? ??????
            {//???? ????? ???????? ????????????
                upgrate_build_city(c);//???????? ???????????? ? ??????
                create_new_unit(c);//??????? ? ?????? ? ???? ?????? ????
            }
            if (c.can_any_build())//???? ????? ????? ??????? ??????? ???? ???? ???-??
                create_new_unit(c);//??????? ? ?????? ? ???? ?????? ????
            
            //if (c.count_hod_start<0)
                //c.setting_activ_city(bot_army_create_city);//?????? ????? bot_army_create_city ?? ??????
            if (accumulation_garnison(c))//?????? ????????
                accumulation_army(c);//???? ???????? ?????, ?????? ????? ?????

        }
        //???? ???? ??? ?????? ????? ?????
        s_army_to_target_list.Clear();
        foreach (s_army a in s_army_list)
        {
            switch (a.get_status())
            {//?????????? ??? ?????
                case 2://???? ????? ????????? ? ?????
                    search_target(a);//???? ????????? ????????? ?????, ????? ???????? ????????? ?????
                    if (training_army_compleed(a,a.get_target_city()))//????????? ?????? ?? ??? ??? ?????
                    {
                        s_army_to_target_list.Add(a);//????????? ? ??????
                    }
                    break;
                case 3://???? ???? ? ?????
                    s_army_to_target_list.Add(a);//????????? ? ??????
                    break;
                case 4://???? ???? ? ???? ?????
                    s_army_to_target_list.Add(a);//????????? ? ??????
                    break;
                default:
                    break;
            }
            
        }
        if (s_army_to_target_list.Count>0)
        {
            data.set_flag_army_is_move(false);//????, ??? ??? ????? ?? ?????????? ???? ????????
            num_tek_army_to_taget = 0;//????????? ?? ?????? ?????
            start_move_to_target = true;//????? ???????? ????? ?? ??????
        }
    }

    public void create_new_unit(city c)
    {//????????????? ??????
     //????????? ?? ?????????? ? ??????? ? ??????
        int tmp_id=-1;
        for (int i=data.count_type_unit-1;i>=0;i--)
        {//???? ????????? ???? ??? ?????????????
            if (c.can_build_flag[i])
            {//???? ???????????? ????
                tmp_id = i;//???????? ????? ????? ??? ?????????????
                break;
            }
        }
        if (tmp_id > c.id_unit)//???? ????? ?????? ??????? 
        {
            c.setting_activ_city(tmp_id);//?????? ????????????
        }
    }

    public void upgrate_build_city(city c)
    {// ????????? ????????????
     //????????? ?? ?????????? ? ??????? ? ???????? ?? ??? ?????
     //???? ???? ?????? ???? ??????, ?? ?????? ???
        for (int i = data.count_type_unit - 1; i >= 0; i--)
        {
            if (!c.can_build_flag[i])
            {//???? ?????????????? ????
                if (gold >= data.get_cost_build()[i]) //????????? ?? ?? ??? ?????? ?????? ????????? ???????
                {
                    if (change_gold(0 - data.get_cost_build()[i]))//???????? ?????? ????????????
                    {//???? ????? ????????
                        c.can_build_flag[i] = true;//?????? ???? ???? ????? ???????
                        break;
                    }
                }
            }
        }
    }

    public bool check_upgrate_city(city c)
    {//???? ?????? ?????? ?????? ?????? ????????? ????
        bool flag = false;
        for (int i=0;i<data.count_type_unit;i++)
        {//?????????? ??? ???? ??????
            if (!c.can_build_flag[i])
            {//???? ?????????????? ????
                if (gold >= data.get_cost_build()[i])
                {
                    flag = true;//????????? ?? ?? ??? ?????? ?????? ????????? ???????
                    
                }
            }
            else flag = false;//???? ?????? ????? ???????, ?? ?????? ????
        }
        return flag;
    }

    //?????????? ?????????? ???????
    public bool accumulation_garnison(city c)
    {
        List<unit> unit_city_list = new List<unit>();//?????? ???? ?????? ??????
        bool flag_garnison_ready = false;
        s_army tmp_army_garnison = new s_army();//????????
        int count_ready_garnison_unit = 0;//????????? ?????? ??? ????????? ? ????????
        List<unit> garnison_unit_list;//?????? ?????? ? ??????
        garnison_unit_list = c.get_garnison_unit_list();
        foreach (unit u in garnison_unit_list)
        {
            if (u.get_status_unit()==0)  
                unit_city_list.Add(u);//?????????? ????????? ???? ? ??????
            if (u.get_status_unit() == 1)
            {
                count_ready_garnison_unit++;
                tmp_army_garnison = u.sc_army;
            }
        }

        //???????? ????????
        if (count_ready_garnison_unit >= bot_min_garnison)//???? ????? ??? ???? ???????, ?? ?? ????? ?????? ???????? ????????
        {
            flag_garnison_ready = true;
        }
        else
        {
            if (unit_city_list.Count >= bot_min_garnison)
            {
                //???? ?????? ???? ??????? ? ??????? ?????, ?? ???????? ??? ????
                tmp_army_garnison = unit_city_list[0].sc_army;
                unit_city_list[0].set_status_unit(1);//???????? ?????? ?????
                tmp_army_garnison.set_status(1);//???????? ?????? ?????
                    //????? ?????????? ? ????? ??????? ?????
                for (int i = 1; i < bot_min_garnison; i++)
                {
                    tmp_army_garnison.add_unit(unit_city_list[i]);//?????????? ? ????? ????????? ?????????? ?????
                    unit_city_list[i].set_status_unit(1);//????? ?????? ??? ??? ? ?????????
                }
                tmp_army_garnison.move_army(c.koordinat_garnizon);//?????????? ? ????? ?????????
                tmp_army_garnison.set_status(1);
                if (tmp_army_garnison.get_unit_list().Count >= bot_min_garnison) flag_garnison_ready = true;
            }
        }
        return flag_garnison_ready;
    }
   
    public void accumulation_army(city c)
    { //?????????? ??????? ????? ? ??????
        List<unit> unit_city_list = new List<unit>();//?????? ???? ?????? ??????
        s_army tmp_army_atack=new s_army();//????? ?????
        bool flag_unit_atack_is = false;//???? ??????? ? ?????? ????? ?????
        List<unit> garnison_unit_list;//?????? ?????? ? ??????
        garnison_unit_list = c.get_garnison_unit_list();
        foreach (unit u in garnison_unit_list)
        {
            if (u.get_status_unit() == 0) unit_city_list.Add(u);//?????????? ????????? ???? ? ??????
            if (u.get_status_unit() == 2)
            {
                tmp_army_atack = u.sc_army;
                flag_unit_atack_is = true;
            }
        }

        //???????? ????????? ?????
        if (unit_city_list.Count > 0)// ???? ???? ?????????? ?????
        {
            //???? ? ?????? ?? ???? ????? ?????, ?? ???????? ?? ?? ??????? ?????
            if (!flag_unit_atack_is)
            {
                tmp_army_atack = unit_city_list[0].sc_army;
                unit_city_list[0].set_status_unit(2);//???????? ?????? ?????
                tmp_army_atack.set_status(2); //???????? ?????? ?????
            }
            //????? ?????????? ? ????? ??????? ?????
            for (int i = 0; i < unit_city_list.Count; i++)
            {
                if (unit_city_list[i].contains_to_list(tmp_army_atack.get_unit_list())) continue;//???? ???? ??? ? ????? ?? ???????? ? ??????????
                if (tmp_army_atack.add_unit(unit_city_list[i]))//?????????? ? ????? ????? ?????????? ????? ???? ???? ?????
                    unit_city_list[i].set_status_unit(2);//???????? ?????? ?????
                else
                {//????? ? ????? ????? ???
                    break;//??????????? ?????
                }
            }
            tmp_army_atack.move_army(c.koordinat_atack);//?????????? ? ????? ????? ?????
            tmp_army_atack.set_status(2);
        }
    }

    //????? ????? ??? ?????? ????????? ?????
    public void search_target(s_army arm)
    {//???? ????????? ????????? ?????
        List<city> tmp_all_city_list = data.game_s.get_city_list();
        List<city> tmp_alien_city_list = new List<city>();
        
        foreach (city c in tmp_all_city_list)
        {//???? ????????? ??????
            if (c.vladelec.id != id) tmp_alien_city_list.Add(c);
        }
        int min_point = 10000000;//??????????? ?????????? ?????
        int next_min_pount;
        city cit_near;//????????? ?????
        foreach (city c in tmp_alien_city_list)
        {
            next_min_pount = calculate_put(arm,c);
            if (next_min_pount <= min_point)
            {
                min_point = next_min_pount;
                cit_near = c;//??????? ?????????
                arm.set_target_city(c);//????? ???????? ????????? ????? ??? ?????
                arm.set_target_koordinat(tmp_target_koordinat);//????? ???????? ??????????? ??????????
                bot_put_cell_list = tmp_bot_put_cell_list;//??? ??????? ????????? ????
                tmp_bot_put_cell_list.Clear();

            }
        }
    }
    //???????? ?????, ??? ?????? ??? ?????
    public bool training_army_compleed(s_army arm, city cit)
    {//???????? ?? ?? ??? ????? ?????? ??? ????? ?? ?????? ?????
        bool army_compleed = false;
        int other_strenght_total = 0;//???? ?????????? ??????????, ???????? ????? ??? ???? ??????
        int this_strenght_total = 0;//???? ?????????? ??????, ???????? ????? ??? ???? ??????
        List<unit> other_unit_list;//?????? ????? ??????? ?????????? ? ?????? ??????
        other_unit_list = cit.get_garnison_unit_list();//???????? ?????? ????? ?????????
        foreach (unit u in other_unit_list) other_strenght_total += u.strength;
        foreach (unit u in arm.get_unit_list()) this_strenght_total += u.strength;
        if (((this_strenght_total/2) > other_strenght_total)||(arm.get_unit_list().Count>7)) army_compleed = true;//??????? ?????? ?????? ?? ????????? ??????
        return army_compleed;
    }
    //????? ? ????
    public void atack_to_target(s_army arm)
    {
        arm.set_status(3);//????? ? ?????
        data.set_activ_army(arm);//???????? ? ?????? ???????? ?????
        data.move_cam(arm.koordinat);
        data.game_s.bot_atack_target(arm.get_target_koordinat());
    }
    public int check_target(s_army arm)
    {//???????? ???? ??? ?????
        int status = 0;
        if (arm.get_target_city().vladelec.id == id)//???? ????? ??? ???, ?? ???????? ???? ????? ??? ??????????
        {
            arm.set_status(4);
            status = 4;
        }
        else status = 3;
        return status;
    }
    public void move_to_target(s_army arm)
    {
        arm.set_status(4);//????? ? ????
        data.set_activ_army(arm);//???????? ? ?????? ???????? ?????
        data.move_cam(arm.koordinat);
        data.game_s.bot_move_target(arm.get_target_koordinat());
    }
    //??????? ????? ?? ??????
    public int calculate_put(s_army arm, city c)
    {//?????? ????????? ???? ?? ?????? 
        //?????? ????? ????? ?? 4 ???????, ?????? ?????????? ???? ?? ?????????
        List<Vector3> koor_city_list = new List<Vector3>();
        //????????? ?????? ???????? ? ???????? ?? ?????? ?? ??? 4 ??????
        koor_city_list.Add(new Vector3(c.koordinat.x - 0.2f, c.koordinat.y - 0.2f, c.koordinat.z));
        koor_city_list.Add(new Vector3(c.koordinat.x + 0.2f, c.koordinat.y - 0.2f, c.koordinat.z));
        koor_city_list.Add(new Vector3(c.koordinat.x + 0.2f, c.koordinat.y + 0.2f, c.koordinat.z));
        koor_city_list.Add(new Vector3(c.koordinat.x - 0.2f, c.koordinat.y + 0.2f, c.koordinat.z));
        //?????? ??????????? ???? ?? ??????
        List<item_cell> tmp_cell_list;
        int min_count = 1000000000;
        int summ_cost_mov;
        Vector3 min_v_city_put;//?????????? ??????? ??? ???????????? ????
        foreach (Vector3 v in koor_city_list)
        {//?????????? 4 ?????
            summ_cost_mov = 0;
            tmp_cell_list = data.game_s.get_put_cell(arm.koordinat, v);//??????? ?????? ????
            foreach (item_cell ic in tmp_cell_list) summ_cost_mov += ic.get_cost_move();
            if (summ_cost_mov <= min_count)
            {//?????????
                min_count = summ_cost_mov;
                min_v_city_put = v;
                //???????? ????? ??? ????? ? ????
                //?.?. ?????? ??????????? ??? ???? ???????, ? ????? ??? ?????????
                //????? ??????????? ?????????? ????????? ?????? ?? tmp ? ?????
                tmp_target_koordinat = min_v_city_put;//????? ???????? ??????????? ??????????
                tmp_bot_put_cell_list = tmp_cell_list;
            }
        }
        return min_count;
    }
    //??????? ???? ? ????????? ??????
    public void get_move_to_target()
    {
       
        if (start_move_to_target)//???? ?????????? ?? ???????? ?????
        {

            if (!data.get_flag_army_is_move())//???? ?????????? ????? ????????? ????????
            {
                
                if (num_tek_army_to_taget >= s_army_to_target_list.Count)//??????? ????? ??? ???????? ???????? ????????? ????? ??? ????????, ???????? ?????? ??????
                {
                    start_move_to_target = false;//???????? ???? ????? ????????? ?????? ????
                    data.set_flag_army_is_move(false);//??? ????? ? ????????
                    num_tek_army_to_taget = 0;
                }
                else
                {
                    data.set_flag_army_is_move(true);
                    if (check_target(s_army_to_target_list[num_tek_army_to_taget]) == 4)//???????? ????, ???? ????? ??? ???????? ?? ???????? ?????? ?????
                        move_to_target(s_army_to_target_list[num_tek_army_to_taget]);//? ???????? ?? ? ???????????? ?????
                    else
                        atack_to_target(s_army_to_target_list[num_tek_army_to_taget]);//????? ???????? ???? ????? ???????????
                    num_tek_army_to_taget++;//??????????? ????? ?? ????????? ?????
                }
            }
        }
    }
    public bool change_gold(float ch_g)
    {//????? ????????? ????? 
        float tmp = gold;
        bool flag = false;
        tmp = gold + ch_g;
        if (tmp>=0)
        {//???????? ?? ?? ??? ? ?????? ?????? ?????
            gold = tmp;
            flag = true;
            delta_gold = calculate_cost_army_gold();
            if (data.get_activ_igrok().id == id) data.main_panel.set_main_panel(gold, delta_gold);//???? ????? ???????? ?? ????????? ????????? ?? ???????
        }
        else flag = false;
        return flag;
    }
    public void set_delta_gold()
    {//????? ?????????? ??????? ?????
        delta_gold = calculate_cost_army_gold(); ;
        if (data.get_activ_igrok().id == id) data.main_panel.set_main_panel(gold, delta_gold);//???? ????? ???????? ?? ????????? ????????? ?? ???????
    }
    public void collect_cost()
    {//????? ?? ???? ?????
        float cost = get_cost_army();
        if (gold - cost < 0) 
            delete_extra_unit();//???? ????????? ?????????? ?????? ??? ???? ????? ?????? ?????? ?????
        cost = get_cost_army();//??? ??? ??????? ????????? ??????????
        gold = gold - cost;
        delta_gold = calculate_cost_army_gold();
        if (data.get_activ_igrok().id == id) data.main_panel.set_main_panel(gold, delta_gold);//???? ????? ???????? ?? ????????? ????????? ?? ???????
    }
    public float get_cost_army()
    {//????????? ????????? ?????????? ?????
        float cost = 0f;
        foreach (s_army a in s_army_list)
        {
            foreach (unit u in a.get_unit_list())
            {
                cost = cost + (u.price * data.koef_cost);//??????? ????????? ?????????? ???? ??????
            }
        }
        return cost;
    }
    public void delete_extra_unit()
    {
        float cost = get_cost_army();
        float extra_cost = gold - cost;
        List<unit> all_unit_list = new List<unit>();
        if (extra_cost<0)
        {
            foreach (s_army a in s_army_list)
                foreach (unit u in a.get_unit_list())
                { 
                    all_unit_list.Add(u);//???????? ???? ?????? ? ???? ???????
                }
            while (all_unit_list.Count > 0)
            {// ??????? ?????? ?????
                cost = get_cost_army();
                if (gold - cost < 0)
                {
                    unit tmp_u = all_unit_list[all_unit_list.Count - 1];
                    all_unit_list.RemoveAt(all_unit_list.Count - 1);
                    tmp_u.sc_army.sub_unit_destroy(tmp_u);//??????? ????????? ????
                }
                else break;
            }
        }
    }
    public float calculate_cost_army_gold()
    {//??????? ???????=??? ??????-??????? ?? ?????
        int dohod=0;
        float cost = get_cost_army();
        foreach (city c in city_list) dohod = dohod + c.get_profit();
        delta_gold = dohod - cost;
        return delta_gold;
    }
    
}
