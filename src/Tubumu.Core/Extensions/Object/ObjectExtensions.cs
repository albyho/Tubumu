using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Tubumu.Core.FastReflection;

namespace Tubumu.Core.Extensions.Object
{
    /// <summary>
    /// ObjectExtensions
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// ToJson
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToJson(this object source)
        {
            return JsonConvert.SerializeObject(source);
        }

        /// <summary>
        /// FromJson
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T FromJson<T>(string json) where T : class
        {
            if (string.IsNullOrWhiteSpace(json))
                return default(T);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 深度克隆
        /// </summary>
        /// <param name="source">源对象</param>
        /// <returns>新对象</returns>
        public static object DeepClone(this object source)
        {
            if (source == null)
                return null;

            using (var memStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter(null,
                     new StreamingContext(StreamingContextStates.Clone));
                binaryFormatter.Serialize(memStream, source);
                memStream.Position = 0;
                return binaryFormatter.Deserialize(memStream);
            }
        }

        /// <summary>
        /// 深度克隆
        /// </summary>
        /// <param name="source">源对象</param>
        /// <returns>新对象</returns>
        public static T DeepClone<T>(this object source) where T : class
        {
            if (source == null || source.GetType() != typeof(T))
                return null;

            using (var memStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter(null,
                     new StreamingContext(StreamingContextStates.Clone));
                binaryFormatter.Serialize(memStream, source);
                memStream.Position = 0;
                return (T)binaryFormatter.Deserialize(memStream);
            }
        }

        /// <summary>
        /// 创建一个新的类型的对象，并将现有对象的属性值赋给新对象相同名称的属性
        /// </summary>
        /// <typeparam name="T">新对象的类型</typeparam>
        /// <param name="source">现有对象</param>
        /// <returns>新的对象</returns>
        public static T ToModel<T>(this object source) where T : new()
        {
            if (source == null) return default(T);

            T target = new T();

            return UpdateFrom(target, source);
        }

        /// <summary>
        /// 将目标对象的属性值赋给源对象相同名称的属性
        /// </summary>
        /// <typeparam name="T">泛型类型参数</typeparam>
        /// <param name="source">源对象</param>
        /// <param name="target">目标对象</param>
        /// <returns>源对象</returns>
        public static T UpdateFrom<T>(this T source, object target)
        {
            if (source == null) return default(T);
            if (target == null) return source;

            Type type = typeof(T);

            foreach (PropertyDescriptor targetPropertyDescriptor in TypeDescriptor.GetProperties(target))
            {
                PropertyInfo sourcePropertyInfo = type.GetProperty(targetPropertyDescriptor.Name, BindingFlags.Instance | BindingFlags.Public);
                if (sourcePropertyInfo != null && sourcePropertyInfo.CanWrite)
                {
                    var targetPropertyAccessor = new PropertyAccessor(sourcePropertyInfo);
                    var value = targetPropertyDescriptor.GetValue(target);
                    if (value != null)
                    {
                        if (sourcePropertyInfo.PropertyType.IsEnum)
                            targetPropertyAccessor.SetValue(source, Enum.ToObject(sourcePropertyInfo.PropertyType, value));
                        else
                            targetPropertyAccessor.SetValue(source, value);
                    }
                    else
                    {
                        targetPropertyAccessor.SetValue(source, null);
                    }
                }
            }
            return source;
        }

        /// <summary>
        /// FromXml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializedObject"></param>
        /// <returns></returns>
        public static T FromXml<T>(string serializedObject) where T : class
        {
            return FromXml(typeof(T), serializedObject) as T;
        }

        /// <summary>
        /// FromXml
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serializedObject"></param>
        /// <returns></returns>
        public static object FromXml(this Type type, string serializedObject)
        {
            object filledObject = null;
            if (!string.IsNullOrEmpty(serializedObject))
            {
                try
                {
                    var serializer = new XmlSerializer(type);
                    using (var reader = new StringReader(serializedObject))
                    {
                        filledObject = serializer.Deserialize(reader);
                    }
                }
                catch
                {
                    filledObject = null;
                }
            }
            return filledObject;
        }

        /// <summary>
        /// ToXml
        /// </summary>
        /// <param name="source"></param>
        /// <param name="noneXsn"></param>
        /// <returns></returns>
        public static string ToXml(this object source, bool noneXsn = false)
        {
            string serializedObject = String.Empty;

            if (source != null)
            {

                var serializer = new XmlSerializer(source.GetType());

                if (noneXsn)
                {
                    var sb = new StringBuilder();

                    //去除xml version...
                    var settings = new XmlWriterSettings
                    {
                        Indent = true,
                        Encoding = Encoding.UTF8,
                        OmitXmlDeclaration = true, //Remove the <?xml version="1.0" encoding="utf-8"?>
                    };
                    var xmlWriter = XmlWriter.Create(sb, settings);

                    //去除默认命名空间
                    var xsn = new XmlSerializerNamespaces();
                    xsn.Add(String.Empty, String.Empty);

                    serializer.Serialize(xmlWriter, source, xsn);
                    return sb.ToString();
                }
                else
                {
                    using (var writer = new StringWriter())
                    {
                        serializer.Serialize(writer, source);
                        return writer.ToString();
                    }
                }
            }
            return serializedObject;
        }

        /// <summary>
        /// ToByteArray
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// FromByteArray
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static T FromByteArray<T>(this byte[] byteArray) where T : class
        {
            if (byteArray == null)
            {
                return default(T);
            }
            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream(byteArray))
            {
                return binaryFormatter.Deserialize(memoryStream) as T;
            }
        }
    }
}
