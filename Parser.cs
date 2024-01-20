using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class Parser
    {

        Tokenizer _tokenizer;

        public Parser(Tokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }

        // Parse an entire expression and check End was reached
        public Node ParseExpression()
        {
            var expr = ParseAddSubtract();

            // Check everything was consumed
            if (_tokenizer.Token != Token.EndOfExpression)
                throw new ArgumentException("Unexpected characters at end of expression");

            return expr;
        }

        // Parse a sequence of add / subtract operators
        Node ParseAddSubtract()
        {
            // Parse the left hand side
            var lhs = ParseMultiplyDivide();

            while (true)
            {
                // Work out the operator
                Func<decimal, decimal, decimal> op = null;
                if (_tokenizer.Token == Token.Add)
                {
                    op = (a, b) => a + b;
                }
                else if (_tokenizer.Token == Token.Subtract)
                {
                    op = (a, b) => a - b;
                }

                // Binary operator found?
                if (op == null)
                    return lhs;      // no

                // Skip the operator
                _tokenizer.NextToken();

                // Parse the right hand side of the expression
                var rhs = ParseMultiplyDivide();

                // Create a binary node and use it as the lhs
                lhs = new NodeBinary(lhs, rhs, op);
            }
        }

        // Parse a leaf node
        Node ParseLeaf()
        {
            // number?
            if (_tokenizer.Token == Token.Number)
            {
                var node = new NodeNumber(_tokenizer.Number);
                _tokenizer.NextToken();
                return node;
            }

            // Parenthesis?
            if (_tokenizer.Token == Token.OpenParenthesis)
            {
                // Skip '('
                _tokenizer.NextToken();

                // Parse a top-level expression
                var node = ParseAddSubtract();

                // Check and skip ')'
                if (_tokenizer.Token != Token.CloseParenthesis)
                    throw new ArgumentException("Missing close parenthesis");
                _tokenizer.NextToken();

                // Return
                return node;
            }

            throw new ArgumentException($"Unexpected token: {_tokenizer.Token}");
        }

        // Parse a sequence of multiply / divide operators
        Node ParseMultiplyDivide()
        {
            // Parse lhs
            var lhs = ParseUnary();

            while (true)
            {
                // Work out the operator
                Func<decimal, decimal, decimal> op = null;
                if (_tokenizer.Token == Token.Multiply)
                {
                    op = (a, b) => a * b;
                }
                else if (_tokenizer.Token == Token.Divide)
                {
                    op = (a, b) => a / b;
                }

                // Binary operator found
                if (op == null)
                    return lhs;      // no

                // Skip the operator
                _tokenizer.NextToken();

                // Parse rhs
                var rhs = ParseUnary();

                // Create a binary node and use it as the lhs
                lhs = new NodeBinary(lhs, rhs, op);
            }
        }

        // Parse a unary operator (eg: negative/positive)
        Node ParseUnary()
        {
            // Positive operator is a no-op
            if (_tokenizer.Token == Token.Add)
            {
                _tokenizer.NextToken();
                return ParseUnary();
            }

            // Negative operator
            if (_tokenizer.Token == Token.Subtract)
            {
                // Skip
                _tokenizer.NextToken();

                // Parse RHS, recurse to support "--..."
                var rhs = ParseUnary();

                // Create unary node
                return new NodeUnary(rhs, (a) => -a);
            }

            // No positive/negative operator so parse a leaf node
            return ParseLeaf();
        }

        #region Helpers

        public static Node Parse(string input)
        {
            using (StringReader reader = new StringReader(input))
            {
                return Parse(new Tokenizer(reader));
            }
        }

        public static Node Parse(Tokenizer tokenizer)
        {
            var parser = new Parser(tokenizer);
            return parser.ParseExpression();
        }

        #endregion
    }

}
