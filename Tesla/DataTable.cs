using System;
using System.Collections;
using System.Collections.Generic;

namespace Tesla {
    /// <summary>
    /// Представляет неизменяемую таблицу данных с эффективной адресацией колонок.
    /// </summary>
    public class DataTable<T> : IEnumerable<DataTable<T>.DataTableRow> {
        /// <summary>
        /// Представляет одну запись в таблице данных.
        /// </summary>
        public sealed class DataTableRow {
            private readonly DataTable<T> _parent;
            private readonly int _selfIndex;

            internal DataTableRow(DataTable<T> parent, int selfIndex) {
                _parent = parent;
                _selfIndex = selfIndex;
            }

            public T this[string i] {
                get {
                    unchecked {
                        return _parent._data[_selfIndex][(int) _parent._columns[i]];
                    }
                }
            }

            public T this[int i] {
                get {
                    unchecked {
                        return _parent._data[_selfIndex][i];
                    }
                }
            }

            public bool HasColumn(string name) {
                return _parent._columns.ContainsKey(name);
            }
        }

        protected readonly Hashtable _columns = new Hashtable();
        protected readonly List<T[]> _data = new List<T[]>();

        public bool HasData { get; protected set; } = true;
        public int RowsCount => _data.Count;
        public int ColumnsCount { get; protected set; }

        /// <summary>
        /// Создать новый экземпляр пустой таблицы.
        /// </summary>
        public DataTable() {}

        /// <summary>
        /// Создать новый экземпляр таблицы на основе предоставленного заголовка и массива данных.
        /// </summary>
        /// <param name="header">Заголовок таблицы.</param>
        /// <param name="rows">Данные.</param>
        public DataTable(string[] header, T[][] rows) {
            if (rows.Length != header.Length) {
                throw new ArgumentException("<header> and <rows> lengthes are not equal.");
            }

            ColumnsCount = header.Length;
            //_columns = new Hashtable();
            //_data = new List<object[]>(rows.Length);

            unchecked {
                for (var i = 0; i < ColumnsCount; i++) {
                    _columns.Add(header[i], i);
                }

                if (rows.Length == 0) {
                    HasData = false;
                }
                else {
                    for (var i = 0; i < rows.Length; i++) {
                        _data.Add(rows[i]);
                    }
                }
            }
        }

        public DataTableRow this[int i] => new DataTableRow(this, i);

        public IEnumerator<DataTableRow> GetEnumerator() {
            for (var i = 0; i < _data.Count; i++) {
                yield return new DataTableRow(this, i);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
