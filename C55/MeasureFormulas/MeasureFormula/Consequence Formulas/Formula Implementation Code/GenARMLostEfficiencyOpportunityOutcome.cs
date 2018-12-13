using System;
using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormula.Common_Code;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class GenARMLostEfficiencyOpportunityOutcome : GenARMLostEfficiencyOpportunityOutcomeBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            if (timeInvariantData.AssetGenerationGroup == null
                || !timeInvariantData.AssetGenerationGroup.UnitCapacity.HasValue
                || !timeInvariantData.SystemCondition_32_Score_32_Best.HasValue
                || !timeInvariantData.AssetAnnual_32_Degradation_32__37_.HasValue)
            {
                // Without a generation group we do not know the unit capacity and so cannot
                // determine the lost efficiency opportunity.  Same if there's no "best" score 
                // provided
                return null;
            }

            var annualDegradation = timeInvariantData.AssetAnnual_32_Degradation_32__37_.Value * 0.01;

            var unitCapacity = timeInvariantData.AssetGenerationGroup.UnitCapacity;
            
            var inServiceMonthOffset = HelperUtility.LastInServiceMonthOffset(timeInvariantData.GenARM_Condition_ConsqUnitOutput,
                timeInvariantData.AssetInServiceDate, startFiscalYear, timeInvariantData.SystemCondition_32_Score_32_Best.Value);
            if (!inServiceMonthOffset.HasValue) return null;
            
            var retStartIndex = Math.Max(0, inServiceMonthOffset.Value);
            var ageInMonthsStartIndex = retStartIndex - inServiceMonthOffset.Value;

            var energyValues = timeInvariantData.AnalyticsStrategyAlternativeEnergyValues ?? timeInvariantData.SystemEnergyValues;
            var energyBaseYear = energyValues.BaseYear ?? startFiscalYear;
            
            var assetAvoidedCO2DollarsPerMWh = timeInvariantData.AnalyticsStrategyAlternativeAvoidedCO2Values ?? timeInvariantData.SystemAvoidedCO2Values;

            var lostEfficiencyOpportunity = new double?[months];
            for (int monthOffset = retStartIndex, ageInMonths = ageInMonthsStartIndex; monthOffset < months; monthOffset++, ageInMonths++)
            {
                var fiscalYearOffset = monthOffset / 12;
                var currentFiscalYear = startFiscalYear + fiscalYearOffset;
                if (currentFiscalYear >= energyBaseYear)
                {
                    int fiscalMonthOffset = monthOffset % 12;
                    double monthlyEnergyValue = energyValues.GetMonthlyValue(currentFiscalYear, fiscalMonthOffset);
                    var co2Value = HelperUtility.AvoidedCo2InDollarsPerMWh(startFiscalYear,assetAvoidedCO2DollarsPerMWh, monthOffset);                       
                    double monthlyValue = monthlyEnergyValue + co2Value;
                    double ageInYears = ageInMonths / 12.0;

                    lostEfficiencyOpportunity[monthOffset] = (ageInYears * annualDegradation) * unitCapacity / 12.0 * monthlyValue;
                }
            }

            return lostEfficiencyOpportunity;
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
