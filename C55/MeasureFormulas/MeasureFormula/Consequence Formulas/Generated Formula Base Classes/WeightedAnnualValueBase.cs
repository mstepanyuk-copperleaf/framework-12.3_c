// GENERATED CODE - DO NOT EDIT !!!
using System.Collections.Generic;
using CL.FormulaHelper;
using CL.FormulaHelper.Attributes;
using CL.FormulaHelper.DTOs;
using System.Runtime.Serialization;

namespace MeasureFormulas.Generated_Formula_Base_Classes
{
    [FormulaBase]
    public abstract class WeightedAnnualValueBase : FormulaConsequenceBase
    {
        [DataContract]
        public class TimeInvariantInputDTO
        {
            public TimeInvariantInputDTO(
                CL.FormulaHelper.DTOs.TimeSeriesDTO p_AnalyticsStrategyAlternativeAvoidedCO2Values,
                CL.FormulaHelper.DTOs.TimeSeriesDTO p_AnalyticsStrategyAlternativeEnergyValues,
                System.Boolean? p_AssetContributes_32_to_32_Lost_32_Generation,
                CL.FormulaHelper.DTOs.ConsequenceGroupDTO p_AssetGenerationGroup,
                CL.FormulaHelper.DTOs.TimeSeriesDTO p_SystemAvoidedCO2Values,
                CL.FormulaHelper.DTOs.TimeSeriesDTO p_SystemEnergyValues)
            {
                AnalyticsStrategyAlternativeAvoidedCO2Values = p_AnalyticsStrategyAlternativeAvoidedCO2Values;
                AnalyticsStrategyAlternativeEnergyValues = p_AnalyticsStrategyAlternativeEnergyValues;
                AssetContributes_32_to_32_Lost_32_Generation = p_AssetContributes_32_to_32_Lost_32_Generation;
                AssetGenerationGroup = p_AssetGenerationGroup;
                SystemAvoidedCO2Values = p_SystemAvoidedCO2Values;
                SystemEnergyValues = p_SystemEnergyValues;
            }
            
            [CustomFieldInput("AvoidedCO2Values", FormulaInputAssociatedEntity.AnalyticsStrategyAlternative)]
            [DataMember]
            public CL.FormulaHelper.DTOs.TimeSeriesDTO AnalyticsStrategyAlternativeAvoidedCO2Values  { get; private set; }
            
            [CustomFieldInput("EnergyValues", FormulaInputAssociatedEntity.AnalyticsStrategyAlternative)]
            [DataMember]
            public CL.FormulaHelper.DTOs.TimeSeriesDTO AnalyticsStrategyAlternativeEnergyValues  { get; private set; }
            
            [CustomFieldInput("Contributes to Lost Generation", FormulaInputAssociatedEntity.Asset)]
            [DataMember]
            public System.Boolean? AssetContributes_32_to_32_Lost_32_Generation  { get; private set; }
            
            [CoreFieldInput(FormulaCoreFieldInputType.AssetGenerationGroup)]
            [DataMember]
            public CL.FormulaHelper.DTOs.ConsequenceGroupDTO AssetGenerationGroup  { get; private set; }
            
            [CustomFieldInput("AvoidedCO2Values", FormulaInputAssociatedEntity.System)]
            [DataMember]
            public CL.FormulaHelper.DTOs.TimeSeriesDTO SystemAvoidedCO2Values  { get; private set; }
            
            [CustomFieldInput("EnergyValues", FormulaInputAssociatedEntity.System)]
            [DataMember]
            public CL.FormulaHelper.DTOs.TimeSeriesDTO SystemEnergyValues  { get; private set; }
        }
        
        [DataContract]
        public class TimeVariantInputDTO : ITimeVariantInputDTO
        {
            public TimeVariantInputDTO(
                CL.FormulaHelper.DTOs.TimePeriodDTO p_TimePeriod)
            {
                TimePeriod = p_TimePeriod;
            }
            
            [CoreFieldInput(FormulaCoreFieldInputType.TimePeriod)]
            [DataMember]
            public CL.FormulaHelper.DTOs.TimePeriodDTO TimePeriod  { get; private set; }
        }
        
        public abstract double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData);
            
        public abstract double?[] GetZynos(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData,
            IReadOnlyList<TimeVariantInputDTO> timeVariantData,
            double?[] unitOutput);
        
        ///
        /// Class to enable formula debugging
        ///
        [DataContract]
        public class FormulaParams : IFormulaParams
        {
            public FormulaParams(CL.FormulaHelper.MeasureOutputType measureOutputType,
                string measureName,
                long alternativeId,
                string formulaImplClassName,
                bool isProbabilityFormula,
                int fiscalYearEndMonth,
                int startFiscalYear,
                int months,
                TimeInvariantInputDTO timeInvariantData,
                IReadOnlyList<TimeVariantInputDTO> timeVariantData,
                double?[] unitOutput,
                object formulaOutput,
                string exceptionMessage)
            {
                MeasureOutputType = measureOutputType;
                MeasureName = measureName;
                AlternativeId = alternativeId;
                FormulaImplClassName = formulaImplClassName;
                IsProbabilityFormula = isProbabilityFormula;
                FiscalYearEndMonth = fiscalYearEndMonth;
                StartFiscalYear = startFiscalYear;
                Months = months;
                TimeInvariantData = timeInvariantData;
                TimeVariantData = timeVariantData;
                UnitOutput = unitOutput;
                FormulaOutput = formulaOutput;
                ExceptionMessage = exceptionMessage;
            }
            [DataMember(Order = 0)]
            public CL.FormulaHelper.MeasureOutputType MeasureOutputType { get; set; }
            [DataMember(Order = 1)]
            public string MeasureName { get; set; }
            [DataMember(Order = 2)]
            public long AlternativeId { get; set; }
            [DataMember(Order = 3)]
            public string FormulaImplClassName { get; set; }
            [DataMember(Order = 4)]
            public bool IsProbabilityFormula { get; set; }
            [DataMember(Order = 5)]
            public int FiscalYearEndMonth { get; set; }
            [DataMember(Order = 6)]
            public int StartFiscalYear { get; set; }
            [DataMember(Order = 7)]
            public int Months { get; set; }
            [DataMember(Order = 8)]
            public TimeInvariantInputDTO TimeInvariantData { get; set; }
            [DataMember(Order = 9)]
            public IReadOnlyList<TimeVariantInputDTO> TimeVariantData { get; set; }
            [DataMember(Order = 10)]
            public double?[] UnitOutput { get; set; }
            [DataMember(Order = 11)]
            public object FormulaOutput { get; set; }
            [DataMember(Order = 12)]
            public string ExceptionMessage { get; set; }
        }
    }
}
// GENERATED CODE - DO NOT EDIT !!!
