using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzCoreKit.Spider {
    public abstract class PageLoaderBase<T> : IEnumerator<T>, IEnumerable<T> {
        public int PageIndex { get; private set; } = -1;
        private int PageItemIndex = 0;

        public T Current => currentPageItems[PageItemIndex];
        object IEnumerator.Current => Current;

        private T[] currentPageItems = null;

        protected abstract T[] LoadPage(int pageIndex);

        public bool MoveNext() {
            if (currentPageItems == null || 
                currentPageItems.Length == 0 ||
                PageItemIndex == currentPageItems.Length -1) {
                PageIndex++;
                PageItemIndex = 0;
                currentPageItems = LoadPage(PageIndex);

            } else {
                PageItemIndex++;
            }

            return currentPageItems.Length > 0;
        }

        public void Reset() {
            PageIndex = 0;
            PageItemIndex = 0;
            LoadPage(0);
        }

        public void Dispose() {
            this.currentPageItems = null;
        }

        public IEnumerator<T> GetEnumerator() {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this;
        }
    }
}
