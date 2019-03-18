using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Tubumu.Modules.Core.FastLambda
{
    /// <summary>
    /// PartialEvaluatorBase
    /// </summary>
    public abstract class PartialEvaluatorBase : ExpressionVisitor
    {
        private IEvaluator m_evaluator;
        private HashSet<Expression> m_candidates;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="evaluator"></param>
        protected PartialEvaluatorBase(IEvaluator evaluator)
        {
            this.m_evaluator = evaluator;
        }

        /// <summary>
        /// Eval
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public Expression Eval(Expression exp)
        {
            this.m_candidates = new Nominator().Nominate(exp);
            return this.m_candidates.Count > 0 ? this.Visit(exp) : exp;
        }

        /// <summary>
        /// Visit
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        protected override Expression Visit(Expression exp)
        {
            if (exp == null)
            {
                return null;
            }

            if (this.m_candidates.Contains(exp))
            {
                return exp.NodeType == ExpressionType.Constant ? exp :
                    Expression.Constant(this.m_evaluator.Eval(exp), exp.Type);
            }

            return base.Visit(exp);
        }

        /// <summary>
        /// Performs bottom-up analysis to determine which nodes can possibly
        /// be part of an evaluated sub-tree.
        /// </summary>
        private class Nominator : ExpressionVisitor
        {
            private Func<Expression, bool> m_fnCanBeEvaluated;
            private HashSet<Expression> m_candidates;
            private bool m_cannotBeEvaluated;

            public Nominator()
                : this(CanBeEvaluatedLocally)
            { }

            public Nominator(Func<Expression, bool> fnCanBeEvaluated)
            {
                this.m_fnCanBeEvaluated = fnCanBeEvaluated ?? CanBeEvaluatedLocally;
            }

            private static bool CanBeEvaluatedLocally(Expression exp)
            {
                return exp.NodeType != ExpressionType.Parameter;
            }

            internal HashSet<Expression> Nominate(Expression expression)
            {
                this.m_candidates = new HashSet<Expression>();
                this.Visit(expression);
                return this.m_candidates;
            }

            protected override Expression Visit(Expression expression)
            {
                if (expression != null)
                {
                    bool saveCannotBeEvaluated = this.m_cannotBeEvaluated;
                    this.m_cannotBeEvaluated = false;

                    base.Visit(expression);

                    if (!this.m_cannotBeEvaluated)
                    {
                        if (this.m_fnCanBeEvaluated(expression))
                        {
                            this.m_candidates.Add(expression);
                        }
                        else
                        {
                            this.m_cannotBeEvaluated = true;
                        }
                    }

                    this.m_cannotBeEvaluated |= saveCannotBeEvaluated;
                }

                return expression;
            }
        }
    }
}
