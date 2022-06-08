using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Module;
using Ninject;
using UI.ViewModel;

namespace UI.Module
{
    public class MainViewModule
    {
        private IKernel kernel;

        public MainViewModule()
        {
            this.kernel = new StandardKernel();
            kernel.Load(new BllModule());

        }

        public MainViewModel MainViewModel => kernel.Get<MainViewModel>();
    }
}
