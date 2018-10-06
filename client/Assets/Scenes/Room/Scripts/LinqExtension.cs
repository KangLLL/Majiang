using System;
using System.Collections;
using System.Collections.Generic;
namespace System.Linq
{
    
    public static class Enumerable
    {
        public static int Min_<T>(this IEnumerable<T> source, Func<T, int> selector)
        {
            int min = 0;
            int order = 0;
            foreach (T t in source)
            {
                int temp = selector(t);
                if (order == 0)
                {
                    min = temp;
                    ++order;
                    continue;
                }
                if (temp < min) min = temp;

            }
            return min;
        }
        public static int Max_<T>(this IEnumerable<T> source, Func<T, int> selector)
        {
            int max = 0;
            int order = 0;
            foreach (T t in source)
            {
                int temp = selector(t);
                if (order == 0)
                {
                    max = temp;
                    ++order;
                    continue;
                }
                if (temp > max) max = temp;
            }
            return max;
        }
        public static T First_<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            T result = default(T);
            foreach (T t in source)
            {
                if (predicate(t))
                {
                    result = t;
                    break;
                }
            }
            return result;
        }
    }
}