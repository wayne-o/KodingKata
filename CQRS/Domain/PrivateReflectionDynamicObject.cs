// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrivateReflectionDynamicObject.cs" company="Project Attack Ltd">
//   2012 Project Attack Ltd
// </copyright>
// <summary>
//   The private reflection dynamic object.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Domain
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// The private reflection dynamic object.
    /// </summary>
    internal class PrivateReflectionDynamicObject : DynamicObject
    {
        #region Constants and Fields

        /// <summary>
        /// The internal binding flags.
        /// </summary>
        private const BindingFlags InternalBindingFlags =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        /// <summary>
        /// The _properties on type.
        /// </summary>
        private static readonly IDictionary<Type, IDictionary<string, IProperty>> _propertiesOnType =
            new ConcurrentDictionary<Type, IDictionary<string, IProperty>>();

        #endregion

        // Simple abstraction to make field and property access consistent
        #region Interfaces

        /// <summary>
        /// The i property.
        /// </summary>
        private interface IProperty
        {
            #region Public Properties

            /// <summary>
            /// Gets Name.
            /// </summary>
            string Name { get; }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// The get value.
            /// </summary>
            /// <param name="obj">
            /// The obj.
            /// </param>
            /// <param name="index">
            /// The index.
            /// </param>
            /// <returns>
            /// The get value.
            /// </returns>
            object GetValue(object obj, object[] index);

            /// <summary>
            /// The set value.
            /// </summary>
            /// <param name="obj">
            /// The obj.
            /// </param>
            /// <param name="val">
            /// The val.
            /// </param>
            /// <param name="index">
            /// The index.
            /// </param>
            void SetValue(object obj, object val, object[] index);

            #endregion
        }

        #endregion

        // IProperty implementation over a PropertyInfo
        #region Properties

        /// <summary>
        /// Gets or sets RealObject.
        /// </summary>
        private object RealObject { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The to string.
        /// </returns>
        public override string ToString()
        {
            return this.RealObject.ToString();
        }

        /// <summary>
        /// The try convert.
        /// </summary>
        /// <param name="binder">
        /// The binder.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// The try convert.
        /// </returns>
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            result = Convert.ChangeType(this.RealObject, binder.Type);
            return true;
        }

        /// <summary>
        /// The try get index.
        /// </summary>
        /// <param name="binder">
        /// The binder.
        /// </param>
        /// <param name="indexes">
        /// The indexes.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// The try get index.
        /// </returns>
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            // The indexed property is always named "Item" in C#
            IProperty prop = this.GetIndexProperty();
            result = prop.GetValue(this.RealObject, indexes);

            // Wrap the sub object if necessary. This allows nested anonymous objects to work.
            result = WrapObjectIfNeeded(result);

            return true;
        }

        /// <summary>
        /// The try get member.
        /// </summary>
        /// <param name="binder">
        /// The binder.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// The try get member.
        /// </returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            IProperty prop = this.GetProperty(binder.Name);

            // Get the property value
            result = prop.GetValue(this.RealObject, index: null);

            // Wrap the sub object if necessary. This allows nested anonymous objects to work.
            result = WrapObjectIfNeeded(result);

            return true;
        }

        // Called when a method is called
        /// <summary>
        /// The try invoke member.
        /// </summary>
        /// <param name="binder">
        /// The binder.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// The try invoke member.
        /// </returns>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = InvokeMemberOnType(this.RealObject.GetType(), this.RealObject, binder.Name, args);

            // Wrap the sub object if necessary. This allows nested anonymous objects to work.
            result = WrapObjectIfNeeded(result);

            return true;
        }

        /// <summary>
        /// The try set index.
        /// </summary>
        /// <param name="binder">
        /// The binder.
        /// </param>
        /// <param name="indexes">
        /// The indexes.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The try set index.
        /// </returns>
        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            // The indexed property is always named "Item" in C#
            IProperty prop = this.GetIndexProperty();
            prop.SetValue(this.RealObject, value, indexes);
            return true;
        }

        /// <summary>
        /// The try set member.
        /// </summary>
        /// <param name="binder">
        /// The binder.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The try set member.
        /// </returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            IProperty prop = this.GetProperty(binder.Name);

            // Set the property value
            prop.SetValue(this.RealObject, value, index: null);

            return true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The wrap object if needed.
        /// </summary>
        /// <param name="o">
        /// The o.
        /// </param>
        /// <returns>
        /// The wrap object if needed.
        /// </returns>
        internal static object WrapObjectIfNeeded(object o)
        {
            // Don't wrap primitive types, which don't have many interesting internal APIs
            if (o == null || o.GetType().IsPrimitive || o is string)
            {
                return o;
            }

            return new PrivateReflectionDynamicObject { RealObject = o };
        }

        /// <summary>
        /// The get type properties.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// </returns>
        private static IDictionary<string, IProperty> GetTypeProperties(Type type)
        {
            // First, check if we already have it cached
            IDictionary<string, IProperty> typeProperties;
            if (_propertiesOnType.TryGetValue(type, out typeProperties))
            {
                return typeProperties;
            }

            // Not cache, so we need to build it
            typeProperties = new ConcurrentDictionary<string, IProperty>();

            // First, add all the properties
            foreach (PropertyInfo prop in type.GetProperties(InternalBindingFlags).Where(p => p.DeclaringType == type))
            {
                typeProperties[prop.Name] = new Property { PropertyInfo = prop };
            }

            // Now, add all the fields
            foreach (FieldInfo field in type.GetFields(InternalBindingFlags).Where(p => p.DeclaringType == type))
            {
                typeProperties[field.Name] = new Field { FieldInfo = field };
            }

            // Finally, recurse on the base class to add its fields
            if (type.BaseType != null)
            {
                foreach (IProperty prop in GetTypeProperties(type.BaseType).Values)
                {
                    typeProperties[prop.Name] = prop;
                }
            }

            // Cache it for next time
            _propertiesOnType[type] = typeProperties;

            return typeProperties;
        }

        /// <summary>
        /// The invoke member on type.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The invoke member on type.
        /// </returns>
        private static object InvokeMemberOnType(Type type, object target, string name, object[] args)
        {
            try
            {
                // Try to incoke the method
                return type.InvokeMember(name, BindingFlags.InvokeMethod | InternalBindingFlags, null, target, args);
            }
            catch (MissingMethodException)
            {
                // If we couldn't find the method, try on the base class
                if (type.BaseType != null)
                {
                    return InvokeMemberOnType(type.BaseType, target, name, args);
                }

                throw;
            }
        }

        /// <summary>
        /// The get index property.
        /// </summary>
        /// <returns>
        /// </returns>
        private IProperty GetIndexProperty()
        {
            // The index property is always named "Item" in C#
            return this.GetProperty("Item");
        }

        /// <summary>
        /// The get property.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        private IProperty GetProperty(string propertyName)
        {
            // Get the list of properties and fields for this type
            IDictionary<string, IProperty> typeProperties = GetTypeProperties(this.RealObject.GetType());

            // Look for the one we want
            IProperty property;
            if (typeProperties.TryGetValue(propertyName, out property))
            {
                return property;
            }

            // The property doesn't exist

            // Get a list of supported properties and fields and show them as part of the exception message
            // For fields, skip the auto property backing fields (which name start with <)
            IOrderedEnumerable<string> propNames =
                typeProperties.Keys.Where(name => name[0] != '<').OrderBy(name => name);
            throw new ArgumentException(
                string.Format(
                    "The property {0} doesn't exist on type {1}. Supported properties are: {2}", 
                    propertyName, 
                    this.RealObject.GetType(), 
                    string.Join(", ", propNames)));
        }

        #endregion

        /// <summary>
        /// The field.
        /// </summary>
        private class Field : IProperty
        {
            #region Explicit Interface Properties

            /// <summary>
            /// Gets Name.
            /// </summary>
            string IProperty.Name
            {
                get
                {
                    return this.FieldInfo.Name;
                }
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets or sets FieldInfo.
            /// </summary>
            internal FieldInfo FieldInfo { get; set; }

            #endregion

            #region Explicit Interface Methods

            /// <summary>
            /// The get value.
            /// </summary>
            /// <param name="obj">
            /// The obj.
            /// </param>
            /// <param name="index">
            /// The index.
            /// </param>
            /// <returns>
            /// The get value.
            /// </returns>
            object IProperty.GetValue(object obj, object[] index)
            {
                return this.FieldInfo.GetValue(obj);
            }

            /// <summary>
            /// The set value.
            /// </summary>
            /// <param name="obj">
            /// The obj.
            /// </param>
            /// <param name="val">
            /// The val.
            /// </param>
            /// <param name="index">
            /// The index.
            /// </param>
            void IProperty.SetValue(object obj, object val, object[] index)
            {
                this.FieldInfo.SetValue(obj, val);
            }

            #endregion
        }

        /// <summary>
        /// The property.
        /// </summary>
        private class Property : IProperty
        {
            #region Explicit Interface Properties

            /// <summary>
            /// Gets Name.
            /// </summary>
            string IProperty.Name
            {
                get
                {
                    return this.PropertyInfo.Name;
                }
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets or sets PropertyInfo.
            /// </summary>
            internal PropertyInfo PropertyInfo { get; set; }

            #endregion

            #region Explicit Interface Methods

            /// <summary>
            /// The get value.
            /// </summary>
            /// <param name="obj">
            /// The obj.
            /// </param>
            /// <param name="index">
            /// The index.
            /// </param>
            /// <returns>
            /// The get value.
            /// </returns>
            object IProperty.GetValue(object obj, object[] index)
            {
                return this.PropertyInfo.GetValue(obj, index);
            }

            /// <summary>
            /// The set value.
            /// </summary>
            /// <param name="obj">
            /// The obj.
            /// </param>
            /// <param name="val">
            /// The val.
            /// </param>
            /// <param name="index">
            /// The index.
            /// </param>
            void IProperty.SetValue(object obj, object val, object[] index)
            {
                this.PropertyInfo.SetValue(obj, val, index);
            }

            #endregion
        }
    }
}