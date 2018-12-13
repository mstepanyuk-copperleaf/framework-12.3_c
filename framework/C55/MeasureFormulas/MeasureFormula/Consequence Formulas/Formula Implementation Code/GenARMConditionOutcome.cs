using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class GenARMConditionOutcome : GenARMConditionOutcomeBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            return MonthlyConditionScores<TimeVariantInputDTO>(
                   startFiscalYear,
                   months,
                   null,
                   timeVariantData,
                   timeInvariantData.AssetConditionDecayCurve,
                   x => x.ImpactCondition);
        }

        public override double?[] GetZynos(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData,
            IReadOnlyList<TimeVariantInputDTO> timeVariantData,
            double?[] unitOutput)
        {
            return null;
        }
    }
}
