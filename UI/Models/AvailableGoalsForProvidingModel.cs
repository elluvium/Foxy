using System;
using System.Collections.Generic;
using System.Linq;
using Data.BusinessStructures;

namespace UI.Models
{
    public abstract class AbstractGoalModel
    {
        private Goal _goal;

        public uint Index => _goal.Index;
        public string Content => _goal.Content;

        public AbstractGoalModel(Goal goal)
        {
            _goal = goal;
        }

        public Goal ToGoal() => _goal;
    }


    public class GoalModel : AbstractGoalModel
    {
        public bool ProvidedBy { get; set; }

        public GoalModel(Goal goal, bool isProvided) : base(goal)
        {
            ProvidedBy = isProvided;
        }
    }

    public class GoalModelWithProvidings : AbstractGoalModel
    {
        public IEnumerable<Goal> UpperGoals { get; private set; }

        public string ProvidesFor => string.Join(", ", UpperGoals.Select(x => x.Index.ToString()));

        public GoalModelWithProvidings(Goal goal, IEnumerable<Goal> upperGoals) : base(goal)
        {
            UpperGoals = upperGoals;
        }
    }


}
