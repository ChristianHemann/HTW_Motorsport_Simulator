using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace CalculationComponents.TrackComponents
{
    public class StartLine : TrackSegment
    {
        public StartLine(TrackSegment previousTrackSegment, float trackWidthEnd, Vector<float> endPoint, Vector<float> endDirection) 
            : base(previousTrackSegment, trackWidthEnd, endPoint, endDirection)
        {
            
        }
    }
}
