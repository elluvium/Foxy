﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BusinessStructures
{
    [Serializable]
    public class Aspect
    {
        public string Name { get; set; }

        public Intensity Intensity { get; set; }

        public Aspect(string name, Intensity intensity)
        {
            Name = name;
            Intensity = intensity;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
