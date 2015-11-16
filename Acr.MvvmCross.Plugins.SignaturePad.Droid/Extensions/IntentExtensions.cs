using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Linq.Expressions;
using System.Reflection;

namespace Acr.MvvmCross.Plugins.SignaturePad.Droid.Extensions {
    public static class IntentExtensions {

        public static void GetExtra(this Intent intent, Expression<Func<string>> property) {
            GetToProperty(property, (p) => intent.GetStringExtra(p));
        }

        public static void GetExtra(this Intent intent, Expression<Func<int>> property) {
            GetToProperty(property, (p, d) => intent.GetIntExtra(p, d));
        }

        public static void GetExtra(this Intent intent, Expression<Func<float>> property) {
            GetToProperty(property, (p, d) => intent.GetFloatExtra(p, d));
        }

        public static void GetExtraEnum<TEnum>(this Intent intent, Expression<Func<TEnum>> property)
                                    where TEnum : struct {
            if (!typeof(TEnum).IsEnum) { throw new ArgumentException("TEnum must be an enumerated type"); }

            //This should be a tryparse thing, big chance of failure here.
            GetToPropertyEnum(property, (p) => intent.GetStringExtra(p));
        }

        public static void PutExtra(this Intent intent, Expression<Func<string>> property) {
            PutFromProperty(property, (p, v) => intent.PutExtra(p, v));
        }

        public static void PutExtra(this Intent intent, Expression<Func<int>> property) {
            PutFromProperty(property, (p, v) => intent.PutExtra(p, v));
        }
        public static void PutExtra(this Intent intent, Expression<Func<float>> property) {
            PutFromProperty(property, (p, v) => intent.PutExtra(p, v));
        }

        public static void PutExtraEnum<TEnum>(this Intent intent, Expression<Func<TEnum>> property)
                                    where TEnum : struct {
            if (!typeof(TEnum).IsEnum) { throw new ArgumentException("TEnum must be an enumerated type"); }

            PutFromProperty(property, (p, v) => intent.PutExtra(p, v.ToString()));
        }





        private static void GetToPropertyEnum<TEnum>(Expression<Func<TEnum>> property, Func<string, string> setter)
                                    where TEnum : struct {

            var expression = GetMemberInfo(property);
            var path = PropertyPath(expression);

            var pi = expression.Member as PropertyInfo;
            var parent = Expression.Lambda(expression.Expression).Compile().DynamicInvoke();
            var @value = setter(path);
            TEnum enumValue;
            if (@value != null) {
                if (Enum.TryParse(@value, true, out enumValue)) {
                    pi.SetValue(parent, enumValue, null);
                }
            }

        }

        private static void GetToProperty<T>(Expression<Func<T>> property, Func<string, T> setter) {

            var expression = GetMemberInfo(property);
            var path = PropertyPath(expression);

            var pi = expression.Member as PropertyInfo;
            var parent = Expression.Lambda(expression.Expression).Compile().DynamicInvoke();
            var @value = setter(path);
            if (@value != null) {
                pi.SetValue(parent, @value, null);
            }

        }

        private static void GetToProperty<T>(Expression<Func<T>> property, Func<string, T, T> setter) {

            var expression = GetMemberInfo(property);
            var path = PropertyPath(expression);
            var pi = expression.Member as PropertyInfo;
            var parent = Expression.Lambda(expression.Expression).Compile().DynamicInvoke();
            var compiled = property.Compile();

            var @value = setter(path, compiled());

            pi.SetValue(parent, @value, null);

        }

        private static void PutFromProperty<T>(Expression<Func<T>> property, Action<string, T> setter) {
            
            var expression = GetMemberInfo(property);
            var path = PropertyPath(expression);
            var compiled = property.Compile();

            setter(path, compiled());
        }

        private static MemberExpression GetMemberInfo(Expression method) {
            LambdaExpression lambda = method as LambdaExpression;
            if (lambda == null)
                throw new ArgumentNullException("method");

            MemberExpression memberExpr = null;

            if (lambda.Body.NodeType == ExpressionType.Convert) {
                memberExpr =
                    ((UnaryExpression)lambda.Body).Operand as MemberExpression;
            } else if (lambda.Body.NodeType == ExpressionType.MemberAccess) {
                memberExpr = lambda.Body as MemberExpression;
            }

            if (memberExpr == null)
                throw new ArgumentException("method");

            return memberExpr;
        }

        private static string PropertyPath(MemberExpression expression) {
            var names = new List<string>();
            while (expression != null) {
                if (expression.Expression is ConstantExpression)
                  break;

                names.Add(expression.Member.Name);

                expression = expression.Expression as MemberExpression;
            }

            names.Reverse();
            var path = string.Join(".", names);

            return path;
        }

    }
}