using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormula.Common_Code;
using MeasureFormula.SharedCode;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class GenARMAvoidedCarbonEmissions : GenARMAvoidedCarbonEmissionsBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            if (!(timeInvariantData.AssetContributes_32_to_32_Lost_32_Generation ?? false))
            {
                return null;
            }

            var baselineMonthlyProbabilities = ConvertConditionToMonthlyProbability(
                    timeInvariantData.GenARM_Condition_ConsqUnitOutput_B,
                    timeInvariantData.ConditionToFailureCurve,
                    treatProbabilityAsFrequency: true);

            var impactMonthlyProbabilities = ConvertConditionToMonthlyProbability(
                    timeInvariantData.GenARM_Condition_ConsqUnitOutput,
                    timeInvariantData.ConditionToFailureCurve,
                    treatProbabilityAsFrequency: true);

            // Need both baseline and impact probabilities to determine the avoided carbon emissions benefit.
            if (baselineMonthlyProbabilities == null || impactMonthlyProbabilities == null) return null;

            var downtimeInWeeks = (timeInvariantData.AssetIsSpareAvailable ?? false)
                            ? (timeInvariantData.AssetTypeDowntimeWeeksWithSpare ?? 0d)
                            : (timeInvariantData.AssetTypeDowntimeWeeksWithoutSpare ?? 0d);

            // Need downtime when a failure occurs to calculate the amount of MWh that will be lost due to a failure.
            if (downtimeInWeeks <= 0d) return null;

            var annualDowntime = downtimeInWeeks / CommonConstants.WeeksPerYear;

            // Need lost capacity associated with a generation group when a failure occurs 
            // to calculate the amount of MWh that will be lost due to a failure.
            if (timeInvariantData.AssetGenerationGroup == null) return null;
            if (timeInvariantData.AssetGenerationGroup.Loss == null || timeInvariantData.AssetGenerationGroup.Loss.Count == 0) return null;

            // Determine the amount of MWh that will be lost due to a failure.  Only use the first entry
            // in the Loss array for now - this is how all analytics customers are currently calculating
            // generation loss.
            var weightedAnnualValueMWh = annualDowntime * timeInvariantData.AssetGenerationGroup.Loss[0];
            var assetAvoidedCO2ValuesDollarsPerMWh = timeInvariantData.AnalyticsStrategyAlternativeAvoidedCO2Values ?? timeInvariantData.SystemAvoidedCO2Values;

            var ret = new double?[months];
            for (var i = 0; i < months; i++)
            {
                var mitigatedMonthlyProbabilities = baselineMonthlyProbabilities[i] - impactMonthlyProbabilities[i];

                var avoidedCO2InDollarsPerMWh = HelperUtility.AvoidedCo2InDollarsPerMWh(startFiscalYear, assetAvoidedCO2ValuesDollarsPerMWh, i);                   
                ret[i] = weightedAnnualValueMWh * avoidedCO2InDollarsPerMWh * mitigatedMonthlyProbabilities;
            }

            return ret;
        }

        public override double?[] GetZynos(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData,
            IReadOnlyList<TimeVariantInputDTO> timeVariantData,
            double?[] unitOutput)
        {
            if (timeInvariantData.IgnoreLGR ?? false) return null;

            return unitOutput;
        }
    }
}
