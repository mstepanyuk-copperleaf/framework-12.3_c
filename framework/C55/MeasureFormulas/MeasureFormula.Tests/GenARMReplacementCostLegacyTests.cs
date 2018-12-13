using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using CL.FormulaHelper;
using CL.FormulaHelper.DTOs;
using MeasureFormula.SharedCode;
using MeasureFormula.TestHelpers;
using NUnit.Framework;

using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.GenARMReplacementCostLegacyBase;
using formulaClass = CustomerFormulaCode.GenARMReplacementCostLegacy;

namespace MeasureFormula.Tests
{
    [TestFixture]
    public class GenARMReplacementCostLegacyTests : MeasureFormulaTestsBase
    {
        private formulaClass formulas;

        private double? PowerSharePercent;
        private decimal? ReplacementCost;
        private XYCurveDTO TwoYearSimpleSpendProfileCurve;
        private double? CostVariationFactor;
        private double? BurdenFactor;

        private int ThreeYearsInMonths;

        private XYCurveDTO TwoSpendsWithOneYearGapProfile;

        private baseClass.TimeInvariantInputDTO TimeInvariantInput;
        private baseClass.TimeVariantInputDTO[] TimeVariantInput;

        [SetUp]
        public void FixtureSetup()
        {
            formulas = new formulaClass();
            ThreeYearsInMonths = 36;

            PowerSharePercent = 100.0;
            DataPrep.SetConstructorParameter(fixture, "p_AssetFacilityCapital_32_Power_32_Share_32__40__37__41_", PowerSharePercent);
            
            ReplacementCost = 10000 + Math.Abs(fixture.Create<decimal>());
            DataPrep.SetConstructorParameter(fixture, "p_AssetReplacementCost", ReplacementCost);

            CostVariationFactor = 0.5 + Math.Abs(fixture.Create<double>() % 2);
            DataPrep.SetConstructorParameter(fixture, "p_AssetTypeCostVariationFactor", CostVariationFactor);

            BurdenFactor = 0.25 + Math.Abs(fixture.Create<double>() % 2);
            DataPrep.SetConstructorParameter(fixture, "p_SystemBurden_32_Factor", BurdenFactor);

            TwoYearSimpleSpendProfileCurve = new XYCurveDTO{Points = new [] { new CurvePointDTO{X=1, Y=0.5}, new CurvePointDTO{X=2, Y=0.5}}};
            DataPrep.SetConstructorParameter(fixture, "p_AssetTypeAnnualSpendProfileCurve", TwoYearSimpleSpendProfileCurve);

            TimeInvariantInput = fixture.Create<baseClass.TimeInvariantInputDTO>();
            TimeVariantInput = new []  {fixture.Create<baseClass.TimeVariantInputDTO>()};
            
            var firstYearProportion = 0.7;
            var secondProportion = 1.0 - firstYearProportion;
            var yearAfterGap = 3;
            TwoSpendsWithOneYearGapProfile = new XYCurveDTO
            {
                Points = new[] {new CurvePointDTO {X = 1, Y = firstYearProportion}, new CurvePointDTO {X = yearAfterGap, Y = secondProportion}}
            };            
        }

        [Test]
        public void NullTests()
        {
            Func<object, object, double?[]> getUnitsCall = (x, y) => formulas.GetUnits(ArbitraryStartYear, ThreeYearsInMonths,
                (baseClass.TimeInvariantInputDTO) x, (IReadOnlyList<baseClass.TimeVariantInputDTO>) y);
            
            var nullCheck = new NullablePropertyCheck();
            
            nullCheck.RunNullTestsIncludingCustomFields(TimeInvariantInput, TimeVariantInput, getUnitsCall);
        }

        [Test]
        public void IsValidSpendProfile_WhenNullSpendProfile_ReturnsFalse()
        {
            Assert.That(CurveHelpers.IsValidSpendProfile(null), Is.False);
        }
        
        [Test]
        public void GetUnits_WhenIsValidSpendProfileReturnsFalse_ReturnsNull()
        {
            XYCurveDTO nullSpendProfile = null;
            DataPrep.SetConstructorParameter(fixture, "p_AssetTypeAnnualSpendProfileCurve", nullSpendProfile);
            var timeIvariantDataNullCurve = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = formulas.GetUnits(ArbitraryStartYear, ThreeYearsInMonths, timeIvariantDataNullCurve, TimeVariantInput );
            
            Assert.That(results, Is.Null);
        }

