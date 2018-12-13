using System;
using System.Collections.Generic;
using System.Linq;
using CL.FormulaHelper.Attributes;
using MeasureFormula.Common_Code;
using MeasureFormula.SharedCode;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class NormalizedAgeOutcome : NormalizedAgeOutcomeBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            // Cannot determine normalized age without the asset useful lifetime years.
            if (!timeInvariantData.AssetUsefulLifetimeYears.HasValue
                || timeInvariantData.AssetUsefulLifetimeYears.Value == 0 // lifetime years zero means age won't degrade
                || timeInvariantData.SystemCondition_32_Score_32_Best == null) // can't recognize when to reset age
            {
                return null;
            }

            var conditionScoreAnswers = timeVariantData.Select(x => new ConditionScoreAnswerDto
            {
                Date = x.TimePeriod.StartTime,
                ConditionScore = x.ImpactCondition
            }).ToList();

            var bestScoreAnswers = conditionScoreAnswers.Where(x => Math.Abs(x.ConditionScore - timeInvariantData.SystemCondition_32_Score_32_Best.Value) < 1e-6)
                .ToList();

            if (bestScoreAnswers.Count <= 0) return null;
            
            return HelperUtility.CalculateNormalizedAgeOutcome(startFiscalYear, months, bestScoreAnswers[0].Date,
                timeInvariantData.AssetUsefulLifetimeYears.Value, bestScoreAnswers,
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
