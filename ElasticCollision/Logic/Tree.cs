using ElasticCollision.Data;
using System.Collections.Generic;
using System.Linq;

/// allows for efficient checking of possibly overlapping balls
namespace ElasticCollision.Logic
{
    public interface IBallContainer
    {
        public void Insert(Ball b);
        public List<Ball> Neighbors(Ball b);
    }

    public abstract class Tree : IBallContainer
    {
        public IBallContainer Container { get; protected set; }
        public Tree A { get; private set; }
        public Tree B { get; private set; }
        public ISection Basis { get; private set; }
        public bool Initialized { get; private set; } = false;


        public Tree(ISection basis) => Basis = basis;

        private void MakeChildren()
        {
            if (!Initialized)
            {
                var (a, b) = Basis.SplitSection();
                A = CreateChild(a);
                B = CreateChild(b);
                Initialized = true;
            }
        }

        protected abstract Tree CreateChild(ISection s);

        public void Insert(Ball ball)
        {
            MakeChildren();
            if (A.Basis.FullyContains(ball)) { A.Insert(ball); }
            else if (B.Basis.FullyContains(ball)) { B.Insert(ball); }
            else { Container.Insert(ball); }
        }

        public List<Ball> Neighbors(Ball ball)
        {
            if (!Basis.Intersects(ball)) { return new List<Ball>(); }
            else if (Initialized)
            {
                return Container.Neighbors(ball)
                    .Concat(A.Neighbors(ball))
                    .Concat(B.Neighbors(ball))
                    .ToList();
            }
            else { return Container.Neighbors(ball); }
        }
    }

    // one-dimensional tree, uses List as storage
    public class BinaryTree : Tree
    {
        // just a list wrapper
        private class BallList : IBallContainer
        {
            private List<Ball> _lst;
            public BallList() => _lst = new List<Ball>();
            public void Insert(Ball b) => _lst.Add(b);
            public List<Ball> Neighbors(Ball b) => _lst;
        }

        public BinaryTree(ISection s) : base(s) => Container = new BallList();
        protected override Tree CreateChild(ISection s) => new BinaryTree(s);
    }

    // two-dimensional tree, uses BinaryTree as storage
    public class NonBinaryTree : Tree
    {
        public NonBinaryTree(ISection s) : base(s)
        {
            var a = (Area)s;
            Container = new BinaryTree(a.ShorterInterval());
        }

        protected override Tree CreateChild(ISection s) => new NonBinaryTree(s);

        public static NonBinaryTree MakeTree(Area area, IEnumerable<Ball> balls)
        {
            var tree = new NonBinaryTree(area);
            foreach (var ball in balls) { tree.Insert(ball); }
            return tree;
        }
    }
}
