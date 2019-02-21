using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Tubumu.Modules.Framework.Infrastructure.FastLambda
{
    /// <summary>
    /// PartialEvaluator
    /// </summary>
    public class PartialEvaluator : PartialEvaluatorBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PartialEvaluator()
            : base(new Evaluator())
        { }
    }
}
