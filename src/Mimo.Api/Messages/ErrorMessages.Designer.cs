﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mimo.Api.Messages {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ErrorMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ErrorMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Mimo.Api.Messages.ErrorMessages", typeof(ErrorMessages).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Chapter name must be unique for a course.
        /// </summary>
        public static string ChapterNameUnique {
            get {
                return ResourceManager.GetString("ChapterNameUnique", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Chapter not found.
        /// </summary>
        public static string ChapterNotFound {
            get {
                return ResourceManager.GetString("ChapterNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot complete a lesson for a course which has not been published.
        /// </summary>
        public static string CompleteLessonCourseNotPublished {
            get {
                return ResourceManager.GetString("CompleteLessonCourseNotPublished", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot update course structure, after a course has been published.
        /// </summary>
        public static string CourseAlreadyPublished {
            get {
                return ResourceManager.GetString("CourseAlreadyPublished", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Course name must be unique.
        /// </summary>
        public static string CourseNameUnique {
            get {
                return ResourceManager.GetString("CourseNameUnique", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Course not found.
        /// </summary>
        public static string CourseNotFound {
            get {
                return ResourceManager.GetString("CourseNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot delete course, after a course has been published.
        /// </summary>
        public static string DeleteCourseAlreadyPublished {
            get {
                return ResourceManager.GetString("DeleteCourseAlreadyPublished", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {PropertyName} cannot be empty.
        /// </summary>
        public static string FieldCannotBeEmpty {
            get {
                return ResourceManager.GetString("FieldCannotBeEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {PropertyName} must be less than {MaxLength} characters.
        /// </summary>
        public static string LengthLowerThen {
            get {
                return ResourceManager.GetString("LengthLowerThen", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Lesson name must be unique for a chapter.
        /// </summary>
        public static string LessonNameUnique {
            get {
                return ResourceManager.GetString("LessonNameUnique", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Lesson not found.
        /// </summary>
        public static string LessonNotFound {
            get {
                return ResourceManager.GetString("LessonNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Entity cannot be blank.
        /// </summary>
        public static string MissingRequestEntity {
            get {
                return ResourceManager.GetString("MissingRequestEntity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot republish course.
        /// </summary>
        public static string PublishCourseAlreadyPublished {
            get {
                return ResourceManager.GetString("PublishCourseAlreadyPublished", resourceCulture);
            }
        }
    }
}
