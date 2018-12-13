// GENERATED CODE - DO NOT EDIT !!!
using System.Collections.Generic;
using CL.FormulaHelper;
using CL.FormulaHelper.Attributes;
using CL.FormulaHelper.DTOs;
using System.Runtime.Serialization;

namespace MeasureFormulas.Generated_Formula_Base_Classes
{
    [FormulaBase]
    public abstract class GenARMTurbineTechnologyImprovementOutcomeBase : FormulaConsequenceBase
    {
        [DataContract]
        public class TimeInvariantInputDTO
        {
            public TimeInvariantInputDTO(
                CL.FormulaHelper.DTOs.TimeSeriesDTO p_AnalyticsStrategyAlternativeAvoidedCO2Values,
                CL.FormulaHelper.DTOs.TimeSeriesDTO p_AnalyticsStrategyAlternativeEnergyValues,
                CL.FormulaHelper.DTOs.ConsequenceGroupDTO p_AssetGenerationGroup,
                System.Double? p_AssetTechnology_32_Improvement_32__37_,
                System.Double?[] p_GenARM_Condition_ConsqUnitOutput_B,
                CL.FormulaHelper.DTOs.TimeSeriesDTO p_SystemAvoidedCO2Values,
                System.Double? p_SystemCondition_32_Score_32_Best,
                System.Double? p_SystemCondition_32_Score_32_Worst,
                CL.FormulaHelper.DTOs.TimeSeriesDTO p_SystemEnergyValues)
            {
                AnalyticsStrategyAlternativeAvoidedCO2Values = p_AnalyticsStrategyAlternativeAvoidedCO2Values;
                AnalyticsStrategyAlternativeEnergyValues = p_AnalyticsStrategyAlternativeEnergyValues;
                AssetGenerationGroup = p_AssetGenerationGroup;
                AssetTechnology_32_Improvement_32__37_ = p_AssetTechnology_32_Improvement_32__37_;
                GenARM_Condition_ConsqUnitOutput_B = p_GenARM_Condition_ConsqUnitOutput_B;
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
            
            [CoreFieldInput(FormulaCoreFieldInputType.AssetGenerationGroup)]
            [DataMember]
            public CL.FormulaHelper.DTOs.ConsequenceGroupDTO AssetGenerationGroup  { get; private set; }
            
            [CustomFieldInput("Technology Improvement %", FormulaInputAssociatedEntity.Asset)]
            [DataMember]
            public System.Double? AssetTechnology_32_Improvement_32__37_  { get; private set; }
            
            [MeasureInput("GenARM", "Condition", MeasureOutputType.ConsqUnitOutput, true)]
            [DataMember]
            public System.Double?[] GenARM_Condition_ConsqUnitOutput_B  { get; private set; }
            
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
                System.Double p_ImpactCondition,
                CL.FormulaHelper.DTOs.TimePeriodDTO p_TimePeriod)
            {
                ImpactCondition = p_ImpactCondition;
                TimePeriod = p_TimePeriod;
            }
            
            [PromptInput("ImpactCondition")]
            [DataMember]
            public System.Double ImpactCondition  { get; private set; }
            
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
