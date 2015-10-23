using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Tesla.Data {
    public sealed class DataResult
        : IEnumerable, IEnumerable<DataResult.DataResultRow> {
        public sealed class DataResultRow {
            private readonly DataResult _parent;
            private readonly int _selfIndex;

            internal DataResultRow(DataResult parent, int selfIndex) {
                _parent = parent;
                _selfIndex = selfIndex;
            }

            public object this[string i] {
                get {
                    unchecked {
                        return _parent._data[_selfIndex][(int)_parent._columns[i]];
                    }
                }
            }
        }

        private readonly Hashtable _columns;
        private readonly List<object[]> _data;
        private readonly int _columnsCount;

        public bool HasData { get; private set; } = true;
        public int RowsCount => _data.Count;
        public int ColumnsCount => _columnsCount;

        public DataResult(IDataReader reader) {
            unchecked {
                _columnsCount = reader.FieldCount;
                _columns = new Hashtable();
                _data = new List<object[]>();

                if (!reader.Read()) {
                    HasData = false;
                } else {
                    for (var i = 0; i < _columnsCount; i++) {
                        _columns.Add(reader.GetName(i), i);
                    }

                    do {
                        var row = new object[_columnsCount];
                        for (var i = 0; i < _columnsCount; i++) {
                            row[i] = reader[i];
                        }
                        _data.Add(row);
                    } while (reader.Read());
                }
            }
        }

        public DataResultRow this[int i] => new DataResultRow(this, i);

        IEnumerator<DataResultRow> IEnumerable<DataResultRow>.GetEnumerator() {
            for (var i = 0; i < _data.Count; i++) {
                yield return new DataResultRow(this, i);
            }
        }

        public IEnumerator GetEnumerator() {
            for (var i = 0; i < _data.Count; i++) {
                yield return new DataResultRow(this, i);
            }
        }
    }
}
