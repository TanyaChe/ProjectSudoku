using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUDOKU
{
    class StartingGenerator
    {
        /*public */
        Random rnd;
        int rndMin, rndMax;
        int[,] fieldGen;//поле сгененирированных значений, просто абстракция массива startVals
        public StartingGenerator(int diff)
        {
            fieldGen = new int[9, 9];
            rnd = new Random();
            switch (diff)
            {
                case (1): //Легкая сложность - 3-5 начальных цифры в районе (numbs - параметры для функции Random.Next)
                    rndMin = 3;
                    rndMax = 6;
                    break;
                case (2)://Средняя сложность - 2-5 начальных цифры в районе
                    rndMin = 2;
                    rndMax = 6;
                    break;
                case (3)://Сложная сложность - 2-4 начальных цифры в районе
                    rndMin = 2;
                    rndMax = 5;
                    break;
                default: break;
            }
        }
        private void NewField()
        {
        re:
            int[,] last = new int[9, 2];
            bool[,] check = new bool[9, 9];
            StartingValue sv;

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    sv = new StartingValue(rnd.Next(1, 10), j, i);
                    int d = 0;
                    while (!FullCheck(sv))
                    {
                        sv.val = rnd.Next(1, 10);
                        if (d++ > 20)//рестарт
                        {
                            for (int i2 = 0; i2 <= i; i2++)
                            {
                                for (int j2 = 0; j2 < 9; j2++)
                                {
                                    fieldGen[j2, i2] = 0;
                                }
                            }
                            goto re;
                        }
                    }
                    fieldGen[sv.x, sv.y] = sv.val;
                    check[j, sv.val - 1] = true;
                }
            }

            for (int i = 0; i < 9; i++)
            {
                int l = 0;
                for (int j = 0; j < 9; j++)
                {
                    if (!check[i, j])
                        last[i, l++] = j + 1;
                }

            }

            for (int i = 0; i < 511; i++)//2^9
            {
                int[] a = ToBinary(i);
                bool b = true;
                for (int j = 0; j < 9; j++)
                {
                    sv = new StartingValue(last[j, a[j]], j, 7);
                    if (!FullCheck(sv))//если не удв условию
                    {
                        for (int k = 0; k < j; k++)
                            fieldGen[k, 7] = 0;//стираем
                        b = false;
                        break;//проверяем следующий
                    }
                    fieldGen[j, 7] = last[j, a[j]];
                }
                if (b)
                {
                    a = Invert(ref a);//инвертируем 
                    for (int j = 0; j < 9; j++)
                    {
                        fieldGen[j, 8] = last[j, a[j]];
                    }
                }
            }

        }

        private int[] ToBinary(int v)
        {
            int[] a = new int[9];
            for (int i = 0; i < 9; i++)
            {
                a[i] = v % 2;
                v /= 2;
            }
            return a;
        }
        private int[] Invert(ref int[] a)
        {
            for (int i = 0; i < a.Length; i++)
                if (a[i] == 0)
                    a[i] = 1;
                else
                    a[i] = 0;
            return a;
        }
        public Queue<StartingValue> Generate() //генерирует стартовые значения и сразу возвращает их (в теории, можно начать заново без изменения сложности)
        {
            StartingValue[] svs;
            Queue<StartingValue> qsv = new Queue<StartingValue>();
            int rx = 0, ry = 0;

            NewField();

            for (int i = 0; i < 9; i++)
            {
                int numbs = rnd.Next(rndMin, rndMax);
                svs = new StartingValue[numbs];
                for (int j = 0; j < numbs; j++)//весь этот интересный цикл нужен, чтобы после генерации координат rx И ry проверить на повтор
                {
                    bool check = false;
                    while (!check)//хитрая проверка на повтор
                    {
                        bool check2 = true;
                        rx = rnd.Next(i * 3 % 9, i * 3 % 9 + 3);
                        ry = rnd.Next(i / 3 * 3, i / 3 * 3 + 3);

                        for (int k = 0; k < j; k++)
                            if (rx == svs[k].x && ry == svs[k].y)
                            {
                                check2 = false;
                            }
                        if (check2)
                            check = true;
                    }
                    svs[j] = new StartingValue(fieldGen[rx, ry], rx, ry);
                }

                for (int j = 0; j < numbs; j++)//вносим все, что сгенерировали в очередь
                    qsv.Enqueue(svs[j]);
            }


            return qsv;
        }

        /*public bool NoDeadLockCheck(int x, int y)//проверка по строке, столбцу и квадрату, что в ячейку по коорд. x,y можно 
        {
            bool[] vals = new bool[10];
            vals[0] = true;
            for (int i = 0; i < x; i++)
            {
                if (fieldGen[i, y] != 0)
                    vals[fieldGen[i, y]] = true;
            }
            for (int i = x; i < 9; i++)
            {
                if (fieldGen[i, y] != 0)
                    vals[fieldGen[i, y]] = true;
            }
            for (int i = 0; i < y; i++)
            {
                if (fieldGen[x, i] != 0)
                    vals[fieldGen[i, y]] = true;
            }
            for (int i = y; i < 9; i++)
            {
                if (fieldGen[x, i] != 0)
                    vals[fieldGen[i, y]] = true;
            }

            return true;
        }*/
        public bool AreaCheck(StartingValue sv)//проверка одного района
        {
            for (int i = sv.y / 3 * 3; i < sv.y / 3 * 3 + 3; i++)
            {
                for (int j = sv.x / 3 * 3; j < sv.x / 3 * 3 + 3; j++)
                {
                    if (fieldGen[j, i] == sv.val)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        /*public bool GridCheck(int d)//проверка одного Grid, указывать порядковый номер в field
        {
            bool[] check = new bool[9];
            object v;

            for (int i = 0; i < 9; i++)
            {
                v = field[d].Rows[i / 3].Cells[i % 3].Value;
                if (v != null)
                {// value ВСЕГДА должен быть в промежутке 1..9
                    if (!check[(int)v - 1])//массив check 0..8, а значение value 1..9
                        check[(int)v - 1] = true;
                    else
                        return false;
                }
            }
            return true;
        }*/
        public bool RowCheck(StartingValue sv)//проверка по строке
        {
            for (int j = 0; j < 9; j++)
            {
                if (fieldGen[j, sv.y] == sv.val)
                {
                    return false;
                }
            }
            return true;
        }
        public bool ColumnCheck(StartingValue sv)//проверка по столбцу
        {
            for (int i = 0; i < 9; i++)
            {
                if (fieldGen[sv.x, i] == sv.val)
                {
                    return false;
                }
            }
            return true;
        }

        public bool FullCheck(StartingValue sv)
        {
            if (AreaCheck(sv) == true)
                if (RowCheck(sv) == true)
                    if (ColumnCheck(sv) == true)
                        return true;
                    else
                        return false;
                else
                    return false;
            else
                return false;

        }

    }
}

