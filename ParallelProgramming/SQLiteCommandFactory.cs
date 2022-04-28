using System.Text;

namespace POI
{
    public static class SQLiteCommandFactory
    {
        /// <summary>
        /// Generates SQLiteCommand for creating InMemory table
        /// </summary>
        /// <typeparam name="T">The type, that describes table's structure</typeparam>
        /// <param name="tableName">Name of the table</param>        
        /// <returns>Stringified 'create table' SQLiteCommand</returns>
        /// <exception cref="NotSupportedException">A collumn's type is currently unable to be matched with SQL type</exception>
        public static string CreateTable<T>(string tableName)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"create table {tableName} (");
            var enumerator = typeof(T).GetProperties().Where(x => Type.GetTypeCode(x.PropertyType) != TypeCode.Object).GetEnumerator();
            if (enumerator.MoveNext())
            {
                while (true)
                {
                    switch (Type.GetTypeCode(enumerator.Current.PropertyType))
                    {
                        case TypeCode.Boolean: builder.Append($"{enumerator.Current.Name} bit"); break;
                        case TypeCode.Int16: builder.Append($"{enumerator.Current.Name} int"); break;
                        case TypeCode.Decimal: builder.Append($"{enumerator.Current.Name} decimal"); break;
                        case TypeCode.String: builder.Append($"{enumerator.Current.Name} nvarchar(100)"); break; //TODO: could the size be dynamically set?
                                                                                                                 //more types could be supported, but the basics are specified above
                        default: throw new NotSupportedException();
                    }
                    if (enumerator.MoveNext())
                    {
                        builder.Append(", ");
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                throw new InvalidOperationException($"{nameof(T)} contains no properties that can be matched with any SQL types. No table definition was created.");
            }
            builder.Append(')');
            return builder.ToString();
        }
    }
}
