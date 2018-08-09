
**NOTE:** Be extremely careful when dealing with SCCM.  Every Windows desktop, CLC, traveling, and every other non-server system on campus communicates with this server for software and OS deployment.  If you do something wrong, you could force a large software package or an OS reinstall to one or more production systems.  **If you are ever unsure about something, ask.**

## Prerequisites

This documentation will cover the procedure to create a new **package**; not an application.
You should _also_ be very familiar with SCCM terminology, processes, and layout. That information can also be found [here](docs:developer:windows:sccm2012:sccm07migrate).

### Define the package

- Before you create a package, be sure that you know the location of its files on the K drive. It _should_ be located in ```k:\win7\appdist\```.
- Open the SCCM Console and browse to the **Software Library** then **Application Management**.
- Once there, next click on the arrow next to **Packages** to expand the package tree.
- Now click the arrow next to **Production Packages**, and then click on the **Applications & Settings** folder.
- Now, to create a new package, ensure that the ribbon (at the top of the SCCM window) is on the Home tab, and click the **Create Package** button. A window will popup that looks like the picture below. 

![alt text](/Content/images/ToolsDocs/createprogramsccm2012.png "Create Program")

- In the **Name** box, type in the name of the program that you are packaging, such as iTunes.
- You don't need to fill out the **Description** box if you don't want to.
- In the **Manufacturer** box, type the name of the company that created the product you are packaging, if you know who made it, like Apple.
- You don't need to fill in the **Language** box.
- In the **Version** box, put in the version number of the program, such as 5.67 or 12.1.2
- Now, make sure to check the box next to **This package contains source files**. If you don't, then when someone goes to install the program you are packaging, SCCM won't know what to install, or what to run, because it doesn't have any source files to draw from for the package.
- Now, hit browse, and a window should pop up. 

![alt text](/Content/images/ToolsDocs/browseforsourcefiles.png "Browse for Source Files")

- The **Source Folder** box is where you will need to tell SCCM where the source files exist. The source files exist in the ```k:\win7\appdist\``` folder, but you can't use this to tell SCCM where the files exist. You need to use a _servername_ instead.
- In the box, instead of using ```K:\```, you need to use ```\\minerfiles\dfs\software\itwindist\```. You should notice that as you are typing in the box, it will show suggestions with the folders that exist in the last folder you have typed in.
- After this, the path is the same as in the ```K:\``` directory. So type in ```\win7\appdist\``` and then the name of the program you are packaging, then add ```\prod``` to the end. IE, ```\\minerfiles\dfs\software\itwindist\win7\appdist\itunes.12_1_2\prod```
- Now click the **OK** button to save the path for the package, and then click the **Next** button.
- On this page, make sure to not change anything. It should be set to **Standard program**. Click **Next**.
- On this next page, there are a few things to do: 

![alt text](/Content/images/ToolsDocs/programinformation.png "Program Information")

- In the **Name** box, make sure to type ```Default Install``` with a capital 'D' and a capital 'I'.
- Next in the **Command line** box, don't type anything just yet. Hit the **Browse...** button. In the file explorer window that pops up, make sure to change the search option from _Executable Files (\*.exe)_ to _All Files (\*.\*)_. If you typed in the source file path correctly, you should see the ```update.pl``` file. Click on the perl file, and then click **Open**.
- Don't put anything in the **Startup folder** box.
- On the dropdown next to **Run**, change the option to _Hidden_.
- The dropdown next to **Program can run**, change it to _Whether or not a user is logged on_
- **DO NOT** check the box next to **Allow users to view and interact with the program installation**. And don't change the **Drive mode** drop down. Hit **Next**.
- On this page, don't check the **Run another program first**, unless required by the program you are packaging.
- On the **Platform requirements** section, change the option to **This program can run only on specified platforms**, and then check *All Windows 7 (64-bit)*  

![alt text](/Content/images/ToolsDocs/programrequirements.png "Program Requirements")

- On the **Estimated disk space** field, go back to the program's folder under ```K:\``` and navigate to it's prod folder; there should be a data folder there. Now, right click on the data folder, and select properties. Look at the _Size_. IE for iTunes 12.1.2, the size is 156 MB. Now go back to the SCCM console. Since SCCM downloads the data folder, and then installs it, we need to account for twice the size of the data folder. So put two times the size that you just found of the data folder in the **Estimated disk space** field. Also be sure to change the dropdown next to the field to the appropriate bite size, IE: KB, MB, or GB.
- For the **Maximum allowed run time (minutes)** field, figure out how long the program takes to install (you can figure this out by running the perl script for the program you are packaging) and then type in 2 to 3 times as long as it took. 
- Hit **Next**
- This is the summary page, check it over to make sure everything looks good and all of the options are correct. Hit **Next**.
- Wait for it to finish, and then you are done, so hit **Close**

### Add a Distribution Point

- First make sure that you have created a [package](#define-the-package).
- To add a distribution point, find the package you just created in the SCCM Console, and right click on it to bring up a list of options. Select **Distribute Content** and a window should pop up that looks like so: 

![alt text](/Content/images/ToolsDocs/distributecontent-general.png "Distribute Content General")

- There is nothing to do on the first page, so go ahead and click **Next**.
- On this page, we need to tell SCCM where we are going to distribute the content. Notice the red Exclamation mark on the right side of the box. This means that you can't proceed until you do something.
- To fix the issue, click on the **Add** drop down list, and select **Distribution Point Group**. 

![alt text](/Content/images/ToolsDocs/adddistributionpoint.png "Add Distribution Point")

- On the window that pops up, hit the checkbox next to **Default_DPs**, then hit **OK**. 

![alt text](/Content/images/ToolsDocs/selectdistributiongroup.png "Select Distribution Point")

- Now notice that the red exclamation mark is gone. This means you can continue to the next step, so hit **Next**.
- This is the summary page, glance over it to make sure everything looks right, then hit **Next**.
- Finally, wait for it to finish, and then hit **Close**.

### Finding your package on the Software Center

- The software center is located at https://cmdesk-p1.srv.mst.edu/CMApplicationCatalog/#/SoftwareCatalog.
- More than likely, the package you just deployed will not show up here. That doesn't mean you did anything wrong. In fact, you probably didn't. 
- We are currently unsure about how to make a new package show up on the Software Center, but it is something we are working on figuring out.