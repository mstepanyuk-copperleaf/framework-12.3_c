using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormula.Common_Code;
using MeasureFormula.SharedCode;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class ManualOMFinancialBenefitFormula : ManualOMFinancialBenefitFormulaBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            // Without the SystemLabourHourRate we cannot calculate O&M Financial Benefits
            if (timeInvariantData.SystemLabour_32_Hour_32_Rate == null)
            {
                return null;
            }

            var result = new double?[months];
            foreach (var data in timeVariantData)
            {
                var firstMonth = System.Math.Max(ConvertDateTimeToOffset(data.TimePeriod.StartTime, startFiscalYear), 0);
                var lastMonth = System.Math.Min((firstMonth + data.TimePeriod.DurationInMonths ?? months), months);
                for (int i = firstMonth; i < lastMonth; i++)
                {
                    result[i] =
                        (data.OMFinBenefLaborHr * timeInvariantData.SystemLabour_32_Hour_32_Rate.GetMonthlyValue(startFiscalYear, i)
                        + data.OMFinBenefMaterialCost + data.OMFinBenefOtherDirectCost) / CommonConstants.MonthsPerYear;
                }
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
