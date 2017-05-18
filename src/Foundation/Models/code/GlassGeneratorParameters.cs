namespace Car.Foundation.Models
{
  public class GlassGeneratorParameters
  {
    public string TemplateDB { get; set; }
    public string TemplateDirectory { get; set; }
    public string [] TemplateSitecorePaths { get; set; }

    public string RootNamespace { get; set; }
    
    public CustomFieldOptions GlassCustomFieldOptions { get; set; }
    public GlassGeneratorParameters()
    {
      GlassCustomFieldOptions = new CustomFieldOptions();
      TemplateDB = "master";
    }
  }
}