using Cake.AssemblyInfoSetter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cake.AssemblyInfoSetter.Tests;

[TestClass]
public class AssemblyInfoPropertiesTests
{
    [TestMethod]
    public void PropertiesShouldReturnValidDictionary()
    {
        var properties = new AssemblyInfoProperties () {
            AssemblyFileVersion = "1.1.0.4",
            AssemblyVersion = "1.7.0.8",
            AssemblyCompany = "My Company"
        };

        var propertiesDictExpected = new Dictionary <string, string> ()
        {
            {"AssemblyFileVersion", "1.1.0.4"},
            {"AssemblyVersion", "1.7.0.8"},
            {"AssemblyCompany", "My Company"}
        };

        var propertiesDictActual = properties.ConvertToDictionary();

        Assert.AreEqual(propertiesDictExpected["AssemblyFileVersion"], propertiesDictActual["AssemblyFileVersion"]);
        Assert.AreEqual(propertiesDictExpected["AssemblyVersion"], propertiesDictActual["AssemblyVersion"]);
        Assert.AreEqual(propertiesDictExpected["AssemblyCompany"], propertiesDictActual["AssemblyCompany"]);
        Assert.AreEqual(propertiesDictExpected, propertiesDictExpected);
    }
}