# Install Monkey Shared Library

The subroutines documented herein are not listed in alphabetic order!
I'm keeping the docs with the code, and the code is organized by function,
not name. Sorry.

Begin-Doc
Modified: $Date: 2015-11-09 14:45:24 -0600 (Mon, 09 Nov 2015) $
Name: InstallMonkey::Shared
Type: script
Description: A list of utility routines commonly used in install scripts,
   accesed C-style.
Language: Perl
LastUpdatedBy: $Author: t-dwlfk2 $
Version: $Revision: 124 $
Doc-Package-Info: file://///minerfiles.mst.edu/dfs/Software/loginscripts/im/InstallMonkey/doc/Shared.html
Doc-SVN-Repository: $URL: https://svn.mst.edu/project/instmonkeylib/trunk/InstallMonkey/Shared.pm $
RCSId: $Id: Shared.pm 124 2015-11-09 20:45:24Z t-dwlfk2 $
End-Doc

# InstallMonkey::Shared

InstallMonkey Shared Code Library

Version: **1.6.29**

## get\_windows\_version()

### Returns

a string containing the Windows OS version number

### OS Version Constants

- **OSVER\_XP\_32**

    32-bit Windows XP

    lexically equivalent to `'5.1.2600'`

- **OSVER\_XP\_64**

    64-bit Windows XP

    lexically equivalent to `'5.2.3790'`

- **OSVER\_VISTA\_SP0**

    Windows Vista (SP0)

    lexically equivalent to `'6.0.6000'`

- **OSVER\_VISTA\_SP1**

    Windows Vista SP1

    lexically equivalent to `'6.0.6001'`

- **OSVER\_VISTA\_SP2**

    Windows Vista SP2

    lexically equivalent to `'6.0.6002'`

- **OSVER\_WIN7\_SP0**

    Windows 7 (SP0)

    lexically equivalent to `'6.1.7600'`

- **OSVER\_WIN7\_SP1**

    Windows 7 (SP1)

    lexically equivalent to `'6.1.7601'`

- **OSVER\_WIN8\_SP0**

    Windows 8 (SP0)

    lexically equivalent to `'6.2.9200'`

### Examples

```
$version = get_windows_version();

if (get_windows_version() eq OSVER_XP_32) {
    output("32-bit version of XP...\n");
}
```

