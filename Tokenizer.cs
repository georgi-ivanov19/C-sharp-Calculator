using Calculator;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class Tokenizer
    {
        TextReader _reader;
        char _currentChar;
        Token _currentToken;
        decimal _number;

        public Tokenizer(TextReader reader)
        {
            _reader = reader;
            NextChar();
            NextToken();
        }

        public Token Token
        {
            get { return _currentToken; }
        }

        public decimal Number
        {
            get { return _number; }
        }

        // Read the next character from the input and store it in _currentChar, or load '\0' if End
        void NextChar()
        {
            int ch = _reader.Read();
            _currentChar = ch < 0 ? '\0' : (char)ch;
        }

        // Read the next token from the input
        public void NextToken()
        {
            // Skip whitespace
            while (char.IsWhiteSpace(_currentChar))
            {
                NextChar();
            }

            // Special characters
            switch (_currentChar)
            {
                case '\0':
                    _currentToken = Token.EndOfExpression;
                    return;
                case '+':
                    NextChar();
                    _currentToken = Token.Add;
                    return;
                case '-':
                    NextChar();
                    _currentToken = Token.Subtract;
                    return;
                case '*':
                    NextChar();
                    _currentToken = Token.Multiply;
                    return;
                case '/':
                    NextChar();
                    _currentToken = Token.Divide;
                    return;
                case '(':
                    NextChar();
                    _currentToken = Token.OpenParenthesis;
                    return;
                case ')':
                    NextChar();
                    _currentToken = Token.CloseParenthesis;
                    return;
            }

            // Number?
            if (char.IsDigit(_currentChar) || _currentChar == '.')
            {
                // Capture digits/decimal point
                var sb = new StringBuilder();
                bool havedecimalPoint = false;
                while (char.IsDigit(_currentChar) || (!havedecimalPoint && _currentChar == '.'))
                {
                    sb.Append(_currentChar);
                    havedecimalPoint = _currentChar == '.';
                    NextChar();
                }

                // Parse it
                _number = decimal.Parse(sb.ToString(), CultureInfo.InvariantCulture);
                _currentToken = Token.Number;
                return;
            }

            throw new InvalidDataException($"Unexpected character: {_currentChar}");
        }
    }
}