using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
namespace LVGLSharp.Forms
{
    


    [ListBindable(false)]
    public class ControlCollection : IList<Control>, ICollection, IEnumerable
    {
        
        private Collection<Control> _ctls = new Collection<Control>();
        public Control? this[int index] { get => _ctls[index]; set => _ctls[index] = value; }

        public int Count => _ctls.Count;

        public bool IsReadOnly => false;

        public bool IsSynchronized => false;

        public object SyncRoot => _ctls;
        public void Add(Control item)
        {
            _ctls.Add(item);
        }
        public void Add(Control item, int v, int v1)
        {
            _ctls.Add(item);
        }

        public void Clear()
        {
            _ctls.Clear();
        }



        public bool Contains(Control item)
        {
            return _ctls.Contains(item);
        }

        public void CopyTo(Control[] array, int arrayIndex)
        {
            _ctls.CopyTo(array, arrayIndex);
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Control> GetEnumerator()
        {
            return _ctls.GetEnumerator();
        }

        public int IndexOf(Control item) => _ctls.IndexOf(item);

        public void Insert(int index, Control item) => _ctls.Insert(index, item);

        public bool Remove(Control item) { return _ctls.Remove(item); }

        public void RemoveAt(int index) { _ctls.RemoveAt(index); }

        IEnumerator IEnumerable.GetEnumerator() { return _ctls.GetEnumerator(); }
    }
}