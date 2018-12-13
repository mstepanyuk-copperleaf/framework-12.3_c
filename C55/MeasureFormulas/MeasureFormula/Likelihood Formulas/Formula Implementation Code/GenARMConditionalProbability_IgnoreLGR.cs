using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class GenARMConditionalProbability_IgnoreLGR : GenARMConditionalProbability_IgnoreLGRBase
    {
        public override double?[] GetLikelihoodValues(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            //do nothing if IgnoreDCR parameter is true
            if (timeInvariantData.IgnoreLGR ?? false)
            {
                return null;
            }

            return ConvertConditionToMonthlyProbability(
                timeInvariantData.GenARM_Condition_ConsqUnitOutput,
                timeInvariantData.ConditionToFailureCurve,
                treatProbabilityAsFrequency: true);
        }
    }
}
