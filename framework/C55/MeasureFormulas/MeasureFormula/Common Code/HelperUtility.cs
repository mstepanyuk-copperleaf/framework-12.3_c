using System;
using System.Collections.Generic;
using System.Linq;
using CL.FormulaHelper;
using CL.FormulaHelper.DTOs;
using MeasureFormula.SharedCode;

namespace MeasureFormula.Common_Code
{
    public static class HelperUtility
    {
        public static DateTime? FindLastInServiceCalendarDate(double?[] conditions, int startFiscalYear, double bestCondition)
        {
            if (conditions == null) return null;
            
            var lastBestConditionOffset = Array.FindLastIndex(conditions, x => Equals(x, bestCondition));
            if (lastBestConditionOffset  < 0) return null;

            return ConvertMonthOffsetToCalendarDateTime(startFiscalYear, lastBestConditionOffset );
        }

        public static DateTime ConvertMonthOffsetToCalendarDateTime(int startFiscalYear, int monthOffset)
        {
            var year = startFiscalYear + monthOffset / CommonConstants.MonthsPerYearInt;
            var month = monthOffset % CommonConstants.MonthsPerYearInt + 1;
            return FormulaBase.GetCalendarDateTime(year, month);
        }

        public static int? LastInServiceMonthOffset(double?[] conditions, DateTime? inServiceDate, int startFiscalYear, double bestConditionScore)
        {
            var inServiceCalendarDate = FindLastInServiceCalendarDate(conditions, startFiscalYear, bestConditionScore);
            if (!inServiceCalendarDate.HasValue)
            {
                if (inServiceDate.HasValue)
                {
                    inServiceCalendarDate = inServiceDate;
                }
            }
            if (!inServiceCalendarDate.HasValue)
            {
                return null;
            }
            return FormulaBase.ConvertDateTimeToOffset(inServiceCalendarDate.Value, startFiscalYear);
        }

        public static bool IsFirstReplacement(double?[] conditions, double bestCondition)
        {
            if (conditions != null)
            {
                return !conditions.Any(x => Equals(x, bestCondition));
            }
            return true;
        }
        
        public static double?[] CalculateNormalizedAgeBaseline(int startFiscalYear, int months, DateTime startOfConditionObservation,
            int assetUsefulLifetimeInYears, List<ConditionScoreAnswerDto> conditionScores, double? bestConditionScore)
        {
            return CalculateNormalizedAge(startFiscalYear, months, startOfConditionObservation, assetUsefulLifetimeInYears, 
                conditionScores, bestConditionScore, isBaselineFormula: true);
        }

        public static double?[] CalculateNormalizedAgeOutcome(int startFiscalYear, int months, DateTime startOfConditionObservation,
            int assetUsefulLifetimeInYears, List<ConditionScoreAnswerDto> conditionScores, double? bestConditionScore)
        {
            return CalculateNormalizedAge(startFiscalYear, months, startOfConditionObservation, assetUsefulLifetimeInYears,
                conditionScores, bestConditionScore, isBaselineFormula: false);
        }
        
