using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class s_panel_attack : MonoBehaviour
{
    public data_game data;//класс где буду хранится все данные игры
    public Sprite[] spr_atack = new Sprite[8];
    public Sprite[] spr_deff = new Sprite[8];
    List<bool> flags_atack ;//флаги состояния атакующих
    List<bool> flags_def ;//флаги состояния атакующих
    public List<GameObject> obj_img_list_atack = new List<GameObject>();// список объектов изображений атакующей армии
    public List<GameObject> obj_img_list_def = new List<GameObject>();// список объектов изображений защищающей армии

    // Start is called before the first frame update
    private void Awake()
    {
        //найдем ссылки на все элементы окна
        spr_atack = new Sprite[8];
        spr_deff = new Sprite[8];
        for (int i = 0; i < 8; i++)
        {
            string tmp_atack = "img_atack_" + i.ToString();
            string tmp_def = "img_def_" + i.ToString();
            
            obj_img_list_atack.Add(this.transform.Find(tmp_atack).gameObject);//добавим в массив 8 изображений атаки
            obj_img_list_def.Add(this.transform.Find(tmp_def).gameObject);//добавим в массив 8 изображений атаки
            
        }

    }

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
    public void exit()
    {
        data.attack_window.SetActive(false);
    }
    public void set_panel_atack(List<unit> unit_list_atack,List<bool> f_a, List<unit> unit_list_def, List<bool> f_d)
    {//метод натройки панели атаки, получает два списка юнитов: атаки и защиты
        flags_atack = f_a;
        flags_def = f_d;
        Sprite tmp_spr;
        for (int i=0;i<8;i++)
        {
            //порядок отображения армий - от центра
            int[] por = new int[8] {3,4,2,5,1,6,0,7};
            int j;
            for (i=0;i<8;i++)
            {
                j = por[i];//получим индекс при нумерации от центра
                //настроим изображение атакующих
                if (i< unit_list_atack.Count)// если в переданом списке юнитов больше чем номер текущ
                {
                    if(flags_atack[i]) tmp_spr= unit_list_atack[i].spr_unit;
                    else tmp_spr = unit_list_atack[i].spr_unit_off;//если юнит погиб, то спрайт будет выключен
                    obj_img_list_atack[j].GetComponent<Image>().sprite = tmp_spr;//выставим изображение юнита
                    obj_img_list_atack[j].SetActive(true);//покажем изображение июнита
                }
                else obj_img_list_atack[j].SetActive(false);//не покажем изображение июнита
                
                //настроим изображение защищающих
                if (i < unit_list_def.Count)// если в переданом списке юнитов больше чем номер текущ
                {
                    if(flags_def[i]) tmp_spr = unit_list_def[i].spr_unit;
                    else tmp_spr = unit_list_def[i].spr_unit_off;//если юнит погиб, то спрайт будет выключен
                    obj_img_list_def[j].GetComponent<Image>().sprite = tmp_spr;//выставим изображение юнита
                    obj_img_list_def[j].SetActive(true);//покажем изображение июнита
                }
                else obj_img_list_def[j].SetActive(false);//не покажем изображение июнита

            }
        }
    }
}
