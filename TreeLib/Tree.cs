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

        public virtual TreeNode<T> Add(T value, IList<TreeNode<T>> ancestors)
        {
            var newNode = new TreeNode<T>(value);
            foreach (var ancestor in ancestors)
            {
                if (!_nodesHeap.Contains(ancestor))
                {
                    throw new InvalidOperationException();
                }
                ancestor.descendants.Add(newNode);
                newNode.ancestors.Add(ancestor);
            }
            _nodesHeap.Add(newNode);
            return newNode;
        }

        public virtual void Remove(TreeNode<T> node)
        {
            foreach(var ancestor in node.ancestors)
            {
                ancestor.descendants.Remove(node);
            }
            _nodesHeap.Remove(node);
            removeDescendants(node);
        }

        private void removeDescendants(TreeNode<T> node)
        {
            if (node.HasDescendants)
            {
                foreach (var descendant in node.Descendants)
                {
                    if (descendant.Ancestors.Count() == 1)
                    {
                        _nodesHeap.Remove(descendant);
                        removeDescendants(descendant);
                    }
                    else
                    {
                        descendant.ancestors.Remove(node);
                    }
                }
            }
        }

        public IEnumerator<TreeNode<T>> GetEnumerator() => _nodesHeap.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public T[] ToArrayByDepthIndexation() => CollectByDepthIndexation(Root).ToArray();

        private HashSet<T> CollectByDepthIndexation(TreeNode<T> node)
        {
            var set = new HashSet<T>();
            set.Add(node.Value);
            foreach(var descendant in node.Descendants)
            {
                var nextLevel = CollectByDepthIndexation(descendant);
                foreach (var element in nextLevel)
                {
                    set.Add(element);
                }
            }
            return set;
        }

        public IEnumerable<TreeNode<T>> GetByTheSameLevel(int level) => Root.GetSameLevelNodes(level);

    }
}
