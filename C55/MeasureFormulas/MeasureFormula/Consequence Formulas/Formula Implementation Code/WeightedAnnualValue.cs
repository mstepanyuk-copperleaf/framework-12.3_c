using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormula.Common_Code;
using MeasureFormula.SharedCode;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class WeightedAnnualValue : WeightedAnnualValueBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            if (!(timeInvariantData.AssetContributes_32_to_32_Lost_32_Generation ?? false))
            {
                return null;
            }

            // No generation loss consequence if the asset is not associated with a generation group.
            if (timeInvariantData.AssetGenerationGroup == null)
            {
                return null;
            }

            //the loss array must have at least one value, otherwise do nothing
            if (timeInvariantData.AssetGenerationGroup.Loss == null ||
                timeInvariantData.AssetGenerationGroup.Loss.Count < 1)
            {
                return null;
            }

            var energyValues = timeInvariantData.AnalyticsStrategyAlternativeEnergyValues ?? timeInvariantData.SystemEnergyValues;
            if (energyValues == null || !energyValues.BaseYear.HasValue)
            {
                return null;
            }            
            var baseYear = energyValues.BaseYear.Value;

            var avoidedCO2Values = timeInvariantData.AnalyticsStrategyAlternativeAvoidedCO2Values ?? timeInvariantData.SystemAvoidedCO2Values;

            var generationLoss = timeInvariantData.AssetGenerationGroup.Loss[0];

            var weightedAnnualValue = new double?[months];
            //number of months between base year and start fiscal year
            var monthOffset = (baseYear - startFiscalYear) * CommonConstants.MonthsPerYearInt;

            //index j will be used as a relative counter to retrieve monthly values
            for (int i = monthOffset, j = 0; i < months; i++, j++)
            {
                //do nothing till we have a non-negative month index
                if (i < 0)
                {
                    continue;
                }

                //get the correct monthly value from the energy values. baseYear + j/12 will give us the current year
                //j%12 will give us the current month
                var co2Value = HelperUtility.AvoidedCo2InDollarsPerMWh(baseYear + (j / CommonConstants.MonthsPerYearInt), avoidedCO2Values,
                    j % CommonConstants.MonthsPerYearInt);
                weightedAnnualValue[i] = generationLoss *
                         (energyValues.GetMonthlyValue(baseYear + (j / CommonConstants.MonthsPerYearInt), j % CommonConstants.MonthsPerYearInt) + co2Value);
            }

            return weightedAnnualValue;
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
