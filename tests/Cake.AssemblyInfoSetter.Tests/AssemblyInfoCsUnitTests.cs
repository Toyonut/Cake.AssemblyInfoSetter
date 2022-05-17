using Cake.AssemblyInfoSetter;
using Cake.Core;
using Cake.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Xml;

namespace Cake.AssemblyInfoSetter.Tests;

[TestClass]
public class AssemblyInfoCsUnitTests
{
    public AssemblyInfoCsReplacer csReplacer;
    public AssemblyInfoProperties properties;
    public string filePath;
    Mock<ICakeContext> cakeContextMock;
    Mock<ICakeArguments> cakeArgumentsMock;
    Mock<ICakeEnvironment> cakeEnvironmentMock;

    public AssemblyInfoCsUnitTests ()
    {
        cakeContextMock = new Mock<ICakeContext>();
        cakeArgumentsMock = new Mock<ICakeArguments>();
        cakeEnvironmentMock = new Mock<ICakeEnvironment>();
        cakeContextMock.Setup(cakeContext => cakeContext.Arguments).Returns(cakeArgumentsMock.Object);
        cakeContextMock.Setup(cakeContext => cakeContext.Environment).Returns(cakeEnvironmentMock.Object);

        properties = new AssemblyInfoProperties () {
            AssemblyFileVersion = "1.1.0.4",
            AssemblyVersion = "1.7.0.8",
            AssemblyCompany = "My Company"
        };
        // Step out of the bin/Release/net6.0 directory
        var workDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        filePath = Path.GetFullPath($"{workDir}/testfiles/assemblyinfo.cs.txt");
        csReplacer = new AssemblyInfoCsReplacer (cakeContextMock.Object, filePath, properties);
    }

    [TestMethod]
    public void ShouldReplacePropertyFromDictionaryAssemblyInfoCs()
    {
        var initialAssemblyInfo = @"
            using System.Reflection;
            using System.Runtime.InteropServices;
            using System.Runtime.Serialization;

            // General Information about an assembly is controlled through the following 
            // set of attributes. Change these attribute values to modify the information
            // associated with an assembly.
            [assembly: AssemblyTitle(""ServiceStack.Examples.ServiceModel"")]
            [assembly: AssemblyDescription("""")]
            [assembly: AssemblyConfiguration("""")]
            [assembly: AssemblyCompany(""My Company"")]
            [assembly: AssemblyProduct(""ServiceStack.Examples.ServiceModel"")]
            [assembly: AssemblyCopyright(""Copyright ©  2010"")]
            [assembly: AssemblyTrademark("""")]
            [assembly: AssemblyCulture("""")]
            // You can specify all the values or you can default the Build and Revision Numbers 
            // by using the '*' as shown below:
            // [assembly: AssemblyVersion(""1.7.0.6"")]
            [assembly: AssemblyVersion(""1.7.0.6"")]
            [assembly: AssemblyFileVersion(""1.1.0.9"")]
        ";
        

        var propertiesDict = new Dictionary<string, string> () {
            {"AssemblyFileVersion", "1.0.0.0"}
        };

        var expectedAssemblyInfo = @"
            using System.Reflection;
            using System.Runtime.InteropServices;
            using System.Runtime.Serialization;

            // General Information about an assembly is controlled through the following 
            // set of attributes. Change these attribute values to modify the information
            // associated with an assembly.
            [assembly: AssemblyTitle(""ServiceStack.Examples.ServiceModel"")]
            [assembly: AssemblyDescription("""")]
            [assembly: AssemblyConfiguration("""")]
            [assembly: AssemblyCompany(""My Company"")]
            [assembly: AssemblyProduct(""ServiceStack.Examples.ServiceModel"")]
            [assembly: AssemblyCopyright(""Copyright ©  2010"")]
            [assembly: AssemblyTrademark("""")]
            [assembly: AssemblyCulture("""")]
            // You can specify all the values or you can default the Build and Revision Numbers 
            // by using the '*' as shown below:
            // [assembly: AssemblyVersion(""1.7.0.6"")]
            [assembly: AssemblyVersion(""1.7.0.6"")]
            [assembly: AssemblyFileVersion(""1.0.0.0"")]
        ";
        
        var replacedAssemblyInfo = csReplacer.ReplaceProperties(initialAssemblyInfo, propertiesDict);

        Assert.AreEqual(replacedAssemblyInfo, expectedAssemblyInfo);
    }

    [TestMethod]
    public void ShouldReplaceMultiplePropertiesFromDictionaryAssemblyInfoCs()
    {
        var initialAssemblyInfo = @"
            using System.Reflection;
            using System.Runtime.InteropServices;
            using System.Runtime.Serialization;

            // General Information about an assembly is controlled through the following 
            // set of attributes. Change these attribute values to modify the information
            // associated with an assembly.
            [assembly: AssemblyTitle(""ServiceStack.Examples.ServiceModel"")]
            [assembly: AssemblyDescription("""")]
            [assembly: AssemblyConfiguration("""")]
            [assembly: AssemblyCompany(""Not My Company"")]
            [assembly: AssemblyProduct(""ServiceStack.Examples.ServiceModel"")]
            [assembly: AssemblyCopyright(""Copyright ©  2010"")]
            [assembly: AssemblyTrademark("""")]
            [assembly: AssemblyCulture("""")]
            // You can specify all the values or you can default the Build and Revision Numbers 
            // by using the '*' as shown below:
            // [assembly: AssemblyVersion(""1.7.0.6"")]
            [assembly: AssemblyVersion(""1.7.0.6"")]
            [assembly: AssemblyFileVersion(""1.1.0.9"")]
        ";
        

        var propertiesDict = new Dictionary<string, string> () {
            {"AssemblyFileVersion", "1.0.0.0"},
            {"AssemblyCompany", "My Company"},
        };

        var expectedAssemblyInfo = @"
            using System.Reflection;
            using System.Runtime.InteropServices;
            using System.Runtime.Serialization;

            // General Information about an assembly is controlled through the following 
            // set of attributes. Change these attribute values to modify the information
            // associated with an assembly.
            [assembly: AssemblyTitle(""ServiceStack.Examples.ServiceModel"")]
            [assembly: AssemblyDescription("""")]
            [assembly: AssemblyConfiguration("""")]
            [assembly: AssemblyCompany(""My Company"")]
            [assembly: AssemblyProduct(""ServiceStack.Examples.ServiceModel"")]
            [assembly: AssemblyCopyright(""Copyright ©  2010"")]
            [assembly: AssemblyTrademark("""")]
            [assembly: AssemblyCulture("""")]
            // You can specify all the values or you can default the Build and Revision Numbers 
            // by using the '*' as shown below:
            // [assembly: AssemblyVersion(""1.7.0.6"")]
            [assembly: AssemblyVersion(""1.7.0.6"")]
            [assembly: AssemblyFileVersion(""1.0.0.0"")]
        ";
        
        var replacedAssemblyInfo = csReplacer.ReplaceProperties(initialAssemblyInfo, propertiesDict);

        Assert.AreEqual(replacedAssemblyInfo, expectedAssemblyInfo);
    }
}