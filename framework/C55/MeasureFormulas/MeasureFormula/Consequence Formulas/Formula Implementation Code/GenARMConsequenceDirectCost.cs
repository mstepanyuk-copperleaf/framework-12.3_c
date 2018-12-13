using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class GenARMConsequenceDirectCost : GenARMConsequenceDirectCostBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            var consequence = DirectCostRiskConsequence(
                timeInvariantData.AssetReplacementCost,
                timeInvariantData.SystemBurden_32_Factor ?? 1.0,
                timeInvariantData.AssetTypeCostVariationFactor,
                timeInvariantData.AssetTypeExtraDamageFactor);

            var capitalPowerShare = (timeInvariantData.AssetJointly_32_Funded_63_ ?? false)
                ? (timeInvariantData.AssetFacilityCapital_32_Power_32_Share_32__40__37__41_ / 100.0) ?? 1.0
                : 1.0;

            return PopulateOutputWithValue(months, consequence * capitalPowerShare);
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
