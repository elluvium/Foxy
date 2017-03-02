using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeLib
{
    public class Tree<T> : IEnumerable<TreeNode<T>>
    {
        private TreeNode<T> _root;

        public TreeNode<T> Root => _root;

        private HashSet<TreeNode<T>> _nodesHeap = new HashSet<TreeNode<T>>();

        public Tree(T rootValue)
        {
            _root = new TreeNode<T>(rootValue);
            _nodesHeap.Add(_root);
        }

        public TreeNode<T> Add(T value, TreeNode<T> ancestor)
        {
            if (!_nodesHeap.Contains(ancestor))
            {
                throw new InvalidOperationException();
            }
            var newNode = new TreeNode<T>(value);
            ancestor.descendants.Add(newNode);
            _nodesHeap.Add(newNode);
            return newNode;
        }

        public void Remove(TreeNode<T> node)
        {
            var ancestor = _nodesHeap.FirstOrDefault(x => x.Descendants.Contains(node));
            if (ancestor == null)
            {
                throw new InvalidOperationException();
            }
            ancestor.descendants.Remove(node);
            _nodesHeap.Remove(node);
            removeDescendants(node);
        }

        private void removeDescendants(TreeNode<T> node)
        {
            if (node.HasDescendants)
            {
                foreach (var descendant in node.Descendants)
                {
                    _nodesHeap.Remove(descendant);
                    removeDescendants(descendant);
                }
            }
        }

        public IEnumerator<TreeNode<T>> GetEnumerator() => _nodesHeap.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public T[] ToArrayByBreadthFirstIndexation() { throw new NotImplementedException(); }

    }
}
