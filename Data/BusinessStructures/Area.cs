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
        public string Name { get; set; }

        protected HashSet<Aspect> prosAspects = new HashSet<Aspect>();
        protected HashSet<Aspect> consAspects = new HashSet<Aspect>();

        protected IEnumerable<Aspect> pros => ProsMatrix.Variables;

        protected IEnumerable<Aspect> cons => ConsMatrix.Variables;

        public NamedSquareMatrix<Aspect, double> ProsMatrix => AgregateExpertMatrixes(ExpertsProsMatrixes, pros);
        public NamedSquareMatrix<Aspect, double> ConsMatrix => AgregateExpertMatrixes(ExpertsProsMatrixes, pros);

        private IList<PairwiseComparisonsMatrix<Aspect>> _expertsProsMatrixes = new List<PairwiseComparisonsMatrix<Aspect>>();
        private IList<PairwiseComparisonsMatrix<Aspect>> _expertsConsMatrixes = new List<PairwiseComparisonsMatrix<Aspect>>();

        public IEnumerable<PairwiseComparisonsMatrix<Aspect>> ExpertsProsMatrixes => _expertsProsMatrixes;
        public IEnumerable<PairwiseComparisonsMatrix<Aspect>> ExpertsConsMatrixes => _expertsConsMatrixes;

        internal IDictionary<Side, NamedSquareMatrix<Aspect, double>> matrixes
        {
            get
            {
                var result = new Dictionary<Side, NamedSquareMatrix<Aspect, double>>();
                result.Add(Side.Strong, ProsMatrix);
                result.Add(Side.Weak, ConsMatrix);
                return result;
            }
        }


        public void AddProsAspect(Aspect aspect)
        {
            prosAspects.Add(aspect);
            foreach(var matrix in ExpertsProsMatrixes)
            {
                matrix.AddVariable(aspect);
            }
        }
        public void AddConsAspect(Aspect aspect)
        {
            consAspects.Add(aspect);
            foreach (var matrix in ExpertsConsMatrixes)
            {
                matrix.AddVariable(aspect);
            }
        }

        public void RemoveProsAspect(Aspect aspect)
        {
            prosAspects.Remove(aspect);
            foreach (var matrix in ExpertsProsMatrixes)
            {
                matrix.RemoveVariable(aspect);
            }
        }
        public void RemoveConsAspect(Aspect aspect)
        {
            consAspects.Remove(aspect);
            foreach (var matrix in ExpertsConsMatrixes)
            {
                matrix.RemoveVariable(aspect);
            }
        }

        public void AddExpertOpinionAboutPros()
        {
            _expertsProsMatrixes.Add(new PairwiseComparisonsMatrix<Aspect>(prosAspects));
        }

        public void AddExpertOpinionAboutCons()
        {
            _expertsConsMatrixes.Add(new PairwiseComparisonsMatrix<Aspect>(consAspects));
        }

        public void RemoveExpertOpinionAboutPros(PairwiseComparisonsMatrix<Aspect> matrix)
        {
            _expertsProsMatrixes.Remove(matrix);
        }

        public void RemoveExpertOpinionAboutCons(PairwiseComparisonsMatrix<Aspect> matrix)
        {
            _expertsConsMatrixes.Remove(matrix);
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
