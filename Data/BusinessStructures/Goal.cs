using System;

namespace Data.BusinessStructures
{
    [Serializable]
    public class Goal
    {
        private string _content = string.Empty;

        public uint Index { get; set; }
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                if(value != null)
                {
                    _content = value;
                }
            }
        }

        public override bool Equals(object obj)
        {
            var goal = obj as Goal;
            return goal == null
                ?
                false
                :
                goal.Index == Index && goal.Content == Content;
        }

        public override int GetHashCode()
        {
            return Index.GetHashCode() ^ Content.GetHashCode();
        }


    }
}
