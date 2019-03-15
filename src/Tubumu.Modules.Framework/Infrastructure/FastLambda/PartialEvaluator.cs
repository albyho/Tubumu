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
        /// Constructor
        /// </summary>
        public PartialEvaluator()
            : base(new Evaluator())
        { }
    }
}
