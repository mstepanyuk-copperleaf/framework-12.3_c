// GENERATED CODE - DO NOT EDIT !!!
using System.Collections.Generic;
using CL.FormulaHelper;
using CL.FormulaHelper.Attributes;
using CL.FormulaHelper.DTOs;
using System.Runtime.Serialization;

namespace MeasureFormulas.Generated_Formula_Base_Classes
{
    [FormulaBase]
    public abstract class GenARMTurbineAgeDegradationBaselineBase : FormulaConsequenceBase
    {
        [DataContract]
        public class TimeInvariantInputDTO
        {
            public TimeInvariantInputDTO(
                CL.FormulaHelper.DTOs.TimeSeriesDTO p_AnalyticsStrategyAlternativeAvoidedCO2Values,
                CL.FormulaHelper.DTOs.TimeSeriesDTO p_AnalyticsStrategyAlternativeEnergyValues,
                System.Double? p_AssetAnnual_32_Degradation_32__37_,
                CL.FormulaHelper.DTOs.ConsequenceGroupDTO p_AssetGenerationGroup,
                System.DateTime? p_AssetInServiceDate,
                System.Double?[] p_GenARM_Condition_ConsqUnitOutput,
                CL.FormulaHelper.DTOs.TimeSeriesDTO p_SystemAvoidedCO2Values,
                System.Double? p_SystemCondition_32_Score_32_Best,
                System.Double? p_SystemCondition_32_Score_32_Worst,
                CL.FormulaHelper.DTOs.TimeSeriesDTO p_SystemEnergyValues)
            {
                AnalyticsStrategyAlternativeAvoidedCO2Values = p_AnalyticsStrategyAlternativeAvoidedCO2Values;
                AnalyticsStrategyAlternativeEnergyValues = p_AnalyticsStrategyAlternativeEnergyValues;
                AssetAnnual_32_Degradation_32__37_ = p_AssetAnnual_32_Degradation_32__37_;
                AssetGenerationGroup = p_AssetGenerationGroup;
                AssetInServiceDate = p_AssetInServiceDate;
                GenARM_Condition_ConsqUnitOutput = p_GenARM_Condition_ConsqUnitOutput;
                SystemAvoidedCO2Values = p_SystemAvoidedCO2Values;
                SystemCondition_32_Score_32_Best = p_SystemCondition_32_Score_32_Best;
                SystemCondition_32_Score_32_Worst = p_SystemCondition_32_Score_32_Worst;
                SystemEnergyValues = p_SystemEnergyValues;
            }
            
            [CustomFieldInput("AvoidedCO2Values", FormulaInputAssociatedEntity.AnalyticsStrategyAlternative)]
            [DataMember]
            public CL.FormulaHelper.DTOs.TimeSeriesDTO AnalyticsStrategyAlternativeAvoidedCO2Values  { get; private set; }
            
            [CustomFieldInput("EnergyValues", FormulaInputAssociatedEntity.AnalyticsStrategyAlternative)]
            [DataMember]
            public CL.FormulaHelper.DTOs.TimeSeriesDTO AnalyticsStrategyAlternativeEnergyValues  { get; private set; }
            
            [CustomFieldInput("Annual Degradation %", FormulaInputAssociatedEntity.Asset)]
            [DataMember]
            public System.Double? AssetAnnual_32_Degradation_32__37_  { get; private set; }
            
            [CoreFieldInput(FormulaCoreFieldInputType.AssetGenerationGroup)]
            [DataMember]
            public CL.FormulaHelper.DTOs.ConsequenceGroupDTO AssetGenerationGroup  { get; private set; }
            
            [CoreFieldInput(FormulaCoreFieldInputType.AssetInServiceDate)]
            [DataMember]
            public System.DateTime? AssetInServiceDate  { get; private set; }
            
            [MeasureInput("GenARM", "Condition", MeasureOutputType.ConsqUnitOutput, false)]
            [DataMember]
            public System.Double?[] GenARM_Condition_ConsqUnitOutput  { get; private set; }
            
            [CustomFieldInput("AvoidedCO2Values", FormulaInputAssociatedEntity.System)]
            [DataMember]
            public CL.FormulaHelper.DTOs.TimeSeriesDTO SystemAvoidedCO2Values  { get; private set; }
            
            [CustomFieldInput("Condition Score Best", FormulaInputAssociatedEntity.System)]
            [DataMember]
            public System.Double? SystemCondition_32_Score_32_Best  { get; private set; }
            
            [CustomFieldInput("Condition Score Worst", FormulaInputAssociatedEntity.System)]
            [DataMember]
            public System.Double? SystemCondition_32_Score_32_Worst  { get; private set; }
            
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
