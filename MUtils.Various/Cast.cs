using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Serialization;

namespace MUtils.Various
{
    public static class Cast
    {
        #region Cast

        #region ToIntOrNull
        public static int? ToIntOrNull(string inputString)
        {
            int result;
            if (Int32.TryParse(inputString, out result))
                return result;
            return null;
        }
        #endregion

        #region ToDateOrNull
        public static DateTime? ToDateOrNull(string inputString)
        {
            DateTime result;
            if (DateTime.TryParse(inputString, out result))
                return result;
            return null;
        }
        #endregion

        #region ToDateTimeOrDefault
        public static DateTime ToDateTimeOrDefault(string inputString)
        {
            DateTime result;
            if (DateTime.TryParse(inputString, out result))
                return result;
            return DateTime.UtcNow;
        }
        #endregion

        #region ToIntOrZero
        public static int ToIntOrZero(string inputString)
        {
            int result;
            if (!string.IsNullOrEmpty(inputString) && Int32.TryParse(inputString, out result))
                return result;
            return 0;
        }
        #endregion

        #region ToFloatOrZero
        public static float ToFloatOrZero(string inputString)
        {
            float result;
            if (!string.IsNullOrEmpty(inputString) && float.TryParse(inputString, out result))
                return result;
            return 0.0f;
        }
        #endregion

        #region ToDateTimeOrMinValue
        public static DateTime ToDateTimeOrMinValue(string inputString)
        {
            DateTime result;
            if (!string.IsNullOrEmpty(inputString) && DateTime.TryParse(inputString, out result))
                return result;
            return DateTime.MinValue;
        }
        #endregion

        #region ToInt64OrZero
        public static Int64 ToInt64OrZero(string inputString)
        {
            Int64 result;
            if (Int64.TryParse(inputString, out result))
                return result;
            return 0;
        }
        #endregion

        #region ToInt64OrNull
        public static Int64? ToInt64OrNull(string inputString)
        {
            Int64 result;
            if (Int64.TryParse(inputString, out result))
                return result;
            return null;
        }
        #endregion

        #region ToSortOrNull
        public static short? ToSortOrNull(string inputString)
        {
            short result;
            if (Int16.TryParse(inputString, out result))
                return result;
            return null;
        }
        #endregion

        #region ToDoubleOrNull
        public static double? ToDoubleOrNull(string inputString)
        {
            double result;
            if (Double.TryParse(inputString, out result))
                return result;
            return null;
        }
        #endregion

        #region ToGuidOrNull
        public static Guid? ToGuidOrNull(string guid)
        {
            Guid result;
            if (String.IsNullOrEmpty(guid) || guid == Guid.Empty.ToString())
                return null;
            if (Guid.TryParse(guid, out result))
                return result;
            return null;
        }
        #endregion

        #region ToGuidOrEmpty
        public static Guid ToGuidOrEmpty(string guid)
        {
            Guid result;
            if (String.IsNullOrEmpty(guid) || guid == Guid.Empty.ToString())
                return Guid.Empty;
            return Guid.TryParse(guid, out result) ? result : Guid.Empty;
        }
        #endregion

        #region ToIdOrNull
        public static int? ToIdOrNull(string id)
        {
            int result;
            if (String.IsNullOrEmpty(id) || id == "0")
                return null;
            if (Int32.TryParse(id, out result))
                return result;
            return null;
        }
        #endregion

        #region ToIdOrNull
        public static string ToIdOrNull(int? id)
        {
            return id == null || id == 0 ? null : id.ToString();
        }
        #endregion

        #region ToStringOrNull
        public static string ToStringOrNull(string inputString)
        {
            return String.IsNullOrEmpty(inputString) ? null : inputString;
        }
        #endregion

        #region ToStringOrNull
        public static string ToStringOrNull(Guid? guid)
        {
            return guid == null || guid == Guid.Empty ? null : guid.ToString();
        }
        #endregion

        #region ToStringOrNull
        public static string ToStringOrNull(int? id)
        {
            return id == null || id == 0 ? null : id.ToString();
        }
        #endregion

        #region ToShortOrNull
        public static short? ToShortOrNull(string inputString)
        {
            short result;
            if (short.TryParse(inputString, out result))
                return result;
            return null;
        }
        #endregion

        #region ToShortOrZero
        public static short ToShortOrZero(string inputString)
        {
            short result;
            if (short.TryParse(inputString, out result))
                return result;
            return 0;
        }
        #endregion

