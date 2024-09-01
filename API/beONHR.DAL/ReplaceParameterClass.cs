using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.DAL
{
    static class ExpressionExtensions
    {
        public static Expression ReplaceParameter(this Expression expression, ParameterExpression source, ParameterExpression target)
        {
            return new ParameterReplacerVisitor(source, target).Visit(expression);
        }

        class ParameterReplacerVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _source;
            private readonly ParameterExpression _target;

            public ParameterReplacerVisitor(ParameterExpression source, ParameterExpression target)
            {
                _source = source;
                _target = target;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == _source ? _target : base.VisitParameter(node);
            }
        }
    }
}
