using AutoFixture;
using CL.FormulaHelper;
using CL.FormulaHelper.DTOs;
using NUnit.Framework;
using MeasureFormula.TestHelpers;

namespace MeasureFormula.Tests
{
    [TestFixture]
    public class MeasureFormulaTestsBase
    {
        protected Fixture fixture;

        protected int ArbitraryStartYear;
        protected int ArbitraryMonths;

        protected TimePeriodDTO PeriodFromBeginningForAllTime;
        
        protected const double BestConditionScore = 10.0;

        [SetUp]
        public void RunBeforeEachTest()
        {
            // Match what the customer uses
            FormulaBase.FiscalYearEndMonth = 9;
            
            fixture = new Fixture();
            
            ArbitraryStartYear = 1900 + fixture.Create<int>() % 100;
            ArbitraryMonths = 24 + fixture.Create<int>() % 20;
            
            PeriodFromBeginningForAllTime = new TimePeriodDTO {StartTime = FormulaBase.GetCalendarDateTime(ArbitraryStartYear, 1), DurationInMonths = null};
            DataPrep.SetConstructorParameter(fixture, "p_TimePeriod", PeriodFromBeginningForAllTime);            
        }
    }
}
