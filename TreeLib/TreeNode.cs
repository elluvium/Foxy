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

        internal TreeNode<T> ancestor;

        internal HashSet<TreeNode<T>> descendants = new HashSet<TreeNode<T>>();

        public TreeNode<T> Ancestor => ancestor;

        public HashSet<TreeNode<T>> Descendants => descendants;

        public bool HasDescendants => descendants != null && descendants.Count() != 0;

        public TreeNode(TreeNode<T> ancestor)
        {
            this.ancestor = ancestor;
        }

        public TreeNode(T value, TreeNode<T> ancestor) : this(ancestor)
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
    }
}
