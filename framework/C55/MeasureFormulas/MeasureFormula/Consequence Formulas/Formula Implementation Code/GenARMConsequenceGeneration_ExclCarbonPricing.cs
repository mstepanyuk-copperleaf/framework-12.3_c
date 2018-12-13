using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormula.SharedCode;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class GenARMConsequenceGeneration_ExclCarbonPricing : GenARMConsequenceGeneration_ExclCarbonPricingBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            if (!(timeInvariantData.AssetContributes_32_to_32_Lost_32_Generation ?? false))
            {
                return null;
            }

            var assetEnergyValues = timeInvariantData.AnalyticsStrategyAlternativeEnergyValues ?? timeInvariantData.SystemEnergyValues;

            if (assetEnergyValues == null) return null;

            return GenerationHelpers.GenerationRiskConsequenceCurrencyUnits(
                        startFiscalYear,
                        months,
                        timeInvariantData.AssetGenerationGroup,
                        timeInvariantData.AssetIsSpareAvailable,
                        timeInvariantData.AssetTypeDowntimeWeeksWithSpare,
                        timeInvariantData.AssetTypeDowntimeWeeksWithoutSpare,
                        avoidedCO2Values: null,    // in order to exclude carbon pricing
                        energyValues: assetEnergyValues); 
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
