using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BusinessStructures
{
    using Matrixes;

    public class Area
    {
        public string Name { get; set; }

        protected IEnumerable<Aspect> pros => prosMatrix.Variables;

        protected IEnumerable<Aspect> cons => consMatrix.Variables;

        public PairwiseComparisonsMatrix<Aspect> prosMatrix { get; private set; }
        public PairwiseComparisonsMatrix<Aspect> consMatrix { get; private set; }

        internal IDictionary<Side, PairwiseComparisonsMatrix<Aspect>> matrixes
        {
            get
            {
                var result = new Dictionary<Side, PairwiseComparisonsMatrix<Aspect>>();
                result.Add(Side.Strong, prosMatrix);
                result.Add(Side.Weak, consMatrix);
                return result;
            }
        }


        public void AddProsAspect(Aspect aspect)
        {
            prosMatrix.AddVariable(aspect);
        }
        public void AddConsAspect(Aspect aspect)
        {
            consMatrix.AddVariable(aspect);
        }

        public void RemoveProsAspect(Aspect aspect)
        {
            consMatrix.RemoveVariable(aspect);
        }
        public void RemoveConsAspect(Aspect aspect)
        {
            consMatrix.RemoveVariable(aspect);
        }


    }
}
