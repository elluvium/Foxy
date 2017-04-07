using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BusinessStructures
{

    [Serializable]
    public class Scope
    {
        private HashSet<Area> _areas;
        public IEnumerable<Area> Areas => _areas;
        public int Count => _areas.Count;

        public Scope()
        {
            _areas = new HashSet<Area>();
        }

        public Scope(HashSet<Area> areas)
        {
            _areas = areas;
        }

        public void Add(Area area)
        {
            _areas.Add(area);
        }

        public void Remove(Area area)
        {
            _areas.Remove(area);
        }

        public IDictionary<Area, double> CalculateLocalStrongPrioritiesOfAreas()
        {
            return CalculateLocalPrioritiesOfAreas(Side.Strong);
        }

        public IDictionary<Area, double> CalculateLocalWeakPrioritiesOfAreas()
        {
            return CalculateLocalPrioritiesOfAreas(Side.Weak);
        }

        public static IDictionary<Aspect, double> CalculateLocalPriorities(Matrixes.NamedSquareMatrix<Aspect,double> matrix)
        {
            var variables = matrix.Variables;
            var result = new Dictionary<Aspect, double>(variables.Count());

            int length = variables.Length;
            double sum = 0;
            foreach (var variableRow in variables)
            {
                double prod = 1.0;
                foreach (var variableColumn in variables)
                {
                    prod *= Math.Pow(matrix[variableRow, variableColumn], 1.0 / length);
                }
                result.Add(variableRow, prod);
                sum += prod;
            }
            foreach (var variable in variables)
            {
                result[variable] /= sum;
            }
            return result;
        }

        public IDictionary<Area, double> CalculateLocalPrioritiesOfAreas(Side side)
        {
            var result = new Dictionary<Area, double>(Count);
            double sum = 0;
            foreach (var area in _areas)
            {
                double prod = 1.0;
                var prosPriorities = CalculateLocalPriorities(area.GeneralisedComparisons[side]);
                foreach (var aspect in prosPriorities.Keys)
                {
                    prod *= Math.Pow(prosPriorities[aspect], 1.0 / prosPriorities.Count);
                }
                result.Add(area, prod);
                sum += prod;
            }
            foreach (var area in _areas)
            {
                result[area] /= sum;
            }
            return result;
        }

        public IDictionary<Area, IDictionary<Aspect, double>> CalculateGlobalPrioritiesOfAspectsByAreas(Side side)
        {
            var localAreaPriorities = CalculateLocalPrioritiesOfAreas(side);
            var globalAspectPriorities = new Dictionary<Area, IDictionary<Aspect, double>>();
            foreach (var area in _areas)
            {
                var globalAspectAreaPriorities = CalculateLocalPriorities(area.GeneralisedComparisons[side]);
                foreach (var aspect in globalAspectAreaPriorities.Keys)
                {
                    globalAspectAreaPriorities[aspect] *= localAreaPriorities[area];
                }
                globalAspectPriorities.Add(area, globalAspectAreaPriorities);
            }
            return globalAspectPriorities;
        }


        public static IDictionary<Aspect, double> CalculateGlobalPrioritiesOfAspects(IDictionary<Area, IDictionary<Aspect, double>> globalAspectPrioritiesByAreas)
        {
            Dictionary<Aspect, double> globalAspectPriorities = new Dictionary<Aspect, double>();
            foreach (var areaAspect in globalAspectPrioritiesByAreas)
            {
                globalAspectPriorities.Union(areaAspect.Value);
            }
            return globalAspectPriorities;
        }

        public static IDictionary<Aspect, double> ExtractTheMostValueableGlobalPrioritiesOfAspects(IDictionary<Aspect, double> globalPrioritiesOfAspects, double border = 0.8)
        {
            var globalAspectPriorities = globalPrioritiesOfAspects.OrderByDescending(x => x.Value);
            var result = new Dictionary<Aspect, double>();
            double sum = 0;
            foreach (var aspect in globalAspectPriorities)
            {
                result.Add(aspect.Key, aspect.Value);
                sum += aspect.Value;
                if (sum > border)
                {
                    break;
                }
            }
            return result;

        }

        public static double CalculateStrategicEstimation(IDictionary<Aspect, double> theMostValuableGlobalPrioritiesOfAspects)
        {
            double sum = theMostValuableGlobalPrioritiesOfAspects.Sum(x => x.Value);
            return theMostValuableGlobalPrioritiesOfAspects.Select(aspect => aspect.Value / sum
                                                    * (aspect.Key.Intensity.NominalValue - aspect.Key.Intensity.LowerBorder)
                                                    / (aspect.Key.Intensity.UpperBorder - aspect.Key.Intensity.LowerBorder))
                                                    .Sum();
        }

        public static double CalculateSystemParameter(double positiveStrategicEstimation, double negativeStrategicEstimation)
        {
            return Math.Sqrt(positiveStrategicEstimation * negativeStrategicEstimation);
        }

    }
}
