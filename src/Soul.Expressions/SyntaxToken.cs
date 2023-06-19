using System.Linq.Expressions;

namespace Soul.Expressions
{
    public class SyntaxToken
    {
        public string Token { get; }
        public Expression Expression { get; }
        public ExpressionType ExpressionType => Expression.NodeType;

        public SyntaxToken(string token, Expression expression)
        {
            Token = token;
            Expression = expression;
        }
    }
}
