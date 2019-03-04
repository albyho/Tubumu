﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Tubumu.Modules.Framework.Extensions
{
    /// <summary>
    /// EnumerableExtensions
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 对枚举器的每个元素执行指定的操作
        /// </summary>
        /// <typeparam name="T">枚举器类型参数</typeparam>
        /// <param name="source">枚举器</param>
        /// <param name="action">要对枚举器的每个元素执行的委托</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null || action == null)
            {
                return;
            }
            foreach (var item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// 指示指定的枚举器是否为 null 或没有任何元素
        /// </summary>
        /// <typeparam name="T">枚举器类型参数</typeparam>
        /// <param name="source">要测试的枚举器</param>
        /// <returns>true:枚举器是null或者没有任何元素 false:枚举器不为null并且包含至少一个元素</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        /// <summary>
        /// 判断指定的集合是否为 null 或没有任何元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source == null || source.Count == 0;
        }

        /// <summary>
        /// 判断指定的数组是否为 null 或没有任何元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this T[] source)
        {
            return source == null || source.Length == 0;
        }

        /// <summary>
        /// 对String型序列的每个元素进行字符串替换操作
        /// </summary>
        /// <param name="source">源序列</param>
        /// <param name="oldValue">查找字符串</param>
        /// <param name="newValue">替换字符串</param>
        /// <returns>新的String型序列</returns>
        public static IEnumerable<string> Replace(IEnumerable<string> source, string oldValue, string newValue)
        {
            return source.Select(format => format.Replace(oldValue, newValue));
        }

        /// <summary>
        /// 将序列转化为ReadOnlyCollection
        /// </summary>
        /// <typeparam name="T">类型参数</typeparam>
        /// <param name="source">源序列</param>
        /// <returns></returns>
        public static IList<T> ToReadOnlyCollection<T>(this IEnumerable<T> source)
        {
            return new ReadOnlyCollection<T>(source.ToList());
        }
    }
}
