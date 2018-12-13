using System;
using System.Collections.Generic;
using System.Linq;
using CL.FormulaHelper.Attributes;
using MeasureFormulas.Generated_Formula_Base_Classes;
using MeasureFormula.Common_Code;

namespace CustomerFormulaCode
{
    [Formula]
    public class GenARMTurbineTechnologyImprovementOutcome : GenARMTurbineTechnologyImprovementOutcomeBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            //----- Without a generation group we do not know the unit 
            //      capacity and so cannot determine the lost efficiency 
            //      opportunity. Also, without a "best" condition, we 
            //      can't know if the impact is a "replacement".
            //      And withut a technology improvement there is nothing to calculate.
            if (timeInvariantData.AssetGenerationGroup == null
                || !timeInvariantData.AssetGenerationGroup.UnitCapacity.HasValue
                || !timeInvariantData.SystemCondition_32_Score_32_Best.HasValue
                || !timeInvariantData.AssetTechnology_32_Improvement_32__37_.HasValue)
            {
                return null;
            }

            //----- Turbine improvement is only applicable to the first replacement
            if (!HelperUtility.IsFirstReplacement(timeInvariantData.GenARM_Condition_ConsqUnitOutput_B, timeInvariantData.SystemCondition_32_Score_32_Best.Value))
            {
                return null;
            }

            //----- The technical improvement applies from the replacement on...
            DateTime? inServiceFiscalDate = null;
            foreach (var ans in timeVariantData.Reverse())
            {
                if (Equals(ans.ImpactCondition, timeInvariantData.SystemCondition_32_Score_32_Best))
                {
                    inServiceFiscalDate = ans.TimePeriod.StartTime;
                    break;
                }
            }

            if (!inServiceFiscalDate.HasValue)
            {
                return null;
            }

            double techImprovement = timeInvariantData.AssetTechnology_32_Improvement_32__37_.Value * 0.01;
            double unitCapacity = timeInvariantData.AssetGenerationGroup.UnitCapacity.Value;
            var energyValues = timeInvariantData.AnalyticsStrategyAlternativeEnergyValues ?? timeInvariantData.SystemEnergyValues;
            int energyBaseYear = energyValues.BaseYear ?? startFiscalYear;
            var assetAvoidedCO2DollarsPerMWh = timeInvariantData.AnalyticsStrategyAlternativeAvoidedCO2Values ?? timeInvariantData.SystemAvoidedCO2Values;

            var inServiceMonthOffset = ConvertDateTimeToOffset(inServiceFiscalDate.Value, startFiscalYear);
            var retStartIndex = Math.Max(0, inServiceMonthOffset);

            double?[] ret = new double?[months];
            for (int monthOffset = retStartIndex; monthOffset < months; monthOffset++)
            {
                int fiscalYearOffset = monthOffset / 12;
                int currentFiscalYear = startFiscalYear + fiscalYearOffset;
                if (currentFiscalYear >= energyBaseYear)
                {
                    int fiscalMonthOffset = monthOffset % 12;
                    double monthlyEnergyValue = energyValues.GetMonthlyValue(currentFiscalYear, fiscalMonthOffset);
                    var avoidedCO2InDollarsPerMWh = HelperUtility.AvoidedCo2InDollarsPerMWh(startFiscalYear, assetAvoidedCO2DollarsPerMWh, monthOffset);                      
                    double monthlyValue = monthlyEnergyValue + avoidedCO2InDollarsPerMWh;
                    ret[monthOffset] = techImprovement * unitCapacity * monthlyValue / 12.0;
                }
            }

            return ret;
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
