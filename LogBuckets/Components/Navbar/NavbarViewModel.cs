using LogBuckets.Shared;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LogBuckets.Components.Navbar
{
    internal sealed class NavbarViewModel
    {

        #region Private Fields

        private readonly ObservableCollection<IMenuAction> _buttons;

        #endregion

        #region Ctor

        public NavbarViewModel()
        {
            _buttons = new ObservableCollection<IMenuAction>();
        }

        #endregion

        #region Public Interface

        public ICollection<IMenuAction> Buttons => _buttons;

        #endregion

        #region Private Methods



        #endregion

    }
}
