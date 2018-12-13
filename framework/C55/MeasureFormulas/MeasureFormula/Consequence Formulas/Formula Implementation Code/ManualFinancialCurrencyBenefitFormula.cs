using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormula.Common_Code;
using MeasureFormula.SharedCode;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class ManualFinancialCurrencyBenefitFormula : ManualFinancialCurrencyBenefitFormulaBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            return InterpolatePropagate<TimeVariantInputDTO>(timeVariantData,
                startFiscalYear,
                months, x => (x.FinCurBenefAmount / CommonConstants.MonthsPerYear));
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
