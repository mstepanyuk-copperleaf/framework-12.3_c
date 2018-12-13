using System;
using System.Collections.Generic;
using System.Linq;
using CL.FormulaHelper.Attributes;
using MeasureFormula.Common_Code;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class GenARMTurbineAgeDegradationOutcome : GenARMTurbineAgeDegradationOutcomeBase
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
            //      to know when it was put into service.  On the baseline,
            //      this will be done by looking at the most recent "best" 
            //      condition or, in the absence of this, the in-service date 
            //      of the asset.  For the outcome, we only check the impact 
            //      questionairre answers and find the most recent "best"
            DateTime? inServiceFiscalDate = null;
            foreach(var ans in timeVariantData.Reverse())
            {
                if(Equals(ans.ImpactCondition, timeInvariantData.SystemCondition_32_Score_32_Best))
                {
                    inServiceFiscalDate = ans.TimePeriod.StartTime;
                    break;
                }
            }

            if (!inServiceFiscalDate.HasValue)
            {
                return null;
            }

            double annualDegradation = timeInvariantData.AssetAnnual_32_Degradation_32__37_.Value * 0.01;
            double unitCapacity = timeInvariantData.AssetGenerationGroup.UnitCapacity.Value;
            var energyValues = timeInvariantData.AnalyticsStrategyAlternativeEnergyValues ?? timeInvariantData.SystemEnergyValues;
            var energyBaseYear = energyValues.BaseYear ?? startFiscalYear;
            
            var assetAvoidedCO2DollarsPerMWh = timeInvariantData.AnalyticsStrategyAlternativeAvoidedCO2Values ?? timeInvariantData.SystemAvoidedCO2Values;

            var inServiceMonthOffset = ConvertDateTimeToOffset(inServiceFiscalDate.Value, startFiscalYear);
            var retStartIndex = Math.Max(0, inServiceMonthOffset);
            var ageInMonthsStartIndex = retStartIndex - inServiceMonthOffset;

            double?[] degradationOutcome = new double?[months];
            for (int monthOffset = retStartIndex, ageInMonths = ageInMonthsStartIndex; monthOffset < months; monthOffset++, ageInMonths++)
            {
                int fiscalYearOffset = monthOffset / 12;
                int currentFiscalYear = startFiscalYear + fiscalYearOffset;
                if (currentFiscalYear >= energyBaseYear)
                {
                    int fiscalMonthOffset = monthOffset % 12;
                    double monthlyEnergyValue = energyValues.GetMonthlyValue(currentFiscalYear, fiscalMonthOffset);
                    var co2Value = HelperUtility.AvoidedCo2InDollarsPerMWh(currentFiscalYear, assetAvoidedCO2DollarsPerMWh, fiscalMonthOffset);                      
                    double monthlyValue = monthlyEnergyValue + co2Value;
                    degradationOutcome[monthOffset] = ((ageInMonths / 12.0) * annualDegradation * unitCapacity) / 12.0 * monthlyValue;
                }
            }

            return degradationOutcome;
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
