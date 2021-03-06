using Cake.AssemblyInfoSetter;
using Cake.Core;
using Cake.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Xml;

namespace Cake.AssemblyInfoSetter.Tests;

[TestClass]
public class AssemblyInfoCsProjUnitTests
{
	public AssemblyInfoCsprojReplacer csprojReplacer;
	public AssemblyInfoProperties properties;
	public string filePath;
	Mock<ICakeContext> cakeContextMock;
	Mock<ICakeArguments> cakeArgumentsMock;
	Mock<ICakeEnvironment> cakeEnvironmentMock;

	public AssemblyInfoCsProjUnitTests()
	{
		cakeContextMock = new Mock<ICakeContext>();
		cakeArgumentsMock = new Mock<ICakeArguments>();
		cakeEnvironmentMock = new Mock<ICakeEnvironment>();
		cakeContextMock.Setup(cakeContext => cakeContext.Arguments).Returns(cakeArgumentsMock.Object);
		cakeContextMock.Setup(cakeContext => cakeContext.Environment).Returns(cakeEnvironmentMock.Object);

		properties = new AssemblyInfoProperties()
		{
			AssemblyFileVersion = "1.1.0.4",
			AssemblyVersion = "1.7.0.8",
			AssemblyCompany = "My Company"
		};
		// Step out of the bin/Release/net6.0 directory
		var workDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
		filePath = Path.GetFullPath($"{workDir}/testfiles/assemblyinfo.csproj.txt");
		csprojReplacer = new AssemblyInfoCsprojReplacer(cakeContextMock.Object, filePath, properties);
	}

	[TestMethod]
	public void ShouldAddPropertyFromDictionaryCsproj()
	{
		var initialDoc = @"
            <Project Sdk=""Microsoft.NET.Sdk"">
            <PropertyGroup>
                <TargetFramework>net6.0</TargetFramework>
                <ImplicitUsings>enable</ImplicitUsings>
                <Nullable>enable</Nullable>
                <PublishSingleFile>true</PublishSingleFile>
                <SelfContained>true</SelfContained>
            </PropertyGroup>
            </Project>
        ";

		var expectedValue = "1.0.0.0";

		var propertiesDict = new Dictionary<string, string>() {
			{"AssemblyFileVersion", "1.0.0.0"}
		};

		var xmlInitial = new XmlDocument();
		xmlInitial.LoadXml(initialDoc);

		var replacedXml = csprojReplacer.ReplaceProperties(xmlInitial, propertiesDict);
		var actualValue = replacedXml.SelectSingleNode("Project/PropertyGroup/AssemblyFileVersion").InnerXml;

		Assert.IsNotNull(actualValue);
		Assert.AreEqual(expectedValue, actualValue);
	}

	[TestMethod]
	public void ShouldAddMultiplePropertiesFromDictionaryCsproj()
	{
		var initialDoc = @"
            <Project Sdk=""Microsoft.NET.Sdk"">
            <PropertyGroup>
                <TargetFramework>net6.0</TargetFramework>
                <ImplicitUsings>enable</ImplicitUsings>
                <Nullable>enable</Nullable>
                <PublishSingleFile>true</PublishSingleFile>
                <SelfContained>true</SelfContained>
            </PropertyGroup>
            </Project>
        ";


		var propertiesDict = new Dictionary<string, string>() {
			{"AssemblyFileVersion", "1.0.0.0"},
			{"AssemblyTitle", "My Company"},
		};
		var expectedValueAssemblyTitle = "My Company";
		var expectedValueAssemblyFileVersion = "1.0.0.0";

		var xmlInitial = new XmlDocument();
		xmlInitial.LoadXml(initialDoc);

		var replacedXml = csprojReplacer.ReplaceProperties(xmlInitial, propertiesDict);
		var actualValueAssemblyFileVersion = replacedXml.SelectSingleNode("Project/PropertyGroup/AssemblyFileVersion").InnerXml;
		var actualValueAssemblyTitle = replacedXml.SelectSingleNode("Project/PropertyGroup/AssemblyTitle").InnerXml;

		Assert.IsNotNull(actualValueAssemblyFileVersion);
		Assert.AreEqual(expectedValueAssemblyFileVersion, actualValueAssemblyFileVersion);
		Assert.IsNotNull(actualValueAssemblyTitle);
		Assert.AreEqual(expectedValueAssemblyTitle, actualValueAssemblyTitle);
	}
	[TestMethod]
	public void ShouldReplacePropertyFromDictionaryCsproj()
	{
		var initialDoc = @"
            <Project Sdk=""Microsoft.NET.Sdk"">
            <PropertyGroup>
                <TargetFramework>net6.0</TargetFramework>
                <ImplicitUsings>enable</ImplicitUsings>
                <Nullable>enable</Nullable>
                <PublishSingleFile>true</PublishSingleFile>
                <SelfContained>true</SelfContained>
                <AssemblyFileVersion>0.0.0.3</AssemblyFileVersion>
            </PropertyGroup>
            </Project>
        ";


		var propertiesDict = new Dictionary<string, string>() {
			{"AssemblyFileVersion", "1.0.0.0"},
			{"AssemblyTitle", "My Company"},
		};
		var expectedValueAssemblyFileVersion = "1.0.0.0";

		var xmlInitial = new XmlDocument();
		xmlInitial.LoadXml(initialDoc);

		var replacedXml = csprojReplacer.ReplaceProperties(xmlInitial, propertiesDict);
		var actualValueAssemblyFileVersion = replacedXml.SelectSingleNode("Project/PropertyGroup/AssemblyFileVersion").InnerXml;

		Assert.IsNotNull(actualValueAssemblyFileVersion);
		Assert.AreEqual(expectedValueAssemblyFileVersion, actualValueAssemblyFileVersion);
	}

	[TestMethod]
	public void ShouldReplaceMultiplePropertiesFromDictionaryCsproj()
	{
		var initialDoc = @"
            <Project Sdk=""Microsoft.NET.Sdk"">
            <PropertyGroup>
                <TargetFramework>net6.0</TargetFramework>
                <ImplicitUsings>enable</ImplicitUsings>
                <Nullable>enable</Nullable>
                <PublishSingleFile>true</PublishSingleFile>
                <SelfContained>true</SelfContained>
                <AssemblyFileVersion>0.0.0.3</AssemblyFileVersion>
                <AssemblyTitle>Not My Company</AssemblyTitle>
            </PropertyGroup>
            </Project>
        ";


		var propertiesDict = new Dictionary<string, string>() {
			{"AssemblyFileVersion", "1.0.0.0"},
			{"AssemblyTitle", "My Company"},
		};
		var expectedValueAssemblyTitle = "My Company";
		var expectedValueAssemblyFileVersion = "1.0.0.0";

		var xmlInitial = new XmlDocument();
		xmlInitial.LoadXml(initialDoc);

		var replacedXml = csprojReplacer.ReplaceProperties(xmlInitial, propertiesDict);
		var actualValueAssemblyFileVersion = replacedXml.SelectSingleNode("Project/PropertyGroup/AssemblyFileVersion").InnerXml;
		var actualValueAssemblyTitle = replacedXml.SelectSingleNode("Project/PropertyGroup/AssemblyTitle").InnerXml;

		Assert.IsNotNull(actualValueAssemblyFileVersion);
		Assert.AreEqual(expectedValueAssemblyFileVersion, actualValueAssemblyFileVersion);
		Assert.IsNotNull(actualValueAssemblyTitle);
		Assert.AreEqual(expectedValueAssemblyTitle, actualValueAssemblyTitle);
	}
}
