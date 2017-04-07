using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BusinessStructures
{
    using Matrixes;

    [Serializable]
    public class Area
    {
        public string Name { get; set; } = string.Empty;

        protected Dictionary<Side, HashSet<Aspect>> aspects = new Dictionary<Side, HashSet<Aspect>>();

        public Dictionary<Side, IEnumerable<Aspect>> Aspects => aspects.ToDictionary(x => x.Key, y => (IEnumerable<Aspect>)y.Value);

        public Dictionary<Side, NamedSquareMatrix<Aspect, double>> GeneralisedComparisons => ExpertsComparisons.ToDictionary(x => x.Key, y => AgregateExpertMatrixes(y.Value, Aspects[y.Key]));

        private Dictionary<Side, List<PairwiseComparisonsMatrix<Aspect>>> _expertsComparisons = new Dictionary<Side, List<PairwiseComparisonsMatrix<Aspect>>>();

        public Dictionary<Side, IEnumerable<PairwiseComparisonsMatrix<Aspect>>> ExpertsComparisons => _expertsComparisons.ToDictionary(x => x.Key, y => (IEnumerable<PairwiseComparisonsMatrix<Aspect>>) y.Value);


        public Area()
        {
            aspects.Add(Side.Strong, new HashSet<Aspect>());
            aspects.Add(Side.Weak, new HashSet<Aspect>());
            _expertsComparisons.Add(Side.Strong, new List<PairwiseComparisonsMatrix<Aspect>>());
            _expertsComparisons.Add(Side.Weak, new List<PairwiseComparisonsMatrix<Aspect>>());
        }

        public void AddAspect(Aspect aspect, Side side)
        {
            aspects[side].Add(aspect);
            foreach(var matrix in _expertsComparisons[side])
            {
                matrix.AddVariable(aspect);
            }
        }

        public void RemoveAspect(Aspect aspect, Side side)
        {
            aspects[side].Remove(aspect);
            foreach (var matrix in _expertsComparisons[side])
            {
                matrix.RemoveVariable(aspect);
            }
        }

        public void AddExpertOpinion(Side side)
        {
            _expertsComparisons[side].Add(new PairwiseComparisonsMatrix<Aspect>(aspects[side]));
        }


        public void RemoveExpertOpinion(PairwiseComparisonsMatrix<Aspect> matrix, Side side)
        {
            _expertsComparisons[side].Remove(matrix);
        }

        private static NamedSquareMatrix<Aspect, double> AgregateExpertMatrixes(IEnumerable<PairwiseComparisonsMatrix<Aspect>> matrixes, IEnumerable<Aspect> aspects)
        {
            double k = matrixes.Count();
            NamedSquareMatrix<Aspect, double> result = new NamedSquareMatrix<Aspect, double>(aspects, 1.0);
            foreach (var aspect1 in aspects)
            {
                foreach(var aspect2 in aspects)
                {
                    foreach(var matrix in matrixes)
                    {
                        result[aspect1, aspect2] *= matrix[aspect1, aspect2];
                    }
                    result[aspect1, aspect2] = Math.Pow(result[aspect1, aspect2], 1.0 / k);
                }
            }
            return result;            
        }

    }
}
