using System;
using System.Linq.Expressions;

namespace Tubumu.Core.FastLambda
{
    /// <summary>
    /// CacheEvaluator
    /// </summary>
    public class CacheEvaluator : IEvaluator
    {
        private static IExpressionCache<Delegate> s_cache = new HashedListCache<Delegate>();

        private WeakTypeDelegateGenerator _delegateGenerator = new WeakTypeDelegateGenerator();
        private ConstantExtractor _constantExtrator = new ConstantExtractor();

        private IExpressionCache<Delegate> _cache;
        private Func<Expression, Delegate> _creatorDelegate;

        /// <summary>
        /// Constructor
        /// </summary>
        public CacheEvaluator()
            : this(s_cache)
        { }

        /// <summary>
        /// CacheEvaluator
        /// </summary>
        /// <param name="cache"></param>
        public CacheEvaluator(IExpressionCache<Delegate> cache)
        {
            _cache = cache;
            _creatorDelegate = (key) => _delegateGenerator.Generate(key);
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

            var parameters = _constantExtrator.Extract(exp);
            var func = _cache.Get(exp, _creatorDelegate);
            return func.DynamicInvoke(parameters.ToArray());
        }
    }
}
