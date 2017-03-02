using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeLib
{
    public class TreeNode<T>
    {
        public T Value { get; set; }

        internal HashSet<TreeNode<T>> ancestors = new HashSet<TreeNode<T>>();

        internal HashSet<TreeNode<T>> descendants = new HashSet<TreeNode<T>>();

        public IEnumerable<TreeNode<T>> Ancestors => ancestors;

        public IEnumerable<TreeNode<T>> Descendants => descendants;

        public bool HasDescendants => descendants != null && descendants.Count() != 0;

        public TreeNode()
        {
        }

        public TreeNode(T value) 
        {
            Value = value;
        }

        internal IEnumerable<TreeNode<T>> GetLeafs()
        {
            List<TreeNode<T>> result = new List<TreeNode<T>>();
            foreach (var descendant in Descendants)
            {
                if (!descendant.HasDescendants)
                {
                    result.Add(descendant);
                }
                else
                {
                    result.AddRange(descendant.GetLeafs());
                }
            }
            return result;
        }

        internal HashSet<TreeNode<T>> GetSameLevelNodes(int level)
        {
            if (level == 0)
            {
                return new HashSet<TreeNode<T>>() { this };
            }
            var result = new HashSet<TreeNode<T>>();
            foreach (var descendant in Descendants)
            {
                result.UnionWith(descendant.GetSameLevelNodes(level - 1));
            }
            return result;
        }

    }
}
