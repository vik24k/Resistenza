using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace client
{
    static class WindowsUtils
    {
        public static void CenterWindow(Window Parent, Window Child)
        {
            double parentLeft = Parent.Left;
            double parentTop = Parent.Top;
            double parentWidth = Parent.Width;
            double parentHeight = Parent.Height;

            Child.Left = parentLeft + (parentWidth - Child.Width) / 2;
            Child.Top = parentTop + (parentHeight - Child.Height) / 2;


        }
    }
}
