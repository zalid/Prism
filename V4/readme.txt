				--------------------------------------------------------------------------
				Prism 4.0 (formerly known as Composite Application Guidance) - Readme file 
											CTP - August 2010
				--------------------------------------------------------------------------
				
This file provides information that supplements the Prism documentation and can be useful when using the guidance.

--------
CONTENTS
--------

1.> DROP INFORMATION
	1.1 What's Included in This Drop
	1.2 Major Changes in This Drop
	1.3 System Requirements
	1.4 Installation
	1.5 Please Note - Known Issues
	1.6 Prism Library and Code Access Security

2.> PRISM LIBRARY BREAKING CHANGES
	2.1 New Assemblies
	2.2 Bootstrapper API Changes

3.> BASIC MODEL-VIEW-VIEWMODEL QUICKSTART
	3.1 Overview
	3.2 Scenario
	3.3 Running Instructions
	
4.> MODEL-VIEW-VIEWMODEL QUICKSTART
	4.1 Overview
	4.2 Scenario
	4.3 Running Instructions
	
5.> MODULARITY QUICKSTARTS
	5.1 Overview
	5.2 Scenario
	5.3 Running Instructions

6.> MODEL-VIEW-VIEWMODEL REFERENCE IMPLEMENTATION
	6.1 Overview
	6.2 Scenario
	6.3 Running Instructions

7.> STOCK TRADER REFERENCE IMPLEMENTATION
	7.1 Overview
	7.2 Scenario
	7.3 Running Instructions
	
	
