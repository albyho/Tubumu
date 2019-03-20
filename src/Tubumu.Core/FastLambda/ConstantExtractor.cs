using System.Collections.Generic;
using System.Linq.Expressions;

namespace Tubumu.Core.FastLambda
{
    /// <summary>
    /// ConstantExtractor
    /// </summary>
    public class ConstantExtractor : ExpressionVisitor
    {
        private List<object> _constants;

        /// <summary>
        /// Extract
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public List<object> Extract(Expression exp)
        {
            _constants = new List<object>();
            Visit(exp);
            return _constants;
        }

        /// <summary>
        /// VisitConstant
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression c)
        {
            _constants.Add(c.Value);
            return c;
        }
    }
}
