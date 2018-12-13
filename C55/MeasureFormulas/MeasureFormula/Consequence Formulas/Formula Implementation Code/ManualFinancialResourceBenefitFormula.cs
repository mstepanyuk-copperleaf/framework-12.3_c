using System;
using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using CL.FormulaHelper.DTOs;
using MeasureFormula.Common_Code;
using MeasureFormula.SharedCode;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    public enum Supplier
    {
        High_Flat = 1,
        Low_Flat = 2, 
        Expected_Flat = 3,
        CO2_aMW_Variable = 4,
        CO2_MWh_Variable = 5
    }

    public enum Resource
    {
        aMW = 1,
        MWh = 2,
        CO2_aMW = 3,
        CO2_MWh = 4
    }

    [Formula]
    public class ManualFinancialResourceBenefitFormula : ManualFinancialResourceBenefitFormulaBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            if (timeInvariantData.FinResBenefSupplier == null || timeInvariantData.FinResBenefResource == null)
            {
                return null;
            }

            TimeSeriesDTO energyValues;
            switch (timeInvariantData.FinResBenefSupplier.ValueAsInteger)
            {
                case (int)Supplier.High_Flat:
                    energyValues = timeInvariantData.SystemHigh_Flat;
                    break;
                case (int)Supplier.Low_Flat:
                    energyValues = timeInvariantData.SystemLow_Flat;
                    break;
                case (int)Supplier.Expected_Flat:
                    energyValues = timeInvariantData.SystemExpected_Flat;
                    break;
                case (int)Supplier.CO2_aMW_Variable:
                case (int)Supplier.CO2_MWh_Variable:
                    energyValues = timeInvariantData.SystemCO2_aMW_Variable;
                    break;
                default:
                    energyValues = null;
                    break;
            }

            if (energyValues == null)
            {
                return null;
            }

            var hoursPerYear = 365 * 24;

            Func<TimeVariantInputDTO, int, double?> resourceBenefitAt = (data, i) =>
            {
                if (Math.Abs(data.FinResBenefAmount) < CommonConstants.DoubleDifferenceTolerance)
                    return null;
                var energyValue = energyValues.GetMonthlyValue(startFiscalYear, i);
                switch (timeInvariantData.FinResBenefResource.ValueAsInteger)
                {
                    case (int)Resource.aMW:
                    case (int)Resource.CO2_aMW:
                        return (data.FinResBenefAmount * energyValue * hoursPerYear) / CommonConstants.MonthsPerYear;
                    case (int)Resource.MWh:
                    case (int)Resource.CO2_MWh:
                        return (data.FinResBenefAmount * energyValue) / CommonConstants.MonthsPerYear;
                    default:
                        return null;
                }
            };
            
            return HelperFunctions.InterpolatePropagateWithMonthIndex(timeVariantData, startFiscalYear, months, resourceBenefitAt);
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