        #region ToBoolOrNull
        public static bool? ToBoolOrNull(string inputString)
        {
            bool result;
            if (bool.TryParse(inputString, out result))
                return result;
            return null;
        }
        #endregion

        #region ToBoolOrFalse
        public static bool ToBoolOrFalse(string inputString)
        {
            bool result;
            return bool.TryParse(inputString, out result) && result;
        }
        #endregion

        #region ToBoolOrFalse
        public static bool ToBoolOrFalse(bool? input)
        {
            return input ?? false;
        }
        #endregion

        #region Convert number to letters

        #region Fields

        // Convert Num To String
        private static readonly string[] Basex = new[] { "", "هزار", "میلیون", "میلیارد", "تریلیون" };
        private readonly static string[] Dahgan = new[] { "", "", "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود" };
        private readonly static string[] Dahyek = new[] { "ده", "یازده", "دوازده", "سیزده", "چهاگروه", "پانزده", "شانزده", "هفده", "هجده", "نوزده" };
        //array[10..19]
        private readonly static string[] Sadgan = new[] { "", "یکصد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد", "نهصد" };
        private readonly static string[] Yekan = new[] { "صفر", "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه" };

        #endregion

        #region Getnum3
        private static string Getnum3(int num3)
        {
            var s = "";
            var d12 = num3 % 100;
            var d3 = num3 / 100;
            if (d3 != 0)
                s = Sadgan[d3] + " و ";
            if ((d12 >= 10) && (d12 <= 19))
                return s + Dahyek[d12 - 10];
            var d2 = d12 / 10;
            if (d2 != 0)
                s = s + Dahgan[d2] + " و ";
            var d1 = d12 % 10;
            if (d1 != 0)
                s = s + Yekan[d1] + " و ";
            return s.Substring(0, s.Length - 3);
        }
        #endregion

        #region NumberToLetters
        public static string NumberToLetters(int number)
        {
            return NumberToLetters(number.ToString());
        }
        #endregion

        #region NumberToLetters
        public static string NumberToLetters(string number)
        {
            var stotal = "";
            if (number == "0")
                return Yekan[0];
            number = number.PadLeft(((number.Length - 1) / 3 + 1) * 3, '0');
            var l = number.Length / 3 - 1;
            for (var i = 0; i <= l; i++)
            {
                var b = Int32.Parse(number.Substring(i * 3, 3));
                if (b != 0)
                    stotal = stotal + Getnum3(b) + " " + Basex[l - i] + " و ";
            }
            stotal = stotal.Substring(0, stotal.Length - 3);
            return stotal;
        }
        #endregion

        #endregion

        #region Convert datatable to type free model

        public static List<T> GetModelFromDt<T>(DataTable dt, Dictionary<string, object> addModelValues = null) where T : new()
        {
            var myModel = new List<T>();
            var props = typeof(T).GetProperties();

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var modelInstanse = new T();


                if (addModelValues != null)
                {
                    foreach (
                        var itemValues in
                            addModelValues.Where(
                                itemValues =>
                                    props.Any(
                                        k => k.Name.Equals(itemValues.Key, StringComparison.CurrentCultureIgnoreCase))))
                    {
                        props.First(k => k.Name.Equals(itemValues.Key, StringComparison.CurrentCultureIgnoreCase))
                            .SetValue(modelInstanse, itemValues.Value);
                    }
                }

                for (int j = 0; j < dt.Rows[i].Table.Columns.Count; j++)
                {
                    var columnName = dt.Rows[i].Table.Columns[j].ColumnName;

                    var property =
                        props.FirstOrDefault(k => k.Name.Equals(columnName, StringComparison.CurrentCultureIgnoreCase));

                    if (property == null) continue;
                    var prType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;//fix by JBA 951102 For nullable value
                    if (prType.IsGenericType &&
                        Nullable.GetUnderlyingType(prType) != typeof(DateTime) &&
                        prType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        continue;
                    var value = dt.Rows[i][j];
                    if (value == DBNull.Value) continue;

                    property.SetValue(modelInstanse, value);

                }

                myModel.Add(modelInstanse);
            }

            return myModel;
        }

