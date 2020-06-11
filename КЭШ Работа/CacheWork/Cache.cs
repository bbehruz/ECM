using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheWork
{
    class Cache
    {
        int[,] strings; // массив из строк кэша
        int[] tags; // тэги

        public Cache(int n, int m)
        {
            strings = new int[n, m];
            tags = new int[n];

            for (int i = 0; i < n; i++)
            {
                tags[i] = -1;
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    strings[i,j] = 0;
                }
            }
        }

        // Доступ к элементам массива кэша
        public int this [int i, int j]
        {
            get
            {
                return strings[i, j];
            }

            set
            {
                strings[i,j] = value;
            }
        }

        //Доступ к тэгам
        public int this [int i]
        {
            get
            {
                return tags[i];
            }

            set
            {
                tags[i] = value;
            }
        }

        // метод, проверяющий, есть ли строка j страницы i в кэше
        public bool isThereATag(int i, int j)
        {
            return tags[j] == i ? true : false;
        }

        // Записать новую строку размерностью n в кэш память
        public void WriteLine(int [] str, int n, int indexLine)
        {
            for (int i = 0; i < n; i++)
            {
                strings[indexLine, i] = str[i];
            }
        }
    }
}
