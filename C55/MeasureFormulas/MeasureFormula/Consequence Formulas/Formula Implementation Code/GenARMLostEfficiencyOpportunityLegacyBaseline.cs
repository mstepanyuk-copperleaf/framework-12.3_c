using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormulas.Generated_Formula_Base_Classes;
using MeasureFormula.Common_Code;

namespace CustomerFormulaCode
{
    [Formula]
    public class GenARMLostEfficiencyOpportunityLegacyBaseline : GenARMLostEfficiencyOpportunityLegacyBaselineBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            if (timeInvariantData.SystemCondition_32_Score_32_Best == null) return null;

            var inServiceOffset = HelperUtility.LastInServiceMonthOffset(timeInvariantData.GenARM_Condition_ConsqUnitOutput_B,
                timeInvariantData.AssetInServiceDate,
                startFiscalYear, timeInvariantData.SystemCondition_32_Score_32_Best.Value);
            if (!inServiceOffset.HasValue) return null;
            
            var startOffset = inServiceOffset.Value;
            var energyValues = timeInvariantData.AnalyticsStrategyAlternativeEnergyValues ?? timeInvariantData.SystemEnergyValues;
            var assetAvoidedCO2DollarsPerMWh = timeInvariantData.AnalyticsStrategyAlternativeAvoidedCO2Values ?? timeInvariantData.SystemAvoidedCO2Values;

            return HelperUtility.CalculateLostEfficiencyOpportunity(startFiscalYear, months,
                timeInvariantData.AssetAnnual_32_Degradation_32__37_, timeInvariantData.AssetTechnology_32_Improvement_32__37_,
                timeInvariantData.SystemCondition_32_Score_32_Best, timeInvariantData.AssetGenerationGroup,
                energyValues, assetAvoidedCO2DollarsPerMWh, timeInvariantData.GenARM_Condition_ConsqUnitOutput_B,
                startOffset, endOffset: null);
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
