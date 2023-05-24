// <copyright file="PexAssemblyInfo.cs">Copyright ©  2023</copyright>
using Microsoft.Pex.Framework.Coverage;
using Microsoft.Pex.Framework.Creatable;
using Microsoft.Pex.Framework.Instrumentation;
using Microsoft.Pex.Framework.Settings;
using Microsoft.Pex.Framework.Validation;

// Microsoft.Pex.Framework.Settings
[assembly: PexAssemblySettings(TestFramework = "VisualStudioUnitTest")]

// Microsoft.Pex.Framework.Instrumentation
[assembly: PexAssemblyUnderTest("GameShop.BLL")]
[assembly: PexInstrumentAssembly("FluentValidation")]
[assembly: PexInstrumentAssembly("log4net")]
[assembly: PexInstrumentAssembly("EntityFramework")]
[assembly: PexInstrumentAssembly("AutoMapper")]
[assembly: PexInstrumentAssembly("System.Core")]
[assembly: PexInstrumentAssembly("GameShop.DAL")]
[assembly: PexInstrumentAssembly("Unity.Abstractions")]

// Microsoft.Pex.Framework.Creatable
[assembly: PexCreatableFactoryForDelegates]

// Microsoft.Pex.Framework.Validation
[assembly: PexAllowedContractRequiresFailureAtTypeUnderTestSurface]
[assembly: PexAllowedXmlDocumentedException]

// Microsoft.Pex.Framework.Coverage
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "FluentValidation")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "log4net")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "EntityFramework")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "AutoMapper")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Core")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "GameShop.DAL")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Unity.Abstractions")]

