namespace Data
{
    public class Goal
    {
        public uint Index { get; set; }
        public string Content { get; set; }

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