        public static List<T> GetModelFromDt<T>(T instanse, DataTable dt, Dictionary<string, object> addModelValues = null) where T : new()
        {
            var myModel = new List<T>();
            var props = typeof(T).GetProperties();

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var modelInstanse = new T();


                if (addModelValues != null)
                {
                    foreach (
                        var itemValues in
                            addModelValues.Where(
                                itemValues =>
                                    props.Any(
                                        k => k.Name.Equals(itemValues.Key, StringComparison.CurrentCultureIgnoreCase))))
                    {
                        props.First(k => k.Name.Equals(itemValues.Key, StringComparison.CurrentCultureIgnoreCase))
                            .SetValue(modelInstanse, itemValues.Value);
                    }
                }

                for (int j = 0; j < dt.Rows[i].Table.Columns.Count; j++)
                {
                    var columnName = dt.Rows[i].Table.Columns[j].ColumnName;

                    var property =
                        props.FirstOrDefault(k => k.Name.Equals(columnName, StringComparison.CurrentCultureIgnoreCase));

                    if (property == null) continue;
                    if (property.PropertyType.IsGenericType &&
                        property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        continue;
                    var value = dt.Rows[i][j];
                    if (value == DBNull.Value) continue;
                    property.SetValue(modelInstanse, value);
                }

                myModel.Add(modelInstanse);
            }

            return myModel;
        }

        //public static T GetModelFromDtAsList<T>(DataTable dt, Dictionary<string, object> addModelValues = null) where T : new()
        //{
        //    //todo tuye T ye list az model hast ke bayad model ro dar biarim bezarim tu in ziri
        //    var myModel = new List<object>();//.ConvertTo(typeof (T));
        //    var myType = typeof(T).GetGenericArguments()[0];
        //    var props = myType.GetProperties();

        //    //var a= new T().ConvertTo(myType);


        //    //var d1 = typeof(List<>);
        //    //Type[] typeArgs = { myType };
        //    //var makeme = d1.MakeGenericType(typeArgs);
        //    //var myModel = Activator.CreateInstance(makeme);

        //    for (var i = 0; i < dt.Rows.Count; i++)
        //    {
        //        var modelInstanse = Activator.CreateInstance(myType);


        //        if (addModelValues != null)
        //        {
        //            foreach (
        //                var itemValues in
        //                    addModelValues.Where(
        //                        itemValues =>
        //                            props.Any(
        //                                k => k.Name.Equals(itemValues.Key, StringComparison.CurrentCultureIgnoreCase))))
        //            {
        //                props.First(k => k.Name.Equals(itemValues.Key, StringComparison.CurrentCultureIgnoreCase))
        //                    .SetValue(modelInstanse, itemValues.Value);
        //            }
        //        }

        //        for (int j = 0; j < dt.Rows[i].Table.Columns.Count; j++)
        //        {
        //            var columnName = dt.Rows[i].Table.Columns[j].ColumnName;

        //            var property =
        //                props.FirstOrDefault(k => k.Name.Equals(columnName, StringComparison.CurrentCultureIgnoreCase));

        //            if (property == null) continue;
        //            if (property.PropertyType.IsGenericType &&
        //                property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
        //                continue;
        //            var value = dt.Rows[i][j];
        //            if (value == DBNull.Value) continue;
        //            property.SetValue(modelInstanse, value);
        //        }

        //        myModel.Add(modelInstanse);
        //    }
        //    //T listOfY = myModel.Cast<>().ToList()
        //    //return (T) Convert.ChangeType(myModel, typeof (T));
        //    var a = JsonConvert.SerializeObject(myModel);
        //    return JsonConvert.DeserializeObject<T>(a);
        //    //return (T) (object) (myModel);
        //}


        #endregion

