// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Gov.Jag.Embc.Interfaces.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq; using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// OrganizationInfo
    /// </summary>
    public partial class MicrosoftDynamicsCRMOrganizationInfo
    {
        /// <summary>
        /// Initializes a new instance of the
        /// MicrosoftDynamicsCRMOrganizationInfo class.
        /// </summary>
        public MicrosoftDynamicsCRMOrganizationInfo()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the
        /// MicrosoftDynamicsCRMOrganizationInfo class.
        /// </summary>
        /// <param name="instanceType">Possible values include: 'Customer',
        /// 'Monitoring', 'Support', 'BackEnd', 'Secondary', 'CustomerTest',
        /// 'CustomerFreeTest', 'CustomerPreview', 'Placeholder', 'TestDrive',
        /// 'MsftInvestigation', 'EmailTrial', 'Default', 'Developer'</param>
        public MicrosoftDynamicsCRMOrganizationInfo(string instanceType = default(string), IList<MicrosoftDynamicsCRMSolution> solutions = default(IList<MicrosoftDynamicsCRMSolution>))
        {
            InstanceType = instanceType;
            Solutions = solutions;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets possible values include: 'Customer', 'Monitoring',
        /// 'Support', 'BackEnd', 'Secondary', 'CustomerTest',
        /// 'CustomerFreeTest', 'CustomerPreview', 'Placeholder', 'TestDrive',
        /// 'MsftInvestigation', 'EmailTrial', 'Default', 'Developer'
        /// </summary>
        [JsonProperty(PropertyName = "InstanceType")]
        public string InstanceType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Solutions")]
        [NotMapped] public IList<MicrosoftDynamicsCRMSolution> Solutions { get; set; }

    }
}
