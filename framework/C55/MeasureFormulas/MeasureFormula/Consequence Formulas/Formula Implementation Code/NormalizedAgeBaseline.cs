using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using MeasureFormula.Common_Code;
using MeasureFormula.SharedCode;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class NormalizedAgeBaseline : NormalizedAgeBaselineBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            // Cannot determine normalized age without the asset in-service date, or the asset useful lifetime years.
            if (!timeInvariantData.AssetInServiceDate.HasValue
                || !timeInvariantData.AssetUsefulLifetimeYears.HasValue
                || timeInvariantData.AssetUsefulLifetimeYears.Value == 0) // lifetime years cannot be zero
            {
                return null;
            }

            var conditionScoreAnswers = timeVariantData.Select(x => new ConditionScoreAnswerDto
            {
                Date = x.TimePeriod.StartTime,
                ConditionScore = x.BaseCondition
            }).ToList();

            return HelperUtility.CalculateNormalizedAgeBaseline(startFiscalYear, months, timeInvariantData.AssetInServiceDate.Value,
                timeInvariantData.AssetUsefulLifetimeYears.Value, conditionScoreAnswers,
                timeInvariantData.SystemCondition_32_Score_32_Best);
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
