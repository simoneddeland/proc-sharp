using System;
using System.Collections.Generic;
using System.Text;

namespace ProcSharpCore
{
    public class MouseEvent
    {

        private int count;

        public int GetCount()
        {
            return count;
        }

        internal void SetCount(int count)
        {
            this.count = count;
        }
    }
}
