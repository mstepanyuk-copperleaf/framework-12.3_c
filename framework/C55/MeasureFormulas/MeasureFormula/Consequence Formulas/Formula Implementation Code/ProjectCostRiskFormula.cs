using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormula.Common_Code;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class ProjectCostRiskFormula : ProjectCostRiskFormulaBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            // Spread the risk evenly over the duration of spend for the project.
            var minSpendIndex = FindStartOfSpendMonth(timeInvariantData.InvestmentSpendByAccountType);
            var maxSpendIndex = FindEndOfSpendMonth(timeInvariantData.InvestmentSpendByAccountType);

            // Cannot determine the duration of spend if the spend indexes are null (which would be the case,
            // for example, if there is no spend).
            if (minSpendIndex == null || maxSpendIndex == null) return null;

            // If all spend values are in the past, they are not included in the requested time period.
            if (maxSpendIndex < 0) return null;

            // If all spend values are in the future, they are not included in the requested time period.
            if (minSpendIndex >= months) return null;

            var projectAnnualCostRisk = timeInvariantData.ExpectedLikelihoodPercentage / 100 * timeInvariantData.ExpectedCost;
            var monthsDuration = System.Math.Abs(maxSpendIndex.Value - minSpendIndex.Value) + 1;
            var monthlyRisk = projectAnnualCostRisk / monthsDuration;

            var result = new double?[months];
            // The offset of the start or end month of spend could be negative - convert them to positive values
            var minIndex = System.Math.Max(minSpendIndex.Value, 0);
            var maxIndex = System.Math.Min(maxSpendIndex.Value + 1, months);
            for (var i = minIndex; i < maxIndex; i++)
            {
                result[i] = monthlyRisk; 
            }

            return result; 
        }

        public override double?[] GetZynos(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData,
            IReadOnlyList<TimeVariantInputDTO> timeVariantData,
            double?[] unitOutput)
        {
            return ConvertUnitsToZynos(unitOutput, CustomerConstants.DollarToZynoConversionFactor);
        }
    }
}
