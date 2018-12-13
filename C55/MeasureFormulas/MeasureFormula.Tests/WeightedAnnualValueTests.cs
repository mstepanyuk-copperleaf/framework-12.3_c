using System;
using System.Collections.Generic;
using AutoFixture;
using MeasureFormula.TestHelpers;
using NUnit.Framework;

using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.WeightedAnnualValueBase;
using formulaClass = CustomerFormulaCode.WeightedAnnualValue;
 
namespace MeasureFormula.Tests
{
    [TestFixture]
    public class WeightedAnnualValueTests : AssetModelTestsBase
    {
        private readonly formulaClass _formulas = new formulaClass();
        private baseClass.TimeInvariantInputDTO _timeInvariantInput;
        private IReadOnlyList<baseClass.TimeVariantInputDTO> _timeVariantInput;

        [SetUp]
        public void FixtureSetup()
        {
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
