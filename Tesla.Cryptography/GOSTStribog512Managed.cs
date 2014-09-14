using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesla.Cryptography
{
    public class GOSTStribog512Managed
        : GOSTStribog512
    {
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            throw new NotImplementedException();
        }

        protected override byte[] HashFinal()
        {
            throw new NotImplementedException();
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