        [Test]
        public void IsValidSpendProfile_SpendCurveHasNullPoints_ReturnsFalse()
        {
            var nullPointsSpendProfile = new XYCurveDTO();
            
            Assert.That(CurveHelpers.IsValidSpendProfile(nullPointsSpendProfile), Is.False);
        }      
        
        [Test]
        public void IsValidSpendProfile_SpendCurveHasNoYears_ReturnsFalse()
        {
            var noPointsSpendProfile = new XYCurveDTO {Points = new CurvePointDTO[] { }};
            
            Assert.That(CurveHelpers.IsValidSpendProfile(noPointsSpendProfile), Is.False);
        }
        
        [Test]
        public void IsValidSpendProfile_SpendCurveHasNonPositiveYear_ReturnsFalse()
        {
            var nonPositiveYearSpendProfile = new XYCurveDTO {Points = new[] {new CurvePointDTO {X = -1, Y = 0.5}, new CurvePointDTO {X = 2, Y = 0.5}}};
            
            Assert.That(CurveHelpers.IsValidSpendProfile(nonPositiveYearSpendProfile), Is.False);
        }

        [Test]
        public void IsValidSpendProfile_SpendCurveProportionsSumToGreaterThanOne_ReturnsFalse()
        {
            var totalGreaterThan1 = 2.0;
            var firstYearProportion = 0.7;
            var secondYearPropotion = totalGreaterThan1 - firstYearProportion;
            var sumToGreaterThanOne = new XYCurveDTO
            {
                Points = new[] {new CurvePointDTO {X = 1, Y = firstYearProportion}, new CurvePointDTO {X = 2, Y = secondYearPropotion}}
            };
            
            Assert.That(CurveHelpers.IsValidSpendProfile(sumToGreaterThanOne), Is.False);
        }  
        
        [Test]
        public void IsValidSpendProfile_SpendCurveProportionsSumToLessThanOne_ReturnsFalse()
        {
            var totalLessThan1 = 0.5;
            var firstYearProportion = 0.3;
            var secondYearPropotion = totalLessThan1 - firstYearProportion;
            var proportionsSumToLessThanOne = new XYCurveDTO
            {
                Points = new[] {new CurvePointDTO {X = 1, Y = firstYearProportion}, new CurvePointDTO {X = 2, Y = secondYearPropotion}}
            };
            
            Assert.That(CurveHelpers.IsValidSpendProfile(proportionsSumToLessThanOne), Is.False);
        }

        [Test]
        public void IsValidSpendProfile_SpendCurveProportionIsNegative_ReturnsFalse()
        {
            var nonPositiveProportionInSpendProfile = new XYCurveDTO
            {
                Points = new[] {new CurvePointDTO {X = 1, Y = 0.7}, new CurvePointDTO {X = 2, Y = 0.7}, new CurvePointDTO {X = 3, Y = -0.4}}
            };
            
            Assert.That(CurveHelpers.IsValidSpendProfile(nonPositiveProportionInSpendProfile), Is.False);
        }
        
        [Test]
        public void GetUnits_SpendCurveHasVariableProportions_ReturnsVariableCosts()
        {
            var firstYearProportion = 0.7;
            var secondYearPropotion = 1.0 - firstYearProportion;
            var variableSpendProfile = new XYCurveDTO
            {
                Points = new[] {new CurvePointDTO {X = 1, Y = firstYearProportion}, new CurvePointDTO {X = 2, Y = secondYearPropotion}}
            };
            DataPrep.SetConstructorParameter(fixture, "p_AssetTypeAnnualSpendProfileCurve", variableSpendProfile);
            var timeInvariantDataVariableSpend = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var startTimeAtEndOfSpend = FormulaBase.GetCalendarDateTime(ArbitraryStartYear + 1, 1);
            var periodWithFirstSpendInStartFiscalYear = new TimePeriodDTO {StartTime = startTimeAtEndOfSpend, DurationInMonths = 0};
            DataPrep.SetConstructorParameter(fixture, "p_TimePeriod", periodWithFirstSpendInStartFiscalYear);
            var timeVaryingStartsAtEndOfSpendProfile = new[] {fixture.Create<baseClass.TimeVariantInputDTO>()};

            var results = formulas.GetUnits(ArbitraryStartYear, ThreeYearsInMonths, timeInvariantDataVariableSpend, timeVaryingStartsAtEndOfSpendProfile);            

            var totalSpend = FormulaBase.LoadedReplacementCost(ReplacementCost, BurdenFactor ?? 1.0, CostVariationFactor);
            
            Assert.That(results[0], Is.EqualTo(firstYearProportion * totalSpend / CommonConstants.MonthsPerYear).Within(CommonConstants.DoubleDifferenceTolerance));
            Assert.That(results[12], Is.EqualTo(secondYearPropotion * totalSpend / CommonConstants.MonthsPerYear).Within(CommonConstants.DoubleDifferenceTolerance));
        }  
        
