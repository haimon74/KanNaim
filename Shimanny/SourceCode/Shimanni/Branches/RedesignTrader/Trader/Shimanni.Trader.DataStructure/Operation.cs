using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shimanni.Trader.DataStructure
{
    public abstract class Operation<T>
    {
        private T m_operand1;
        private T m_operand2;

        public T Operand1
        {
            get { return m_operand1; }
            set { m_operand1 = value; }
        }

        public T Operand2
        {
            get { return m_operand2; }
            set { m_operand2 = value; }
        }

        public Operation(T operand1, T operand2)
        {
            m_operand1 = operand1;
            m_operand2 = operand2;
        }

        public override string ToString()
        {
            return m_operand1.ToString() + "," + m_operand1.ToString();
        }

        public abstract T Run();

        public virtual void DoSomething()
        {
            Console.WriteLine(this);
        }
    }
}
