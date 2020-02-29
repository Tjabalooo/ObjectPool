using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectPool.Example
{
    internal class ViewRepository
    {
        public MainWindowViewModel GetMainWindowView()
        {
            return new MainWindowViewModel();
        }
    }
}
