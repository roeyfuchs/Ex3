using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ex3.Models
{
    public class SamplingData
    {
        public SamplingData(int amountOfSamplingDesired)
        {
            this.amountOfSamplingDesired = amountOfSamplingDesired;
            samplingSoFar = 0;
        }

        private int samplingSoFar;
        private int amountOfSamplingDesired;
        public bool Sample()
        {
            ++samplingSoFar;
            if (samplingSoFar < amountOfSamplingDesired)
            {
                return true;
            }
            return false;
        }
    }
}