using System;
using System.Collections.Generic;

namespace Car.Foundation.Models
{
  /// <summary>
  /// Class to configure custom field types for glass mapped objects. 
  /// 
  /// Ex. 
  /// </summary>
  public class CustomFieldOptions
  {

    private Dictionary<string, FieldOptions> fieldDictionary = new Dictionary<string, FieldOptions>();

    /// <summary>
    /// initialize the custom field options with a list of fields that use custom types.
    /// ex
    ///  options.Initialize( (opt) => 
    ///  {
    ///   opt.Add("IContact", "{2BFA75D7-D5AC-4C45-94A5-178C0CC01298}");
    ///  });
    /// </summary>
    /// <param name="action"></param>
    public void Initialize(Action<CustomFieldOptions> action)
    {
      action(this);
    }

    /// <summary>
    /// Add the type name for the given field id ex. options.Add("IContact", "{409F883A-0DC8-431A-9508-7316B59B92BE}");
    /// </summary>
    /// <param name="fieldTypeName"></param>
    /// <param name="id"></param>
    public void Add(string fieldTypeName, string id)
    {
      fieldDictionary.Add(id, new FieldOptions { GlassFieldTypeName = fieldTypeName });
    }

    /// <summary>
    /// Add the type name and glass attributes for the given field id ex. options.Add("IContact", "ReadOnly = true" ,"{409F883A-0DC8-431A-9508-7316B59B92BE}");
    /// </summary>
    /// <param name="fieldTypeName"></param>
    /// <param name="attributeExtras"></param>
    /// <param name="id"></param>
    public void Add(string fieldTypeName, string attributeExtras, string id)
    {
      fieldDictionary.Add(id, new FieldOptions { GlassFieldTypeName = fieldTypeName, AttributeExtras = attributeExtras });
    }

    /// <summary>
    /// sets the custom type for an array of fields
    /// ex
    /// options.Add("IContact",new string[] {"{91F40B47-00AF-4EE7-AA10-307F9411B54E}", "{265D2CB3-5013-4164-9C3D-7A3516F84BBE}"})
    /// </summary>
    /// <param name="fieldTypeName"></param>
    /// <param name="ids"></param>
    public void Add(string fieldTypeName, string[] ids)
    {
      foreach (string id in ids)
      {
        fieldDictionary.Add(id, new FieldOptions { GlassFieldTypeName = fieldTypeName });
      }
    }

    /// <summary>
    /// sets the custom type and attributes for an array of fields
    /// ex
    /// options.Add("IContact","ReadOnly = true",new string[] {"{91F40B47-00AF-4EE7-AA10-307F9411B54E}", "{265D2CB3-5013-4164-9C3D-7A3516F84BBE}"})
    public void Add(string fieldTypeName, string attributeExtras, string[] ids)
    {
      foreach (string id in ids)
      {
        fieldDictionary.Add(id, new FieldOptions { GlassFieldTypeName = fieldTypeName, AttributeExtras = attributeExtras });
      }
    }

    /// <summary>
    /// method used by the generator to output field options
    /// </summary>
    /// <param name="field"></param>
    /// <param name="options"></param>
    public void SetCustomFieldOptions(string field, FieldOptions options)
    {
      if (fieldDictionary.ContainsKey(field))
      {
        FieldOptions custom = fieldDictionary[field];
        options.GlassFieldTypeName = custom.GlassFieldTypeName;
        options.AttributeExtras = custom.AttributeExtras;
      }
    }
  }

}
