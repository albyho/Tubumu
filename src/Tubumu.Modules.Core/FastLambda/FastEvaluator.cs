using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Tubumu.Modules.Core.FastLambda
{
    /// <summary>
    /// FastEvaluator
    /// </summary>
    public class FastEvaluator : IEvaluator
    {
        private static IExpressionCache<Func<List<object>, object>> s_cache =
            new HashedListCache<Func<List<object>, object>>();

        private readonly DelegateGenerator m_delegateGenerator = new DelegateGenerator();
        private readonly ConstantExtractor m_constantExtrator = new ConstantExtractor();

        private readonly IExpressionCache<Func<List<object>, object>> m_cache;
        private readonly Func<Expression, Func<List<object>, object>> m_creatorDelegate;

        /// <summary>
        /// Constructor
        /// </summary>
        public FastEvaluator()
            : this(s_cache)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cache"></param>
        public FastEvaluator(IExpressionCache<Func<List<object>, object>> cache)
        {
            this.m_cache = cache;
            this.m_creatorDelegate = (key) => this.m_delegateGenerator.Generate(key);
        }

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

            var parameters = this.m_constantExtrator.Extract(exp);
            var func = this.m_cache.Get(exp, this.m_creatorDelegate);
            return func(parameters);
        }
    }
}
