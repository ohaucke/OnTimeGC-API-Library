using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace OnTimeGC_API
{
    /// <summary>
    /// Collection of usefull extension
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Serialize an object to a Json string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T s)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, s);
                ms.Position = 0;
                using (StreamReader sr = new StreamReader(ms))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Deserialize a string to an string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string s)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(s)))
            {
                DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(T));
                return (T)deserializer.ReadObject(stream);
            }
        }

        internal static string ToUrl(this OnTimeGC_API.Client.EndPoint e)
        {
            return e.ToString().ToLower().Insert(0, "/");
        }

        /// <summary>
        /// Convert a OnTime DateTime to an DateTime object
        /// </summary>
        /// <param name="s">A integer indicating number of seconds since 1/1-2000</param>
        /// <returns>a DateTime object</returns>
        public static DateTime ToDateTime(this int s)
        {
            return new System.DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(s);
        }

        /// <summary>
        /// Convert a DateTime object to the OnTime DateTime format
        /// </summary>
        /// <param name="s">DateTime object which will be converted</param>
        /// <returns>A integer indicating number of seconds since 1/1-2000, in UTC time - Can be negative for date/times before 1/1-2000</returns>
        public static int ToOTDateTime(this DateTime s)
        {
            return (Int32)(s == null ? 0 : (Int64)(new TimeSpan(((DateTime)s).Ticks - new DateTime(2000, 1, 1).Ticks).TotalSeconds));
        }
    }

    /// <summary>
    /// A part of data structure
    /// </summary>
    public enum AppointmentType
    {
        /// <summary>
        /// Appointment
        /// </summary>
        Appointment = 0,
        /// <summary>
        /// AllDayEvent
        /// </summary>
        AllDayEvent = 2,
        /// <summary>
        /// Meeting
        /// </summary>
        Meeting = 3
    }

    /// <summary>
    /// A part of data structure
    /// </summary>
    public enum UsersAllTyp
    {
        Person = 1,
        Room = 2
    }

    /// <summary>
    /// A part of data structure
    /// </summary>
    public enum UsersInfoTyp
    {
        Person = 1,
        Room = 2
    }
}
