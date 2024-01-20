using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public abstract class Node
    {
        public abstract decimal Eval();
    }

    class NodeNumber : Node
    {
        decimal _number;
        public NodeNumber(decimal number)
        {
            _number = number;
        }

        public override decimal Eval()
        {
            // Just return it.  Too easy.
            return _number;
        }
    }

    // NodeBinary for binary operations such as Add, Subtract etc...
    class NodeBinary : Node
    {
        Node _lhs;
        Node _rhs;
        Func<decimal, decimal, decimal> _op;

        // Constructor accepts the two nodes to be operated on and function that performs the actual operation
        public NodeBinary(Node lhs, Node rhs, Func<decimal, decimal, decimal> op)
        {
            _lhs = lhs;
            _rhs = rhs;
            _op = op;
        }

        public override decimal Eval()
        {
            // Evaluate both sides
            var lhsVal = _lhs.Eval();
            var rhsVal = _rhs.Eval();

            // Evaluate and return
            var result = _op(lhsVal, rhsVal);
            return result;
        }
    }

    class NodeUnary : Node
    {
        Node _rhs;
        Func<decimal, decimal> _op;

        // Constructor accepts the two nodes to be operated on and function that performs the actual operation
        public NodeUnary(Node rhs, Func<decimal, decimal> op)
        {
            _rhs = rhs;
            _op = op;
        }

        public override decimal Eval()
        {
            // Evaluate RHS
            var rhsVal = _rhs.Eval();

            // Evaluate and return
            var result = _op(rhsVal);
            return result;
        }
    }
}
