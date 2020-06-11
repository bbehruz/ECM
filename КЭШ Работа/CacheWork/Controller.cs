using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheWork
{
    class Controller
    {
        public static MainMemory memory;
        public static Cache cache;
        public static int[,,] arr;
        bool isCache = false; // данные загружены из Кэш памяти

        public Controller(int countPages, int countLines, int countElements, string filename)
        {
            memory = new MainMemory(filename + ".txt", countPages, countLines, countElements);
            arr = new int[countPages, countLines, countElements];
            memory.RandomArray(arr, countPages, countLines, countElements);
            memory.WriteArray(arr, countPages, countLines, countElements);
            cache = new Cache(countLines, countElements);
        }

        public int this[int i, int j, int k]
        {
            get
            {
                return arr[i, j, k];
            }
        }

        public int this [int i, int j]
        {
            get
            {
                return cache[i, j];
            }
        }

        public int this[int i]
        {
            get
            {
                return cache[i];
            }
        }

        public bool IsCache
        {
            get
            {
                return isCache;
            }
        }

        /// <summary>
        /// Поиск строки в кэше, либо в ОП
        /// </summary>
        public int[] SearchLine(int indexPage, int indexLine)
        {
            int[] buf = new int[memory.CountElements];

            // если строка с индексом indexLine находится в кэше с тэгом, равному indexPage
            // считываем строку из кэша
            if (cache.isThereATag(indexPage, indexLine))
            {
                for (int i = 0; i < memory.CountElements; i++)
                {
                    buf[i] = cache[indexLine, i];
                }
                isCache = true;
                return buf;
            }
            isCache = false;

            // иначе считываем строку из ОП
            buf = memory.ReadLine(indexPage, indexLine);

            // если данный тэг уже занят другой строкой, то эту строку нужно
            // скопировать и записать в файл (и в массив)
            if (cache[indexLine] != -1)
            {
                int[] old_str = new int[memory.CountElements];
                for (int i = 0; i < memory.CountElements; i++)
                {
                    old_str[i] = cache[indexLine, i];
                }
                // вернем строку в файл в нужную страницу
                memory.WriteLine(cache[indexLine], indexLine, old_str);
                // вернем строку в массив в нужную страницу
                memory.SetLineOnArray(ref arr, old_str, cache[indexLine], indexLine, memory.CountElements);
            }
            cache[indexLine] = indexPage; // присваиваем новому тэгу значение
            SetLineOnCache(buf, memory.CountElements, indexLine); // добавляем строку в кэш
            return buf;
        }

        /// <summary>
        /// Записать строку в кэш
        /// </summary>
        public void SetLineOnCache(int [] buf, int countElements, int indexLine)
        {
            cache.WriteLine(buf, memory.CountElements, indexLine);
        }
    }
}
