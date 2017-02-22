using System;
using System.Linq.Expressions;
using System.Reflection;

namespace schedule.Extensions
{
    public static class ExpressionExtensions
    {
        public static void SetValue<TObject, TProperty, T>(this Expression<Func<TObject, TProperty>> propertyPicker, T obj, TProperty value)
        {
            var memberExpression = (MemberExpression)propertyPicker.Body;
            var property = (PropertyInfo)memberExpression.Member;

            property.SetValue(obj, value);
        }
    }
}
