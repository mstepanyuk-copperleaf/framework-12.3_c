using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using CL.FormulaHelper.DTOs;
using static CL.FormulaHelper.DTOs.TimeSeriesDTO;
using MeasureFormula.TestHelpers;
using NUnit.Framework;

using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.ManualFinancialResourceBenefitFormulaBase;
using formulaClass = CustomerFormulaCode.ManualFinancialResourceBenefitFormula;
 
namespace MeasureFormula.Tests
{
    [TestFixture]
    public class ManualFinancialResourceBenefitFormulaTests : MeasureFormulaTestsBase 
    {
        private readonly formulaClass _formulas = new formulaClass();

        private CustomFieldListItemDTO BenefitResource;
        private CustomFieldListItemDTO Supplier;
        private TimeSeriesDTO CO2aMWTimeSeries;
        private TimeSeriesDTO ExpectedFlat;
        private TimeSeriesDTO HighFlat;
        private TimeSeriesDTO LowFlat;
        private double BenefitAmount;
        
        private baseClass.TimeInvariantInputDTO _timeInvariantInput;
        private IReadOnlyList<baseClass.TimeVariantInputDTO> _timeVariantInput;
        
        [SetUp]
        public void FixtureSetup()
        {
            // to facilitate comparing with a one time generated values from the old implementation, freeze the number of months
            ArbitraryMonths = 24;
            
            BenefitResource = new CustomFieldListItemDTO{Value = (double) CustomerFormulaCode.Resource.CO2_aMW};
            DataPrep.SetConstructorParameter(fixture, "p_FinResBenefResource", BenefitResource);
            
            Supplier = new CustomFieldListItemDTO{Value = (double) CustomerFormulaCode.Supplier.High_Flat};
            DataPrep.SetConstructorParameter(fixture, "p_FinResBenefSupplier", Supplier);

            var numYearsInOutputResult = 1 + ArbitraryMonths / SharedCode.CommonConstants.MonthsPerYearInt;
            CO2aMWTimeSeries = DataPrep.CreateRandomTimeSeriesDto(TimeSeriesOffsetType.AbsoluteCalendarYearly, numYearsInOutputResult, ArbitraryStartYear);
            DataPrep.SetConstructorParameter(fixture, "p_SystemCO2_aMW_Variable", CO2aMWTimeSeries);
            
            ExpectedFlat = DataPrep.CreateRandomTimeSeriesDto(TimeSeriesOffsetType.AbsoluteCalendarYearly, numYearsInOutputResult, ArbitraryStartYear);
            DataPrep.SetConstructorParameter(fixture, "p_SystemExpected_Flat", ExpectedFlat);
            
            HighFlat = DataPrep.CreateRandomTimeSeriesDto(TimeSeriesOffsetType.AbsoluteCalendarYearly, numYearsInOutputResult, ArbitraryStartYear);
            DataPrep.SetConstructorParameter(fixture, "p_SystemHigh_Flat", HighFlat);
            
            LowFlat = DataPrep.CreateRandomTimeSeriesDto(TimeSeriesOffsetType.AbsoluteCalendarYearly, numYearsInOutputResult, ArbitraryStartYear);
            DataPrep.SetConstructorParameter(fixture, "p_SystemLow_Flat", LowFlat);             
         
            _timeInvariantInput = fixture.Create<baseClass.TimeInvariantInputDTO>();

            BenefitAmount = 1000.0 + fixture.Create<double>() % 100000;
            DataPrep.SetConstructorParameter(fixture, "p_FinResBenefAmount", BenefitAmount);             
            _timeVariantInput = new[] {fixture.Create<baseClass.TimeVariantInputDTO>()};
        }

   
        [Test]
        public void NullTests()
        {
            double?[] getUnitsCall(object x, object y) => _formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, 
                (baseClass.TimeInvariantInputDTO) x,
                (IReadOnlyList<baseClass.TimeVariantInputDTO>) y);

            var nullCheck = new NullablePropertyCheck();
            Assert.DoesNotThrow(() =>
            {
                nullCheck.RunNullTestsIncludingCustomFields(
                    _timeInvariantInput,
                    _timeVariantInput,
                    getUnitsCall);
            });
        }

        [Test]
        public void CompareRefactor()
        {           
            //  Before the refactor, gathered the computation for these specific inputs so we can compare against them.
            BenefitResource = new CustomFieldListItemDTO{Value = (double) CustomerFormulaCode.Resource.CO2_aMW};
            DataPrep.SetConstructorParameter(fixture, "p_FinResBenefResource", BenefitResource);
            
            Supplier = new CustomFieldListItemDTO{Value = (double) CustomerFormulaCode.Supplier.High_Flat};
            DataPrep.SetConstructorParameter(fixture, "p_FinResBenefSupplier", Supplier);
            
            var specificHighFlat = new TimeSeriesDTO
            {
                OffsetType = TimeSeriesOffsetType.AbsoluteCalendarYearly,
                BaseYear = ArbitraryStartYear,
                ValuesDoubleArray = new [] {0.46194376445465896, 0.41293066805830725, 0.56920169646348884}
            };
            DataPrep.SetConstructorParameter(fixture, "p_SystemHigh_Flat", specificHighFlat);
            
            _timeInvariantInput = fixture.Create<baseClass.TimeInvariantInputDTO>();

            BenefitAmount = 1013;
            DataPrep.SetConstructorParameter(fixture, "p_FinResBenefAmount", BenefitAmount);             
            _timeVariantInput = new[] {fixture.Create<baseClass.TimeVariantInputDTO>()};
            
            var beforeRefactor = Enumerable.Repeat(341602.79437657574, 15).Concat(Enumerable.Repeat(305358.09972243762, 9)).ToArray();
            var refactoredResult = _formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, _timeInvariantInput, _timeVariantInput);
            
            Assert.That(refactoredResult, Is.EqualTo(beforeRefactor).Within(SharedCode.CommonConstants.DoubleDifferenceTolerance));
        }
    }
}
