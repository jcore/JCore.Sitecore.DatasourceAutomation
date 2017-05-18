using System;

namespace JCore.Foundation.Testing.Attributes
{
    public class RegisterViewAttribute : Attribute
  {
    public string Name { get; private set; }

    public RegisterViewAttribute(string name)
    {
      this.Name = name;
    }
  }
}