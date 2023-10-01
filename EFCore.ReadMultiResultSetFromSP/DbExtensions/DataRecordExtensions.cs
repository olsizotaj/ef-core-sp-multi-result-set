using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;

namespace EFCore.ReadMultiResultSetFromSP.DbExtensions
{
    public static class DataRecordExtensions
    {
        private static readonly ConcurrentDictionary<Type, object> Materializers = new();

        public static async Task<List<T>> TranslateAsync<T>(this DbDataReader reader, CancellationToken token = default) where T : new()
        {
            var materializer = (Func<IDataRecord, T>)Materializers.GetOrAdd(typeof(T), (Func<IDataRecord, T>)Materializer.Materialize<T>);
            return await TranslateAsync(reader, materializer, token);
        }

        private static async Task<List<T>> TranslateAsync<T>(this DbDataReader reader, Func<IDataRecord, T> objectMaterializer, CancellationToken token = default)
        {
            var results = new List<T>();
            if (typeof(T).BaseType == null)
            {
                await reader.NextResultAsync(token);
                return results;
            }

            var isObject = typeof(T).IsValueType;
            
            while (await reader.ReadAsync(token))
            {
                T obj;
                var record = (IDataRecord)reader;

                if (!isObject)
                {
                    obj = objectMaterializer(record);
                }
                else
                {
                    obj = (T)ConvertToValue(record.GetValue(0), Type.GetTypeCode(typeof(T)));
                }
                results.Add(obj);
            }

            await reader.NextResultAsync(token);

            return results;
        }

        /// <summary>
        /// Note: This method is designed to read only a single/first value from the current result set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="token"></param>
        /// <returns>A single value of the generic type</returns>
        public static async Task<T> TranslateSingleValueAsync<T>(this DbDataReader reader, CancellationToken token = default)
        {
            T obj = default;

            while (await reader.ReadAsync(token))
            {
                var record = (IDataRecord)reader;
                obj = (T)ConvertToValue(record.GetValue(0), Type.GetTypeCode(typeof(T)));
                break;
            }

            await reader.NextResultAsync(token);

            return obj;
        }

        private static object ConvertToValue(object data, TypeCode typeCode)
        {
            object obj;
            switch (typeCode)
            {
                case TypeCode.String:
                    obj = data;
                    break;
                case TypeCode.Int32:
                    obj = Convert.ToInt32(data);
                    break;
                case TypeCode.Double:
                    obj = Convert.ToDouble(data);
                    break;
                case TypeCode.Decimal:
                    obj = Convert.ToDecimal(data);
                    break;
                case TypeCode.Boolean:
                    obj = Convert.ToBoolean(data);
                    break;
                case TypeCode.DateTime:
                    obj = Convert.ToDateTime(data);
                    break;
                default:
                    obj = default;
                    if (Guid.TryParse(data.ToString(), out var g))
                    {
                        obj = g;
                    }
                    break;
            }
            return obj;
        }
    }
}