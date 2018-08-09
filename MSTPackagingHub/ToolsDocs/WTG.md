
# Add a Package to Web Template Generator

1. Navigate to https://itweb.mst.edu/auth-cgi-bin/cgiwrap/deskwtg/menu.pl
2. Click ```Edit Source Configuration Data```
3. Exand the Template Source Data table under the heading ```{OS Version}-x64-sccm2012``` (do not expand ```{OS Version}-x64-sccm2012-image```)
4. Make a copy of an existing page by finding a row with a ```Section``` equal to ```{OS Version}-x64-sccm2012 / packages``` and clicking ```Copy```. **DO NOT CLICK ```EDIT```**
5. Update the following fields:
	-
	-
	-

# Advanced Documentation

## Package Developer Documentation
In general, comments begin with a semicolon.  Keywords are, by convention, entirely UPPERCASE.

### Configuration Sections
These can be found under within the ```Edit Source Configuration Data``` section of the WTG.

#### platforms / platforms / config
This is a special area for the configuration of the template generator itself.
The format for each available platform is:

```
BEGINPLATFORM:(platform-key)
NAME:(Platform Label, displayed to installers)
PATH:(Path to be added before every package installer line -- UNC or otherwise.  May be blank.)
TYPE:(OS type: one of windows, mac, or linux)
ORDER:(Numerical order to sort on for the menu)
DISABLED
ENDPLATFORM
```

Disabled is an optional flag that allows the platform to be unavailable to installers without desktop:wtg:admin.  This obsoletes the DISABLEDPLATFORM package key that was previously used on a per-platform basis.

Changes made to this file will be reflected on the menu **immediately**.  To prevent changes from being visible to normal installers, mark the platform DISABLED.

For safety, this package cannot be deleted.  Really.  Don't attempt it.  The application should error, but if this package is removed for some reason, the entire template generator will fail to operate.

#### config
These files are specific to each platform.

Config is broken into four sections (each with their own individual "package" data).  Anything after the "config-" in the name is displayed as a template option in the actual generator.  For example, "config-desktop" creates the "desktop" type in the template generator.

The four package names are:
  * pkgs-mandatory
  * pkgs-optional
  * pkgs-post
  * pkgs-pre

**pkgs-mandatory**

pkgs-mandatory is the "Mandatory Software, Manual Install Optional."  In other words, these are packages required to be a "standard" build, but as such, the installer may choose to disable these packages at their own risk.  By default, all packages are selected.

**pkgs-optional**

pkgs-optional is the "Optional Software" section, performed after pre and mandatory, but before post and custom.  These packages should be tested for each possible configuration to ensure compatibility, and each package specified here should include its own dependencies; that is, the package should not require any other package in the optional or mandatory section be selected.  By default, no packages are selected.

**pkgs-post**

pkgs-post is the "Additional Mandatory Software" section.  These packages are required to finish configuring the system as a standard build, and as such, are required.  All packages are selected by default, and normal installers cannot deselect these packages (installers with desktop:wtg:bypass can).

**pkgs-pre**

pkgs-pre is the "Mandatory Software" section.  These packages are required to set up the install for a standard build, and as such, cannot be disabled.  All packages are selected by default, and normal installers cannot deselect these packages (installers with desktop:wtg:bypass can).

**Format**

The format for these configuration sections is identical.  Each line is PACKAGE:(packagename), where packagename exists in the current platform.  Groups (different versions of the same package, generally) can be created with STARTGROUP:(Group Label).  A default package is usually specified first, but may be manually changed by using DEFAULTPACKAGE:(packagename) instead.  If the DEFAULTPACKAGE keyword is not used, the first package will be assumed default.  End the group with the ENDGROUP line.  Also, if this is in a mandatory section (pkgs-pre or pkgs-post), only the default option will be selectable by default: the installer will be shown all choices, but the default will be installed.  If the installer should be able to change the version (but must install ONE version), use ENABLEDSTARTGROUP:(Group Label) instead.  ENABLEDSTARTGROUP has no special meaning outside of pre/post.  Putting it all together, you get something looking like this:

```
ENABLEDSTARTGROUP:Adobe Acrobat
DEFAULTPACKAGE:acroreader.8_1_3
PACKAGE:adobereader.9
PACKAGE:acrobat.8
ENDGROUP
```

This translates to allow the installer to choose to install Acrobat Reader 8.1.3, Adobe Reader 9, or Acrobat 8, but by default, install 8.1.3, and do not allow the installer to not install any package.

## Application (deskwtg) Developer Documentation
Basic file structure:
  * auth-cgi-bin/
    * browse.pl -- search/view templates (end-user)
    * generate.pl -- handles actual template creation (end-user)
    * menu.pl -- provides menu based on PrivSys privileges (end-user)
    * source-edit.pl -- edit template configurations (admin)
    * source-export.pl -- export template configurations to CSV (admin)
    * template-purge.pl -- manually purge old templates (admin)
  * bin/
    * clean-templates.pl -- automatically remove templates older than 30 days
    * deskwtg.crontab -- crontab to run clean-templates daily
  * cgi-bin/
    * template.pl -- fetch a template for install
    * rpc/
      * GetTemplate -- fetch a template for install (SimpleRPC)
  * html/
    * icons/
      * blue-minus.gif -- icon for source-edit.pl (collapsible tables)
      * blue-plus.gif -- icon for source-edit.pl (collapsible tables)
    * index.html -- redirect to menu.pl
  * libs/
    * WTG.pm -- common modules and functions

Main functions are all documented using mstdoc-style documentation.  Thus, I won't attempt to replicate this documentation.  I will, however, attempt to point out some of the nuances of the code.

First, it is a simple object-oriented style application.  WTG.pm has a few non-object-oriented methods being used as "helpers" internally, that are not exported.  The new method of WTG requires both an AppTemplate object and a OracleObject in order to function--absence of either **will not** work (only the AppTemplate object needs to be passed in to new; new will create the DB connection).  Some of the methods do have "suppress" options to prevent AppTemplate errors from spewing out (such as when in a plain-text document, or an RPC).  It is up to the calling methods to handle these cases appropriately.

The design of this has been altered such that as much as possible can be changed through the web interface (to allow routine updates to happen without code review), while still keeping the same look/feel/behavior as the existing app.  Common sections of code have been separated by "platform types"--for example, windows, mac, linux.  These determine the behavior of generate.pl (and related functions) when handling the template.  This way, only options specific to Mac will be shown to Macs, Windows to Windows, etc.

Templates are stored internally in a "platform/packagename" format, with custom packages denoted between ```;CUSTOM``` and ```;ENDCUSTOM``` lines.  This allows for templates to be edited, as well as for problems with packages to be corrected without regenerating templates.