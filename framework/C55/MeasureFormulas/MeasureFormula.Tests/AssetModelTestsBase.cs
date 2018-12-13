using System;
using CL.FormulaHelper.DTOs;
using AutoFixture;
using MeasureFormula.TestHelpers;
using NUnit.Framework;

namespace MeasureFormula.Tests
{
    [TestFixture]
    public class AssetModelTestsBase : MeasureFormulaTestsBase
    {
        [SetUp]
        public void AssetCommonFixtureSetup()
        {
            var energyValuesTimeSeries = DataPrep.CreateConstantTimeSeries(Math.Abs(fixture.Create<double>()));
            DataPrep.SetConstructorParameter(fixture, "p_SystemEnergyValues", energyValuesTimeSeries);
            
            var strategyAlternativeEnergyValues = DataPrep.CreateConstantTimeSeries(Math.Abs(fixture.Create<double>()));
            DataPrep.SetConstructorParameter(fixture, "p_AnalyticsStrategyAlternativeEnergyValues", strategyAlternativeEnergyValues);

            
            var avoidedCO2TimeSeries = DataPrep.CreateConstantTimeSeries(Math.Abs(fixture.Create<double>()));
            DataPrep.SetConstructorParameter(fixture, "p_SystemAvoidedCO2Values", avoidedCO2TimeSeries);
            
            var strategyAlternativeAvoidedCO2 = DataPrep.CreateConstantTimeSeries(Math.Abs(fixture.Create<double>()));            
            DataPrep.SetConstructorParameter(fixture, "p_AnalyticsStrategyAlternativeAvoidedCO2Values", strategyAlternativeAvoidedCO2);
        
           
            DataPrep.SetConstructorParameter(fixture, "p_AssetGenerationGroup", MakeNonNullConsequenceGroup());

            DataPrep.SetConstructorParameter(fixture, "p_AssetContributes_32_to_32_Lost_32_Generation", true);            
        }
        
        public ConsequenceGroupDTO MakeNonNullConsequenceGroup()
        {
            var capacity = Math.Abs(fixture.Create<double>());
            var priceValues = DataPrep.CreateConstantTimeSeries(Math.Abs(fixture.Create<double>()));
            var loss = Math.Abs(fixture.Create<double>());
            var consequenceGroup = new ConsequenceGroupDTO
            {
                UnitCapacity = capacity,
                PriceValues = priceValues,
                Loss = new[] {loss}
            };

            return consequenceGroup;
        }

        public ConsequenceGroupDTO[] MakeNulledConsequenceGroups(ConsequenceGroupDTO nonNullConsequenceGroup)
        {
            var nullUnitCapacity = new ConsequenceGroupDTO
            {
                UnitCapacity = null,
                PriceValues = nonNullConsequenceGroup.PriceValues,
                Loss = nonNullConsequenceGroup.Loss
            };
            
            var nullPriceValues = new ConsequenceGroupDTO
            {
                UnitCapacity = nonNullConsequenceGroup.UnitCapacity,
                PriceValues = null,
                Loss = nonNullConsequenceGroup.Loss
            };
            
            var nullLoss = new ConsequenceGroupDTO
            {
                UnitCapacity = nonNullConsequenceGroup.UnitCapacity,
                PriceValues = nonNullConsequenceGroup.PriceValues,
                Loss = null
            };            

            var nullConsequences = new[]
            {
                nullUnitCapacity, nullPriceValues, nullLoss
            };

            return nullConsequences;
        }        
    }
}
