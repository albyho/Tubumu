using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Tubumu.Modules.Framework.Infrastructure.FastLambda
{
    /// <summary>
    /// Evaluator
    /// </summary>
    public class Evaluator : IEvaluator
    {
        /// <summary>
        /// Eval
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public object Eval(Expression exp)
        {
            if (exp.NodeType == ExpressionType.Constant)
            {
                return ((ConstantExpression)exp).Value;
            }

            LambdaExpression lambda = Expression.Lambda(exp);
            Delegate fn = lambda.Compile();

            return fn.DynamicInvoke(null);
        }
    }
}
