using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CX.Commands;

namespace CX.Interfaces
{
    public interface IPresenter
    {
        CommandDispatcher Dispatcher { get;}
        List<IPresenter> ChildPresenters { get;}
    }
    
    public class Presenter<TView> : IDisposable, IPresenter
    {
        public Presenter()
        {
            dispatcher = new CommandDispatcher(this as IPresenter);
        }


        private List<IPresenter> childPresenters = new List<IPresenter>();
        public List<IPresenter> ChildPresenters
        {
            get { return childPresenters; }
        }

        private TView view;

        public TView View
        {
            get { return view; }
            set { view = value; OnViewSet(); }
        }

        private CommandDispatcher dispatcher; 

        public CommandDispatcher Dispatcher
        {
            get{ return dispatcher;}
        }

        public virtual void OnViewReady() { }
        protected virtual void OnViewSet() { }

       ~Presenter()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }
    }
}
