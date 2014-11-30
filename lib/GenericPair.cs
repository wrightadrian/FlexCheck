
namespace lib {
    /// <summary>
    /// A generic Pair class.
    /// TODO: use a Tuple instead
    /// </summary>
    /// <typeparam name="L"></typeparam>
    /// <typeparam name="R"></typeparam>
    public class GenericPair<L, R> {
        public L Left { get; set; }
        public R Right { get; set; }

        public GenericPair(L left, R right) {
            this.Left = left;
            this.Right = right;
        }
    }
}