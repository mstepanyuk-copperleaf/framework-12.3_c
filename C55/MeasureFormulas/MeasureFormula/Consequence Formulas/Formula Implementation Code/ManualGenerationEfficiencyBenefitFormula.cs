using System;
using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormula.Common_Code;
using MeasureFormula.SharedCode;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class ManualGenerationEfficiencyBenefitFormula : ManualGenerationEfficiencyBenefitFormulaBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            if (timeInvariantData.SystemMWh_32_Price == null)
            {
                return null;
            }

            Func<TimeVariantInputDTO, int, double?> func = (data, i) =>
                (data.GenEffBenefAvgAnnualOutput * (data.GenEffBenefForecastIncreasePcn / 100) *
                 timeInvariantData.SystemMWh_32_Price.GetMonthlyValue(startFiscalYear, i)) / CommonConstants.MonthsPerYear;
            
            return HelperFunctions.InterpolatePropagateWithMonthIndex(timeVariantData, startFiscalYear, months, func);
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
