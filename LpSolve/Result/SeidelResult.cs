using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LpSolve.Elements;

namespace LpSolve.Result
{
    /// <summary>
    /// Base class for LP result
    /// </summary>
    public abstract class SeidelResult
    {
        public Point Point { get; protected set; }

        public abstract SeidelResult Resolve(HalfSpace halfSpace);
    }
}
