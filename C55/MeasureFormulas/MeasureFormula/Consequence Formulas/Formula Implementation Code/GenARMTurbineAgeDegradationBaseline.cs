using System;
using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormula.Common_Code;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class GenARMTurbineAgeDegradationBaseline : GenARMTurbineAgeDegradationBaselineBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            //----- Without a generation group we do not know the unit 
            //      capacity and so cannot determine the lost efficiency 
            //      opportunity. Also, without a "best" condition, we 
            //      can't know if the impact is a "replacement".
            //      And withut an asset annual degradation there is nothing to calculate.
            if (timeInvariantData.AssetGenerationGroup == null 
                || !timeInvariantData.AssetGenerationGroup.UnitCapacity.HasValue
                || !timeInvariantData.SystemCondition_32_Score_32_Best.HasValue
                || !timeInvariantData.AssetAnnual_32_Degradation_32__37_.HasValue)
            {
                return null;
            }

            //----- Turbine degradation is a strict function of age so we need 
            //      to know when it was put into service.  This will be done
            //      by looking at the most recent "best" condition or, in the
            //      absence of this, the in-service date of the asset.
            var inServiceMonthOffset = HelperUtility.LastInServiceMonthOffset(timeInvariantData.GenARM_Condition_ConsqUnitOutput,
                timeInvariantData.AssetInServiceDate, startFiscalYear, timeInvariantData.SystemCondition_32_Score_32_Best.Value);
            if (!inServiceMonthOffset.HasValue) return null;

            var retStartIndex = Math.Max(0, inServiceMonthOffset.Value);
            var ageInMonthsStartIndex = retStartIndex - inServiceMonthOffset.Value;

            var annualDegradation = timeInvariantData.AssetAnnual_32_Degradation_32__37_.Value * 0.01;
            var unitCapacity = timeInvariantData.AssetGenerationGroup.UnitCapacity.Value;
            var energyValues = timeInvariantData.AnalyticsStrategyAlternativeEnergyValues ?? timeInvariantData.SystemEnergyValues;
            int energyBaseYear = energyValues.BaseYear ?? startFiscalYear;

            var assetAvoidedCO2DollarsPerMWh = timeInvariantData.AnalyticsStrategyAlternativeAvoidedCO2Values ?? timeInvariantData.SystemAvoidedCO2Values;

            var ageDegradation = new double?[months];
            for (int monthOffset = retStartIndex, ageInMonths = ageInMonthsStartIndex; monthOffset < months; monthOffset++, ageInMonths++)
            {
                var fiscalYearOffset = monthOffset / 12;
                var currentFiscalYear = startFiscalYear + fiscalYearOffset;
                if (currentFiscalYear >= energyBaseYear)
                {
                    int fiscalMonthOffset = monthOffset % 12;
                    var monthlyEnergyValue = energyValues.GetMonthlyValue(currentFiscalYear, fiscalMonthOffset);
                    var co2Value = HelperUtility.AvoidedCo2InDollarsPerMWh(startFiscalYear, assetAvoidedCO2DollarsPerMWh, monthOffset);                    
                    double monthlyValue = monthlyEnergyValue + co2Value;
                    ageDegradation[monthOffset] = ((ageInMonths / 12.0) * annualDegradation * unitCapacity) / 12.0 * monthlyValue;  
                }
            }

            return ageDegradation;
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
