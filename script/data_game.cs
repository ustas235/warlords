using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// в данном классе мы храним все данные игры
public class data_game : MonoBehaviour
{
    // Start is called before the first frame update
    private unit activ_unit;//активный игрок
    public Camera Cam;//камера
    public Vector2Int st_p, fin_p;//индексы координат х и у в массиве координат grid_x и grid_y
    public float[] grid_x=new float[18];
    public float[] grid_y = new float[18];
    public item_cell[,] kletki = new item_cell[18, 18];//двумерный массив с объектами клеток
    void Start()
    {
        //заполнение сетки
        for (int i=0;i<18;i++)
        {
            grid_x[i] = -3.4f + i * 0.4f;
            grid_y[i] = 3.4f - i * 0.4f;
        }
        for (int i=0; i<18;i++)
            for (int j = 0; j < 18; j++)
            {
                kletki[i, j] = new item_cell(new Vector2Int(i, j));
                kletki[i, j].set_kordinat(new Vector2(grid_x[i], grid_y[i]));//заполняем индексы и координаты клеток
                kletki[i, j].set_cost_move(10);//стоимость движения по клеткам 10
            }
    }
    public void set_activ_untit(unit u)//установка активного игрока
    {
        activ_unit = u;
    }
    public unit get_activ_unit()//получение активного игрока
    {
        return activ_unit;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    //метод получения ближайшей точки
    public Vector3 get_grid_step(Vector3 point)
    {
        float x_p = point.x;
        float y_p = point.y;
        float z_p = point.z;
        float x_g=0, y_g=0, z_g = z_p;
        float min_x = 100, min_y = 100;
        for (int i=0;i<18;i++)//перебираем массивы сточкми
        {
            if (Math.Abs(grid_x[i]-x_p)<= min_x)//ищем минимальное расхождение по х
            { 
                min_x = Math.Abs(grid_x[i] - x_p);
                x_g = grid_x[i];
            }
            if (Math.Abs(grid_y[i] - y_p)<= min_y)//ищем минимальное расхождение по у
            {
                min_y = Math.Abs(grid_y[i] - y_p);
                y_g = grid_y[i];
            }
        }
        return new Vector3(x_g, y_g, z_p);
    }
    public void set_st_f_point(Vector3 point_s, Vector3 point_f)//функкця установки стартовой финишной точки для поиска пути, получает координаты старотовой точки и запоминает их индекс
    {
        for (int i = 0; i < 18; i++)//перебираем массивы сточкми
        {
            if (grid_x[i] == point_s.x) st_p.x = i;//ищем старт х
            if (grid_x[i] == point_f.x) fin_p.x = i;//ищем финиш х
            if (grid_y[i] == point_s.y) st_p.y = i;//ищем старт y
            if (grid_y[i] == point_f.y) fin_p.y = i;//ищем финиш y
        }

    }
}
