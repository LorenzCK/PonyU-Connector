using Microsoft.AspNetCore.Http.Extensions;
using System.Globalization;

namespace Ponyu.Connector
{
    internal static class QueryBuilderExtensions
    {
        public static QueryBuilder Add(this QueryBuilder builder, string key, string value)
        {
            builder.Add(key, value);
            return builder;
        }

        public static QueryBuilder Add(this QueryBuilder builder, string key, double value)
        {
            builder.Add(key, value.ToString(CultureInfo.InvariantCulture));
            return builder;
        }

        public static QueryBuilder Add(this QueryBuilder builder, string key, int value)
        {
            builder.Add(key, value.ToString(CultureInfo.InvariantCulture));
            return builder;
        }
    }
}