        [Test]
        public void GetUnits_SpendCurveHasGaps_ReturnsCostsWithMatchingGaps()
        {
            DataPrep.SetConstructorParameter(fixture, "p_AssetTypeAnnualSpendProfileCurve", TwoSpendsWithOneYearGapProfile);
            var timeInvariantDataVariableSpend = fixture.Create<baseClass.TimeInvariantInputDTO>();

            var fiscalYearOfLastSpend = ArbitraryStartYear + (int) TwoSpendsWithOneYearGapProfile.Points.Last().X - 1;
            var startTimeAtEndOfSpend = FormulaBase.GetCalendarDateTime( fiscalYearOfLastSpend, 1);
            var periodSoFirstSpendIsStartFiscalYearDto = new TimePeriodDTO {StartTime = startTimeAtEndOfSpend, DurationInMonths = 0};
            DataPrep.SetConstructorParameter(fixture, "p_TimePeriod", periodSoFirstSpendIsStartFiscalYearDto);
            var timeVaryingStartsAtEndOfSpendProfile = new[] {fixture.Create<baseClass.TimeVariantInputDTO>()};

            var results = formulas.GetUnits(ArbitraryStartYear, ThreeYearsInMonths, timeInvariantDataVariableSpend, timeVaryingStartsAtEndOfSpendProfile);

            var totalSpend = FormulaBase.LoadedReplacementCost(ReplacementCost, BurdenFactor ?? 1.0, CostVariationFactor);
            var firstYearSpendMonthlySpend = TwoSpendsWithOneYearGapProfile.Points.First().Y * totalSpend/CommonConstants.MonthsPerYear;
            
            Assert.That(results[0], Is.EqualTo(firstYearSpendMonthlySpend).Within(CommonConstants.DoubleDifferenceTolerance));
            
            Assert.That(results[13], Is.Null);

            var monthlySpendInFinalSpendYear = TwoSpendsWithOneYearGapProfile.Points.Last().Y * totalSpend / CommonConstants.MonthsPerYear;
            Assert.That(results[(fiscalYearOfLastSpend - ArbitraryStartYear) * CommonConstants.MonthsPerYearInt],
                Is.EqualTo(monthlySpendInFinalSpendYear).Within(CommonConstants.DoubleDifferenceTolerance));
        }

        [Test]
        public void GetUnits_SpendStartPreceedsStartFiscalYear_TruncatedResults()
        {

            DataPrep.SetConstructorParameter(fixture, "p_AssetTypeAnnualSpendProfileCurve", TwoSpendsWithOneYearGapProfile);
            var timeInvariantDataVariableSpend = fixture.Create<baseClass.TimeInvariantInputDTO>();

            var startYearPlusNumYearsSpend = ArbitraryStartYear + (int) TwoSpendsWithOneYearGapProfile.Points.Last().X - 1;
            var startTimeAtEndOfSpend = FormulaBase.GetCalendarDateTime( startYearPlusNumYearsSpend - 1, 1);
            var periodSoFirstSpendIsBeforeStartFiscalYear = new TimePeriodDTO {StartTime = startTimeAtEndOfSpend, DurationInMonths = 0};
            DataPrep.SetConstructorParameter(fixture, "p_TimePeriod", periodSoFirstSpendIsBeforeStartFiscalYear);
            var timeVaryingStartsAtEndOfSpendProfile = new[] {fixture.Create<baseClass.TimeVariantInputDTO>()};

            var results = formulas.GetUnits(ArbitraryStartYear, ThreeYearsInMonths, timeInvariantDataVariableSpend, timeVaryingStartsAtEndOfSpendProfile);
            
            var totalSpend = FormulaBase.LoadedReplacementCost(ReplacementCost, BurdenFactor ?? 1.0, CostVariationFactor);
            
            Assert.That(results[0], Is.Null);
            
            var monthlySpendInFinalSpendYear = TwoSpendsWithOneYearGapProfile.Points.Last().Y * totalSpend / CommonConstants.MonthsPerYear;
            Assert.That(results[(startYearPlusNumYearsSpend - ArbitraryStartYear - 1) * CommonConstants.MonthsPerYearInt],
                Is.EqualTo(monthlySpendInFinalSpendYear).Within(CommonConstants.DoubleDifferenceTolerance));
        }