_General OS checking can be accomplished more easily with ["is\_Vista()"](#is_vista),
["is\_XP()"](#is_xp), ["is\_Win7()"](#is_win7), and ["is\_Win8()"](#is_win8)._

## detect\_os\_architecture()

Discern the architecture of the operating system (not the CPU) and
return it as a string

### Returns

- Windows XP x86 running on a 32-bit CPU will return "x86".
- Windows Vista x86 running on a 32-bit CPU will return "x86".
- Windows XP x86 running on a 64-bit CPU will return "x86".
- Windows Vista x86 running on a 64-bit CPU will return "x86".
- Windows XP x64 running on a 64-bit CPU will return "x64".
- Windows Vista x64 running on a 64-bit CPU will return "x64".

### OS Architecture Constants

- **OSARCH\_x86**

    32-bit (Intel x86)

    lexically equivalent to `'x86'`

- **OSARCH\_x64**

    64-bit (amd64), not Itanium (ia64)

    lexically equivalent to `'x64'`

### Examples

```
$arch = detect_os_architecture();


if (detect_os_architecture() eq OSARCH_x86) {
    # 32-bit stuff
} else {
    # 64-bit stuff
}
```

### References

- [http://msdn.microsoft.com/en-us/library/aa394373(VS.85).aspx](http://msdn.microsoft.com/en-us/library/aa394373\(VS.85\).aspx)

    The document claims that Win32\_ComputerSystem will return the type of the
    of the OS, not the CPU. This is incorrect. Vista ALWAYS returns x64.

- [http://msdn.microsoft.com/en-us/library/aa394102(VS.85).aspx](http://msdn.microsoft.com/en-us/library/aa394102\(VS.85\).aspx)

    Win32\_ComputerSystem appears to be more reliable, though the Vista x64 SP1
    I tried it one wasn't in the list of possible types in the documentation.

## Global Options

InstallMonkey attempts to deduce properties of the script based on its
containing directory and other assumptions in order to keep the install
scripts short. This has its limitations. Should you need to alter some
predefined behavior, it is possible using the global variable
**`%::INSTALLMONKEY_OPTIONS`**.

### Options

- **package\_id**

    The package ID specifies the name and version of the package.

    Specifying it will override the default behavior of attempting to get
    the name and version from the containing directory, which it expects to
    be in the form `_package_name_._version_`.

- **package\_revision**

    The package revision is an identifier used to distinguish between
    revisions of the same package.

    A package should be revised if there aren't drastic changes from the previous
    version (minor software updates, install script fixes, etc.) and if the
    old revision doesn't need to be used any longer.

- **output\_log**

    The output log is imputed using the **package\_id**. If you want to use a
    different output log (say, if the default location is unavailable), you
    may specify a path to the desired output log location.

### Examples

#### Common Configuration

```perl
BEGIN {
    %::INSTALLMONKEY_OPTIONS = (
        package_id => 'App.19',
        package_revision => '3.3.2352.20090326'
    );
}
```

#### Common Configuration with Different Output Log

```perl
BEGIN {
    %::INSTALLMONKEY_OPTIONS = (
        package_id => 'App.19',
        package_revision => '3.3.2352.20090326'
        output_log => '\\\\minerfiles.mst.edu\\dfs\\users\\'.
                      $ENV{'USERNAME'}.'\\user_update.log'
    );
}
```

#### Different Package ID (and output log, by default)

```perl
BEGIN {
    %::INSTALLMONKEY_OPTIONS = (
        package_id => 'App.19',
    );
}
```

#### Different Output Log

```perl
BEGIN {
    %::INSTALLMONKEY_OPTIONS = (
        output_log => 'C:\\temp\\temp.log',
    );
}
```

#### Specify Package Revision

```perl
BEGIN {
    %::INSTALLMONKEY_OPTIONS = (
        package_revision => '11.2.3.20090325',
    );
}
```

## process\_command\_line()

Parse the command-line looking for InstallMonkey-specific
flags.

Recognized command-line parameters are removed from @ARGV.
Parameters not recognized are left in @ARGV so that the calling
script can process them.

'%command\_line\_options' (overrides most flags passed to ["do\_install()"](#do_install))
The use of these is largely for testing purposes.

```perl
--data-dir <data_directory>
--IM-debug
--package-id
--package-revision
--exit-on-failure
--remove-local-im
--no-app-log
--no-event-log
--no-install-check
--no-os-arch-check
--no-os-version-check
--no-product-key
--no-registration-check
--no-source-files
--additional-msi-properties <properly_escaped_string>
--msi-name <path_to_msi>
--application-architecture <architecture>
--scope <scope> ('system' or 'user')
--output-log <path_to_log_file>
--already-installed-exit-code <n>
```

This subroutine also checks the 'flags' directory (See
["get\_flags()"](#get_flags)) for flags with names corresponding to the
command-line parameters, with the 'IM\_' prefix prepended.

Command-line parameters override flags. Both override package defaults.

Internally, these flags are represented using underscores instead of
dashes. If you wish to check for one, use

```
$command_line_options{flag_words_sep_by_underscores}
```

### Tests

IMTest-command-line.pl

### Returns

This subroutine does not have a meaningful return value.
It has the side effect of storing configuration options passed
via the command-line into a global hash, **%command\_line\_options**.

### Positional Parameters

This subroutine uses the global **@ARGV** and takes no named parameters.

## get\_flags()

InstallMonkey can take configuration options via the command line
and also by text files in the flags directory. This is primarily useful
when a tester wishes to affect the script behavior of all InstallMonkey
scripts run on a machine (e.g. ignoring the OS architecture for a
test platform).

### Returns

returns the location of the flags directory.

### Examples

- _Set a Flag_

    ```perl
    ...
    $flag_dir = get_flags();
    my $F; if (open($F,'>',$flag_dir.'\\IM_ignore_os_architecture')) {
        close($F);
    }
    ...
    ```

## install\_msi\_internal()

installs the MSI specified by parameter 0 from the
Source Files directory with the given additional MSI properties

Much of the functionality of the original function has been moved to
["install\_msi()"](#install_msi). We recommend that package developers use install\_msi()
instead of this function.

### Returns

returns an "exit code" style boolean (0-true, nonzero-false/error number)
returned from the underlying MsiExec.exe invocation.

### Positional Parameters

1. **MSI name**

    It must be a file or relative path from the source files package directory.

2. **MSI properties** _(as a scalar)_

    These are passed to msiexec on the command-line. They are typically
    specified with a `<NAME`="<VALUE>"> format.

3. **matching\_application\_architecture**

    a boolean value signifying that the application architecture matches the
    underlying OS architecture

### Examples

- _Simple Examples_

    ```
    install_msi_internal("installer.msi", "REBOOT=\"ReallySuppress\"");

    install_msi_internal("setup.msi", "");
    ```

## install\_msi()

Install an MSI with standard settings.

Specified settings override the defaults.

This is meant to allow install scripts the power to install Windows
Installer packages just like IM does internally without having to
specify more options than those which differ from the defaults.

### Returns

returns true/false on success/failure

### Named Parameters

- **msi** => _&lt;path\_to\_msi_>

    You can use an .MSI or an .MSP.

    You should not specify both **app\_id** and **msi**.

- **app\_id** => _&lt;application\_ID_>

    This is the GUID of the application. It is usually only available once
    an application is installed, and is necessary for a repair/reinstall or
    uninstall.

    You should not specify both **app\_id** and **msi**.

- **msi\_action\_flag** => _<MSI\_action\_flag_>

    The flag to use with MsiExec.

    By default, this is 'i' (or 'p' with an .MSP).

- **msi\_properties** => _&lt;msi\_properties\_string_>

    This must include ALL the MSI properties you want to use. It will
    override the default properties.

- **additional\_msi\_properties** => _&lt;additional\_properties\_string_>

    This should include any MSI properties you want to use in addition to

    ```
    InstallMonkey::Shared::DEFAULT_MSI_PROPERTIES
    ```

- **msiexec\_args** => _&lt;additional args_>

    If you need to pass additional arguments to msiexec.exe, use this
    parameter. These arguments are combined with the default properties.

- **logging** => _&lt;msiexec\_logging\_parameters_>

    These are the logging parameters to give to msiexec.

    If you use this, you must specify the flags and the log file location.
    This flag incorporates what could be individually specified by [#logfile](https://metacpan.org/pod/#logfile) or
    [#logfile\_name](https://metacpan.org/pod/#logfile_name) and [#logfile\_location](https://metacpan.org/pod/#logfile_location). The overlap is intended to allow
    you to specify only things that are different from the defaults.

- **logfile** => _&lt;path\_to\_MSI\_log\_file_>

    This is the path to the log file.

    ["logfile\_name"](#logfile_name) and [#logfile\_location](https://metacpan.org/pod/#logfile_location) won't be used if this is specified.

- **logfile\_name** => _<MSI\_log\_file_>

    This isn't a path. It's just the name of the file.

    Use this if you only want the name of the log to be different from
    the default (_&lt;pkg\_name_>-_&lt;pkg\_version_>.txt)

- **logfile\_location** => _<MSI\_log\_file\_directory_>

    This is the directory where the log file will go.

    ["get\_applogs\_dir()"](#get_applogs_dir) is used for this value by default.

- **quiet** => _&lt;quiet\_arg_>

    typically '/qn' (the default)

- **ReturnCommandInfo** => _&lt;hashref_>

    A return parameter (hashref) in which to store command info.
    It is the same as the one used in ["run\_command()"](#run_command).

- **IgnoreExitCodes** => _&lt;arrayref\_of\_int_>

    Ignore certain exit codes as failures.

- **verbose** => _&lt;verbose\_output\_boolean_>

    Whether or not to use verbose output (to the console).

### Examples

- _Simplistic Use_

    ```perl
    install_msi( msi => get_pkg_sourcefiles().'\installer.msi' );
    ```

- _Simple Uninstall_

    ```perl
    my $log = get_default_app_log();
    $log =~ s/(\.[^\.]+)$/_uninstall$1/;

    install_msi( app_id => '{3B410500-1802-488E-9EF1-4B11992E0440}',
                 msi_action_flag => 'x',
                 logfile => $log );
    ```

- _Checking the Return Value_

    ```perl
    if (install_msi( msi => get_pkg_sourcefiles().'\installer.msi' )) {
        output("Install failed.\n");
    } else {
        output("OK\n");
    }
    ```

- _Specifying MSI Properties in Addition to the Defaults_

    ```perl
    install_msi( msi => get_pkg_sourcefiles().'\installer.msi',
                 additional_msi_properties => 'REBOOT=ReallySuppress',
               );
    ```

- _Applying MST Transform(s)_

    ```perl
    my @transforms = (
        build_path_short(get_pkg_sourcefiles(),'xform1.mst'),
        build_path_short(get_pkg_sourcefiles(),'xform2.mst'),
        build_path_short(get_pkg_sourcefiles(),'xform3.mst'),
    );
    my $transforms_list = join(';',@transforms);
    install_msi( msi => get_pkg_sourcefiles().'\installer.msi',
                 additional_msi_properties => 'TRANSFORMS="'.$transforms_list.'"',
               );
    ```

- _Overriding ALL the Default MSI Properties_

    ```perl
    install_msi( msi => get_pkg_sourcefiles().'\installer.msi',
                 msi_properties => 'COMPANYNAME="Not Missouri S&T" ALLUSERS=0'.
                                   'CUSTOM_VALUE=3',
               );
    ```

- _Using the Default MSI Properties, Manually Modified_

    ```perl
    $properties = InstallMonkey::Shared::DEFAULT_MSI_PROPERTIES;
    $properties =~ s/ALLUSERS=.(\s?)/ALLUSERS=0\1/g;
    install_msi( msi => get_pkg_sourcefiles().'\installer.msi',
                 msi_properties => $properties,
               );
    ```

- _Changing the Log File Location (But not the File Name)_

    ```perl
    install_msi( msi => get_pkg_sourcefiles().'\installer.msi',
                 quiet => '/qb',
                 logfile_location => 'C:\temp',
               );
    ```

- _Changing the Quiet Parameter_

    ```perl
    install_msi( msi => get_pkg_sourcefiles().'\installer.msi',
                 quiet => '/qr',
               );
    ```

- _Specify All Logging Options, Returning Command Info_

    ```perl
    install_msi( msi => get_pkg_sourcefiles().'\installer.msi',
                 logging => '/lxv* '.get_default_app_log(),
                 ReturnCommandInfo => \%install_info,
               );
    ```

- _Use Global MSI Logging (When the installer doesn't give you direct control over logging)_

    ```perl
    install_msi( msi => get_pkg_sourcefiles().'\installer.msi',
                 global-MSI-logging => 1,
                 ReturnCommandInfo => \%install_info,
               );
    ```

- _Specify the Log File Name (But not the Log Directory), Returning Command Info_

    ```perl
    install_msi( msi => get_pkg_sourcefiles().'\patch1.msi',
                 logfile_name => get_package_name().'-'.get_package_version().
                                 '-patch1.txt',
                 ReturnCommandInfo => \%install_info,
               );
    ```

- _Specify the Log File (Name and Location), but not the Log Directory_

    ```perl
    install_msi( msi => get_pkg_sourcefiles().'\update.msi',
                 logfile => 'C:\temp\update-log.txt',
               );
    ```

- _Checking the Returned Command Info_

    ```perl
    install_msi( msi => get_pkg_sourcefiles().'\patch1.msi',
                 ReturnCommandInfo => \%install_info,
               );
    if ($install_info->{ReturnCode} == 0 ||
        $install_info->{ReturnCode} == 194) {
        output("OK\n");
        return 1;
    }
    return 0;
    ```

- _Ignore Certain MSIExec Exit Codes_

    ```perl
    sub install {
        return
        install_msi( msi => get_pkg_sourcefiles().'\patch1.msi',
                     'IgnoreExitCodes' => [ 194, 160 ],
                   );
    }
    ```

## start\_global\_MSI\_logging()

Enable global MSI logging.

### Returns nothing

### Positional Parameters

1. **logging\_mode** _(optional)_

    the MSI logging mode to use.

    See [http://msdn.microsoft.com/en-us/library/windows/desktop/aa370322%28v=vs.85%29.aspx](http://msdn.microsoft.com/en-us/library/windows/desktop/aa370322%28v=vs.85%29.aspx) and [http://msdn.microsoft.com/en-us/library/windows/desktop/aa370321%28v=vs.85%29.aspx](http://msdn.microsoft.com/en-us/library/windows/desktop/aa370321%28v=vs.85%29.aspx) for more details.

    Default: `'voicewarmupx'`

### Examples

- _Typical Use_

    ```perl
    #
    # Enable global logging, do install stuff, revert global settings
    #
    start_global_MSI_logging();
    # installation steps that use Windows Installer
    finish_global_MSI_logging();


    #
    # Enable global logging, specifying a particular logging set
    #
    start_global_MSI_logging('I'); # Only log status messages.
    # installation steps that use Windows Installer
    finish_global_MSI_logging();
    ```

## finish\_global\_MSI\_logging()

Copy the logs generated by the Windows Installer to the specified
location (or the system default location, if no prefix is specified).

### Returns nothing

### Positional Parameters

1. **log\_files\_prefix**

    This is the prefix (including the path) for all destination log files.
    A sequence number will be appended to the prefix for each file.

    This MSI logging scheme generates file with random names. These files
    are copied (on ascending order of creation time) to the specified
    location/prefix and renamed.

### Examples

- _Typical Use_

    ```perl
    #
    # Copy log files to AppLogs with the package ID as the prefix.
    # 
    start_global_MSI_logging();
    # installation steps that use Windows Installer
    finish_global_MSI_logging();


    #
    # Copy log files to c:\temp with a specified prefix.
    #
    start_global_MSI_logging();
    # installation steps that use Windows Installer
    finish_global_MSI_logging('C:\temp\app_test');
    ```

## do\_install()

Perform an uber-automated install.

does a standard (or customzied) install

allowed\_versions and allow\_regs are required arguments

set preinstall\_sub, install\_sub, and postinstall\_sub to customize
  install logic

setting install\_sub overrides the default MSI install behavior

pre/postinstall\_sub augment the default copy/log behavior for
  setting file/registry ACLs and such

### Returns

true if successful, false otherwise

### Named Parameters

#### ARCHITECTURE-SPECIFIC OPTIONS

All options (except allowed\_os\_architectures) will allow architecture-
specific specifications, like

```perl
install_sub_x64
prerequisite_sub_x86
data_dir_x64
```

These can be specified in conjunction with non-architicture-specific
parameters.

If both a nonspecific and a specific argument are specified and the
specified OS architecture matches the installed OS architecture,
the architecture-specific parameter is used and all others are ignored.

If no architecture-specific parameter is specified but a nonspecific
one is, the nonspecific parameter (or the default, if unspecified) is
used.

- **output\_log**

    the name of the output log (different from the MSI log)

    default: %SystemRoot%\\System32\\UMRInst\\AppLogs\\&lt;pkgname>\_output.txt

- **allowed\_versions**

    an arrayref of OS versions that are allowed to do the install

- **allowed\_os\_architectures**

    OS architectures (x86, x64) that are allowed to do the install
    ( 'x86' ) is the default.

- **allowed\_regs**

    host "registration" that is allowed to do the install

    Supported choices:

    - **desktop**
    =item **clc**
    =item **traveling**
    =item **virtual-desktop**
    =item **virtual-clc**

- **application\_architecture**

    the compiled architecture of the application; This is particularly
      important if installing a 32-bit application in a 64-bit OS.

    If unspecified, it is assumed that the application architecture
      is the same as the OS architecture.

- **data\_dir**

    the directory in which the data to be copied resides 'data' is the
    default value (specified in ["copy\_source\_files()"](#copy_source_files)).

- **prerequisite\_sub**

    callback subroutine to do any custom prerequisite checking

    This phase occurs before the ["copy\_source\_files()"](#copy_source_files) occurs.

- **preinstall\_sub**

    callback subroutine to be called after the source files are copied
      but before the actual installation is done

    All of the \*install callbacks should return a boolean value
      indicating success (true) or failure (false).

- **msi\_name**

    the name of the Windows installer package (.MSI)

    If this is unspecified and no install\_sub is not specified,
      'installer.msi' is used.

- **additional\_msi\_properties**

    additional command-line arguments passed to msiexec

- **install\_sub**

    callback subroutine to do the install instead of the default
      install method, which is install\_msi\_internal()

    All of the \*install callbacks should return a boolean value
      indicating success (true) or failure (false).

- **postinstall\_sub**

    callback subroutine to be called after the install before the product
    registry key is created or the install is logged via HTTP

    All of the \*install callbacks should return a boolean value indicating
    success (true) or failure (false).

- **validate\_sub**

    callback subroutine to verify that everything is correctly configured

    This provides an installer with one last chance to verify that everything 
    is correct and return a more meaningful exit code.

- **need\_reboot**

    If this is true, a message will be displayed saying that a reboot
    is necessary.

- **scope**

    the scope of the install (user-level or system-wide) 'user' or
    'system' (default)

    This is used for placement and detection of the product install key
    (i.e. HKCU or HKLM).

- **exit\_on\_failure**

    If true, exit if any of the install phases fails with a return code
    corresponding to the type of failure (or phase in which the failure
    occurred).

- **no\_source\_files**

    If true, don't attempt to copy any files to sourcefiles.

- **no\_app\_log**

    If true, don't attempt log the install via HTTP.

- **no\_product\_key**

    If true, don't attempt create the product key.

- **no\_install\_check**

    If true, don't attempt check for an existing package installation.

- **no\_event\_log**

    If true, don't automatically create events in the Windows event log.

- **no\_os\_version\_check**

    If true, don't check the OS version.

- **no\_os\_arch\_check**

    If true, don't check the OS architecture.

- **no\_registration\_check**

    If true, don't check machine registration.

### Examples

- _Simple MSI Install_

    ```perl
    do_install( 
        allowed_versions          => [ OSVER_XP_32, OSVER_VISTA_SP2 ],
        allowed_os_architectures  => [ 'x86', 'x64' ],
        allowed_regs              => ['clc', 'desktop', 'traveling'],
        exit_on_failure           => 1,
        installer                 => 'wildapp.msi',
        additional_msi_properties => "REBOOT=\"ReallySuppress\"");  
    ```

- _MSI Install with Custom Postinstall_

    ```perl
    sub postinst() { 
        set_registry_acls("reg_perms.ini");
        print "settings acls done!\n";
    } 
    do_install( 
        allowed_versions          => [ OSVER_XP_32, OSVER_VISTA_SP2 ],
        allowed_os_architectures  => [ OSARCH_x86, OSARCH_x64 ],
        allowed_regs              => ['clc', 'desktop', 'traveling'],
        postinstall_sub           => \&postinst,
        exit_on_failure           => 1,
        installer                 => 'wildapp.msi',
        additional_msi_properties => "REBOOT=\"ReallySuppress\"");  
    ```

- _Custom Install Subroutine_

    ```perl
    sub install64 {
        return install('64bitparam');
    }

    sub install {
        ...
        return 1;
    }

    do_install(
        allowed_versions => [ OSVER_XP_32, OSVER_VISTA_SP2 ],
        allowed_os_architectures => [ OSARCH_x86, OSARCH_x64 ],
        allowed_regs => ['clc', 'desktop', 'traveling'],
        exit_on_failure => 1,
        install_sub => \&install,
        install_sub_x64 => \&install64,
    );
    ```

- _Custom Architecture-Specific Data Directory_

    **data** is the default data directory (i.e. copied to SourceFiles).
    This example uses a different directory for amd64 OS architectures
    (say, when there's a different installer for the 64-bit version of an
    application.

    ```perl
    do_install(
        allowed_versions => [ OSVER_XP_32, OSVER_VISTA_SP2 ],
        allowed_os_architectures => [ OSARCH_x86, OSARCH_x64 ],
        allowed_regs => ['clc', 'desktop', 'traveling'],
        exit_on_failure => 1,
        msi_name => 'firefox.msi',
        data_dir_x64 => 'data64',
    );
    ```

- _Other Options_

    ```perl
     do_install(
         allowed_versions => [ OSVER_XP_32, OSVER_VISTA_SP2 ],
         allowed_os_architectures => [ OSARCH_x86, OSARCH_x64 ],
         allowed_regs => ['clc', 'desktop', 'traveling'],
         exit_on_failure => 1,
         msi_name => 'firefox.msi',
         application_architecture => OSARCH_x86,
    );
    ```

## get\_cmdline\_arg()

Retrieve the value of a (pre-parsed) InstallMonkey command-line
argument that applies to this invocation (checks OS architecture).

Not all command-line arguments are available via this mechanism. Only
InstallMonkey command-line arguments are available.

### Returns

a the value of a specified command-line argument

`undef` if no matching command-line argument was specified (and no default
was specified)

_default_ if no matching command-line argument was specified and a default
value was specified.

### Parameters

- **cmdline\_arg**

    the command-line argument (prefix) for which to check.

    If a command-line argument exists with an underscore and the OS
    architecture appended, that will be used before the an argument not
    specifying the OS architecture is used.

    The command-line arguments InstallMonkey recognizes get translated
    into variable-like names. Replace dashes with underscores in the
    command-line arguments you specify. For example, the `exit-on-failure`
    command-line argument will be turned into `exit_on_failure` for the
    purposes of checking for command-line arguments.

- **default\_value** _(optional)_

    If specified and no matching command-line argument was found, this value
    is returned instead of `undef`.

### Examples

- _Regular Use_

    ```
    $arg = get_cmdline_arg('data_dir');

    $abort = get_cmdline_arg('exit_on_failure');

    if (get_cmdline_arg('IM_debug')) {
        output("debugging statement\n");
    }
    ```

- _Specify a default value_

    ```perl
    my $data_dir = get_cmdline_arg('data_dir','data');

    my $output_log = get_cmdline_arg('output_log',$ENV{'SystemDrive'}.'\temp\log.txt');
    ```

## local\_copy\_create()

copy InstallMonkey to the local hard drive for off-network operation

### Returns

true/false on success/failure

### Examples

- _Typical Use_

    ```perl
    # Use the default SystemRoot (the one the booted OS uses)
    if (!local_copy_create()) {
        output("Cannot create local copy of InstallMonkey!\n");
        return 0;
    }

    # Use an arbitrary SystemRoot (must be done by overriding
    #   the environment variable %SystemRoot%).
    my $old_systemroot = $ENV{SystemRoot};
    override_environment('SystemRoot','C:\Windows');
    my $status = local_copy_create();
    override_environment('SystemRoot',$old_systemroot);
    if (!$status) {
        output("Cannot create local copy of InstallMonkey!\n");
        return 0;
    }
    ```

## local\_copy\_location()

Retrieve the expected location of the local copy of InstallMonkey, regardless
of whether it's installed or not.

### Returns

an absolute path to the InstallMonkey directory (not the directory
that should be used in the include path---it's parent should be used
in the include path)

### Examples

- _Typical Use_

    ```perl
    my $local_dir = local_copy_location();
    ```

## local\_copy\_reference\_count()

Retrieve the number of local IM "installs" depending on the local copy.

### Returns

the reference count of the local copy (after the optional _additive_
has been added to it)

If the reference is count is more than 1, the local copy was created
more than once and has not yet been "uninstalled" for each of these.

### Positional Parameters

1. **additive** _(optional)_

    This number should be added to the reference count.

### Examples

- _Typical Use_

    ```perl
    my $ref_count = local_copy_reference_count();

    # increase the count
    local_copy_reference_count(1);

    # decrease the count
    if (! local_copy_reference_count(-1)) {
        # what to do if it's no longer needed
    }
    ```

## local\_copy\_delete()

Delete the local copy of InstallMonkey

Technically, this decrements the reference counter on the local copy.
If the reference counter becomes zero, then the local copy is deleted.

### Returns

true/false on success/failure

### Examples

- _Typical Use_

    ```perl
    # Use the default SystemRoot (of the running OS).
    if (!local_copy_delete()) {
        output("Failed to delete the local copy of InstallMonkey!\n");
    }


    # Use an arbitrary SystemRoot
    my $orig_SystemRoot = $ENV{SystemRoot};
    override_environment('SystemRoot','C:\Windows');
    my $status = local_copy_delete();
    override_environment('SystemRoot',$orig_SystemRoot);
    if (!$status);
        output("Failed to delete the local copy of InstallMonkey!\n");
    }
    ```

## push\_file()

Save a copy of the specified file onto a stack (used as a directory).

The purpose of this function is to allow you to save a copy of a file to
be retrieved later using only the stack directory as the identifying
key. It is designed to allow ["push\_file()"](#push_file) to be invoked more than once
before ["pop\_file()"](#pop_file) is called.

### Returns

true/false on success/failure

### Positional Parameters

1. **stack\_directory**

    A path to a (potentially empty) directory where the stack will be stored.

    Typically, this will be somewhere in UMRInst, but it doesn't have to be.

    The directory will be created if it doesn't exist.

2. **file\_to\_push**

    The file specified is copied onto the stack. The stack doesn't need it to
    exist after the call to push\_file().

### Examples

- _Typical Use_

    ```perl
    # Save (push) a (fictional) config file onto a stack.
    my $curr_config = build_path($ENV{WINDIR},'app1_config.xml');
    my $stack_dir = build_path(get_inst(),'app1_config_file');
    if (! push_file($stack_dir,$curr_config)) {
        # More precise error messages will have already been emitted.
        output("Error saving '${curr_config}'!\n");
    }
    ```

## pop\_file()

Pop a file from the specified "stack" (used as a directory), saving it
to the target path specified.

### Returns

true/false on success/failure

### Positional Parameters

1. **stack\_directory**

    The directory where the stack is stored (same value passed to a previous
    invocation of ["push\_file()"](#push_file)).

2. **target\_file**

    A path where the popped file should be copied.

### Examples

- _Typical Use_

    ```perl
    # Restore (pop) a (fictional) config file off the stack.
    my $curr_config = build_path($ENV{WINDIR},'app1_config.xml');
    my $stack_dir = build_path(get_inst(),'app1_config_file');
    if (! pop_file($stack_dir,$curr_config)) {
        # More precise error messages will have already been emitted.
        output("Error restoring '${curr_config}'!\n");
    }
    ```

## get\_arch\_programfiles()

data access method for architecture-dependent Program Files directory.

This is primarily useful for getting the ProgramFiles directory to which
  32-bit applications are redirected so that you don't have to include
  architecture checks in your code. One statement should work for all
  architectures.

### Returns

a path to where `%ProgramFiles%` will point for an application with the
  specified architecture

### Positional Parameters

1. **architecture**

    the architecture of the (hypothetically) calling program

    OSARCH\_x86 and OSARCH\_x64 are the only currently supported values.

### Examples

- _Copy a File to the Install Dir_

    ```perl
    my $source = '...';
    my $target = get_arch_programfiles(OSARCH_x86).'\AppName\license.dat';

    if (!run_command("copy \"${source}\" \"${target}\"")) {
        output("ERROR!");
    }
    ```

## get\_arch\_software\_key()

Return the actual path to the SOFTWARE (Registry) key that will be accessed
by an application of the target architecture as HKLM\\SOFTWARE or HKCU\\SOFTWARE
(depending on the 'scope' parameter).

This is primarily used to handle 32-bit applications that will be installed
on both 32- and 64-bit operating systems.

### Returns

a Registry path (delimited by 'delimiter')

### Positional Parameters

1. **target\_architecture**

    The architecture of the application that will be attempting to access
    the SOFTWARE key. (typically, OSARCH\_x86)

### Named Parameters

- **scope**

    SCOPE\_SYSTEM (for HKEY\_LOCAL\_MACHINE) or SCOPE\_USER (for HKEY\_CURRENT\_USER)

    SCOPE\_SYSTEM is the default.

    _It is unknown as to whether on not Windows redirects 32-bit application access
    to HKEY\_CURRENT\_USER\\SOFTWARE._

- **delimiter**

    The separator character between registry keys.

    The default is backslash (\\), the default for Win32::TieRegistry.
    If you specified a different delimiter for TieRegistry, you'll want to
    specify the separator here.

### Examples

- _Typical Use_

    ```perl
    my $key = get_arch_software_key(OSARCH_x86).
        '\Adobe\Adobe Acrobat\Registration\\\\SERIAL';
    my $serial = $Registry->{$key};
    ```

- _User Scope_

    ```perl
    my $key = get_arch_software_key(OSARCH_x86, 'scope' => SCOPE_USER ).
        '\Adobe\Adobe Acrobat\9.0\JSPrefs\\\\bEnableJS';
    my $js_enabled = $Registry->{$key};
    ```

- _Different Separator_
    use Win32::TieRegistry( Delimiter => '/' );

    ```perl
    ...

    my $key = get_arch_software_key(OSARCH_x86,'delimiter'=>'/').
        '/Adobe/Adobe Acrobat/Registration//SERIAL';
    my $serial = $Registry->{$key};
    ```

## get\_arch\_system32()

Get the System32 directory as it would be seen by an application of the
specified architecture (32-bit or 64-bit).

### Returns

(Actual) path to the System32 directory.

### Positional Parameters

1. **target\_arch**

    OSARCH\_x86 ('x86') or OSARCH\_x64 ('x64')

### Examples

- _Typical Use_

    ```perl
    my $sys32 = get_arch_system32(OSARCH_x86);
    ...
    ```

## error\_notify()

Notify the user that an error occurred. Don't expect any particular
behavior. It's likely to change on a whim.

The point of this is to create a visible notification that something
failed and requires the installer's attention. It shouldn't cause
any sort of hiccups in an automatic reboot.

In my testing, this mechanism allows the calling process to exit.
However, if the calling process is called by perl using system()
(or run\_command()), that calling process may fail to get the
thread of excution returned to it until the dialog boxes are closed.

I have noticed problems using this mechanism, though I don't have a
solution. Even though the spawned process is not listed as a child,
the parent is unable to exit() (or, exit() doesn't complete) until
the spawned wscript.exe processes go away.

### Returns

Returns a boolean value indicating the success of the notification attempt.
Why? Because I'm anal retentive. I don't expect it to be used.

### Positional Parameters

1. **message**

    a string to be displayed in the message box

    This string may contain newlines, quotes, or other special characters.

### Examples

- _Typical Use_

    ```
    error_notify("The update process failed!!!");
    ```

### Links

- [http://msdn.microsoft.com/en-us/library/windows/desktop/ms645505%28v=vs.85%29.aspx](http://msdn.microsoft.com/en-us/library/windows/desktop/ms645505%28v=vs.85%29.aspx)

## create\_activesetup\_action()

### Description

Active Setup allows you to run custom actions once per user. It compares

> `HKLM\Software\Microsoft\Active Setup\Installed Components\_<ID_`>

and 

> `HKCU\Software\Microsoft\Active Setup\Installed Components\_<ID_`>

If the version number in HKLM is greater, it syncs the registry
information to HKCU, running **StubPath** if it's specified.

This does not appear to be a formally documented feature of
Windows. It has been around for a decade, though.

This mechanism is ideal for any changes that need to affect the user's
profile, especially the roaming profile, exactly once. On machines exhibiting
roaming profile behavior, it will not be
effective for making changes outside the user's roaming profile because the
version checking it does will be saved in the roaming profile. The action
will be performed the first time a user logs into any roaming profile machine
with the Active Setup item configured and won't be run again until the user
logs into a machine with that item configured with a newer **Version**.

### Returns

- **true**

    if the registry keys were created successfully

- **false**

    if the keys weren't created successfully

- **undef**

    if the input parameters did not pass validation tests

### Named Parameters

Every named parameter is treated as an additional value to assign
to the Active Setup key. To specify a type, you need to use the syntax
specified by [Win32::TieRegistry](https://metacpan.org/pod/Win32::TieRegistry), as an arrayref with the data as the
first element and the data type as the second. Data that is not specified
in such a way will be treated as if it were a string (**REG\_SZ**).

```
[ '0x0001', 'REG_DWORD' ]
```

> **REG\_DWORD** data must be specified as two complete bytes (four hex digits).

```
[ pack("LL",1600,1200), 'REG_BINARY' ]

[ 'StringData', 'REG_SZ' ]

[ '%USERPROFILE%\\Desktop', 'REG_EXPAND_SZ' ]

[ "Val1\000Value2\000LastVal\000\000", 'REG_MULTI_SZ' ]
```

> **REG\_MULTI\_SZ** are specified as strings separated by the null character
> (`asc(0)`) and the end of the list must have two null characters (one to
> end the string and the other to end the list), just like any C array would
> have.

- Identifier

    A globally unique identifier for this application. This is commonly the
    application GUID.

- ComponentID

    A name for the component this is related to, usually the name of the
    application.

    This value is a **REG\_SZ**.

- Description

    A brief description of what this does. It is stored as the default value
    for the Active Setup entry.

    This value is a **REG\_SZ**.

- StubPath

    The command to execute.

    This value is a **REG\_EXPAND\_SZ**, which means you may embed environment
    variable references within it.

    THIS MUST BE AN 8.3 PATH. I don't know why. The command just never runs
    otherwise.

- Version

    a version number; Components of the version number should be delimited
    using commas.

    This value is a **REG\_SZ**.

#### Other Values

These are some other values that have been seen in the wild. I don't know
what they may be used for. It's possible that the application itself uses
them as opposed to Windows using them.

- CloneUser

    This value is a **REG\_DWORD**.

- DontAsk

    This value is a **REG\_DWORD** and is usually **2**.

- IsInstalled

    This value is a **REG\_DWORD** and is usually **1**.

- KeyFileName

    This value is a **REG\_SZ**.

- Locale

    This value is a **REG\_SZ**.

- LocalizedName

    This value is a **REG\_EXPAND\_SZ**.

- _othervalue_

    Any other namad parameter to create\_activesetup\_action() will be used as
    another value in the key.

### Examples

#### Delete Desktop Icon

```perl
$success = 1;
if (!create_activesetup_action(
         'Identifier' => 'BlastedApp_FixShortcut_20090512',
         'ComponentID' => 'Appname',
         'Description' => 'Remove shortcut autocreated on user desktop',
         'Version' => '9,0,9342,1',
         'StubPath' => "\%COMSPEC\% /c DEL /Q \"\%USERPROFILE\%\\Desktop\\IrritatingShortcut.lnk\"",
    )) {
    output("     Error: cannot create Active Setup action to delete desktop icon.\n");
    $success = 0;
}
```

### Testing

This function is tested in **test-activesetup.pl**.

### References

_MS appears to have no official documentation for this feature_

- [http://www.etlengineering.com/installer/activesetup.txt](http://www.etlengineering.com/installer/activesetup.txt)
- [http://bonemanblog.blogspot.com/2004/12/active-setup-registry-keys-and-their.html](http://bonemanblog.blogspot.com/2004/12/active-setup-registry-keys-and-their.html)
- [http://makemsi-manual.dennisbareis.com/active\_setup.htm](http://makemsi-manual.dennisbareis.com/active_setup.htm)
- [http://msdn.microsoft.com/en-us/library/ms815104.aspx](http://msdn.microsoft.com/en-us/library/ms815104.aspx)
- [http://support.microsoft.com/kb/277552/en-us](http://support.microsoft.com/kb/277552/en-us)
- [http://support.microsoft.com/kb/238441](http://support.microsoft.com/kb/238441)

## ExitCode()

Calculate the exit code (or "ReturnCode") that will be used by
["run\_command()"](#run_command) to discern success/failure (including what is
specified in the `IgnoreExitCodes` parameter).

### Returns

a value that can be compared to the ReturnCode run\_command() saves
when you use the 'ReturnCommandInfo' parameter.

### Examples

- _Typical Use_

    ```perl
    # Ignore exit code 3010 (I want to reboot).
    run_command('start /wait "" installer.exe',
                'IgnoreExitCodes' => [ ExitCode(3010) ]);
    ```

## cmd\_quote()

A general solution is more complex than I wish to attempt right now.
(I'm thinking about how to escape values with double quotes in them, etc.)

For now, just quote anything with a space in it.

### Returns

a string properly enclosed in quotes (if necessary) suitable for using
in a shell command

### Positional Parameters

1. **data\_value**

    the data value to quote

### Examples

- _Typical Use_

    ```
    run_command(join(' ','DEL','/Q',cmd_quote($file_to_delete)));
    ```

## verify\_registry\_data()

This is used to verify data in the registry to ensure that an attempted
registry modification was successful.

Error checking is often overlooked but terribly valuable. Use this any time
you write to the registry and care at all that it succeeded.

### Limitations

Sometimes, data is stored as "binary" (**REG\_BINARY**) even though it might
otherwise be a more readable data type (strings are the most common form).

At the moment, conversion between encodings is not implemented in
InstallMonkey. The Encoding package is not standard of Perl 5.6 (XP) for
one thing, but it's also a great big headache that I don't care to tackle
just yet.

If you need to verify something registered as REG\_BINARY that's
actually a string or something else, you should know about it ahead of
time and check it against a REG\_BINARY. Anything a script puts in the
registry (via [Win32::TieRegistry](https://metacpan.org/pod/Win32::TieRegistry)) ought to keep the type you specified.

### Returns

- true

    if the data in the registry matches the data in the parameters

- false

    otherwise

### Positional Parameters

1. _key\_to\_verify_

    the registry key to verify

2. _registry\_data_

    the data that sholud match what's in the registry

    This should be in the same form as you'd use with [Win32::TieRegistry](https://metacpan.org/pod/Win32::TieRegistry).

### Named Parameters

- **compare\_binary\_data**

    If **true** and if either the corresponding parameter data or data in
    the registry is specified as **REG\_BINARY**, the parameter data is
    compared with the registry data in a binary fashion (byte-by-byte).

    If **false** and if the data type of the registry data doesn't match, the
    comparison will fail without looking at the data.

    Default: **true**

    **Beware Unicode/multibyte strings**

    Don't try this with integers unless you're confident of byte order.

- **exact\_match**

    If **true**, the data in the registry must match exactly what is
    specified by the positional parameter. This verifies that keys/values
    not specified by the positional parameter do not exist in the
    registry.

    If **false**, keys and values are only checked if specified by the
    positional parameter.

    Default: **false**

- **recurse**

    If this evaluates to **true**, recurse into all subkeys. This should be
    used in conjuction with the **exact\_match** parameter.

    If **false**, only the keys and values specified in the positional
    parameters are checked. No keys/values (in any subkeys specified as
    empty) are checked for nonexistence.

    Default: **false**

### Examples

#### Simple Examples

```perl
if (verify_registry_data('HKEY_LOCAL_MACHINE\SOFTWARE\Vendor\App\Info',
                         { 'Installer' => $ENV{'USERNAME'} })) {
    output("Success!\n");
}


# Specifying a REG_SZ is no different than associating the value with
#   scalar data (previous example).
if (verify_registry_data('HKEY_LOCAL_MACHINE\SOFTWARE\Vendor\App\Info',
                         { 'Installer' => [ $ENV{'USERNAME'}, 'REG_SZ' ] })) {
    output("Success!\n");
}


if (verify_registry_data('HKEY_LOCAL_MACHINE\SOFTWARE\Vendor\App\Info',
                         { 'UseDefaultCreds' => [ '0x0001', 'REG_DWORD' ] })) {
    output("Success!\n");
}


if (verify_registry_data(
        'HKEY_LOCAL_MACHINE\SOFTWARE\Vendor\App\Info', {
            'UseDefaultCreds' => [ '0x0001', 'REG_DWORD' ],
            'Installer' => $ENV{'USERNAME'},
            'SubKey1' => {
                '\\' => 'data for the default value',
                'WorkingDir' => [ '%USERPROFILE%\App\Working', 'REG_EXPAND_SZ' ],
            },
        })) {
    output("Success!\n");
}


# This key should have only one value. None of its subkeys are examined.
if (verify_registry_data('HKEY_LOCAL_MACHINE\SOFTWARE\Vendor\App\Info',
                         { 'OnlyValue' => 'Solo' },
                         'exact_match' => 1 )) {
    output("Success!\n");
}


# This key should have only one value and no subkeys.
if (verify_registry_data('HKEY_LOCAL_MACHINE\SOFTWARE\Vendor\App\Info',
                         { 'OnlyValue' => 'Solo' },
                         'exact_match' => 1,
                         'recurse' => 1)) {
    output("Success!\n");
}


# This key should have only one value and two subkeys, each with one value.
if (verify_registry_data('HKEY_LOCAL_MACHINE\SOFTWARE\Vendor\App\Info',
                         {
                             'OnlyValue' => 'Solo'
                             'SubKey1' => { 'val' => 'data1' },
                             'SubKey2' => { 'val' => 'data2' },
                         },
                         'exact_match' => 1,
                         'recurse' => 1)) {
    output("Success!\n");
}


# This check will not attempt to convert REG_BINARY data to a string
#   for comparisons.
if (verify_registry_data('HKEY_LOCAL_MACHINE\SOFTWARE\Vendor\App\Info',
                         { 'OnlyValue' => 'Solo' },
                         'compare_binary_data' => 0 )) {
    output("Success!\n");
}
```

### References

## generate\_GUID()

Generate a globally-unique identifier (GUID)

### Returns

a GUID in string form (stringified hexadecimal with delimiters)

### Examples

```
$guid = generate_GUID();
```

### References

- [http://www.somacon.com/p113.php](http://www.somacon.com/p113.php)

## expand\_environment\_strings()

Expand any shell environment variables in a string.

This is a direct interface to Windows ExpandEnvironmentStrings() function.

### Returns

a string with any environment variable strings expanded

### Positional Parameters

1. **input\_string**

    a string optionally containing shell environment variable references
    (surrounded by %)

### References

- [http://msdn.microsoft.com/en-us/library/ms724265(VS.85).aspx](http://msdn.microsoft.com/en-us/library/ms724265\(VS.85\).aspx)

## configure\_path()

1. Ensure that the system PATH environment variable has system and admin
directories at the front.
2. Ensure that the PATH ends with a dilimiter (semicolon).
3. Remove any duplicate PATH entries. Duplicates are determined using a
case-insensitive comparison of expanded strings.

### Returns

the new value of the PATH, 0 on failure

### Named Parameters

- **path**

    a string to use as the PATH

    If this is specified, no attempt to access the local system PATH is made.
    The passed path will be configured and returned.

- **save\_old\_path**

    a scalar ref in which to store the old PATH

- **append\_paths**

    an arrayref of directories to append to (or ensure their presence in)
    the system path

    Any directories thus specified will be added to the path if they are not
    already there. Directories will be appended in the order they are
    specified.

    Specified append directories that are already in the PATH will be left
    in place (after the system and priority directories) and not appended.

- **priority\_paths**

    an arrayref of directories of which to make a priority in the system path

    Any priority directories will come immediately after the system/admin
    directories placed first in the path. Priority directories will appear
    in the PATH in the order they are specified.

### Examples

- _Ensure the PATH is Good (for Campus Builds)_

    ```
    if (!configure_path()) {
        output("Cannot validate the system PATH!\n");
    }
    ```

- _Add an item to the PATH_

    ```perl
    if (!configure_path('append' => [$ENV{'SystemDrive'}.'\app\bin'])) {
        output("Error configuring PATH.\n");
    }
    ```

- _Add some item near the front of the PATH_

    ```perl
    if (!configure_path('priority_paths' => [
                            $ENV{'SystemDrive'}.'\watcom-1.3\binnt',
                            $ENV{'SystemDrive'}.'\watcom-1.3\binw',
                        ])) {
        output("Error configuring PATH.\n");
    }
    ```

- _Add some items near the front of the PATH and append some items_

    ```perl
    if (!configure_path('priority_paths' => [
                            $ENV{'SystemDrive'}.'\watcom-1.3\binnt',
                            $ENV{'SystemDrive'}.'\watcom-1.3\binw',
                        ],
                        'append_paths' => [
                            $ENV{'SystemDrive'}.'\App\utils\bin',
                            $ENV{'SystemDrive'}.'\App\tools\bin',
                        ])) {
        output("Error configuring PATH.\n");
    }
    ```

- _Remove a directory from the path_

    ```perl
    if (!configure_path('remove_paths' => [
                            $ENV{'SystemDrive'}.'\App\utils\bin',
                        ])) {
        output("Error configuring PATH.\n");
    }
    ```

- _Recondition the PATH and save the old one_

    ```perl
    $old_path = '';
    if (!configure_path('save_old_path' => \$old_path)) {
        output("Error configuring PATH.\n");
    }
    output("Old PATH: ${old_path}\n");
    ```

## ShellNotify\_WM\_SETTINGSCHANGE()

### Returns

Returns true if the invoked PowerShell command (to broadcast a
WM\_SETTINCGSCHANGE message) succeeds, false otherwise.

### Examples

- _Typical Use_

    ```perl
    # Recondition the path.
    configure_path('path' => configure_path());
    ShellNotify_WM_SETTINGSCHANGE();
    ```

## is\_Win10()

### Returns

Returns true if the OS is any version of Windows 10.

### Examples

- _Simple Detection_

    ```
    if (is_Win10()) {
        configure_win10();
    } else {
        configure_default();
    }
    ```

## is\_Win7()

### Returns

Returns true if the OS is any version of Windows 7.

### Examples

- _Simple Detection_

    ```
    if (is_Win7()) {
        configure_win7();
    } else {
        configure_default();
    }
    ```

## is\_Win8()

### Returns

Returns true if the OS is any version of Windows 8.

### Examples

- _Simple Detection_

    ```
    if (is_Win8()) {
        configure_win7();
    } else {
        configure_default();
    }
    ```

## is\_Vista()

### Returns

Returns true if the OS is any version of Windows Vista.

### Examples

- _Simple Detection_

    ```
    if (is_Vista()) {
        configure_vista();
    } else {
        configure_default();
    }
    ```

## is\_XP()

### Returns

Returns true if the OS is any version of Windows XP.

### Examples

- _Simple Detection_

    ```
    if (is_XP()) {
        configure_xp();
    } else {
        configure_default();
    }
    ```

## is\_x86()

### Returns

Returns true if the OS architecture (not necessarily the CPU
architecture) is x86.

### Examples

- _Simple Detection_

    ```
    if (is_x86()) {
        configure_32bit();
    } else {
        configure_default();
    }
    ```

## is\_x86\_64()

### Returns

Returns true if the OS architecture (not necessarily the CPU
architecture) is amd64/x86\_64.

_(Technically, x64, is not the correct name for this architecture,
but since it is not a competing name, we'll use it as a synonym, too.)_

### Examples

- _Simple Detection_

    ```
    if (is_x86_64()) {
        configure_64bit();
    } else {
        configure_32bit();
    }
    ```

## is\_amd64()

synonym for ["is\_x86\_64()"](#is_x86_64)

## is\_x64()

synonym for ["is\_x86\_64()"](#is_x86_64)

## get\_install\_logs()

### Returns

the default install logs directory

## get\_os\_install\_log()

### Returns

the default os install log (not necessarily the same as the log generated
by postloop.pl)

_This is meaningless on Windows XP._

## check\_flag()

check to see if a flag is set

Flags are case-insensitive (due to filesystem limitations).

These is a notion of "boolean" flags vs. "string" flags in some of the
documentation. There is only one kind of flag. The "type" given to a
flag only suggests its use. "Boolean" flags do not have especially
meaningful contents and are checked to see if they exist or
not. "String" flags have contents that are of interest. The string
flags are used to mimic command-line parameters that take an
additional argument.

### Returns

If the flag exists, it returns its contents. **Warning:** the contents of an
empty file will evaluate to false. For flags representing (true) booleans,
put a '1' in the file.

### Positional Parameters

1. **flag\_name**

    the exact name of the flag

2. **flag\_contents\_return\_parameter**

    A scalarref into which the contents of the flag will be put. Empty
    flag files will produce empty strings as content. Nonexistent files will
    result in **undef** being assigned to the return parameter.

### Examples

- _"Boolean" Flag_

    ```
    if (!check_flag('NO_AUTOMATIC_REBOOT')) {
        reboot_cmd();
    }
    ```

- _String Flag_

    ```perl
    my $target_ou = '';
    if ($target_ou = check_flag('TARGET_OU')) {
        use_different_ou($target_ou);
    }
    ```

## clear\_flag()

Delete a flag, if it exists.

### Returns

Returns 0 if the flag was cleared (or if it never existed), false otherwise.

### Positional Parameters

1. **flag\_name**

    the flag to clear (delete)

### Examples

- _Typical Uses_

    ```
    clear_flag('REPORT_SPURIOUS_ERRORS');
    clear_flag('IGNORE_FLAGS');
    ```

## set\_flag()

Set the specified flag.

### Returns

returns true if the flag was created successfully, false otherwise

### Positional Parameters

1. **flag\_name**

    the name of the flag

2. **flag\_contents** _(optional)_

    If specified, this value will be stored (verbatim) in the flag file.
    Otherwise, '1' will be stored in the file as its only contents.

### Examples

- _Boolean Flags_

    ```
    if (!set_flag('NO_HDD_FORMAT')) {
        print("Cannot set flag for skipping HDD format!\n");
    }
    ```

- _String Flags_

    ```
    if (!set_flag('WSUS_SERVER','http://wsus.mst.edu')) {
        print("Cannot set flag for WSUS server!\n");
    }

    if (!set_flag('REBOOT_DELAY',20)) {
        print("Cannot set flag for reboot delay!\n");
    }
    ```

## get\_file\_contents()

Get the contents of a file and return them as a single scalar.

### Returns

If the file cannot be opened or read, undef is returned.

If no scalar ref is passed in, the contents of the file are
   returned. Otherwise a success/failure boolean value is returned.

### Positional Parameters

1. **file\_to\_read**

    the path to the file

2. **return\_arrayref** _(optional)_

    Each line is put as a separate element in the array. Newlines are not
    stripped in either case. This option would be most useful if you
    wanted to process the contents of the file line by line.

3. **options** _(optional)_

    This is a hashref of options:

    - **encoding**

        the encoding to use when opening the file.

### Examples

- _Simple Examples_

    ```perl
    my $file = $SystemDir.'\\settings.ini';
    my $contents = get_file_contents($file);
    OR
    my @lines = ();
    get_file_contents($file,\@lines);

    # Specify the encoding.
    my $file = $SystemDir.'\\WindowsUpdate.log';
    my $contents = get_file_contents($file,undef,{'encoding'=>'UTF-16LE'});

    # Specify the encoding and a return array.
    my $file = $SystemDir.'\\WindowsUpdate.log';
    my @lines;
    my $contents = get_file_contents($file,\@lines,{'encoding'=>'UTF-16LE'});
    ```

### Comments

I did some performance testing between reading into an array and
join()ing it vs. reading it line by line and appending the new line to
a string.

See IMtest.1\\dev\\perf\\perf\_get\_file\_contents.pl for performance testing
of this implementation.

## cache\_output()

Enable or disable output caching. Output caching only captures what would
get sent to the output log, not the console.

All subsequent calls to ["output()"](#output) are saved in the output cache (instead
of being logged to a file) until ["flush\_output\_cache()"](#flush_output_cache) is called.

### Returns

a boolean value indicating whether output caching is enabled or not

### Positional Parameters

1. **cache\_output\_p** _(optional)_

    a boolean value indicating whether output caching should be enabled

    If this parameter is unspecified, no change is effected. The current
    setting for output caching is returned.

### Examples

- _Check Caching_

    ```perl
    if (cache_output()) {
        output("Output caching is enabled...\n", 'only' => 'console');
    } 
    ```

- _Normal Use_

    ```
    cache_output(1);
    ...
    output(...);
    ...
    init_output_log($dest_drive.'\log.txt');
    flush_output_cache();
    ```

## flush\_output\_cache()

Send the contents of the output cache to the output log and empty the cache.

You will need to turn off caching with ["cache\_output()"](#cache_output) if you do not want
to continue caching output.

### Examples

- _Example1 Description_

    ```
    cache_output(1);
    ...
    output(...);
    ...
    init_output_log($dest_drive.'\log.txt');
    flush_output_cache();
    ```

## ISO\_timestamp()

Generate an ISO 8601 timestamp

### Returns

a string as an ISO 8601 timestamp 

### Positional Parameters

1. **unix\_time** _(optional)_

    the time to format as ISO 8601

### Requires

```
POSIX::strftime()
```

## ISO\_timestamp\_fssafe()

Generate an ISO timestamp that is safe as a file name.

### Returns

a string as an ISO 8601 timestamp 

### Positional Parameters

1. **unix\_time** _(optional)_

    the time to format as ISO 8601

## DEBUG()

Send debugging output (if debugging is enabled) to the console and
output log (if configured).

This behavior is governed by the -IM-debug command-line switch.

### Positional Parameters

1. **message**

    the message to output (if debugging is enabled)

    The message is always output on a line by itself.

2. **debug\_level**

    the minimum debugging verbosity required to print this message

    If no level is specified, the message is always output when debugging
    is enabled.

## DEBUG\_SAFE()

This has the same basic behavior as ["DEBUG()"](#debug) except that it does
not use the output logging/cache system.

It should be safe to call from anywhere.

## DEBUG\_LOG()

Send debugging output (if debugging is enabled) to the output log (if
configured).

This behavior is governed by the -IM-debug command-line switch.

### Positional Parameters

1. **message**

    the message to output (if debugging is enabled)

    The message is always output on a line by itself.

2. **debug\_level**

    the minimum debugging verbosity required to print this message

    If no level is specified, the message is always output when debugging
    is enabled.

## DEBUG\_SUB\_ENTRY()

Log a subroutine entry, including the called subroutine name and any arguments.

The debug level must be set to at least DEBUG\_ENTRY for this function to have
  any effects.

This behavior is governed by the -IM-debug and -IM-debug-level command-line
  switches.

### Examples

- _Typical Use_

    ```perl
    sub mysub {
        DEBUG_SUB_ENTRY();

        ...
    }
    ```

## DEBUG\_SAFE\_SUB\_ENTRY()

This is the same as ["DEBUG\_SUB\_ENTRY()"](#debug_sub_entry) except that it sends output via
["DEBUG\_SAFE()"](#debug_safe).

- _Typical Use_

    ```perl
    sub mysub {
        DEBUG_SAFE_SUB_ENTRY(@_);

        ...
    }
    ```

## log\_message()

Send a message to the output log, including the package, file, line number,
and caller.

### Positional Parameters

1. **message**

    message to write to the output log

2. **call\_level** _(optional)_

    the number of stack traces to go back to determine the "caller"

    default: 0

    You might choose something other than the default for this if you
    wanted the log message to look like it were "from" your own caller
    (instead of from you), say, in a library subroutine.

### Examples

- _Typical Use_

    ```
    if ($error) {
        output("Unexpected Failure!\n");
        log_message("return value not recognized: ${error}\n");
    }
    ```

- _Specifying the Call Level_

    ```perl
    sub cmd_interface {
        my $args = shift;
        my %cmdinfo;
        if (!run_command('exename.exe '.@$args,
                         'ReturnCommandInfo' => \%cmdinfo)) {
            output("Unexpected Failure!\n");
            log_message("command invocation failed:\n".
                        Dumper(\%cmdinfo),1);
        }
    }

    sub run_thorough_query {
        return cmd_interface(qw(-query -thorough));
    }
    ```

## override\_environment()

There are occasions when we need to override values InstallMonkey gets
from the environment as 'use' time. This doesn't actually change any
environment variables or settings, only the values InstallMonkey uses
that it originally got from the environment.

Some of these variables are used later, and may need to be overridden,
like %SystemRoot%.

### Returns

The old value (overridden) is returned.

If an unknown variable is attempted to be overridden, undef is returned.

### Positional Parameters

1. **env\_setting**

    the environment variable name or other environment setting

    This name is case insensitive.

2. **new\_value**

    the new value to assign to **env\_setting**

### Examples

- _Change SystemRoot_

    ```
    if (!override_environment('SystemRoot','R:\Win')) {
        output("Error overriding environment!\n");
        return 0;
    }
    ```

- _Change WINDIR Temporarily_

    ```perl
    my $old_windir = override_environment('WINDIR','R:\Win.001');
    if (!$old_windir) {
        output("Error overriding environment!\n");
        return 0;
    }

    ...

    override_environment('WINDIR',$old_windir);
    ```

### Comments

InstallMonkey expects things of certain environment variables. If you
override them, you'll need to ensure that you sufficiently mimic the desired
behavior.

For example, it is assumed that %SystemRoot%\\System32 contains msiexec.exe,
cacls.exe, tasklist.exe, etc. If you override this and use an InstallMonkey
subroutine that makes such an assumption, you'll need to provide those
executables in the correct place.

## installanywhere\_install()

generic wrapper for InstallAnywhere installers

Usually, you'll generate a response file and won't need to pass any
other arguments to this routine besides that file and the installer
executable.

- runs the installer in silent mode
- attempts to use a response file, if given&lt;/li>
- logs to the default app log location
- can handle 32- and 64-bit installers
- attempts to run the installer synchronously

### Returns

returns **true** if the installer reports success, **false** otherwise.

### Named Parameters

- **installer** _(mandatory)_

    the (relative path to the) installer executable

- **response-file**

    the (relative path to the) InstallAnywhere response file (typically
    created with the `-r` flag.

    InstallAnywhere looks for a file named `installer.properties` in the
    same directory as the installer executable. If you don't specify a
    response file and attempt to do a silent install, InstallAnywhere will
    look for this file.

- **variables**

    a hashref of name => value pairs to be given to the installer as run-time
    variables

    See the [https://wiki.mst.edu/deskinst/change/applications/packaging/installanywhere|InstallAnywhere](https://wiki.mst.edu/deskinst/change/applications/packaging/installanywhere|InstallAnywhere)
    documentation for standard variables and other information.

    The corresponding values will be enclosed in double quotes when they are
    given to the installer on the command line: `-D$VARNAME$="value"`

    Beware of embedding spaces in variable values. They may get lost during 
    parameter passing within multiple nested shell invocations.

- **interface-mode**

    One of

    - **silent** _(default)_
    - **console**
    - **gui**
    - **0**

    given to the installer to set the interface mode. See the InstallAnywhere
    documentation for more information.

    If **0** is used, no interface parameter is given to the installer.

- **start-wait**

    Make an extra effort to keep the called process from returning execution
    to the script.

    If `start-wait` is set to 1, the default 'start /wait' command is used.

    If `start-wait` is set to 0, no wait commaind is used. (This is the default.)

    Otherwise, the value of `start-wait` is prepended to the command (say,
    if you wish to use some other flags of the start command).

- **temporary-log**

    a boolean value

    if true, use an intermediate, temporary log file location
    (writable to 32- and 64-bit processes), then copy the log
    file to the final log file location after the installer runs.
    This is necessary if the installer executable is itself a 32-bit
    application.

- **log-file**

    the complete path to the log file

    This option overrides anything set in [#log-file-name](https://metacpan.org/pod/#log-file-name) and [#log-file-location](https://metacpan.org/pod/#log-file-location).

- **log-file-name**

    the destination log file name (only, no directories)

- **log-file-location**

    use this directory as the final destination of the log file

- **additional-parameters**

    an arrayref of additional command-line parameters to be given to the
    installer

- **allowed-exit-values**

    an arrayref of exit values (from the InstallAnywhere installer) to
    interpret as "success" or "sufficiently successful" when calculating
    the (boolean) return value of this subroutine.

    Some installers return nonzero exit values when rebooting is suppressed
    or other default options are not taken. In these cases, it's helpful
    to be able to tell the InstallAnywhere wrapper to treat certain values as
    successful for the sake of this subroutine's return value.

    Be sure to include `0` (typically, success) in the list, otherwise it
    won't be accepted as successful (even though it probably should be).

### Examples

- _Simple Examples_

    ```perl
    $success &=
        installanywhere_install(
            'installer' => '"'.$srcfiles.'\\app-installer.exe"',
            'response-file' => '"'.$srcfiles.'\\mst-install.properties"',
        );

    $success &=
        installanywhere_install(
            'installer' => '"'.$srcfiles.'\\app-installer.exe"',
            # uses the default 'installer.properties'
        );
    ```

- _Setting the Log File Name_

    ```perl
    # We want to have logs for the update installation, too, but it can't
    #   be the same name as the default app log.
    $success &=
        installanywhere_install(
            'installer' => '"'.$srcfiles.'\\app-update.exe"',
            'response-file' => '"'.$srcfiles.'\\update-1.properties"',
            'log-file-name' => 'app-version-update1.txt',
        );
    ```

- _Temporary Install Log_

    ```perl
    # If the installer is a 32-bit app and it's installing on a 64-bit
    #   OS, the default app log location will be (mostly) inaccessible.
    $success &=
        installanywhere_install(
            'installer' => '"'.$srcfiles.'\\app-install.exe"',
            'response-file' => '"'.$srcfiles.'\\mst-app-install.properties"',
            'temporary-log' => 1,
        );
    ```

- _Allowed Exit Codes_

    ```perl
    # Report success if the installer return 0 or 3
    $success &=
        installanywhere_install(
            'installer' => '"'.$srcfiles.'\\app-install.exe"',
            'response-file' => '"'.$srcfiles.'\\mst-app-install.properties"',
            'allowed-exit-codes' => [ 0, 3 ],
        );
    ```

- _Other Examples_

    ```perl
    # interface-mode
    $success &=
        installanywhere_install(
            'installer' => '"'.$srcfiles.'\\app-install.exe"',
            'response-file' => '"'.$srcfiles.'\\mst-app-install.properties"',
            'interface-mode' => 'console',
        );


    # additional-parameters
    $success &=
        installanywhere_install(
            'installer' => '"'.$srcfiles.'\\app-install.exe"',
            'response-file' => '"'.$srcfiles.'\\mst-app-install.properties"',
            'additional-parameters' => [ '-l', 'en' ], # use English
        );


    # variables
    $success &=
        installanywhere_install(
            'installer' => '"'.$srcfiles.'\\app-install.exe"',
            'response-file' => '"'.$srcfiles.'\\mst-app-install.properties"',
            'variables' => {
                '$UNINSTALLER-TITLE$' => 'My_Installer_Title',
                '$REGISTER_UNINSTALLER_WINDOWS$' => 1,
                '$FEATURE_UNINSTALL_LIST$' => 'feature1,feature2,feature4',
            },
        );


    # start /wait (default mode)
    $success &=
        installanywhere_install(
            'installer' => '"'.$srcfiles.'\\app-install.exe"',
            'response-file' => '"'.$srcfiles.'\\mst-app-install.properties"',
            'start-wait' => '1',
        );

    # start /wait (special parameters)
    $success &=
        installanywhere_install(
            'installer' => '"'.$srcfiles.'\\app-install.exe"',
            'response-file' => '"'.$srcfiles.'\\mst-app-install.properties"',
            'start-wait' => 'start /wait /i "Launching IA Install"',
        );
    ```

### Requires

- [File::Temp::tempfile](https://metacpan.org/pod/File::Temp::tempfile)
- [File::Basename::fileparse](https://metacpan.org/pod/File::Basename::fileparse)
- [InstallMonkey::Shared::generate\_log\_file\_from\_args](https://metacpan.org/pod/InstallMonkey::Shared::generate_log_file_from_args)

### Last Updated

2010-01-21 by Todd Hartman

## installshield\_install()

Perform an automated InstallShield install.

### Returns

true/false on success/failure returned from the installer

### Named Parameters

- **setup** _(optional)_

    the path to the installer executable

    Default: &lt;srcfiles>\\setup.exe

- **iss** (optional)

    the path to the InstallShield response (.ISS) file

    Default: unspecified; If this is unspecified, InstallShield looks for
    `setup.iss` in the directory where `setup.exe` is.

- **additional-parameters**

    an arrayref of additional parameters to pass to the installer on the 
    command line

- **additional-v-parameters**

    an arrayref of additional parameters to pass to the installer on the 
    command line within a single /v"..." parameter.

- **temporary-log**

    use 32-bit accessible locations for log files, then copy them to the
    standard location

- **log-file**

    the complete path to the log file generated by the underlying install 
    mechanism (not the InstallShield log, which contains very little useful
    information other than the return code.

    This option overrides anything set in [#log-file-name](https://metacpan.org/pod/#log-file-name) and [#log-file-location](https://metacpan.org/pod/#log-file-location).

    If this is value evaluates to (boolean) false, no default logging parameters
    will be used. You may wish to use this if you want to specify your own
    logging parameters or the default mechanism for enabling logging
    doesn't work.

- **log-file-name**

    the destination log file name (only, no directories)

- **log-file-location**

    use this directory as the final destination of the log file

- **is-log-file**

    the complete path to the log file InstallShield, which contains little
    other information than the return code.

    This option overrides anything set in [#is-log-file-name](https://metacpan.org/pod/#is-log-file-name) and [#is-log-file-location](https://metacpan.org/pod/#is-log-file-location).

    If this is value evaluates to (boolean) false, no default
    InstallShield log parameters will be used.

- **is-log-file-name**

    the destination InstallShield log file name (only, no directories)

- **is-log-file-location**

    use this directory as the final destination of the InstallShield log file

- **allowed-result-codes**

    an arrayref of InstallShield result codes that should be interpreted
    as "success" or "sufficiently successful"

    Some installers give nonzero result codes when rebooting is suppressed
    or other default options are not taken. In these cases, it's helpful
    to be able to tell the InstallShield wrapper to treat certain codes as
    successful for the sake of this subroutine's return value.

    Be sure to include `0` (typically, success) in the list, otherwise it
    won't be accepted as successful.

- **ignore-is-log**

    Don't use any information \[not\] contained in the IS log file to discern
    success.

    It is all-too common for installers not to create this file (or create
    an empty one), even if the `/f2` is specified. Specify this flag (as
    true) to skip checking the IS log to discern success.

### Requires

- [Win32#GetShortPathName](https://metacpan.org/pod/Win32#GetShortPathName)

### Examples

- _Typical Use_

    ```perl
    output("   Invoking Vendor-Supplied Installer: ");
    if (installshield_install('iss' => 'mst-install.iss')) {
        output("OK\n");
    } else {
        output("FAILED\n");
    }
    ```

- _Specifying Setup.exe_

    ```perl
    output("   Invoking Vendor-Supplied Installer: ");
    if (installshield_install('setup' => 'x64\\setup.exe',
                              'iss' => 'mst-install.iss')) {
        output("OK\n");
    } else {
        output("FAILED\n");
    }
    ```

- _Ignore the IS Log_

    ```perl
    output("   Invoking Vendor-Supplied Installer: ");
    if (installshield_install('ignore-is-log' => 1)) {
        output("OK\n");
    } else {
        output("FAILED\n");
    }
    ```

- _Enable Global MSI Logging During Install_

    ```perl
    output("   Invoking Vendor-Supplied Installer: ");
    if (installshield_install('global-MSI-logging' => 1)) {
        output("OK\n");
    } else {
        output("FAILED\n");
    }
    ```

- _Use intermediate log file location accessible to 32-bit programs_

    ```perl
    output("   Invoking Vendor-Supplied Installer: ");
    if (installshield_install('iss' => 'mst-install.iss',
                              'temporary-log' => 1)) {
        output("OK\n");
    } else {
        output("FAILED\n");
    }
    ```

- _InstallShield Quirks_
    # Sometimes, /v needs a space between it and its args.
    output("   Invoking Vendor-Supplied Installer: ");
    if (installshield\_install('setup' => build\_path('dir','setup.exe'),
                              'quirks-v-space' => 1)) {
        output("OK\\n");
    } else {
        output("FAILED\\n");
    }
- _Change the default logging behavior_

    ```perl
    # Specify only the file name; use the default location.
    output("   Invoking Vendor-Supplied Installer: ");
    if (installshield_install('iss' => 'mst-update-01.iss',
                              'log-file-name' => 'app-update-01.txt')) {
        output("OK\n");
    } else {
        output("FAILED\n");
    }

    # Specify only the location; use the default name (based on the package ID).
    output("   Invoking Vendor-Supplied Installer: ");
    if (installshield_install('iss' => 'mst-configure.iss',
                              'log-file-location' => $ENV{TEMP})) {
        output("OK\n");
    } else {
        output("FAILED\n");
    }

    # Specify the complet path to the log file.
    output("   Invoking Vendor-Supplied Installer: ");
    if (installshield_install('iss' => 'mst-process.iss',
                              'log-file' => $ENV{SystemDrive}.'\\temp\\process.txt')) {
        output("OK\n");
    } else {
        output("FAILED\n");
    }

    # Do not use the default logging parameters.
    output("   Invoking Vendor-Supplied Installer: ");
    if (installshield_install('iss' => 'mst-process.iss',
                              'log-file' => 0)) {
        output("OK\n");
    } else {
        output("FAILED\n");
    }

    # Do not use the default logging parameters and specify your own.
    # The example parameter is completely fictional.
    output("   Invoking Vendor-Supplied Installer: ");
    if (installshield_install('iss' => 'mst-process.iss',
                              'additional-parameters' => [ 'WIERD_LOG_LEVEL=5' ],
                              'log-file' => 0)) {
        output("OK\n");
    } else {
        output("FAILED\n");
    }

    ```

### Last Updated

2010-01-25 by Todd Hartman

## install\_inno()

invoke an Inno Setup installer

### Returns

true/false on success/failure (pass-through)

### Named Parameters

- **setup**

    the .EXE to use as the installer entry point

    By default it looks in SourceFiles.

- **setup-inf**

    The .INF file used to automate the install.

    For Inno, these are usually created by calling the installer with the
      command-line parameter `/SAVEINF=<path_to_file`>

- **logfile**

    The log file to which to write Inno's setup log.

- **additional-parameters**

    Any additional command-line parameters to give to the Inno installer.
    These are appended to the other parameters set by the install.

### Positional Parameters

1. **param1**

    pparam1 description

2. **param2**

    pparam2 description

### Examples

- _Example1 Description_

    ```
    Example 1
    Indented
    ```

## create\_file()

Create a file (and any ancestor directories).

### Returns

returns true if the fie was created successfully, false otherwise.

### Positional Parameters

1. **file\_name**

    The script file to create.

2. **contents**

    The contents of the file.

3. **options** _(optional)_

    a hashref of options:

    - **erase\_directory**

        If true, erase the directory before attempting to create the file.

    - **overwrite**

        If false, return false (and don't replace the file) if it already exists.

### Examples

- _Typical Use_

    ```perl
    # create a file (default options: don't clear dir, overwrite if exists)
    if (!create_file('C:\temp\marker.txt',get_ISO_timestamp())) {
        output("Error creating marker! See the output log for details.\n");
    }

    # create several files (erasing the directory first)
    if (!create_file('C:\temp\dir\marker.txt',get_ISO_timestamp(),
                     {'erase_directory' => 1}) ||
         !create_file('C:\temp\dir\file1.txt',$PID) ||
         !create_file('C:\temp\dir\file2.txt',$PPID))
        ) {
        output("Error creating set of files! See the output log for details.\n");
    }

    # create a file (don't overwrite)
    if (!create_file('C:\temp\marker.txt',get_ISO_timestamp()),
                     {'overwrite' => 0}) {
        output("Error creating marker! See the output log for details.\n");
    }
    ```

## build\_path()

Concatenate a list of directory/filee names together to build a path,
and return the canonical representation of it.

This is strictly a convenience sub for shortening code.

### Returns

a path to the directory/file

### Positional Parameters

a list of directory and file names to concatenate

### Examples

- _Typical Use_

    ```perl
    my $install_dir = build_path(get_arch_programfiles(OSARCH_x86),
                                 'Mozilla Firefox');
    ```

## build\_path\_short()

Concatenate a list of directory/file names together to build a path,
and return the short name to it.

The directory/file must exist in order to get the short name.

This is strictly a convenience sub for shortening code.

### Returns

a short name (8.3) to a file/directory

### Positional Parameters

a list of directory and file names to concatenate

### Examples

- _Typical Use_

    ```perl
    my $short_loc = build_path_short(get_pkg_sourcefiles(),'my setup.exe');
    ```

## LogFreeDiskSpace()

Write a summary of disk usage (for non-network drives) to the output log.

### Returns

true/false on success/failure

### Examples

- _Typical Use_

    ```
    LogFreeDiskSpace();
    ```

## FormatBytes()

Format an integer (bytes) using standard units (KiB, MiB, GiB, ...).

### Returns

a string representation of the storage quantity

### Positional Parameters

1. **bytes**

    integer number of bytes

### Examples

- _Typical Uses_

    ```
    $mbs = FormatBytes('1509949');
    $size = FormatBytes('2034825252355242');
    ```

## CheckForProcess()

Check to see if a process is running. Optionally, wait for it to exit.

### Returns

If WaitForExit is not specified, returns true if the process is running,
  false otherwise.

If WaitForExit is specified, returns true if the process exits before
  the WaitForExit timeout, false otherwise.

### Positional Parameters

1. **hashref\_of\_named\_parameters** (optional)

    If specified, this should be a hashref of the named parameters described below.

2. **process\_match\_criteria**

    The following match criteria may be specified.

    - _exact\_process\_name_

        This is the exact process name, including the extension.

    - regex:_regular\_expression_

        The regular expression string must be preceded by `regex:`

    - pid:_process\_id_

        The process ID number string must be preceded by `pid:`

### Named Parameters

- **ReturnProcessInfo**

    an arrayref into which to store information about matching processes

    All fields returned by tasklist.exe are saved in a hashref, one hashref
    per matching process.

- **WaitForExit**

    the number of seconds to wait for the process to exit

    0: do not wait for the process to exit

    \-1: wait indefinitely for the process to exit

    Default: 120 seconds

- **WaitToStart**

    the maximum number of seconds to wait for the process to start

    0: do not wait for the process to start

    \-1: wait indefinitely for the process to start

- **TasklistFilter**

    any additional filter critera to give to tasklist.exe

    This data should be in the format that tasklist.exe expects, like

    ```
    /FI "USERNAME eq NT AUTHORITY\SYSTEM"
    /FI "CPUTIME gt 00:00:30"
    ```

### Examples

- _Exact Name_

    ```
    if (!CheckForProcess('setup.exe')) {
        output("setup.exe is not running!\n");
    }
    ```

- _Regular Expression_

    ```
    if (!CheckForProcess('regex:setup[0-9A-F]+.exe')) {
        output("setup*.exe is not running!\n");
    }
    ```

- _Process ID_

    ```perl
    if (my $pid = open(my $PIPE,'install.exe |')) {
        if (!CheckForProcess("pid:${pid}")) {
            output("No process with PID ${pid} is running!\n");
        }
    }
    ```

- _Multiple Criteria (Match ANY)_

    ```
    if (!CheckForProcess('setup.exe',
                         'regex:setup\d+.exe',
                         "pid:${PID}",
                         )) {
        output("No matching processes are running!\n");
    }
    ```

- _Wait for Process to Exit_

    ```perl
    # Wait up to 30 minutes for the process to exit.
    if (!CheckForProcess({ 'WaitForExit' => 30*60 },
                         'setup.exe')) {
        output("No matching processes are running!\n");
    }

    # Wait indefinitely for the process to exit.
    if (!CheckForProcess({ 'WaitForExit' => -1 },
                         'setup.exe')) {
        output("No matching processes are running!\n");
    }
    ```

- _Wait for Process to Start_

    ```perl
    # Wait up to one minute for the process to start.
    # Wait the default time for it to exit.
    if (!CheckForProcess({ 'WaitToStart' => 60 },
                         'setup.exe')) {
        output("No matching processes are running!\n");
    }

    # Wait indefinitely for the process to start.
    # Wait the default time for it to exit.
    if (!CheckForProcess({ 'WaitToStart' => -1 },
                         'setup.exe')) {
        output("No matching processes are running!\n");
    }

    # Wait indefinitely for the process to start.
    # Wait indefinitely for it to exit.
    if (!CheckForProcess({ 'WaitToStart' => -1,
                           'WaitForExit' => -1 },
                         'setup.exe')) {
        output("No matching processes are running!\n");
    }

    # Wait up to ten minutes for the process to start.
    # Wait up to an hour for it to exit.
    if (!CheckForProcess({ 'WaitToStart' => 10*60,
                           'WaitForExit' => 60*60 },
                         'setup.exe')) {
        output("No matching processes are running!\n");
    }
    ```

- _Specify Additional tasklist.exe Filter Criteria_

    ```perl
    # Only look at processes owned by the current user.
    my $filter = '/FI "USERNAME ne '.$ENV{USERDOMAIN}.'\\'.$ENV{USERNAME}.'"';
    if (!CheckForProcess({ 'TasklistFilter' => $filter },
                         'setup.exe')) {
        output("No matching processes are running!\n");
    }
    ```

## KillProcess()

Kill any processes matching the specified pattern(s).

### Returns

true if the specified processes are killed (or are not running),
false otherwise.

### Parameters

KillProcess() takes the same parameters as ["CheckForProcesses()"](#checkforprocesses).

Some options don't make sense to use if you want to kill the process,
like [#WaitToExit](https://metacpan.org/pod/#WaitToExit), but no attempt is made to keep you from doing something
so silly.

### Examples

- _Kill Web Browsers_

    ```
    if (!KillProcess(qw(iexplore.exe firefox.exe chrome.exe safari.exe opera.exe))) {
        output("Unsuccessfully attempted to stop web browsers!\n");
        return 0;
    }
    ```

- _Kill with Regexp_

    ```
    if (!KillProcess(q(regexp:acrord(?:32|64)))) {
        output("Unsuccessfully attempted to stop Adobe Reader application!\n");
        return 0;
    }
    ```

- _Kill with PID(s)_

    ```perl
    my @pids_to_kill = qw(1023 1552);
    if (!KillProcess(map { 'pid:'.$_ } @pids_to_kill)) {
        output("Unsuccessfully attempted to kill PIDs: @pids_to_kill!\n");
        return 0;
    }
    ```

## IsAppInstalled()

Check to see whether an application is installed, based on the contents
of the Uninstall (Add/Remove Programs) registry keys.

### Returns

Returns a list of Registry paths corresponding to matching Add/Remove
programs. If no programs match, the list is empty.

### Named Parameters

- **host**

    (optional)

    Attempt to search on the specified host. If no host is specified, the
    local computer is used.

- _Search Criteria_

    You must specify some search criteria. Any one of them may be
    specified as a string, a regex, or a callback coderef.

    One or more of the following search criteria are allowed:

    - **AppID**

        Search for the specified AppID. AppIDs are usually GUIDs, but some
        applications use a human-readable name instead.

    - **AttributeName**

        Search for the specified "attribute." Attributes in this sense correspond
        to Registry values stored in the Add/Remove key.

        If the attribute doesn't exist, this search criterion evaluates to false.

        Typical attributes available: _DisplayName_, _DisplayVersion_, _InstallDate_

    If a callback is specified, it will be invoked for every Add/Remove
    entry that matches the static criteria (if any) and given these
    arguments:

    >     - **Registry\_Path**
    >
    >         the Registry path to the Add/Remove key
    >
    >     - **Registry\_Key**
    >
    >         the Add/Remove registry key (a Win32::TieRegistry tied hashref)
    >
    >     - **Attribute\_Name**
    >
    >         the name of the corresponding attribute (or AppID, if the callback
    >         is for an AppID search criterion)
    >
    >     - **Attribute\_Value**
    >
    >         the value of the corresponding attribute (or AppID, if the callback
    >         is for an AppID search criterion)

### Examples

- _Inspection_

    ```perl
    my $appid = '{AC76BA86-7AD7-1033-7B44-A94000000001}';
    if (my $app_key = IsAppInstalled('AppID' => $appid)) {
        my $name = $app_key->{DisplayName};
        my $version = $app_key->{DisplayVersion};
        output("${name} ${version} is installed.\n");
    }
    ```

- _Uninstall_

    ```perl
    my $appid = '{AC76BA86-7AD7-1033-7B44-A94000000001}';
    if (IsAppInstalled('AppID' => $appid)) {
        my $outcome = install_msi('app_id' => $appid,
                                  'msi_action_flag' => 'x',
                                  'logfile_name' => 'reader9_uninstall.txt');
    }
    ```

- _Search Criteria_

    ```perl
    # Search AppID by Regex
    my $appid = qr/\{AC76BA86-7AD7-1033-7B44-A9[34]000000001\}/;
    if (my @app_keys = IsAppInstalled( 'AppID' => $appid )) {
        ... 
    }

    # Search by an attribute
    if (my @app_keys = IsAppInstalled( 'DisplayName' => qr/Adobe Reader/ )) {
        ... 
    }


    # more than one attribute
    if (my @app_keys = IsAppInstalled( 'DisplayName' => qr/Adobe Reader/,
                                       'DisplayVersion' => '9.3.4' )) {
        ... 
    }

    # Callback on DisplayVersion (to parse the version components)
    if (my @app_keys = IsAppInstalled(
            'DisplayVersion' => sub {
                my ($path,$key,$name,$value) = @_;
                my @ver_parts = split(/\./,$value);
                if ($ver_parts[0] >= 10 ||
                    $ver_parts[0] == 9 && $ver_parts[1] >= 2) {
                    return 1;
                }
                return 0;
            },
        )) {
        ...
    }

    # Callback on AppID (to check for cross-architecture)
    if (my @app_keys = IsAppInstalled(
            'AppID' => sub {
                my ($path,$key,$name,$value) = @_;
                if ($path =~ /Wow6432Node/i) {
                    return 0; # We only want the 64-bit app
                }
                return 1
                    if ($value eq '{AC76BA86-7AD7-1033-7B44-A94000000001}');
                return 0;
            },
        )) {
        ...
    }
    ```

## CompareNumericVersions()

Compare two (strictly) numeric version strings.

Version strings are usually sequences of numbers delimited by periods,
like `11.5.9.620` or `10.2.152.26`.

You can specify a different delimiter (regular expression) to use
version strings delimited with something else.

### Returns

\-1 if the first version number is "less"

0 if the two strings are numerically identical

1 if the second version is "less"

### Positional Parameters

1. **version\_string\_1**
2. **version\_string\_2**

### Named Parameters

- **delimiter**

    a regex string (not a qr// object) matching the version/revision separator

    Default: '\\.'

### Examples

- _Typical Use_

    ```perl
    my $version = get_app_version(); # not an IM function
    my $needed_version = '8.2.23';
    if (CompareNumericVersions($version,$needed_version) >= 0) {
        # proceed
    } else {
        output("Version ${needed_version} is required. Found: ${version}\n");
    }
    ```

- _Alternate Delimiter_

    ```perl
    my $version = get_app_version(); # not an IM function
    my $needed_version = '8,2,23';
    if (CompareNumericVersions($version,$needed_version,
                               'delimiter'=>',') >= 0) {
        # proceed
    } else {
        output("Version ${needed_version} is required. Found: ${version}\n");
    }
    ```

## ProfilingCheckpoint()

Log a profiling checkpoint to the output log.

### Returns

return value(s)

### Positional Parameters

1. **name**

    The name to use for this profiling event.

2. **phase**

    The phase of this profiling event.

    If you specify 'START' or 'FINISH' as the phase, ProfilingCheckpoint()
      will attempt to calculate an elapsed time between 'START' to 'FINISH'
      calls.

    You may specify other "phases," but these will just be included in the
      line sent to the output log and no elapsed time calculation will take
      place.

### Examples

- _Typical Use_

    ```
    ProfilingCheckpoint('APP_INSTALL','START');
    run_command(...);
    ProfilingCheckpoint('APP_INSTALL','FINISH');
    ```

## Sec2HMS()

Convert the specified number of seconds into a (D )HH:MM:SS string.

### Returns

a string representation of the number of seconds

### Positional Parameters

1. **seconds**

    the number of (elapsed) seconds

### Examples

- _Typical Use_

    ```perl
    my $start_time = time();
    ...
    my $elapsed = Sec2HMS(time()-$start_time);
    output("Elapsed time: ${elapsed}\n");
    ```

## WBEMConnect()

Create a WbemServices object, connecting to the specified host and namespace.

### Returns

the WbemServices (via [Win32::OLE](https://metacpan.org/pod/Win32::OLE) object)

### Positional Parameters

- **Server**

    The remote Windows server (any computer) to connect to.

    Default: `.` (localhost)

### Positional Parameters

1. **WMI\_namespace**

    The namespace to access.

    Default: `root/cimv2`

### Examples

- _Typical Uses_

    ```perl
    # Connect to default NS on local machine.
    my $wmi = WBEMConnect();

    # Connect to SMS client namespace.
    my $wmi = WBEMConnect('root/ccm');

    # Connect to a remote host.
    my $wmi = WBEMConnect( Server => 'r01obrennan' );

    # Connect to root/cli on a remote host.
    my $wmi = WBEMConnect( 'root/cli', Server => 'r01obrennan' );
    ```

### Miscellaneous

> [http://msdn.microsoft.com/en-us/library/windows/desktop/aa393720(v=vs.85).aspx](https://metacpan.org/pod/SWbemLocator.ConnectServer\(\)&#x20;Method)
>
> [http://msdn.microsoft.com/en-us/library/windows/desktop/aa389292(v=vs.85).aspx](https://metacpan.org/pod/Constructing&#x20;a&#x20;Moniker&#x20;String)

## WMIGet()

Get a value via a WMI query.

### Returns

- a single (scalar) value, if only one field is specified
- a hashref of (`<name> => <value>`) pairs if multiple fields are specified (If no fields are specified, all fields are returned.)
- an array of hashrefs (if `[#return-multiple](https://metacpan.org/pod/#return-multiple)` is specified)
- undef if the query returns no data, uses invalid field/class names, or an error occurs

### Positional Parameters

1. **WbemServices\_object**

    This is usually a connection to `winmgmts://./root/cimv2`.

2. **WMI\_Class\_Name**

    Win32\_VideoController, Win32\_ComputerSystem, etc.

3. **Selection\_Criteria**

    a (potentially empty) string that is a WQL WHERE clause.

    ```
    'WHERE Size>10'
    'WHERE PhysicalAdapter=1 AND Name LIKE "Intel%"'
    ```

4. + **Fields**  <(multiple)>

    a list of field names (strings) whose values you want to include in the returned result

    ```
    qw(MaxClockSpeed Manufacturer Name NumberOfCores NumberOfLogicalProcessors)
    qw(Name Domain SID Status)
    ```

5. **Options** _(last, optional)_

    A hashref of options:

    - **return-multiple**

        Normally, only a single item (or record) is returned. Specifying this
        option (as true) will result in an arrayref of records to be returned,
        if more than one matches the criteria.

### Examples

- _Typcal Use_

    ```perl
    my $conn = WBemConnect();

    # Find process information.
    my $app_sess = WMIGet($conn,'Win32_Process','WHERE Name="testapp.exe"','SessionId');

    # Free Disk Space on %SystemDrive%
    my $free_space = WMIGet($conn,'Win32_LogicalDisk',
                            'WHERE Name="'.$ENV{SystemDrive}.'"',
                            'FreeSpace');

    # Get all network adapters
    my $nics = WMIGet($conn,'Win32_NetworkAdapter','',{return-multiple=>1});
    ```

## upcase\_r()

Recursively make all scalars uppercase (e.g. for use as case-insensitive 
hash keys) in the specified data structure(s).

### Returns

a copy of the data structure(s) with all scalars upcased

The return value should be stored in an array in more than one parameter
is specified.

### Positional Parameters

1. **item\_n**

    A piece of data to be upcase'd. More than one parameter may be specified.

### Examples

- _Typical Use_

    ```perl
    $struct = {
        'Davis' => [ 'Jim', 'Kyle', 'Perry' ],
        'Smith' => [ 'Sejun', 'Ernest' ],
    };
    my $struct_ucase = upcase_r($struct);
    my $search = 'davis';
    if (exists($struct_ucase->{uc($search)})) {
        print("Known '${search}': ".join(' ',@{$struct_ucase->{uc($search)}})."\n");
    } else {
        print("No '${search}' found.\n");
    }
    ```

- _Multiple Parameters_

    ```perl
    $pgmr_skills = {
        'Davis' => {
           'Jim' => [qw(Perl Java C++)],
           'Kyle' => [qw(c ruby)],
           'Perry' => [qw(ajax)],
        },
        'Smith' => {
            'Sejun' => [qw(C++ Ruby)],
            'Ernest' => [qw(C C++ PYTHON)],
        },
    };
    $lang_skills = {
        'Davis' => {
           'Jim' => [qw(English Spanish)],
           'Kyle' => [qw(English Tagalog)],
           'Perry' => [qw(russian ukranian)],
        },
        'Smith' => {
            'Sejun' => [qw(JAPANESE english)],
            'Ernest' => [qw(esperanto spanish english latin)],
        },
    };
    my @skills = ucase_r($pgmr_skills,$lang_skills);
    ```

## StartProcmonCapture()

Start procmon in capture mode.

It is expected that the script which will also call ["StopProcmonCapture()"](#stopprocmoncapture).

This is designed to allow process-level monitoring for short periods of time
during a deployment.

### Returns

nothing

### Positional Parameters

1. **capture\_name**

    The name of the capture.

    The package name will be prepended and the capture file itself will be
    put it AppLogs.

### Examples

- _Typical Use_

    ```perl
    # Run a capture during some black box step.
    StartProcmonCapture('bb_subcomponent_install');
    my $outcome = 
        run_command(join(' ',$installer,'/Q','/flagA'),
                    Description => 'Invoking strange installer.');
    StopProcmonCapture();
    ```

## StopProcmonCapture()

End all running ProcMon captures.

This is the corresponding call to ["StartProcmonCapture()"](#startprocmoncapture).

### Returns

none

## add\_environment\_variable()
Developed by Billy Rhoades, 2014-05-12

Adds or appends the second parameter to the first parameter. Takes an optional delimiter and an optional
flag to not force an update. Note that if you don't force an update, the variable will not apear until you reboot,
log out / back in, or restart explorer.exe. The update forces windows to reevaluate environment variables from the
registry.

This function modifies the registry to apply the environment changes instead of using setx, which has more interesting
(read: inconsistent) results.

### Returns

true/false on success/failure

### Named Parameters
=over

- **Delimiter**
Custom delimeter. Default: ;
- **NoUpdate** or **No Update**
Don't force update the environment variables from the registry right now. Note that Windows will still force a refresh
on next reboot.

### Positional Parameters

- **Environment Variable Name**

    The Environment variable to modify.

- **Environment Variable Value**

    The value to either assign the variable to, or append with delimiter if the variable already exists.

### Examples

- _Append to Path_
Append to the path with the default delimiter of ;.
    add\_environment\_variable("Path", "C:\\\\Windows\\\\System32\\\\bin");
- _Create New Variable_
Create a brand new variable:
    add\_environment\_variable("somevar", "cmdesk-t1.srv.mst.edu");
- _Append Many Values_
Append several values delimited with the default delimiter ;.
    add\_environment\_variable("somevar", qw(value1 value2 value3 value4));
Or: 
    add\_environment\_variable("somevar", \["value1", "value2", "value3", "value4"\]);
- _Custom Delimiter_
If your environment variable, for some reason, needs a special delimiter.
    add\_environment\_variable("licsrv", "value", Delimiter => '.');
- _Don't Update_
Don't force an update of environment variables from the registry right now.
    add\_environment\_variable("licsrv", "value", NoUpdate => 1 );

## subroutine\_name()

Description of subroutine purpose.

### Returns

return value(s)

### Named Parameters

- **nparam1**

    nparam1 description

- **nparam2**

    nparam2 description

### Positional Parameters

1. **param1**

    pparam1 description

2. **param2**

    pparam2 description

### Examples

- _Example1 Description_

    ```
    Example 1
    Indented
    ```

# POD ERRORS

Hey! **The above document had some coding errors, which are explained below:**

- Around line 11417:

    alternative text 'http://msdn.microsoft.com/en-us/library/windows/desktop/aa393720(v=vs.85).aspx' contains non-escaped | or /

- Around line 11419:

    alternative text 'http://msdn.microsoft.com/en-us/library/windows/desktop/aa389292(v=vs.85).aspx' contains non-escaped | or /

- Around line 11873:

    '=item' outside of any '=over'
