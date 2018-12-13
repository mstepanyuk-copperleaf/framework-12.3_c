// GENERATED CODE - DO NOT EDIT !!!
using System.Collections.Generic;
using CL.FormulaHelper;
using CL.FormulaHelper.Attributes;
using CL.FormulaHelper.DTOs;
using System.Runtime.Serialization;

namespace MeasureFormulas.Generated_Formula_Base_Classes
{
    [FormulaBase]
    public abstract class ManualFinancialResourceBenefitFormulaBase : FormulaConsequenceBase
    {
        [DataContract]
        public class TimeInvariantInputDTO
        {
            public TimeInvariantInputDTO(
                CL.FormulaHelper.DTOs.CustomFieldListItemDTO p_FinResBenefResource,
                CL.FormulaHelper.DTOs.CustomFieldListItemDTO p_FinResBenefSupplier,
                CL.FormulaHelper.DTOs.TimeSeriesDTO p_SystemCO2_aMW_Variable,
                CL.FormulaHelper.DTOs.TimeSeriesDTO p_SystemExpected_Flat,
                CL.FormulaHelper.DTOs.TimeSeriesDTO p_SystemHigh_Flat,
                CL.FormulaHelper.DTOs.TimeSeriesDTO p_SystemLow_Flat)
            {
                FinResBenefResource = p_FinResBenefResource;
                FinResBenefSupplier = p_FinResBenefSupplier;
                SystemCO2_aMW_Variable = p_SystemCO2_aMW_Variable;
                SystemExpected_Flat = p_SystemExpected_Flat;
                SystemHigh_Flat = p_SystemHigh_Flat;
                SystemLow_Flat = p_SystemLow_Flat;
            }
            
            [PromptInput("FinResBenefResource")]
            [DataMember]
            public CL.FormulaHelper.DTOs.CustomFieldListItemDTO FinResBenefResource  { get; private set; }
            
            [PromptInput("FinResBenefSupplier")]
            [DataMember]
            public CL.FormulaHelper.DTOs.CustomFieldListItemDTO FinResBenefSupplier  { get; private set; }
            
            [CustomFieldInput("CO2_aMW_Variable", FormulaInputAssociatedEntity.System)]
            [DataMember]
            public CL.FormulaHelper.DTOs.TimeSeriesDTO SystemCO2_aMW_Variable  { get; private set; }
            
            [CustomFieldInput("Expected_Flat", FormulaInputAssociatedEntity.System)]
            [DataMember]
            public CL.FormulaHelper.DTOs.TimeSeriesDTO SystemExpected_Flat  { get; private set; }
            
            [CustomFieldInput("High_Flat", FormulaInputAssociatedEntity.System)]
            [DataMember]
            public CL.FormulaHelper.DTOs.TimeSeriesDTO SystemHigh_Flat  { get; private set; }
            
            [CustomFieldInput("Low_Flat", FormulaInputAssociatedEntity.System)]
            [DataMember]
            public CL.FormulaHelper.DTOs.TimeSeriesDTO SystemLow_Flat  { get; private set; }
        }
        
        [DataContract]
        public class TimeVariantInputDTO : ITimeVariantInputDTO
        {
            public TimeVariantInputDTO(
                System.Double p_FinResBenefAmount,
                CL.FormulaHelper.DTOs.TimePeriodDTO p_TimePeriod)
            {
                FinResBenefAmount = p_FinResBenefAmount;
                TimePeriod = p_TimePeriod;
            }
            
            [PromptInput("FinResBenefAmount")]
            [DataMember]
            public System.Double FinResBenefAmount  { get; private set; }
            
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
