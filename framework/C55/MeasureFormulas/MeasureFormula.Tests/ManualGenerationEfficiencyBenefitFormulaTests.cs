using System;
using System.Collections.Generic;
using AutoFixture;
using CL.FormulaHelper.DTOs;
using MeasureFormula.TestHelpers;
using NUnit.Framework;

using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.ManualGenerationEfficiencyBenefitFormulaBase;
using formulaClass = CustomerFormulaCode.ManualGenerationEfficiencyBenefitFormula;
 
namespace MeasureFormula.Tests
{
    [TestFixture]
    public class ManualGenerationEfficiencyBenefitFormula : MeasureFormulaTestsBase 
    {
        private readonly formulaClass _formulas = new formulaClass();
        private baseClass.TimeInvariantInputDTO _timeInvariantInput;
        private IReadOnlyList<baseClass.TimeVariantInputDTO> _timeVariantInput;
        
        private TimeSeriesDTO MWhPrice;
        
        [SetUp]
        public void FixtureSetup()     
        {
            MWhPrice = DataPrep.CreateConstantTimeSeries(80.0);
            DataPrep.SetConstructorParameter(fixture, "p_SystemMWh_32_Price", MWhPrice);
         
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
