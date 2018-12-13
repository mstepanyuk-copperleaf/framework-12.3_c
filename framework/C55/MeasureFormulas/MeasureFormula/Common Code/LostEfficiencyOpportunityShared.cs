using System;
using System.Collections.Generic;
using System.Linq;
using CL.FormulaHelper;
using CL.FormulaHelper.DTOs;
using MeasureFormula.SharedCode;

namespace MeasureFormula.Common_Code
{
    public static class LostEfficiencyOpportunityShared
    {
        public static int OffsetFromBaseYearWithStartFiscalYearDefault(TimeSeriesDTO timeSeries, int startFiscalYear)
        {
            var energyBaseYear = timeSeries.BaseYear ?? startFiscalYear;
            return Math.Max(0, (energyBaseYear - startFiscalYear) * CommonConstants.MonthsPerYearInt);
        }

        public static IEnumerable<int> AgeResetOffsets(int startFiscalYear, ConditionScoreAnswerDto[] conditionScoreAnswers, double bestConditionScore)
        {
            var bestScoreAnswers = conditionScoreAnswers.Where(x => ConditionHelpers.IsBestConditionScore(x.ConditionScore, bestConditionScore)).ToList();

            var ageResetOffsets = bestScoreAnswers.Select(x => FormulaBase.ConvertDateTimeToOffset(x.Date, startFiscalYear));
            return ageResetOffsets;
        }

        public static double MonthlyLostEfficiencyOpportunity(double monthlyEnergyValue, double co2CostPerMWh, int ageInMonths, double annualDegradation,
            double unitCapacity)
        {
            var monthlyValue = co2CostPerMWh + monthlyEnergyValue;
            var degradation = ageInMonths * annualDegradation / CommonConstants.MonthsPerYear;
            var monthlyLostEfficiencyOpportunity = (degradation * unitCapacity) / CommonConstants.MonthsPerYear * monthlyValue;
            
            return monthlyLostEfficiencyOpportunity;
        }

        public static double MonthlyLostTechImprovement(double monthlyEnergyValue, double co2CostPerMWh, double unitCapacity, double techImprovementFraction)
        {
            var monthlyValue = co2CostPerMWh + monthlyEnergyValue;
            return techImprovementFraction * unitCapacity * monthlyValue / CommonConstants.MonthsPerYear;
        }

        public static double Co2CostPerMWh(decimal? strategyAlternativeAvoidedCo2Value, double? strategyAlternativeAvoidedCo2Emissions)
        {
            var co2CostDollarsPerTon = (double?) strategyAlternativeAvoidedCo2Value ?? 0.0;
            var co2EmissionsTonsPerMWh = strategyAlternativeAvoidedCo2Emissions ?? 0.0;
            return co2CostDollarsPerTon * co2EmissionsTonsPerMWh;
        }
        
        public static void PopulateLostEfficiencyOpportunity(int startFiscalYear, int months, int[] ageResetOffsetsAfterEnergyValuesKnown,
            int energyValuesAvailableOffset, TimeSeriesDTO energyValuesDollarsPerMWh, TimeSeriesDTO avoidedCo2TimeSeries, double annualDegradation, double unitCapacity,
            double?[] lostEfficiencyOpportunity)
        {
            var resetIndex = 0;
            var resetOffset = ageResetOffsetsAfterEnergyValuesKnown[resetIndex];

            var degradationStartIndex = Math.Max(energyValuesAvailableOffset, resetOffset);
            var ageInMonthsAtDegradationStartIndex = degradationStartIndex - resetOffset;

            resetIndex++;
            resetOffset = resetIndex < ageResetOffsetsAfterEnergyValuesKnown.Length ? ageResetOffsetsAfterEnergyValuesKnown[resetIndex] : months;

            for (int monthOffset = degradationStartIndex, ageInMonths = ageInMonthsAtDegradationStartIndex; monthOffset < months; monthOffset++, ageInMonths++)
            {
                if (monthOffset == resetOffset)
                {
                    resetIndex++;
                    resetOffset = resetIndex < ageResetOffsetsAfterEnergyValuesKnown.Length ? ageResetOffsetsAfterEnergyValuesKnown[resetIndex] : months;
                    ageInMonths = 0;
                }

                var thisMonthEnergyValueDollarsPerMWh = energyValuesDollarsPerMWh.GetMonthlyValue(startFiscalYear, monthOffset);
                var co2CostPerMWh = avoidedCo2TimeSeries == null
                    ? 0.0
                    : HelperFunctions.GetMonthlyValue(avoidedCo2TimeSeries, startFiscalYear, monthOffset);
                var monthlyLostEfficiencyOpportunity = MonthlyLostEfficiencyOpportunity(thisMonthEnergyValueDollarsPerMWh,
                    co2CostPerMWh,
                    ageInMonths, annualDegradation, unitCapacity);

                lostEfficiencyOpportunity[monthOffset] = monthlyLostEfficiencyOpportunity;
            }
        }
    }
}
