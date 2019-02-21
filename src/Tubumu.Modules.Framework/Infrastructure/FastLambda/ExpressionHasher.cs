using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Tubumu.Modules.Framework.Infrastructure.FastLambda
{
    /// <summary>
    /// ExpressionHasher
    /// </summary>
    public class ExpressionHasher : ExpressionVisitor
    {
        /// <summary>
        /// Hash
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public int Hash(Expression exp)
        {
            this.HashCode = 0;
            this.Visit(exp);
            return this.HashCode;
        }

        /// <summary>
        /// HashCode
        /// </summary>
        public int HashCode { get; protected set; }

        /// <summary>
        /// Hash
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual ExpressionHasher Hash(int value)
        {
            unchecked { this.HashCode += value; }
            return this;
        }

        /// <summary>
        /// Hash
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual ExpressionHasher Hash(bool value)
        {
            unchecked { this.HashCode += value ? 1 : 0; }
            return this;
        }

        private static readonly object s_nullValue = new object();

        /// <summary>
        /// Hash
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual ExpressionHasher Hash(object value)
        {
            value = value ?? s_nullValue;
            unchecked { this.HashCode += value.GetHashCode(); }
            return this;
        }

        /// <summary>
        /// Visit
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        protected override Expression Visit(Expression exp)
        {
            if (exp == null) return exp;

            this.Hash((int)exp.NodeType).Hash(exp.Type);
            return base.Visit(exp);
        }

        /// <summary>
        /// VisitBinary
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression b)
        {
            this.Hash(b.IsLifted).Hash(b.IsLiftedToNull).Hash(b.Method);
            return base.VisitBinary(b);
        }

        /// <summary>
        /// VisitBinding
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        protected override MemberBinding VisitBinding(MemberBinding binding)
        {
            this.Hash(binding.BindingType).Hash(binding.Member);
            return base.VisitBinding(binding);
        }

        /// <summary>
        /// VisitConstant
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression c)
        {
            this.Hash(c.Value);
            return base.VisitConstant(c);
        }

        /// <summary>
        /// VisitElementInitializer
        /// </summary>
        /// <param name="initializer"></param>
        /// <returns></returns>
        protected override ElementInit VisitElementInitializer(ElementInit initializer)
        {
            this.Hash(initializer.AddMethod);
            return base.VisitElementInitializer(initializer);
        }

        /// <summary>
        /// VisitLambda
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        protected override Expression VisitLambda(LambdaExpression lambda)
        {
            foreach (var p in lambda.Parameters)
            {
                this.VisitParameter(p);
            }

            return base.VisitLambda(lambda);
        }

        /// <summary>
        /// VisitMemberAccess
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            this.Hash(m.Member);
            return base.VisitMemberAccess(m);
        }

        /// <summary>
        /// VisitMethodCall
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            this.Hash(m.Method);
            return base.VisitMethodCall(m);
        }

        /// <summary>
        /// VisitNew
        /// </summary>
        /// <param name="nex"></param>
        /// <returns></returns>
        protected override NewExpression VisitNew(NewExpression nex)
        {
            this.Hash(nex.Constructor);
            if (nex.Members != null)
            {
                foreach (var m in nex.Members) this.Hash(m);
            }

            return base.VisitNew(nex);
        }

        /// <summary>
        /// VisitParameter
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        protected override Expression VisitParameter(ParameterExpression p)
        {
            this.Hash(p.Name);
            return base.VisitParameter(p);
        }

        /// <summary>
        /// VisitTypeIs
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override Expression VisitTypeIs(TypeBinaryExpression b)
        {
            this.Hash(b.TypeOperand);
            return base.VisitTypeIs(b);
        }

        /// <summary>
        /// VisitUnary
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        protected override Expression VisitUnary(UnaryExpression u)
        {
            this.Hash(u.IsLifted).Hash(u.IsLiftedToNull).Hash(u.Method);
            return base.VisitUnary(u);
        }
    }
}
