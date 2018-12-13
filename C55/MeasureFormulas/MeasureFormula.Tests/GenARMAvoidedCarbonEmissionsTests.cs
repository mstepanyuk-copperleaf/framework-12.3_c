using System;
using System.Collections.Generic;
using AutoFixture;
using MeasureFormula.SharedCode;
using MeasureFormula.TestHelpers;
using NUnit.Framework;

using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.GenARMAvoidedCarbonEmissionsBase;
using formulaClass = CustomerFormulaCode.GenARMAvoidedCarbonEmissions;
 
namespace MeasureFormula.Tests
{
    [TestFixture]
    public class GenARMAvoidedCarbonEmissionsTests : AssetModelTestsBase 
    {
        private readonly formulaClass _formulas = new formulaClass();
        private baseClass.TimeInvariantInputDTO _timeInvariantInput;
        private IReadOnlyList<baseClass.TimeVariantInputDTO> _timeVariantInput;

        private static double?[] MakeDecayingConditions(int months, double initialValue, double finalValue)
        {
            var conditions = new double?[months];
            var monthlyDecay = (initialValue - finalValue) / months;
            var monthlyCondition = initialValue;
            for (var month = 0; month < months; month++)
            {
                conditions[month] = monthlyCondition;
                monthlyCondition -= monthlyDecay;
            }
            return conditions;
        }
        
        [SetUp]
        public void FixtureSetup()     
        {
            DataPrep.SetConstructorParameter(fixture, "p_ConditionToFailureCurve", ConditionHelpers.ConstructConditionDecayCurve(50.0));
            DataPrep.SetConstructorParameter(fixture, "p_GenARM_Condition_ConsqUnitOutput", MakeDecayingConditions(ArbitraryMonths, BestConditionScore, 5.0));
            DataPrep.SetConstructorParameter(fixture, "p_GenARM_Condition_ConsqUnitOutput_B", MakeDecayingConditions(ArbitraryMonths, BestConditionScore - 3.0, 2.0));
            DataPrep.SetConstructorParameter(fixture, "p_IgnoreLGR", false);
            
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
            Assert.DoesNotThrow(() =>
            {
                nullCheck.RunNullTestsIncludingCustomFields(
                    _timeInvariantInput,
                    _timeVariantInput,
                    getUnitsCall);
            });
            
            var nulledConsequenceGroups = MakeNulledConsequenceGroups(MakeNonNullConsequenceGroup());
            foreach (var consequenceGroup in nulledConsequenceGroups)
            {
                DataPrep.SetConstructorParameter(fixture, "p_AssetGenerationGroup", consequenceGroup);
                _timeInvariantInput = fixture.Create<baseClass.TimeInvariantInputDTO>();
                Assert.DoesNotThrow(() =>
                {
                    nullCheck.RunNullTestsIncludingCustomFields(
                        _timeInvariantInput,
                        _timeVariantInput,
                        getUnitsCall);
                });                
            }            
        }
    }
}
