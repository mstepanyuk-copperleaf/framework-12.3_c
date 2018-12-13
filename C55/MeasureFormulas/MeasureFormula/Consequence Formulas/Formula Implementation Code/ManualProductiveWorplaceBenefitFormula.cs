using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormulas.Generated_Formula_Base_Classes;
using MeasureFormula.Common_Code;
using MeasureFormula.SharedCode;

namespace CustomerFormulaCode
{
    [Formula]
    public class ManualProductiveWorplaceBenefitFormula : ManualProductiveWorplaceBenefitFormulaBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            if (timeInvariantData.SystemLabour_32_Hour_32_Rate == null)
            {
                return null;
            }

            var result = new double?[months];
            foreach (var data in timeVariantData)
            {
                var firstMonth = System.Math.Max(ConvertDateTimeToOffset(data.TimePeriod.StartTime, startFiscalYear), 0);
                var lastMonth = System.Math.Min((firstMonth + data.TimePeriod.DurationInMonths ?? months), months);
                for (var i = firstMonth; i < lastMonth; i++)
                {
                    result[i] =
                        (data.ProWorBenefNumOfEmployees * data.ProWorBenefNumOfHours *
                        timeInvariantData.SystemLabour_32_Hour_32_Rate.GetMonthlyValue(startFiscalYear, i)) / CommonConstants.MonthsPerYear;
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
