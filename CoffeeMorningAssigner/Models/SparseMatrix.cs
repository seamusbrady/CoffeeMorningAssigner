using System.Collections.Generic;

namespace CoffeeMorningAssigner.Models
{
    public class SparseMatrix<T>
    {
        public int Width { get; }
        public int Height { get; }
        public long Size { get; }

        private readonly Dictionary<long, T> _cells = new Dictionary<long, T>();

        public SparseMatrix(int w, int h)
        {
            Width = w;
            Height = h;
            Size = w * h;
        }

        public bool IsCellEmpty(int row, int col)
        {
            long index = row * Width + col;
            return _cells.ContainsKey(index);
        }

        public T this[int row, int col]
        {
            get
            {
                long index = row * Width + col;
                T result;
                _cells.TryGetValue(index, out result);
                return result;
            }
            set
            {
                long index = row * Width + col;
                _cells[index] = value;
            }
        }
    }
}