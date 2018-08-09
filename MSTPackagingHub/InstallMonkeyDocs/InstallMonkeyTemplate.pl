# InstallMonkey Template
# Package Created March 2015
# Packaged by ...
# Last Updated ...

=pod

Begin-Doc
Modified: $Date$
Name: 
Type: script
Description: 
Language: Perl
LastUpdatedBy: $Author$
Version: $Revision$
Doc-Package-Info: 
Doc-SVN-Repository: $URL$
RCSId: $Id$
End-Doc

=cut

#
# Many of the comments in this file are only reminders for when you create
#   a package. Remove all of these comments that tell people how to use
#   InstallMonkey before you declare the package finished.
#

use strict;
use warnings;

# Global InstallMonkey options that must be specified before you load the
#   module.
our %INSTALLMONKEY_OPTIONS;
BEGIN {
    %INSTALLMONKEY_OPTIONS = (
        # same name as the appdist directory
        package_id => 'Package.Version',

        # some unique id that's updated any time the package undergoes
        #  any sort of minor revision
        package_revision => '20140115T1200',

#         # if different from default location
#         # Set this to 'DISABLED' to disable output().
#         output_log => 'path_to_output_log',
    );
}

# Add InstallMonkey Library to the path
use lib (
    '\\\\minerfiles.mst.edu\dfs\software\loginscripts\im',
    $ENV{'WINDIR'}.'\SYSTEM32\UMRINST',
    'C:\temp',
);
use InstallMonkey::Shared;

use Getopt::Long;

sub usage {
    print qq(
usage: $0 [--help] [--uninstall]

);
}
GetOptions(
    'help' => sub { usage(); exit(0); },
    'uninstall' => sub { exit(!uninstall()); },
);

sub uninstall {
}

# Be sure that any custom install subs (install_sub, preinstall_sub, or 
# postinstall_sub) return a success/failure value.
# If you don't do any error checking, at least return 1 so that the
# install will continue. The installer aborts (nicely) on failure.
#
# sub install {
#   ...
#
#   return 1;
# }

# You may want to use Getopt::Long to parse command-line flags.
# use Getopt::Long;
# $test = $license = $port = 0;
# @hosts = ();
# GetOptions(
#   'test' => \$test,
#   'license=s' => \$license,
#   'port=i' => \$port,
#   'hostname=s' => \@hosts,
# );

# Use run_command() instead of system() unless you really, really, REALLY
#   don't want to leave any trace of what you're doing. run_command() sends
#   lots of helpful information to the output log so that you don't have to
#   rerun the script with other options to see a reasonable amount of output.
# DIFFERENCE: run_command() returns a boolean value (true: success) whereas
#   system() returned the exit code (and signal) of the command.
#   If you used this:
#     $rc = system($command);
#     if ($rc) {
#        print("ERROR: $rc\n");
#     }
#   Consider using something like this instead:
#     if (!run_command($command)) {
#        output("ERROR running command.\n");
#     }
#   For more verbose checking, consider using the 'ReturnCommandInfo'
#     option. See the InstallMonkey source code for more examples.

# If msi name cannot be "installer.msi" then add this line to do_install:
# msi_name => "foo_bar.msi",

# Only add these as we plan to test them.
# allowed_versions:
#  OSVER_XP_32,  OSVER_VISTA_SP0,  OSVER_VISTA_SP1,  OSVER_VISTA_SP2,
#  OSVER_WIN7_SP0, OSVER_WIN7_SP1
# allowed_os_architectures:
#  OSARCH_x86,  OSARCH_x64
# allowed_regs:
#  qw(clc desktop traveling virtual-clc virtual-desktop)
# application_architecture:
#  OSARCH_x86,  OSARCH_x64
#
# If you need to abort the install outside of do_install(), please
#   use IM_Exit() and return an appropriate (shell) error code.
#   0                => success
#   1 (any non-zero) => failure
# IM has a few predefined exit codes, EXIT_*
#    IM_Exit(EXIT_FAILURE,"The install failed.\n");
#
do_install( 
    allowed_versions => [],
    allowed_os_architectures => [],
    allowed_regs => [],
    exit_on_failure => 1,
#    prerequisite_sub => \&prereq_sub,
#    prerequisite_sub_x86 => \&prereq86_sub,
#    prerequisite_sub_x64 => \&prereq64_sub,
#    preinstall_sub => \&preinst_sub,
#    preinstall_sub_x86 => \&preinst86_sub,
#    preinstall_sub_x64 => \&preinst64_sub,
#    install_sub => \&inst_sub,
#    install_sub_x86 => \&inst86_sub,
#    install_sub_x64 => \&inst64_sub,
#    postinstall_sub => \&postinst_sub,
#    postinstall_sub_x86 => \&postinst86_sub,
#    postinstall_sub_x64 => \&postinst64_sub,
#    msi_name => 'installer.msi',
#    msi_name_x86 => 'installer86.msi',
#    msi_name_x64 => 'installer64.msi',
#    msi_ignore_exit_codes => [ ],
#    msi_ignore_exit_codes_x86 => [ ],
#    msi_ignore_exit_codes_x64 => [ ],
#    application_architecture => OSARCH_x86, # only if it's a 32-bit app
#    additional_msi_properties => 'REBOOT="ReallySuppress"',
#    additional_msi_properties_x86 => 'REBOOT="ReallySuppress"',
#    additional_msi_properties_x64 => 'REBOOT="ReallySuppress"',
);

# ALL installers need to have an explicit exit code.
IM_Exit(EXIT_SUCCESS);

### subroutine documentation block ###
# Begin-Doc
################################
# Name: 
# Description: 
# Returns: 
# Requires: 
# LastUpdated: 
# LastUpdatedBy: 
################################
# End-Doc

