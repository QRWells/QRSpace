﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QRSpace.Shared.Resources.Models {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class RegisterDto_zh_CN {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal RegisterDto_zh_CN() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("QRSpace.Shared.Resources.Models.RegisterDto.zh-CN", typeof(RegisterDto_zh_CN).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 密码不一致.
        /// </summary>
        internal static string ConfirmPwdCompareError {
            get {
                return ResourceManager.GetString("ConfirmPwdCompareError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 密码应至少{2}个，至多{1}个字符.
        /// </summary>
        internal static string PasswordLengthError {
            get {
                return ResourceManager.GetString("PasswordLengthError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 密码不能为空.
        /// </summary>
        internal static string PasswordRequireError {
            get {
                return ResourceManager.GetString("PasswordRequireError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 用户名应至少{2}个，至多{1}个字符.
        /// </summary>
        internal static string UsernameLengthError {
            get {
                return ResourceManager.GetString("UsernameLengthError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 用户名不能为空.
        /// </summary>
        internal static string UsernameRequireError {
            get {
                return ResourceManager.GetString("UsernameRequireError", resourceCulture);
            }
        }
    }
}
