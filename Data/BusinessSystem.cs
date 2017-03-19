using System;
using System.Collections.Generic;

namespace Data
{
    using Matrixes;
    using BusinessStructures;

    [Serializable]
    public class BusinessSystem
    {
        public string Title { get; set; }
        public string Vision { get; set; }
        public string Mission { get; set; }
        public string GlobalGoal { get; set; }

        public List<string> KeyAreas { get; set; } //?

        public Scope FunctionalAreas { get; set; }
        public Scope AmbientAreas { get; set; }



        public IncidenceMatrix<Goal> GoalsIncidenceMatrix { get; set; }
    }
}
