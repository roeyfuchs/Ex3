using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ex3.Models
{
    public class SamplingData
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="amountOfSamplingDesired"></param>
        public SamplingData(int amountOfSamplingDesired)
        {
            this.amountOfSamplingDesired = amountOfSamplingDesired;
            samplingSoFar = 0;
        }

        private int samplingSoFar;
        private int amountOfSamplingDesired;
        /// <summary>
        /// count sampling
        /// </summary>
        /// <returns>true for continue sampling data and false to stop sampling process</returns>
        public bool Sample()
        {
            ++samplingSoFar;
            if (samplingSoFar <= amountOfSamplingDesired)
            {
                return true;
            }
            return false;
        }
    }
}