        public static double?[] CalculateNormalizedAge(int startFiscalYear, int months, DateTime startOfConditionObservation,
            int assetUsefulLifetimeInYears, List<ConditionScoreAnswerDto> conditionScores, double? bestConditionScore, bool isBaselineFormula)
        {
            if (!bestConditionScore.HasValue) return null;
            
            var lifeTimeReciprocalInMonths = 1.0 / (assetUsefulLifetimeInYears * CommonConstants.MonthsPerYear);
            var startOfConditionObservationOffset = FormulaBase.ConvertDateTimeToOffset(startOfConditionObservation, startFiscalYear);
            var ageInMonths = 0;

            // If the in-service date or first condition observation is prior to startFiscalYear,
            // determine the age for offset zero
            // and set the place to start recording normalized age to zero
            if (startOfConditionObservationOffset < 0)
            {
                ageInMonths = -startOfConditionObservationOffset;
                startOfConditionObservationOffset = 0;
            }

            //if user did not answer any condition scores, we can just have a simplified function to calculate age
            if (conditionScores == null || conditionScores.Count < 1)
            {
                return CalculateNormalizedAgeWithNoReplacements(months, startOfConditionObservationOffset, ageInMonths, lifeTimeReciprocalInMonths);
            }
            
            // might be past subsequent condition scores
            // find last one that is on or before start of fiscal year
            Predicate<ConditionScoreAnswerDto> lastBestConditionBeforeStart = x =>
                FormulaBase.ConvertDateTimeToOffset(x.Date, startFiscalYear) <= 0 &&
                Math.Abs(x.ConditionScore - bestConditionScore.Value) < CommonConstants.DoubleDifferenceTolerance;

            var lastIndexOfBestConditionBeforeStart = conditionScores.FindLastIndex(lastBestConditionBeforeStart);
            // when isBaselineFormula the ageInMonths was set from an assetInServiceDate in calling this, not the first ConditionScore
            var nominalIndexOfStartOfConditionObservation = isBaselineFormula ? -1 : 0;  
            if (lastIndexOfBestConditionBeforeStart > nominalIndexOfStartOfConditionObservation)
            {
                var mostRecentPastConditionOffset = FormulaBase.ConvertDateTimeToOffset(conditionScores[lastIndexOfBestConditionBeforeStart].Date, startFiscalYear);
                ageInMonths = -mostRecentPastConditionOffset;
                startOfConditionObservationOffset = 0;
            }

            return CalculateNormalizedAgeWithReplacements(startFiscalYear, months, startOfConditionObservationOffset, 
                ageInMonths, lifeTimeReciprocalInMonths, conditionScores, bestConditionScore.Value, isBaselineFormula);
        }

        private static double?[] CalculateNormalizedAgeWithReplacements(int startFiscalYear, int months, int startingMonth, int ageInMonths,
            double lifeTimeReciprocalInMonths, List<ConditionScoreAnswerDto> conditionScores, double bestConditionScore, bool isBaselineFormula)
        {
            var nowMonthOffset = FormulaBase.ConvertDateTimeToOffset(DateTime.Now, startFiscalYear);
            var currentAnswerIndex = 0;
            while (currentAnswerIndex + 1 < conditionScores.Count &&
                   FormulaBase.ConvertDateTimeToOffset(conditionScores[currentAnswerIndex].Date, startFiscalYear) <= startingMonth)
            {
                currentAnswerIndex++;
            }
            var currentConditionScore = conditionScores[currentAnswerIndex];
            var currentAnswerMonthOffset = FormulaBase.ConvertDateTimeToOffset(currentConditionScore.Date, startFiscalYear);
            ConditionScoreAnswerDto previousConditionScore = null;
            
            var ret = new double?[months];

            for (var indexOffset = startingMonth; indexOffset < months; indexOffset++, ageInMonths++)
            {
                if (indexOffset == currentAnswerMonthOffset)
                {
                    var assetWasReplaced = WasAssetReplaced(bestConditionScore, currentConditionScore,
                        previousConditionScore, indexOffset, nowMonthOffset, isBaselineFormula);

                    if (assetWasReplaced)
                    {
                        ageInMonths = 0;
                    }

                    //point the monthOffset to the next timeVariant tab's start date
                    previousConditionScore = currentConditionScore;
                    currentConditionScore = ++currentAnswerIndex < conditionScores.Count ? conditionScores[currentAnswerIndex] : null;
                    currentAnswerMonthOffset = currentConditionScore != null
                        ? FormulaBase.ConvertDateTimeToOffset(currentConditionScore.Date, startFiscalYear)
                        : Int32.MaxValue;
                }

                ret[indexOffset] = ageInMonths * lifeTimeReciprocalInMonths;
            }

            return ret;
        }

