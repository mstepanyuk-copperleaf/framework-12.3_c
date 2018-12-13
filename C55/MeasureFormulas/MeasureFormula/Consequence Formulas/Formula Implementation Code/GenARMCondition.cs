using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class GenARMCondition : GenARMConditionBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            return MonthlyConditionScores<TimeVariantInputDTO>(
                   startFiscalYear,
                   months,
                   timeInvariantData.AssetInServiceDate,
                   timeVariantData,
                   timeInvariantData.AssetConditionDecayCurve,
                   x => x.BaseCondition);
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