        [Test]
        public void GetUnits_LastSpendPastEndOfResults_TruncatedResults()
        {
            DataPrep.SetConstructorParameter(fixture, "p_AssetTypeAnnualSpendProfileCurve", TwoSpendsWithOneYearGapProfile);
            var timeInvariantDataVariableSpend = fixture.Create<baseClass.TimeInvariantInputDTO>();

            var startYearPlusNumYearsSpend = ArbitraryStartYear + (int) TwoSpendsWithOneYearGapProfile.Points.Last().X - 1;
            var startTimeAtEndOfSpend = FormulaBase.GetCalendarDateTime( startYearPlusNumYearsSpend + 1, 1);
            var periodSoLastSpendAfterResultsArray = new TimePeriodDTO {StartTime = startTimeAtEndOfSpend, DurationInMonths = 0};
            DataPrep.SetConstructorParameter(fixture, "p_TimePeriod", periodSoLastSpendAfterResultsArray);
            var timeVaryingStartsAtEndOfSpendProfile = new[] {fixture.Create<baseClass.TimeVariantInputDTO>()};

            var results = formulas.GetUnits(ArbitraryStartYear, ThreeYearsInMonths, timeInvariantDataVariableSpend, timeVaryingStartsAtEndOfSpendProfile);
            
            var totalSpend = FormulaBase.LoadedReplacementCost(ReplacementCost, BurdenFactor ?? 1.0, CostVariationFactor);
            
            Assert.That(results[0], Is.Null);
            
            var monthlySpendInFinalSpendYear = TwoSpendsWithOneYearGapProfile.Points.First().Y * totalSpend / CommonConstants.MonthsPerYear;
            Assert.That(results[CommonConstants.MonthsPerYearInt],
                Is.EqualTo(monthlySpendInFinalSpendYear).Within(CommonConstants.DoubleDifferenceTolerance));            
        }

        [Test]
        public void GetUnits_MultipleAssetReplacementsUsingTwoYearSpend_ReturnsCorrectSpend()
        {
            var spendLengthYears = (int) TwoYearSimpleSpendProfileCurve.Points.Last().X;
            var startTimeAStartOfSpend = FormulaBase.GetCalendarDateTime( ArbitraryStartYear + spendLengthYears - 1, 1);
            var firstTimePeriod = new TimePeriodDTO
            {
                DurationInMonths = 0,
                StartTime = startTimeAStartOfSpend
            };
            var secondTimePeriod = new TimePeriodDTO
            {
                DurationInMonths = 0,
                StartTime = firstTimePeriod.StartTime.AddYears(1)
            };
            
            var timeVariantDataList = new List<baseClass.TimeVariantInputDTO>
            {
                new baseClass.TimeVariantInputDTO(firstTimePeriod),
                new baseClass.TimeVariantInputDTO(secondTimePeriod)
            };            
            
            var results = new formulaClass().GetUnits(ArbitraryStartYear, ThreeYearsInMonths, TimeInvariantInput, timeVariantDataList);

            var monthlySpendPerSpend = FormulaBase.LoadedReplacementCost(ReplacementCost, BurdenFactor ?? 1.0, CostVariationFactor) *
                                       TwoYearSimpleSpendProfileCurve.Points.First().Y / CommonConstants.MonthsPerYear;
            
            Assert.That(results[0], Is.EqualTo(monthlySpendPerSpend).Within(CommonConstants.DoubleDifferenceTolerance));
            Assert.That(results[12], Is.EqualTo(2 * monthlySpendPerSpend).Within(CommonConstants.DoubleDifferenceTolerance));
            Assert.That(results[24], Is.EqualTo(monthlySpendPerSpend).Within(CommonConstants.DoubleDifferenceTolerance));
        }
    }
}
