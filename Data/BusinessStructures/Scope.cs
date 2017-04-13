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


        private static IDictionary<Aspect, double> CalculateLocalPrioritiesOfAspectsOfArea(Matrixes.NamedSquareMatrix<Aspect,double> matrix)
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


        public static IDictionary<Area, IDictionary<Aspect, double>> CalculateLocalPrioritiesOfAspectsByAreas(IEnumerable<Area> areas, Side side)
        {
            return areas.ToDictionary(x => x, y => CalculateLocalPrioritiesOfAspectsOfArea(y.GeneralisedComparisons[side]));
        }



        public static IDictionary<Area, double> CalculatePrioritiesOfAreas(IDictionary<Area, IDictionary<Aspect, double>> localAspectPrioritiesByAreas, Side side)
        {
            var result = new Dictionary<Area, double>(localAspectPrioritiesByAreas.Count());
            double sum = 0;
            foreach (var area in localAspectPrioritiesByAreas)
            {
                double prod = 1.0;
                foreach (var aspect in area.Key.Aspects[side])
                {
                    prod *= Math.Pow(area.Value[aspect], 1.0 / area.Value.Count);
                }
                result.Add(area.Key, prod);
                sum += prod;
            }
            foreach (var area in localAspectPrioritiesByAreas)
            {
                result[area.Key] /= sum;
            }
            return result;
        }

        public static IDictionary<Area, IDictionary<Aspect, double>> CalculateGlobalPrioritiesOfAspectsByAreas
            (IDictionary<Area, double> prioritiesOfAreas,
            IDictionary<Area, IDictionary<Aspect, double>> localAspectPrioritiesByAreas,
            Side side)
        {
            var globalAspectPrioritiesByAreas = new Dictionary<Area, IDictionary<Aspect, double>>();
            foreach (var area in prioritiesOfAreas.Keys)
            {
                var globalAspectAreaPriorities = localAspectPrioritiesByAreas[area].ToDictionary(x => x.Key, y => y.Value);
                IEnumerable<Aspect> aspects = globalAspectAreaPriorities.Keys.ToList();
                foreach (var aspect in aspects)
                {
                    globalAspectAreaPriorities[aspect] *= prioritiesOfAreas[area];
                }
                globalAspectPrioritiesByAreas.Add(area, globalAspectAreaPriorities);
            }
            return globalAspectPrioritiesByAreas;
        }


        public static IDictionary<Aspect, double> CalculateGlobalPrioritiesOfAspects(IDictionary<Area, IDictionary<Aspect, double>> globalAspectPrioritiesByAreas)
        {
            Dictionary<Aspect, double> globalAspectPriorities = new Dictionary<Aspect, double>();
            foreach (var areaAspect in globalAspectPrioritiesByAreas)
            {
                globalAspectPriorities = globalAspectPriorities.Union(areaAspect.Value).ToDictionary(X => X.Key, Y => Y.Value);
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
                                                    * (aspect.Key.Intensity.NominalValue - aspect.Key.Intensity.LowerBound)
                                                    / (aspect.Key.Intensity.UpperBound - aspect.Key.Intensity.LowerBound))
                                                    .Sum();
        }

        public static double CalculateSystemParameter(double positiveStrategicEstimation, double negativeStrategicEstimation)
        {
            return Math.Sqrt(positiveStrategicEstimation * negativeStrategicEstimation);
        }

    }
}