        private static bool WasAssetReplaced(double bestConditionScore, ConditionScoreAnswerDto currentConditionScore, 
            ConditionScoreAnswerDto previousConditionScore, int indexOffset, int nowMonthOffset, bool isBaselineFormula)
        {
            var currentMonthHasBestCondition = currentConditionScore != null &&
                Math.Abs(currentConditionScore.ConditionScore - bestConditionScore) <= CommonConstants.DoubleDifferenceTolerance;

            bool assetWasReplaced;
            if (isBaselineFormula)
            {
                //we are going to ignore consecutive 10s in the baseline condition scores except for the 
                //first 10 in that consecutive series
                var previousConditionScoreIsNotBest = previousConditionScore == null ||
                                                      previousConditionScore.ConditionScore < bestConditionScore;
                assetWasReplaced = currentMonthHasBestCondition && (previousConditionScoreIsNotBest || indexOffset > nowMonthOffset);
            }
            else
            {
                assetWasReplaced = currentMonthHasBestCondition;
            }

            return assetWasReplaced;
        }

        public static double?[] CalculateNormalizedAgeWithNoReplacements(int months, int startOfConditionObservationOffset, 
            int ageInMonths, double lifeTimeReciprocalInMonths)
        {
            var ret = new double?[months];

            for (var indexOffset = startOfConditionObservationOffset; indexOffset < months; indexOffset++, ageInMonths++)
            {
                ret[indexOffset] = ageInMonths * lifeTimeReciprocalInMonths;
            }

            return ret;
        }

        public static double?[] CalculateLostEfficiencyOpportunity(int startFiscalYear, int months, double? assetAnnualDegradation,
            double? assetTechnologyImprovement, double? conditionScoreBest, ConsequenceGroupDTO assetGenerationGroup, 
            TimeSeriesDTO energyValues, TimeSeriesDTO avoidedCO2, double?[] conditionOutput, int startOffset, int? endOffset)
        {
            var missingRequiredInputs = assetGenerationGroup == null || 
                                        assetGenerationGroup.UnitCapacity == null ||
                                        conditionScoreBest == null || 
                                        energyValues == null ||
                                        energyValues.BaseYear == null;

            if (missingRequiredInputs) return null;

            var annualDegradation = assetAnnualDegradation / CommonConstants.PercentPerProbabilityOne ?? 0d;
            //----- Support multiple replacements... Only include technology improvement if this is the first replacement...
            var technologyImprovement = ConditionHelpers.IsFirstReplacement(conditionOutput, conditionScoreBest.Value)
                ? (assetTechnologyImprovement / CommonConstants.PercentPerProbabilityOne) ?? 0d : 0d;

            var unitCapacity = assetGenerationGroup.UnitCapacity;
            var energyBaseYear = energyValues.BaseYear.Value;

            var ret = new double?[months];
            for (var i = Math.Max(0, startOffset); i < months; i++)
            {
                if (i < (energyBaseYear - startFiscalYear) * CommonConstants.MonthsPerYearInt) continue;

                if ((endOffset.HasValue && i >= endOffset) || !endOffset.HasValue)
                {
                    var co2Value = AvoidedCo2InDollarsPerMWh(startFiscalYear, avoidedCO2, i); 
                    var energyValue = energyValues.GetMonthlyValue(startFiscalYear, i) + co2Value;
                    var age = (i - startOffset) / CommonConstants.MonthsPerYear;
                    ret[i] = (age * annualDegradation + technologyImprovement) * unitCapacity * energyValue / CommonConstants.MonthsPerYear;
                }
                else if (i < endOffset)
                {
                    ret[i] = 0d;
                }
            }

            return ret;
        }

        public static double AvoidedCo2InDollarsPerMWh(int startFiscalYear, TimeSeriesDTO avoidedCO2TimeSeries, int monthIndex)
        {
            return avoidedCO2TimeSeries == null ? 0.0 : HelperFunctions.GetMonthlyValue(avoidedCO2TimeSeries, startFiscalYear, monthIndex);
        }
    }
}
