namespace Cake.AssemblyInfoSetter
{
    public class AssemblyInfoProperties
    {
        public string? AssemblyTitle {get; set;}
        public string? AssemblyCompany {get; set;}
        public string? AssemblyCopyright {get; set;}
        public string? AssemblyVersion {get; set;}
        public string? AssemblyFileVersion {get; set;}
        public string? AssemblyInformationalVersion {get; set;}
        public string? AssemblyProduct {get; set;}
        public string? AssemblyTrademark {get; set;}

        public Dictionary<string, string> ConvertToDictionary()
        {
            var propertiesDictionary = new Dictionary<string, string>();

            foreach (var prop in this.GetType().GetProperties())
            {
                var value = prop.GetValue(this, null);

                if (value is not null)
                {
                    propertiesDictionary.Add(prop.Name, value.ToString()!);
                }
            }

            return propertiesDictionary;
        }
    }
}