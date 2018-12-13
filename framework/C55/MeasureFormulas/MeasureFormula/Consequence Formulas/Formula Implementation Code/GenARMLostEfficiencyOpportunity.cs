using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class GenARMLostEfficiencyOpportunity : GenARMLostEfficiencyOpportunityBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            var ad = timeInvariantData.GenARM_TurbineAgeDegradation_ConsqUnitOutput;
            var ti = timeInvariantData.GenARM_TurbineTechImprovement_ConsqUnitOutput;
            //----- If we have both, then combine them
            if (ad != null && ti != null)
            {
                double?[] ret = new double?[months];
                for (int i = 0; i < months; i++)
                {
                    if (i < ad.Length && ad[i].HasValue && i < ti.Length && ti[i].HasValue)
                    {
                        ret[i] = ad[i] + ti[i];
                    }
                    else if (i < ad.Length && ad[i].HasValue)
                    {
                        ret[i] = ad[i];
                    }
                    else if (i < ti.Length && ti[i].HasValue)
                    {
                        ret[i] = ti[i];
                    }
                }
                return ret;
            }
            //----- Else, return the one that isn't null 
            return (ad != null) ? ad : ti;
        }

        public override double?[] GetZynos(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData,
            IReadOnlyList<TimeVariantInputDTO> timeVariantData,
            double?[] unitOutput)
        {
            return unitOutput;
        }
    }
}
