// GENERATED CODE - DO NOT EDIT !!!
using System.Collections.Generic;
using CL.FormulaHelper;
using CL.FormulaHelper.Attributes;
using CL.FormulaHelper.DTOs;
using System.Runtime.Serialization;

namespace MeasureFormulas.Generated_Formula_Base_Classes
{
    [FormulaBase]
    public abstract class GenARMReplacementCostBase : FormulaConsequenceBase
    {
        [DataContract]
        public class TimeInvariantInputDTO
        {
            public TimeInvariantInputDTO(
                System.Double? p_AssetFacilityCapital_32_Power_32_Share_32__40__37__41_,
                System.Boolean? p_AssetJointly_32_Funded_63_,
                System.Decimal? p_AssetReplacementCost,
                System.Double? p_AssetTypeCostVariationFactor,
                System.Double? p_SystemBurden_32_Factor)
            {
                AssetFacilityCapital_32_Power_32_Share_32__40__37__41_ = p_AssetFacilityCapital_32_Power_32_Share_32__40__37__41_;
                AssetJointly_32_Funded_63_ = p_AssetJointly_32_Funded_63_;
                AssetReplacementCost = p_AssetReplacementCost;
                AssetTypeCostVariationFactor = p_AssetTypeCostVariationFactor;
                SystemBurden_32_Factor = p_SystemBurden_32_Factor;
            }
            
            [CustomFieldInput("Capital Power Share (%)", FormulaInputAssociatedEntity.AssetFacility)]
            [DataMember]
            public System.Double? AssetFacilityCapital_32_Power_32_Share_32__40__37__41_  { get; private set; }
            
            [CustomFieldInput("Jointly Funded?", FormulaInputAssociatedEntity.Asset)]
            [DataMember]
            public System.Boolean? AssetJointly_32_Funded_63_  { get; private set; }
            
            [CoreFieldInput(FormulaCoreFieldInputType.AssetReplacementCost)]
            [DataMember]
            public System.Decimal? AssetReplacementCost  { get; private set; }
            
            [CoreFieldInput(FormulaCoreFieldInputType.AssetTypeCostVariationFactor)]
            [DataMember]
            public System.Double? AssetTypeCostVariationFactor  { get; private set; }
            
            [CustomFieldInput("Burden Factor", FormulaInputAssociatedEntity.System)]
            [DataMember]
            public System.Double? SystemBurden_32_Factor  { get; private set; }
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
