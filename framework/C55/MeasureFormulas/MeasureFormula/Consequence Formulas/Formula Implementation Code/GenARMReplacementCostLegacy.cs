using System;
using System.Collections.Generic;
using System.Linq;
using CL.FormulaHelper.Attributes;
using MeasureFormula.SharedCode;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class GenARMReplacementCostLegacy : GenARMReplacementCostLegacyBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            if (!CurveHelpers.IsValidSpendProfile( timeInvariantData.AssetTypeAnnualSpendProfileCurve))
            {
                return null;
            }

            // XYCurveDTO guarantees that last point has largest X value
            var numYearsOfSpend = (int) timeInvariantData.AssetTypeAnnualSpendProfileCurve.Points.Last().X;

            var replacementCost = LoadedReplacementCost(
                timeInvariantData.AssetReplacementCost,
                timeInvariantData.SystemBurden_32_Factor ?? 1.0,
                timeInvariantData.AssetTypeCostVariationFactor);

            var capitalPowerShare = (timeInvariantData.AssetJointly_32_Funded_63_ ?? false)
                ? (timeInvariantData.AssetFacilityCapital_32_Power_32_Share_32__40__37__41_ / 100.0) ?? 1.0
                : 1.0;
            replacementCost *= capitalPowerShare;

            var monthlySpends = new double?[months];            
            foreach (var timeVariantEntry in timeVariantData)
            {
                var monthOfImpact = ConvertDateTimeToOffset(timeVariantEntry.TimePeriod.StartTime, startFiscalYear);
                
                foreach (var spendSpecification in timeInvariantData.AssetTypeAnnualSpendProfileCurve.Points)
                {
                    var spendYear = (int) spendSpecification.X;
                    var proportion = spendSpecification.Y;
                    var monthlySpend = proportion * replacementCost / CommonConstants.MonthsPerYear;
                    
                    // The assumption is that the last year of spend occurs in the 12 months following the impact.  
                    var startMonthForYearlySpend = monthOfImpact - (numYearsOfSpend - spendYear) * CommonConstants.MonthsPerYearInt;
                    var lastMonthForYearlySpend = Math.Min(months, startMonthForYearlySpend + CommonConstants.MonthsPerYearInt);
                    for (var i = Math.Max(0, startMonthForYearlySpend); i < lastMonthForYearlySpend; i++)
                    {
                        monthlySpends[i] = monthlySpend + (monthlySpends[i] ?? 0d);
                    }
                }
            }
            return monthlySpends;
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
