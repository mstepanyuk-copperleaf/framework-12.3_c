using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormula.SharedCode;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class GenARMConsequenceGeneration : GenARMConsequenceGenerationBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            if(!(timeInvariantData.AssetContributes_32_to_32_Lost_32_Generation ?? false))
            {
                return null;
            }

            return GenerationHelpers.GenerationRiskConsequenceCurrencyUnits(
                        startFiscalYear,
                        months,
                        timeInvariantData.AssetGenerationGroup,
                        timeInvariantData.AssetIsSpareAvailable,
                        timeInvariantData.AssetTypeDowntimeWeeksWithSpare,
                        timeInvariantData.AssetTypeDowntimeWeeksWithoutSpare,
                        timeInvariantData.AnalyticsStrategyAlternativeAvoidedCO2Values ?? timeInvariantData.SystemAvoidedCO2Values,
                        timeInvariantData.AnalyticsStrategyAlternativeEnergyValues ?? timeInvariantData.SystemEnergyValues);
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
