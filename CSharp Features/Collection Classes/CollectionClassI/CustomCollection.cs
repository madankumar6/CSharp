using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionClassI
{
    public class Tokens : IEnumerable
    {
        public string[] elements;

        public Tokens(string source, char[] delimiters)
        {
            // Parse the string into tokens:
            elements = source.Split(delimiters);
        }

        // IEnumerable Interface Implementation: Declaration of the GetEnumerator() method required by IEnumerable

        public IEnumerator GetEnumerator()
        {
            return new TokenEnumerator(this);
        }
    }

    public class TokenEnumerator : IEnumerator
    {
        private int position = -1;
        private Tokens token;

        public TokenEnumerator(Tokens token)
        {
            this.token = token;
        }

        // Declare the MoveNext method required by IEnumerator:

        public bool MoveNext()
        {
            if (position < token.elements.Length - 1)
            {
                position++;
                return true;
            }
            return false;
        }

        // Declare the Reset method required by IEnumerator:
        public void Reset()
        {
            position = -1;
        }

        // Declare the Current property required by IEnumerator:
        public object Current
        {
            get
            {
                return token.elements[position];
            }
        }
    }

}
