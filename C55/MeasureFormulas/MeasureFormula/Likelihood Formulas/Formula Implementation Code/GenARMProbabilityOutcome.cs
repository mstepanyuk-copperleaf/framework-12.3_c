using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class GenARMProbabilityOutcome : GenARMProbabilityOutcomeBase
    {
        public override double?[] GetLikelihoodValues(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            return MonthlyImpactProbabilities(
                 timeInvariantData.GenARM_Condition_ConsqUnitOutput_B,
                 timeInvariantData.ConditionToFailureCurve,
                 timeInvariantData.GenARM_Condition_ConsqUnitOutput,
                 timeInvariantData.ConditionToFailureCurve,
                 timeInvariantData.SystemCondition_32_Score_32_Best,
                 timeInvariantData.SystemCondition_32_Score_32_Worst);
        }
    }
}
