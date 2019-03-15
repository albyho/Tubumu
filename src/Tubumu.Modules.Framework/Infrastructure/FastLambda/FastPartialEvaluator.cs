using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Tubumu.Modules.Framework.Infrastructure.FastLambda
{
    /// <summary>
    /// FastPartialEvaluator
    /// </summary>
    public class FastPartialEvaluator : PartialEvaluatorBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FastPartialEvaluator()
            : base(new FastEvaluator())
        { }
    }
}