        #region Convert Model to datatable
        public static DataTable GetDatatableFromModel<T>(List<T> input)
        {
            var dt = new DataTable();

            var model = typeof(T);
            //var model = typeof(T);
            var props = model.GetProperties();
            foreach (var propertyInfo in props)
            {
                Type type;
                if (propertyInfo.PropertyType.IsGenericType &&
                    propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = propertyInfo.PropertyType.GenericTypeArguments[0];
                else
                    type = propertyInfo.PropertyType;
                var name = propertyInfo.Name;

                if (dt.Columns.Contains(name))
                {
                    if (propertyInfo.DeclaringType.FullName.Equals(model.FullName))
                    {
                        dt.Columns.Remove(name);
                    }
                    else
                    {
                        continue;
                    }
                }

                var dc = new DataColumn(name, type);

                if (propertyInfo.PropertyType.IsGenericType &&
                    propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    dc.AllowDBNull = true;

                dt.Columns.Add(dc);
            }

            if (input != null)
                foreach (var item in input)
                {
                    var dr = dt.NewRow();
                    foreach (var propertyInfo in props)
                    {
                        if (propertyInfo.GetValue(item) != null)
                            dr[propertyInfo.Name] = propertyInfo.GetValue(item);
                        else
                            dr[propertyInfo.Name] = DBNull.Value;
                    }
                    dt.Rows.Add(dr);
                }

            return dt;
        }

        //public static DataTable GetDatatableFromModelHasAttribute<T>(List<T> input)
        //{
        //    return GetDatatableFromModelHasAttributeBase(input, false);
        //}

        //public static DataTable GetDatatableFromModelHasAttributeWithOrder<T>(List<T> input)
        //{
        //    return GetDatatableFromModelHasAttributeBase(input);
        //}
        //public static DataTable GetDatatableFromModelHasAttributeBase<T>(List<T> input, bool order = true)
        //{
        //    var dt = new DataTable();

        //    var model = typeof(T);
        //    //var model = typeof(T);
        //    var props = model.GetProperties().Where(i => i.GetCustomAttributes(typeof(SqlParam), false).Any());
        //    if (order)
        //        props = props.OrderBy(i => i.Name);
        //    var propertyInfos = props as IList<PropertyInfo> ?? props.ToList();
        //    foreach (var propertyInfo in propertyInfos)
        //    {
        //        Type type;
        //        if (propertyInfo.PropertyType.IsGenericType &&
        //            propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
        //            type = propertyInfo.PropertyType.GenericTypeArguments[0];
        //        else
        //            type = propertyInfo.PropertyType;
        //        var name = ((SqlParam)propertyInfo.GetCustomAttributes(typeof(SqlParam), false).First()).Name;

        //        var dc = new DataColumn(name, type);

        //        if (propertyInfo.PropertyType.IsGenericType &&
        //            propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
        //            dc.AllowDBNull = true;

        //        dt.Columns.Add(dc);
        //    }

        //    if (input != null)
        //        foreach (var item in input)
        //        {
        //            var dr = dt.NewRow();
        //            foreach (var propertyInfo in propertyInfos)
        //            {
        //                var name = ((SqlParam)propertyInfo.GetCustomAttributes(typeof(SqlParam), false).First()).Name;
        //                if (propertyInfo.GetValue(item) != null)
        //                    dr[name] = propertyInfo.GetValue(item);
        //                else
        //                    dr[name] = DBNull.Value;
        //            }
        //            dt.Rows.Add(dr);
        //        }

        //    return dt;
        //}
        #endregion

        #region Convert String To Enum
        public static T ToEnum<T>(string enumString)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), enumString, true);
            }
            catch (Exception exception)
            {
                T temp = default(T);
                string message = String.Format("'{0}' is not a valid enumeration of '{1}'", enumString, temp.GetType().Name);
                throw new Exception(message, exception);
            }
        }
        #endregion

        #region datetimes
        public static int getDayOfWeek_Persian(DayOfWeek dayOfWeek)
        {
            if (dayOfWeek == DayOfWeek.Saturday)
                return 0;
            else
            {
                return (int)dayOfWeek + 1;
            }
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime StartOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        public static DateTime StartOfYear(this DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1);
        }

        #endregion

        #region XML

        public static string ToXml<T>(T inputmodel)
        {
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stringwriter, inputmodel);
            return stringwriter.ToString();
        }
        public static T LoadFromXmlString<T>(string xmlText)
        {
            var stringReader = new System.IO.StringReader(xmlText);
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stringReader);
        }
        #endregion

        #region GetDefault
        public static T GetDefaultValue<T>()
        {
            var e = Expression.Lambda<Func<T>>(Expression.Default(typeof(T)));
            return e.Compile()();
        }

        public static object GetDefaultValue(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var e = Expression.Lambda<Func<object>>(Expression.Convert(Expression.Default(type), typeof(object)));
            return e.Compile()();
        }
        #endregion

        #region ToDbType

        #region Convert PropertyInfo
        //public static SqlDbType ToDbType(this PropertyInfo property)
        //{
        //    return property.PropertyType.ToDbType();
        //}
        #endregion

        #region Convert a type

        //public static SqlDbType ToDbType(this Type type)
        //{
        //    var sqlParameter = new SqlParameter();
        //    var typeConverter = TypeDescriptor.GetConverter(sqlParameter.DbType);
        //    if (typeConverter.CanConvertFrom(type))
        //    {
        //        var convertFrom = typeConverter.ConvertFrom(type.Name);
        //        if (convertFrom != null)
        //            sqlParameter.DbType = (DbType)convertFrom;
        //    }
        //    else
        //    {
        //        //Try brute force
        //        try
        //        {
        //            var convertFrom = typeConverter.ConvertFrom(type.Name);
        //            if (convertFrom != null)
        //                sqlParameter.DbType = (DbType)convertFrom;
        //        }
        //        catch (Exception)
        //        {
        //            //Do Nothing; will return NVarChar as default
        //        }
        //    }
        //    return sqlParameter.SqlDbType;
        //}
        #endregion

        #endregion

        #region ConvertTo
        public static T ConvertTo<T>(this object input)
        {
            try
            {
                return (T)input.ConvertTo(typeof(T));
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static object ConvertTo(this object input, Type to)
        {
            return Convert.ChangeType(input, to);
        }
        #endregion

        public static T CastTo<T>(this object obj) where T : new()
        {
            var returnValue = new T();
            try
            {
                var source = obj.GetType().GetProperties();
                var destination = typeof(T).GetProperties();
                foreach (var item in source)
                {
                    var sourceItemName = item.Name;
                    //var property = destination.FirstOrDefault(i. => GetSqlParam(i).Name.Equals(sourceItemName, StringComparison.CurrentCultureIgnoreCase) || GetSqlParam(i).OriginalName.Equals(sourceItemName, StringComparison.CurrentCultureIgnoreCase));
                    var property =
                        destination.FirstOrDefault(
                            i => i.Name.Equals(sourceItemName, StringComparison.CurrentCultureIgnoreCase));
                    if (property == null) continue;
                    var canwrite = property.CanWrite && property.GetSetMethod(true).IsPublic;
                    if (!canwrite) continue;

                    object value;
                    try
                    {
                        value = item.GetValue(obj);
                    }
                    catch (Exception ex1)
                    {
                        value = item.PropertyType.GetDefaultValue();
                    }
                    try
                    {

                        property.SetValue(returnValue, ConvertTo(value, property.PropertyType));

                    }
                    catch (Exception ex2)
                    {
                        try
                        {
                            property.SetValue(returnValue, value.GetType().GetDefaultValue());
                        }
                        catch (Exception ex3)
                        {
                            property.SetValue(returnValue, property.PropertyType.GetDefaultValue());
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                //Todo : Log Error
                return default(T);
            }
            return returnValue;

        }

        public static T Clone<T>(this T obj) where T : new()
        {
            var returnValue = new T();
            try
            {
                var source = obj.GetType().GetProperties();
                var destination = typeof(T).GetProperties();
                foreach (var item in source)
                {
                    var sourceItemName = item.Name;
                    //var property = destination.FirstOrDefault(i. => GetSqlParam(i).Name.Equals(sourceItemName, StringComparison.CurrentCultureIgnoreCase) || GetSqlParam(i).OriginalName.Equals(sourceItemName, StringComparison.CurrentCultureIgnoreCase));
                    var property =
                        destination.FirstOrDefault(
                            i => i.Name.Equals(sourceItemName, StringComparison.CurrentCultureIgnoreCase));
                    if (property == null) continue;
                    var canwrite = property.CanWrite && property.GetSetMethod(true).IsPublic;
                    if (!canwrite) continue;

                    object value;
                    try
                    {
                        value = item.GetValue(obj);
                    }
                    catch (Exception ex1)
                    {
                        value = item.PropertyType.GetDefaultValue();
                    }
                    try
                    {

                        property.SetValue(returnValue, ConvertTo(value, property.PropertyType));

                    }
                    catch (Exception ex2)
                    {
                        try
                        {
                            property.SetValue(returnValue, value.GetType().GetDefaultValue());
                        }
                        catch (Exception ex3)
                        {
                            property.SetValue(returnValue, property.PropertyType.GetDefaultValue());
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                //Todo : Log Error
                return default(T);
            }
            return returnValue;

        }


        #endregion
    }
}