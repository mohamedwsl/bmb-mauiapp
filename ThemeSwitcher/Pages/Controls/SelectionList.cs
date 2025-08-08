using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Specialized;

namespace ThemeSwitcher.Pages.Controls
{
    internal class SelectionList : IList<object>
    {
        private readonly CfMultiPickerPopup _owner;
        private readonly IList<object> _internal;
        private readonly IList<object> _shadow;
        private bool _externalChange;

        public SelectionList(CfMultiPickerPopup owner, IList<object>? items = null)
        {
            _owner = owner ?? throw new ArgumentNullException(nameof(owner));
            _internal = items ?? new List<object>();
            _shadow = new List<object>(_internal);

            if (items is INotifyCollectionChanged incc)
                incc.CollectionChanged += OnCollectionChanged;
        }

        public object this[int index] { get => _internal[index]; set => _internal[index] = value; }
        public int Count => _internal.Count;
        public bool IsReadOnly => false;

        public void Add(object item)
        {
            _externalChange = true;
            _internal.Add(item);
            _externalChange = false;
            _shadow.Add(item);
        }

        public void Clear()
        {
            _externalChange = true;
            _internal.Clear();
            _externalChange = false;
            _shadow.Clear();
        }

        public bool Contains(object item) => _internal.Contains(item);
        public void CopyTo(object[] array, int arrayIndex) => _internal.CopyTo(array, arrayIndex);
        public IEnumerator<object> GetEnumerator() => _internal.GetEnumerator();
        public int IndexOf(object item) => _internal.IndexOf(item);
        public void Insert(int index, object item) => _internal.Insert(index, item);

        public bool Remove(object item)
        {
            var removed = _internal.Remove(item);
            if (removed)
            {
                _shadow.Remove(item);
            }
            return removed;
        }

        public void RemoveAt(int index) => _internal.RemoveAt(index);
        IEnumerator IEnumerable.GetEnumerator() => _internal.GetEnumerator();

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (_externalChange) return;
        }
    }
}
