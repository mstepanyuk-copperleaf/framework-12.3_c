using System;
using System.Collections.Generic;
using AutoFixture;
using CL.FormulaHelper.DTOs;
using MeasureFormula.TestHelpers;
using NUnit.Framework;

using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.ManualProductiveWorplaceBenefitFormulaBase;
using formulaClass = CustomerFormulaCode.ManualProductiveWorplaceBenefitFormula;
 
namespace MeasureFormula.Tests
{
    [TestFixture]
    public class ManualProductiveWorplaceBenefitFormula : MeasureFormulaTestsBase 
    {
        private readonly formulaClass _formulas = new formulaClass();
        
        private baseClass.TimeInvariantInputDTO _timeInvariantInput;
        private IReadOnlyList<baseClass.TimeVariantInputDTO> _timeVariantInput;

        private TimeSeriesDTO LabourConstantTimeSeries;
        
        [SetUp]
        public void FixtureSetup()
        {
            LabourConstantTimeSeries = DataPrep.CreateConstantTimeSeries(80.0);
            DataPrep.SetConstructorParameter(fixture, "p_SystemLabour_32_Hour_32_Rate", LabourConstantTimeSeries);
         
            _timeInvariantInput = fixture.Create<baseClass.TimeInvariantInputDTO>();
            _timeVariantInput = new[] {fixture.Create<baseClass.TimeVariantInputDTO>()};
        }

   
        [Test]
        public void NullTests()
        {
            Func<object, object, double?[]> getUnitsCall =
                (x, y) => _formulas.GetUnits(ArbitraryStartYear,
                    ArbitraryMonths,
                    (baseClass.TimeInvariantInputDTO) x,
                    (IReadOnlyList<baseClass.TimeVariantInputDTO>) y);
 
            var nullCheck = new NullablePropertyCheck();
            nullCheck.RunNullTestsIncludingCustomFields(
                _timeInvariantInput,
                _timeVariantInput,
                getUnitsCall);
        }
  }
}
