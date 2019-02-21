using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Tubumu.Modules.Framework.Infrastructure.FastLambda
{
    /// <summary>
    /// IEvaluator
    /// </summary>
    public interface IEvaluator
    {
        /// <summary>
        /// Eval
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        object Eval(Expression exp);
    }
}
