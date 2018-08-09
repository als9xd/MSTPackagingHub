

![enter image description here](https://wiki.mst.edu/deskinst/_media/docs/developer/windows/application_repackaging/installmonkey_docs/image001.jpg)

# Creating a package with the InstallMonkey shared code libraries
Tim Williams 
June 2007 
updated May 2008 by Todd Hartman
last updated August 2018 by Alex Schrimpf

## Introduction

This document will attempt to explain the procedure for packaging applications within the new automated installation environment, **InstallMonkey** , co-developed by Bryan Thompson and Charles Huber. 

For help in preparing an application to be scripted, please refer to the [Windows Application Packaging and Repackaging website](https://wiki.mst.edu/deskinst/docs/developer/windows/application_repackaging) on the [Desktop Infrastructure page](https://wiki.mst.edu/deskinst/) 

As there are multiple types of packages that get created (repackaged, vendor-supplied MSI, setup.exe, etc.), this document is divided into sections to explain each of them in-turn.
You should be able to package any application by combining the techniques illustrated in this document. 

# Creating a Package

Application packages use a Perl Module to perform an installation.  This means that the actual install script that is created for each application is incredibly short.  Example code will be provided within this document, and a templates is available [here](/Home/InstallMonkey?id=0)

## Steps

1. Create a [**Base Directory**](#package-base-directory) to hold the package
2. Copy any files that are required for your software installation into the [**Data Directory**](#package-data-directory)
3. Create an [**Install Monkey Script**](#install-monkey-script) within your [**Base Directory**](#package-base-directory)
4. *(Optional)* Create a [**Package README File**](#package-readme-file)
5. [**Add the package to SCCM**](/Home/Tools#sccm-console-doc-label)
6. [**Add the package to WTG**](/Home/Tools#sccm-console-doc-label)

### Package Base Directory

All packages' [**Base Directory**](#package-base-directory) are located within your ```K:\``` drive (mapped to ```\\minerfiles.mst.edu\dfs\software\itwindist\```) using this format:

```K:\{OS Version}\appdist\{Application Name}.{Application Version}\{Development Stage}```

#### Examples

| OS Version    | Application Name | Application Version | Development Stage | Resulting Directory Structure             |
| ------------- | ---------------- | ------------------- | ----------------- | ----------------------------------------- |
| Windows 7     | Firefox          | 2.0.0.4             | In Development    | ```K:\win7\appdist\firefox.2_0_0_4\dev``` |
| Windows 10    | Autocad          | 2007                | Production Ready  | ```K:\win10\appdist\autocad.2007\prod```  |


### Package Data Directory

After you have the package's [**Base Directory**](#package-base-directory) created, make a sub-directory within the [**Base Directory**](#package-base-directory) called ```data```.  This directory will hold everything that the package's **Install Monkey Script** will copy to ```C:\SourceFiles\{Application Name}.{Application Version}``` on local hard drive. This is done so that packages can be installed without network access.

Using the Firefox 2.0.0.4 example, the package's [**Data Directory**](#package-data-directory) would look like this:

```
K:\win7\appdist\appdist\firefox.2_0_0_4\prod\data\ 
```

### Install Monkey Script

Now that the package's [**Base Directory**](#package-base-directory) and [**Data Directory**](#package-data-directory) are in place,create a copy the [template update.pl](/Home/InstallMonkey?id=0) to your package's [**Base Directory**](#package-base-directory). 
Open up the readme.txt file and populate it with the required information.  This is all outlined in the template document itself. 
Open the [**Install Monkey Script**](#install-monkey-script) in your text editor of choice and fill in the [**Script Comment Header**](#script-comment-header) information.

#### Script Comment Header

```perl
#VLC Media Player 0.8.5
#Packaged June 2007
#Packaged by Charles Huber
```

Simply enough, the script's [**Script Comment Header**](#script-comment-header) contains the name of the package, when the package was created, and the name of the person who created the package.  For each new package, populate these lines with the appropriate information for your package. 

**Note:** Information about the process by which you created the package should go in the package's [**README File**](#package-readme-file)

#### Include the InstallMonkey Library

```perl
# 1) Global InstallMonkey options that must be specified before you load the module.
our %INSTALLMONKEY_OPTIONS;
BEGIN {
    %INSTALLMONKEY_OPTIONS = (
        # same name as the appdist directory
        package_id => 'Package.Version',

        # some unique id that's updated any time the package undergoes
        #  any sort of minor revision
        package_revision => '20140115T1200',
    );
}

# 2) Add InstallMonkey Library to the path
use lib (
    '\\\\minerfiles.mst.edu\dfs\software\loginscripts\im',
    $ENV{'WINDIR'}.'\SYSTEM32\UMRINST',
    'C:\temp',
);
# 3)
use InstallMonkey::Shared;
```

This portion of the [**Install Monkey Script**](#install-monkey-script) 

1. Sets InstallMonkey's required options
2. Tells the script where to look for the InstallMonkey Libray
3. Imports the InstallMonkey Library

#### Part 3 - The Installation

```perl

# 1) Define a subroutine to use when start the install
sub my_installation
{
	# Run installer.msi located with the package's Data Directory
	my $installer_cmd_command = "\"${get_pkg_sourcefiles()}\"\\installer.msi /qn\" REBOOT=ReallySuppress \"";
	if(!run_command($installer_cmd_command)){
		output("Installer Failed!");
		return 0;
	}
	output("Installer Succeeded.");
	return 1;
}

# 2) Tell InstallMonkey to run the install process
do_install(
	   # Tell InstallMonkey which OS Versions are acceptable to install to
       allowed_versions => [OSVER_WIN7_SP0, OSVER_WIN7_SP1, OSVER_WIN10_SP0],
	   # Tell InstallMonkey which machine types are acceptable to intall to
       allowed_regs => ['clc', 'desktop', 'traveling',
                         'virtual-clc', 'virtual-desktop'],
	   # Tell InstallMonkey to install the software using our "my_installation" subroutine
	   install_sub => \&my_installation,
);
# 3) Tell InstallMonkey to exit successfully
IM_Exit(EXIT_SUCCESS);
```
The `do_install()` subroutine is what actually tells InstallMonkey to run the install process. There are a whole host of different options you can pass as parameters to the ```do_install()``` subroutine for configuring how and under what conditions InstallMonkey should install your software.

A full list of the possible

*  ```allowed_versions``` can be found [here](/Home/InstallMonkey?id=2#os-version-constants)
* ```allowed_regs``` as well as other options can be found [here](/Home/InstallMonkey?id=2#architecture-specific-options)

### Package README File

A [**README File**](#package-readme-file) can be a great way to document difficult packages. If you encountered any problems along the way for creating you package including:

* Ambiguous licensing procedures
* Bugs in the software's installer
* Reasoning behind major deviaions from the standard packaging procedure
* Any insights obtained through trial-and-error

make note of them within a ```README.txt``` file within the package's [**Base Directory**](#package-base-directory)

**Bonus**: If you are creating a large README. You should try typing your documentation in [Markdown](https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet) for fancy styling such as was used in this very guide. By convention [Markdown](https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet) files have the extension ```.md``` so your file should be named ```README.md```.

### Prerequisites and Modifying File and Registry Permissions

Many apps require another application to be installed first.  In addition to this, some applications require that all users have write access to a folder on the hard drive or a registry key.  The culvertmaster.2005 package is an excellent example of both of these things.
```perl

# Add InstallMonkey Library to the path

use lib "K:\\winxp\\appdist\\im";
use InstallMonkey::Shared;
 
sub preinstall
{
       if( !is_installed("msi_dotnetfx_1_1") )
       {
             output("\nA .NET 1.1 installation was not found.\nPlease install the dotnetfx package before running this package.\n”);
             error_notify("\nA .NET 1.1 installation was not found.\nPlease install the dotnetfx package before running this package.\n”);
              return 0;    
       }
 
       return 1;
}
 
sub postinstall
{
       set_registry_acls("reg_perms.ini");
       set_file_acls("c:\\Program Files\\Haestad\\CVM", "/t /c /e /G everyone:f");
       set_file_acls("c:\\lmdebug.log", "/c /e /g everyone:f");
 
       return 1;
}
 
do_install(
       allowed_versions => ["5.1.2600", "6.0.6000", "6.0.6001"],
       allowed_regs => ["clc", "desktop"],
       preistall_sub => \&preinstall,
       postinstall_sub => \&postinstall,
       additional_msi_properties => "REBOOT=\"ReallySuppress\" AUTOUPDATECHECK=\"0\""
);
```

The culvertmaster.2005 example package requires the .NET 1.1 package b e installed before the app is installed.  The sub preinstall checks for the presence of.NET 1.1 and if it is not found, kills the install.

To call sub preinstall, add a line to do_install():
```perl
preinstall_sub => \&preinstall,
```
The culvertmaster 2005 package also requires write access to various locations on the hard drive, as well as to a few registry locations.  The sub postinstall performs the file permission functions in-line, and the registry permission functions with the aid of an .ini file. You can find more information at http://technet.microsoft.com/en-us/library/bb727154.aspx

To call sub postinstall, add a line to do_install():
```perl
postinstall_sub => \&postinstall,
```

Modifying registry permissions is done through the line:
```perl
set_registry_acls("reg_perms.ini");
```