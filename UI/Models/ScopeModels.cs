using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.BusinessStructures;

namespace UI.Models
{
    public class AreaModel
    {
        internal Area _area;

        public double StrongPriority { get; set; } = 0;
        public double WeakPriority { get; set; } = 0;

        public string Name => _area.Name;

        public AreaModel(Area area)
        {
            _area = area;
        }

        public static implicit operator Area(AreaModel areaModel)
        {
            return areaModel._area;
        }
    }

    public class AspectModel
    {
        internal Aspect _aspect;

        public double Priority { get; }

        public string Name => _aspect.Name;

        public AspectModel(Aspect aspect, double priority)
        {
            _aspect = aspect;
            Priority = priority;
        }

        public static implicit operator Aspect(AspectModel areaModel)
        {
            return areaModel._aspect;
        }
    }

    public class AspectModelByArea : AspectModel
    {
        public Area Area { get; }

        public AspectModelByArea(Aspect aspect, Area area, double priority) : base(aspect, priority)
        {
            Area = area;
        }
    }


}
