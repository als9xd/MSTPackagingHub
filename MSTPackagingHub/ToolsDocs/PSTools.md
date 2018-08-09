
# Run CMD as the SYSTEM Account

psexec is a program by [SysInternals](https://wiki.mst.edu/deskinst/training/standard_packaging_procedure#getting_starting) that allows you to launch programs in a variety of useful ways. The feature discussed here is running as a seperate user--- the System account. System is the account that all packages/applications are installed as during an OS Deployment / Software Distribution. Some of the known quirks of running a program as the System account either through SCCM or psexec are:

* All "windows" spawned by this process will be hidden and never be the "active" window. This means that commands such as [WinActivate](https://www.autoitscript.com/autoit3/docs/functions/WinActivate.htm) will work, but subsequent calls to [WinActive](https://www.autoitscript.com/autoit3/docs/functions/WinActive.htm) will fail as the window still lacks focus.
* The user profile is in a location protected by filesystem redirection, so 32-bit processes will see a different user profile than 64-bit processes.
* The account doesn't actually have a desktop, and is often lacking a number of other shell folders.
* The account is prohibited from performing certain operations (i.e. abuse of privilege).

In order to allow psexec to interact with the desktop, you have to associate it with a given session. You can find the session number by inspecting the output of ```qwinsta```. Any session beginning with rdp-tcp is a remote desktop session and console is the computer's console (VMware or monitor). Session 0 is the default session for system-level processes, which is also the default. Interacting with session 0 is difficult, so if you want to test your program in a more sensible fashion, choose one of the normal, interactive sessions.

Pass that session number to ```psexec``` as ```-i```:

```
psexec -s -i 2 \\localhost cmd
```The feature discussed here is running as a seperate user--- the System account. System is the account that all packages/applications are installed as during an OS Deployment / Software Distribution. Some of the known quirks of running a program as the System account either through SCCM or psexec are:
