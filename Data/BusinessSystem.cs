using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class BusinessSystem
    {
        public string Title { get; set; }
        public string Vision { get; set; }
        public string Mission { get; set; }
        public string GlobalGoal { get; set; }

        public List<string> FunctionalZones { get; set; }
        public List<string> KeyAreas { get; set; }

        public IncidenceMatrix<Goal> GoalsIncidenceMatrix { get; set; }
    }
}
