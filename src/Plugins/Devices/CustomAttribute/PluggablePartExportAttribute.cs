using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicPlugin.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    [MetadataAttribute]
    public class PluggablePartExportAttribute : ExportAttribute, IPluggablePartMetadata
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluggablePartExportAttribute"/> class.
        /// </summary>
        public PluggablePartExportAttribute() : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluggablePartExportAttribute"/> class.
        /// </summary>
        /// <param name="contractType">A type from which to derive the contract name that is used to export the type or member marked with this attribute, or <see langword="null" /> to use the default contract name.</param>
        public PluggablePartExportAttribute(Type contractType) : base(contractType)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluggablePartExportAttribute"/> class.
        /// </summary>
        /// <param name="contractName">The contract name that is used to export the type or member marked with this attribute, or <see langword="null" /> or an empty string ("") to use the default contract name.</param>
        public PluggablePartExportAttribute(string contractName) : base(contractName)
        {
            this.Name = contractName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluggablePartExportAttribute"/> class.
        /// </summary>
        /// <param name="contractName">The contract name that is used to export the type or member marked with this attribute, or <see langword="null" /> or an empty string ("") to use the default contract name.</param>
        /// <param name="contractType">The type to export.</param>
        public PluggablePartExportAttribute(string contractName, Type contractType) : base(contractName, contractType)
        {
            this.Name = contractName;
        }
    }
}