--------------------
1.> DROP INFORMATION
--------------------
	
	1.1 >>> What's Included in This Drop <<<
	----------------------------------------
	The following assets are being shipped in the current drop
	• The Prism Library for Windows Presentation Foundation (WPF) and Silverlight
	• Model-View-ViewModel Reference Implementation (MVVM RI)
	• Stock Trader Reference Implementation (Stock Trader RI)
	• Prism 4.0 Work-in-progress documentation
	• QuickStarts:
	  New or updated QuickStarts in Prism 4.0:
		- Basic MVVM QuickStart
		- MVVM QuickStart
		- Modularity QuickStarts		
      QuickStarts ported from Prism 2.x:
		- Commanding QuickStarts
		- Event Aggregation QuickStarts
		- Hello World QuickStarts
		- MultiTargeting QuickStarts
		- UI Composition - ViewDiscovery QuickStarts
		- UI Composition - ViewInjection QuickStarts

     With the exception of the MVVM QuickStarts, there are two solutions for each QuickStart, 
     one targeting the desktop and one targeting Silverlight.
      
	
	1.2 >>> Major Changes in This Drop <<<
	--------------------------------------
	• MVVM RI: Implemented a template-based approach for displaying pop-up windows
	• MVVM RI: Renamed the INotification interface to IInteractionRequest
	• MVVM RI: Added code comments for InteractionRequest classes
	• MVVM RI: Implemented the ViewFactory class to avoid composing the container in itself
	• Added more acceptance tests for the MVVM RI
	• Renamed the Simple MVVM QuickStart to Basic MVVM QuickStart
	• Updated documentation for modularity and MVVM topics
	• Bug fixes


	1.3 >>> System Requirements <<<
	-------------------------------
	This guidance was designed to run on the Microsoft Windows 7, Windows Vista, Windows XP Professional, Windows Server 2003, or Windows Server 2008 operating system. 
	WPF applications built using this guidance will require the .NET Framework 4.0 to run, and Silverlight applications will require the .NET Framework for Silverlight 4.
	
	If you are using a previous version of Silverlight, see "New in This Release" on MSDN:
	http://msdn.microsoft.com/en-us/library/ff649547.aspx
	
	Before you can use the Prism Library, the following must be installed:
	• Microsoft Visual Studio 2010
	• Microsoft .NET Framework 4.0 (the .NET Framework 4.0 includes WPF): http://www.microsoft.com/downloads/details.aspx?familyid=9CFB2D51-5FF4-4491-B0E5-B386F32C0992&displaylang=en
	• Microsoft Silverlight 4 (this is required only if you are creating Silverlight applications): http://www.microsoft.com/silverlight/
	• Microsoft Silverlight 4 Tools for Visual Studio 2010 (this is required only if you are creating Silverlight applications): 
	http://www.microsoft.com/downloads/details.aspx?FamilyID=eff8a0da-0a4d-48e8-8366-6ddf2ecad801&displaylang=en
	• MOQ (Moq.4.0.10531.7-bin.zip - includes Silverlight): http://code.google.com/p/moq/ 
		
	You may also want to install the following:
	• Microsoft Expression Blend 4: http://www.microsoft.com/expression/products/Blend_Overview.aspx
	• Microsoft Visual Studio 2010 SDK to compile Project Linker: http://www.microsoft.com/downloads/details.aspx?FamilyID=47305cf4-2bea-43c0-91cd-1b853602dcc5&displaylang=en
	• Microsoft Silverlight Unit Test Framework to run the unit tests in Silverlight: http://silverlight.codeplex.com/releases/view/43528

	1.4 >>> Installation <<<
	------------------------
	To install the Prism assets, run the Prism.Source.exe file to extract the source into any folder of your choice.
	NOTE: In order to compile from source, you will need to add the Silverlight Unit Testing Framework files to the LIB\Silverlight\UnitTestFramework folder. For the files, see http://code.msdn.microsoft.com/silverlightut for the files.
	
	1.5 >>> Please Note - Known Issues <<<
	------------------------------------
	The Modularity with Unity – Silverlight QuickStart does not load Modules A and C with the application; also they are not clickable.
	
	Several links in the Prism documentation may not be working and some sections and images may be outdated. This will be solved in the final release of the guidance.
	
	Some items are still under construction and might change in future drops.

	1.6 >>> Prism Library and Code Access Security <<<
	--------------------------------------------------
	The Prism Library uses all the default .NET Framework settings with respect to signing assemblies and code access security. It is a recommended practice to strong name all your assemblies, including the Prism Library assemblies, shell assembly, and any modules you might want to create. This is not a requirement. It is possible to load assemblies that have not been signed into a (signed or unsigned) Prism Library application. You can change this behavior by applying a .NET security policy that disallows the use of unsigned assemblies or one that changes the trust level of an assembly. Please note that the .NET Framework does not allow you to load partially trusted assemblies, unless you add the AllowPartiallyTrustedCallers attribute to the Prism Library assemblies. 

	For more information, see "Code Access Security" in the ".NET Framework Developer’s Guide" on MSDN (http://msdn.microsoft.com/en-us/library/930b76w0.aspx).


-----------------------------------------
2.> PRISM LIBRARY BREAKING CHANGES
-----------------------------------------

	2.1 >>> New Assemblies <<<
	------------------------------------
	With the addition of supporting the Managed Extensibility Framework (MEF), there were several changes to the Prism Library. You can now use MEF as the dependency injection container.  This new functionality required two new assemblies in the Prism Library solution: Composite.MefExtensions.Desktop and Composite.MefExtensions.Silverlight. These assemblies are matched with new unit test assemblies.
	For more information about dependency injection containers and how to use MEF, see the topic "Dependency Injection Container and Services" in the Prism4.chm file.

  2.2 >>> Bootstrapper API Changes <<<
	------------------------------------
	With the addition of supporting MEF, there were several changes to the Prism Library's bootstrapper.  These changes include:
  • A new Bootstrapper base class was added to the Microsoft.Practices.Composite.Modularity namespace in the Composite.Presentation.Desktop and Composite.Presentation.Silverlight assemblies.  
    - The UnityBootstrapper class (in the Composite.UnityExtensions.Desktop and Composite.UnityExtensions.Silverlight assemblies) extends the new Bootstrapper base class.
    - A new MefBootstrapper class (in the Composite.MefExtensions.Desktop and Composite.MefExtensions.Silverlight assemblies) extends the new Bootstrapper base class.
  • To facilitate the addition of the Bootstrapper base class several properties and methods changed names for consistency:
    - The LoggerFacade property was renamed to Logger.  
    - The Logger property is set in the Run method of the different bootstrappers using the result of the CreateLogger method rather than the get property on the Logger.
    - The GetModuleCatalog method in the UnityBootstrapper is now called CreateModuleCatalog.
  • Several new methods were added to better separate concepts:
    - The virtual ConfigureModuleCatalog was added to allow modifying the catalog after creation as part of the bootstrapping process.
    - The virtual ConfigureServiceLocator method was added to allow overriding the configuration of the ServiceLocator.
    
	For more information on the Bootstrapper classes, see the topic "Bootstrapper: Starting Your Prism Application" in the Prism4.chm file.  For a full API description of the Bootstrapper, UnityBootstrapper, and MEFBootstrapper, see the Prism4ApiReference.chm


-----------------------------------------
3.> BASIC MODEL-VIEW-VIEWMODEL QUICKSTART
-----------------------------------------

	3.1 >>> Overview <<<
	--------------------
	The Basic Model-View-ViewModel (MVVM) QuickStart demonstrates how to build a very simple application that implements the MVVM presentation pattern. This is provided to help you learn the basic concepts of the MVVM pattern.
		
	3.2 >>> Scenario <<<
	--------------------
    The BasicMVVM QuickStart represents a subset of a survey application. A survey with different types of questions is shown; after the questionnaire is completed, it can be submitted. The user can also choose to reset the answers to the questions.
	
	3.3 >>> Running Instructions <<<
	--------------------------------
	To load the Basic Model-View-ViewModel QuickStart, open the file Quickstarts\BasicMVVM\BasicMVVMQuickStart.sln in Visual Studio.
	To run the QuickStart, set the "BasicMVVMApp.Web" project as the startup project, set BasicMVVMAppTestPage.html as the start page, and then press F5.


-----------------------------------
4.> MODEL-VIEW-VIEWMODEL QUICKSTART
-----------------------------------

	4.1 >>> Overview <<<
	--------------------
	The Model-View-ViewModel (MVVM) QuickStart demonstrates how to build an application that implements the MVVM presentation pattern, showing some of the more common challenges that developers can face, such as validation, user interface (UI) interactions, and data templates.
		
	4.2 >>> Scenario <<<
	--------------------
    The MVVM QuickStart represents a subset of a survey application. A survey with different types of questions is shown; after the questionnaire is completed, it can be submitted. The user can also choose to reset the answers to the questions.
	
	4.3 >>> Running Instructions <<<
	--------------------------------
	To load the Model-View-ViewModel QuickStart, open the solution file Quickstarts\MVVM\MVVM.sln in Visual Studio.
	To run the QuickStart, set the "MVVM" project as the startup project, and then press F5.

--------------------------
5.> MODULARITY QUICKSTARTS
--------------------------

	5.1 >>> Overview <<<
	--------------------
	The Modularity QuickStarts demonstrate how to code, discover, and initialize modules using Prism.
	
	5.2 >>> Scenario <<<
	--------------------
	The Modularity QuickStarts represent an application composed of several modules that are discovered and loaded in the different ways supported by the Prism Library using MEF and Unity as the composition containers. They also show how to use the new download progress features of Prism 4.0.
	
	5.3 >>> Running Instructions <<<
	--------------------------------
	WPF versions:
		>> To load and run the MEF version of the QuickStart, open the solution file Quickstarts\Modularity\Desktop\ModularityWithMef\ModularityWithMef.Desktop.sln in Visual Studio,
		set the ModularityWithMef.Desktop project as the startup project, and then press F5.
		>> To load and run the Unity version of the QuickStart, open the solution file Quickstarts\Modularity\Desktop\ModularityWithUnity\ModularityWithUnity.Desktop.sln in Visual Studio,
		set the ModularityWithUnity.Desktop project as the startup project, and then press F5.

	Silverlight versions:
		>> To load and run the MEF version of the QuickStart, open the solution file Quickstarts\Modularity\Silverlight\ModularityWithMef\ModularityWithMef.Silverlight.sln in Visual Studio,
		set the ModularityWithMef.Silverlight.Web project as the startup project, and then press F5.
		>> To load and run the Unity version of the QuickStart, open the solution file Quickstarts\Modularity\Silverlight\ModularityWithUnity\ModularityWithUnity.Silverlight.sln in Visual Studio,
		set the "ModularityWithUnity.Silverlight.Web" project as the startup project, and then press F5.


-------------------------------------------------
6.> MODEL-VIEW-VIEWMODEL REFERENCE IMPLEMENTATION
-------------------------------------------------	

	6.1 >>> Overview <<<
	--------------------
	The Model-View-ViewModel application is a reference implementation that illustrates a complete survey application and demonstrates complex challenges that developers face when creating applications using the MVVM pattern. 
	
	6.2 >>> Scenario <<<
	--------------------
	The Model-View-ViewModel Reference Implementation (MVVM RI) represents a survey application. The main window shows a list of available questionnaires; when one is selected, an empty survey with different types of questions is shown. After the questionnaire is completed, it can be submitted. After that, the user is returned to the list of available questionnaires.
	
	6.3 >>> Running Instructions <<<
	--------------------------------
	To load the MVVM RI, open the solution file MVVM RI\MVVM.sln in Visual Studio.
	To run the reference implementation, set the MVVM.Web project as the startup project, and then press F5.
	
-----------------------------------------
7.> STOCK TRADER REFERENCE IMPLEMENTATION
-----------------------------------------	

	7.1 >>> Overview <<<
	--------------------
	The Stock Trader application is a reference implementation that illustrates the baseline architecture. Within the application, you will see solutions for common, and recurrent, challenges that developers face when creating composite WPF applications. 
	It is a reference for building composite applications.
	
	7.2 >>> Scenario <<<
	--------------------
	The Stock Trader RI illustrates a fictitious, but realistic financial investments scenario. Contoso Financial Investments (CFI) is a fictional financial organization that is modeled after real financial organizations.
	CFI is building a new composite application to be used by their stock traders.
	
	7.3 >>> Running Instructions <<<
	--------------------------------
	To load the Stock Trader RI open the solution file StockTrader RI\StockTraderRI.sln in Visual Studio.
	To run the WPF version of the reference implementation, set the StockTraderRI project (located at the Desktop solution folder) as the startup project, and then press F5.
	To run the Silverlight version of the reference implementation, set the StockTraderRI project (located at the Silverlight solution folder) as the startup project, and then press F5.
	