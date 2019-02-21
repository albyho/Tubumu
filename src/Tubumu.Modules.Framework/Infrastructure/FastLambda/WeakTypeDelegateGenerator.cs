using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Tubumu.Modules.Framework.Infrastructure.FastLambda
{
    /// <summary>
    /// WeakTypeDelegateGenerator
    /// </summary>
    public class WeakTypeDelegateGenerator : ExpressionVisitor
    {
        private List<ParameterExpression> m_parameters;

        /// <summary>
        /// Generate
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public Delegate Generate(Expression exp)
        {
            this.m_parameters = new List<ParameterExpression>();

            var body = this.Visit(exp);
            var lambda = Expression.Lambda(body, this.m_parameters.ToArray());
            return lambda.Compile();
        }

        /// <summary>
        /// VisitConstant
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression c)
        {
            var p = Expression.Parameter(c.Type, "p" + this.m_parameters.Count);
            this.m_parameters.Add(p);
            return p;
        }
    }
}
