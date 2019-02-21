using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Tubumu.Modules.Framework.Infrastructure.FastLambda
{
    /// <summary>
    /// ConstantExtractor
    /// </summary>
    public class ConstantExtractor : ExpressionVisitor
    {
        private List<object> m_constants;

        /// <summary>
        /// Extract
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public List<object> Extract(Expression exp)
        {
            this.m_constants = new List<object>();
            this.Visit(exp);
            return this.m_constants;
        }

        /// <summary>
        /// VisitConstant
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression c)
        {
            this.m_constants.Add(c.Value);
            return c;
        }
    }        
}
