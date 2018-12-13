// GENERATED CODE - DO NOT EDIT !!!
using System.Collections.Generic;
using CL.FormulaHelper;
using CL.FormulaHelper.Attributes;
using CL.FormulaHelper.DTOs;
using System.Runtime.Serialization;

namespace MeasureFormulas.Generated_Formula_Base_Classes
{
    [FormulaBase]
    public abstract class GenARMAvoidedCarbonEmissionsBase : FormulaConsequenceBase
    {
        [DataContract]
        public class TimeInvariantInputDTO
        {
            public TimeInvariantInputDTO(
                CL.FormulaHelper.DTOs.TimeSeriesDTO p_AnalyticsStrategyAlternativeAvoidedCO2Values,
                System.Boolean? p_AssetContributes_32_to_32_Lost_32_Generation,
                CL.FormulaHelper.DTOs.ConsequenceGroupDTO p_AssetGenerationGroup,
                System.Boolean? p_AssetIsSpareAvailable,
                System.Double? p_AssetTypeDowntimeWeeksWithoutSpare,
                System.Double? p_AssetTypeDowntimeWeeksWithSpare,
                CL.FormulaHelper.DTOs.XYCurveDTO p_ConditionToFailureCurve,
                System.Double?[] p_GenARM_Condition_ConsqUnitOutput,
                System.Double?[] p_GenARM_Condition_ConsqUnitOutput_B,
                System.Boolean? p_IgnoreLGR,
                CL.FormulaHelper.DTOs.TimeSeriesDTO p_SystemAvoidedCO2Values)
            {
                AnalyticsStrategyAlternativeAvoidedCO2Values = p_AnalyticsStrategyAlternativeAvoidedCO2Values;
                AssetContributes_32_to_32_Lost_32_Generation = p_AssetContributes_32_to_32_Lost_32_Generation;
                AssetGenerationGroup = p_AssetGenerationGroup;
                AssetIsSpareAvailable = p_AssetIsSpareAvailable;
                AssetTypeDowntimeWeeksWithoutSpare = p_AssetTypeDowntimeWeeksWithoutSpare;
                AssetTypeDowntimeWeeksWithSpare = p_AssetTypeDowntimeWeeksWithSpare;
                ConditionToFailureCurve = p_ConditionToFailureCurve;
                GenARM_Condition_ConsqUnitOutput = p_GenARM_Condition_ConsqUnitOutput;
                GenARM_Condition_ConsqUnitOutput_B = p_GenARM_Condition_ConsqUnitOutput_B;
                IgnoreLGR = p_IgnoreLGR;
                SystemAvoidedCO2Values = p_SystemAvoidedCO2Values;
            }
            
            [CustomFieldInput("AvoidedCO2Values", FormulaInputAssociatedEntity.AnalyticsStrategyAlternative)]
            [DataMember]
            public CL.FormulaHelper.DTOs.TimeSeriesDTO AnalyticsStrategyAlternativeAvoidedCO2Values  { get; private set; }
            
            [CustomFieldInput("Contributes to Lost Generation", FormulaInputAssociatedEntity.Asset)]
            [DataMember]
            public System.Boolean? AssetContributes_32_to_32_Lost_32_Generation  { get; private set; }
            
            [CoreFieldInput(FormulaCoreFieldInputType.AssetGenerationGroup)]
            [DataMember]
            public CL.FormulaHelper.DTOs.ConsequenceGroupDTO AssetGenerationGroup  { get; private set; }
            
            [CoreFieldInput(FormulaCoreFieldInputType.AssetIsSpareAvailable)]
            [DataMember]
            public System.Boolean? AssetIsSpareAvailable  { get; private set; }
            
            [CoreFieldInput(FormulaCoreFieldInputType.AssetTypeDowntimeWeeksWithoutSpare)]
            [DataMember]
            public System.Double? AssetTypeDowntimeWeeksWithoutSpare  { get; private set; }
            
            [CoreFieldInput(FormulaCoreFieldInputType.AssetTypeDowntimeWeeksWithSpare)]
            [DataMember]
            public System.Double? AssetTypeDowntimeWeeksWithSpare  { get; private set; }
            
            [CoreFieldInput(FormulaCoreFieldInputType.ConditionToFailureCurve)]
            [DataMember]
            public CL.FormulaHelper.DTOs.XYCurveDTO ConditionToFailureCurve  { get; private set; }
            
            [MeasureInput("GenARM", "Condition", MeasureOutputType.ConsqUnitOutput, false)]
            [DataMember]
            public System.Double?[] GenARM_Condition_ConsqUnitOutput  { get; private set; }
            
            [MeasureInput("GenARM", "Condition", MeasureOutputType.ConsqUnitOutput, true)]
            [DataMember]
            public System.Double?[] GenARM_Condition_ConsqUnitOutput_B  { get; private set; }
            
            [PromptInput("IgnoreLGR")]
            [DataMember]
            public System.Boolean? IgnoreLGR  { get; private set; }
            
            [CustomFieldInput("AvoidedCO2Values", FormulaInputAssociatedEntity.System)]
            [DataMember]
            public CL.FormulaHelper.DTOs.TimeSeriesDTO SystemAvoidedCO2Values  { get; private set; }
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
