using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class GenARMProbability : GenARMProbabilityBase
    {
        public override double?[] GetLikelihoodValues(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            return PdfValuesWithSubsampling(
                startFiscalYear,
                months,
                0,
                timeInvariantData.GenARM_Condition_ConsqUnitOutput,
                timeInvariantData.SystemCondition_32_Score_32_Best,
                timeInvariantData.SystemCondition_32_Score_32_Worst,
                timeInvariantData.AssetConditionDecayCurve,
                timeInvariantData.ConditionToFailureCurve,
                (int)(timeInvariantData.SystemNumber_32_of_32_baseline_32_PDF_32_subsamples ?? 0));
        }
    }
}
