using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Tubumu.Modules.Core.FastLambda
{
    /// <summary>
    /// DelegateGenerator
    /// </summary>
    public class DelegateGenerator : ExpressionVisitor
    {
        private static readonly MethodInfo s_indexerInfo = typeof(List<object>).GetMethod("get_Item");

        private int m_parameterCount;
        private ParameterExpression m_parametersExpression;

        /// <summary>
        /// Generate
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public Func<List<object>, object> Generate(Expression exp)
        {
            this.m_parameterCount = 0;
            this.m_parametersExpression =
                Expression.Parameter(typeof(List<object>), "parameters");

            var body = this.Visit(exp); // normalize
            if (body.Type != typeof(object))
            {
                body = Expression.Convert(body, typeof(object));
            }

            var lambda = Expression.Lambda<Func<List<object>, object>>(body, this.m_parametersExpression);
            return lambda.Compile();
        }

        /// <summary>
        /// VisitConstant
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression c)
        {
            Expression exp = Expression.Call(
                this.m_parametersExpression,
                s_indexerInfo,
                Expression.Constant(this.m_parameterCount++));
            return c.Type == typeof(object) ? exp : Expression.Convert(exp, c.Type);
        }
    }
}
